using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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


public class MapGenerater : MonoBehaviour
{

    private List<MapDataSO> datas = new();
    private Dictionary<DirationType, List<Room>> roomContainer = new();

    private void Start()
    {

        LoadResource();

        var dirs = new Vector2[] { Vector2.zero, Vector2.left, Vector2.right, Vector2.up, Vector2.down };

        foreach(var item in dirs)
        {

            var data = datas.GetRandomListObject();
            LoadData(data, item * data.data.size * 30);

        }


    }

    private void LoadResource()
    {

        var datas = Resources.LoadAll<Room>("RoomData");
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

                    Debug.Log(rot.eulerAngles);
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

        var dirT = GetDirationType(dir);
        var prefab = roomContainer[dirT].GetRandomListObject();

        var room = Instantiate(prefab, new Vector3(index.x * 30 + offset.x, 0, -index.y * 30 - offset.y), RotateRoad(dir));

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

}
