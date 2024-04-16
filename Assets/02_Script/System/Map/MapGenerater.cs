using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public enum DirationType
{

    Dir1,
    Dir2_T_I,
    Dir2_T_L,
    Dir3,
    Dir4,
    Room

}


public class MapGenerater : NetworkBehaviour
{

    [SerializeField] private List<Room> constRooms = new();
    [SerializeField] private Room onlySpawn;

    private List<MapDataSO> datas = new();
    private Dictionary<DirationType, List<Room>> roomContainer = new();
    private List<(MapCell cell, Room room)> creationRoomList = new();
    private List<Room> spawnConstRoom = new();
    private StartRoom start;
    private bool onlyObjSpawn;

    private void Start()
    {

        if (!IsServer) return;

        LoadResource();

        var dirs = new Vector2[] { Vector2.zero, Vector2.left, Vector2.right, Vector2.up, Vector2.down };

        foreach(var item in dirs)
        {

            spawnConstRoom = constRooms.ToList();
            var data = datas.GetRandomListObject();
            LoadData(data, item * data.data.size * 30);

        }

        SpawnNetwork();
        Close();

        var pos = FindObjectOfType<StartingRoom>().startingPos;
        PlayerManager.Instance.RequstSpawn(pos);

    }

    private void SpawnNetwork()
    {

        foreach(var item in creationRoomList)
        {

            item.room.NetworkObject.Spawn(true);

        }

    }

    private void LoadResource()
    {

        var datas = Resources.LoadAll<Room>("RoomData/Corridor").ToList();
        datas.AddRange(Resources.LoadAll<Room>("RoomData/Room"));

        this.datas = Resources.LoadAll<MapDataSO>("MapData").ToList();

        foreach(var item in datas)
        {

            if (roomContainer.ContainsKey(item.dir))
            {

                roomContainer[item.dir].Add(item);

            }
            else
            {

                roomContainer.Add(item.dir, new() { item });

            }

        }

    }

    private void LoadData(MapDataSO data, Vector2 offset)
    {

        int mx = data.data.size.x;
        int my = data.data.size.y;
        var mdata = data.data;

        List<(Vector2Int idx, GameObject obj)> rotateRoomConatiner = new();

        for(int x = 0; x < mx; x++)
        {

            for(int y = 0; y < my; y++)
            {

                if (mdata[x, y] == MapCell.None) continue;

                var obj = CreateProperObject((int)mdata[x, y], new Vector2(x, y), offset);

                if(obj != null)
                {

                    rotateRoomConatiner.Add((new Vector2Int(x, y), obj));

                }

            }

        }

        foreach(var obj in rotateRoomConatiner)
        {

            RotateRoom(obj.idx, mdata.size, mdata, obj.obj);

        }

    }

    private void Close()
    {

        foreach(var item in creationRoomList)
        {

            var ls = new RPCList<DirLinks>(GetCloseData(item.cell, item.room));

            item.room.CloseClientRPC(ls.Serialize());

        }

    }

    private void RotateRoom(Vector2Int originIndex, Vector2Int size, Array2D<MapCell> data, GameObject obj)
    {

        var dirs = new Vector2Int[]{ Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };

        foreach(var item in dirs)
        {

            var idx = originIndex + item;

            if (idx.x >= 0 && idx.y >= 0 && idx.x < size.x && idx.y < size.y)
            {

                if (data[idx.x, idx.y] == MapCell.None) continue;

                var c = data[idx.x, idx.y];
                if(CheckRotateAble(data[idx.x, idx.y], item, out var rot))
                {

                    obj.transform.rotation = rot;
                    break;

                }

            }

        }

    }

    private bool CheckRotateAble(MapCell cell, Vector2Int curRot, out Quaternion rot)
    {

        rot = Quaternion.identity;

        if (curRot == Vector2Int.left && cell.HasFlag(MapCell.Right))
        {

            return true; 

        }
        if(curRot == Vector2Int.right && cell.HasFlag(MapCell.Left))
        {

            rot = Quaternion.Euler(0, 180, 0);
            return true;

        }
        if(curRot == Vector2Int.up && cell.HasFlag(MapCell.Forward))
        {

            rot = Quaternion.Euler(0, 270, 0);
            return true;

        }
        if (curRot == Vector2Int.down && cell.HasFlag(MapCell.Back))
        {

            rot = Quaternion.Euler(0, 90, 0);
            return true;

        }

        return false;

    }

    private GameObject CreateProperObject(int dir, Vector2 index, Vector2 offset)
    {

        Room prefab = null;
        var dirT = GetDirationType(dir);

        if (!onlyObjSpawn && dirT == DirationType.Room)
        {

            prefab = onlySpawn;
            onlyObjSpawn = true;

        }
        else if (spawnConstRoom.Count > 0 && dirT == DirationType.Room)
        {

            prefab = spawnConstRoom[0];
            spawnConstRoom.RemoveAt(0);

        }
        else
        {

           prefab = roomContainer[dirT].GetRandomListObject();

        }

        var room = Instantiate(prefab, new Vector3(index.x * 30 + offset.x, 0, -index.y * 30 - offset.y), RotateRoad(dir));

        creationRoomList.Add(((MapCell)dir, room));

        if (dirT == DirationType.Room) return room.gameObject;


        return null;

    }


    private Quaternion RotateRoad(int dir)
    {

        if (dir == 4 || dir == 6 || dir == 14 || dir == 12) return Quaternion.Euler(0, 90, 0);
        if (dir == 2 || dir == 10 || dir == 11) return Quaternion.Euler(0, 180, 0);
        if (dir == 8 || dir == 9 || dir == 13) return Quaternion.Euler(0, 270, 0);

        return Quaternion.identity;

    }

    private DirationType GetDirationType(int dir)
    {

        if (dir == 1 || dir == 2 || dir == 4 || dir == 8) return DirationType.Dir1;
        if (dir == 3 || dir == 12) return DirationType.Dir2_T_I;
        if (dir == 5 || dir == 6 || dir == 9 || dir == 10) return DirationType.Dir2_T_L;
        if (dir == 7 || dir == 11 || dir == 13 || dir == 14) return DirationType.Dir3;
        if (dir == 15) return DirationType.Dir4;

        return DirationType.Room;

    }

    private List<DirLinks> GetCloseData(MapCell cell, Room room)
    {

        List<DirLinks> dirs = new();

        foreach(MapCell item in Enum.GetValues(typeof(MapCell)))
        {

            if (item == MapCell.Room || item == MapCell.None || !cell.HasFlag(item)) continue;

            var checkPos = (GetDirByCell(item) * 30) + room.transform.position;
            var revCell = GetReverceCell(item);

            var obj = creationRoomList.FindIndex(x => 
            (x.cell.HasFlag(revCell) || x.cell == MapCell.Room)
            && x.room.transform.position == checkPos);
            

            if(obj == -1)
            {

                dirs.Add(new DirLinks() { dir =  GetDirationType(item) });

            }

        }

        return dirs;

    }

    private Vector3 GetDirByCell(MapCell cell)
    {

        return cell switch
        {

            MapCell.None => Vector3.zero,
            MapCell.Left => Vector3.left,
            MapCell.Right => Vector3.right,
            MapCell.Forward => Vector3.forward,
            MapCell.Back => Vector3.back,
            MapCell.Room => Vector3.zero,
            _ => Vector3.zero

        };

    }

    private MapCell GetReverceCell(MapCell cell)
    {

        return cell switch
        {


            MapCell.Left => MapCell.Right,
            MapCell.Right => MapCell.Left,
            MapCell.Forward => MapCell.Back,
            MapCell.Back => MapCell.Forward,
            _ => MapCell.None

        };

    }

    private Diractions GetDirationType(MapCell cell)
    {

        return cell switch
        {


            MapCell.Left => Diractions.Lft,
            MapCell.Right => Diractions.Rit,
            MapCell.Forward => Diractions.Fwd,
            MapCell.Back => Diractions.Bak,
            _ => Diractions.Rit

        };

    }

}
