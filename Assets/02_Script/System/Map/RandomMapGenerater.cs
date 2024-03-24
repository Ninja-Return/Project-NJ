using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Netcode;
using UnityEngine;

public class RandomMapGenerater : NetworkBehaviour
{

    [SerializeField] private Transform roomRoot;
    [SerializeField] private RoomData startRoom;
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private float subtractValue = 0.01f;
    [SerializeField] private RoomData constRoom;

    private Dictionary<RoomData.Dir, List<RoomData>> dirByRoom = new();
    private Dictionary<Vector3, RoomData> vecByRoom = new();
    private HashSet<Vector3> constDir = new();
    private List<RoomData> createRoom = new();

    private void Start()
    {

        if(!IsServer) return;

        startRoom = Instantiate(startRoom, Vector3.zero, Quaternion.identity);
        startRoom.NetworkObject.Spawn(true);

        GetRoomResource();
        SpawnRoom();
        CloseAllRoom();

        surface.BuildNavMesh();

        EndSpawnClientRPC();

        Resources.UnloadUnusedAssets();

    }

    private void GetRoomResource()
    {

        var res = Resources.LoadAll<RoomData>("Room");

        foreach(var roomData in res)
        {

            if(roomData == constRoom || roomData == startRoom) continue;

            foreach(var dir in roomData.dirs)
            {


                if (dirByRoom.ContainsKey(dir))
                {

                    dirByRoom[dir].Add(roomData);

                }
                else
                {

                    dirByRoom.Add(dir, new() { roomData });

                }

            }

        }

    }

    private RoomData.Dir GetRevDir(RoomData.Dir dir)
    {

        return dir switch
        {

            RoomData.Dir.Fwd => RoomData.Dir.Bak,
            RoomData.Dir.Bak => RoomData.Dir.Fwd,
            RoomData.Dir.Rit => RoomData.Dir.Lft,
            RoomData.Dir.Lft => RoomData.Dir.Rit,
            _ => RoomData.Dir.Fwd

        };

    }

    private void SpawnRoom()
    {

        var rooms = new Queue<RoomData>();
        rooms.Enqueue(startRoom);
        constDir.Add(Vector3.zero);
        float per = 1;

        bool isControomSpawnd = false;
        int spcnt = 0;

        while(rooms.Count > 0 || !isControomSpawnd)
        {

            var room = rooms.Dequeue();
            room.transform.SetParent(roomRoot);
            createRoom.Add(room);
            vecByRoom.Add(room.transform.position, room);
            
            foreach(var dir in room.dirs)
            {

                if(Random.value < per)
                {

                    var vec = room.transform.position + (RoomData.GetDirVec(dir) * 12);

                    if (constDir.Contains(vec)) continue;
                    constDir.Add(vec);

                    RoomData cloneRoom;

                    if(!isControomSpawnd && spcnt > 40)
                    {

                        isControomSpawnd = true;
                        cloneRoom = Instantiate(
                        constRoom,
                        vec, Quaternion.identity);

                    }
                    else
                    {

                        cloneRoom = Instantiate(
                        dirByRoom[GetRevDir(dir)].GetRandomListObject(),
                        vec, Quaternion.identity);

                    }

                    cloneRoom.NetworkObject.Spawn(true);

                    rooms.Enqueue(cloneRoom);
                    spcnt++;

                    per -= subtractValue;

                }

            }

        }



    }

    private void CloseAllRoom()
    {

        foreach(var item in createRoom)
        {

            foreach(var dir in item.dirs)
            {

                var vec = item.transform.position + (RoomData.GetDirVec(dir) * 12);

                if (!constDir.Contains(vec))
                {

                    item.CloseClientRPC(dir);

                }
                else if(vecByRoom.TryGetValue(vec, out var room))
                {

                    if(room.dirs.FindIndex(x => x == GetRevDir(dir)) == -1)
                    {

                        item.CloseClientRPC(dir);

                    }

                }

            }

        }

    }

    [ClientRpc]
    private void EndSpawnClientRPC()
    {


        StaticBatchingUtility.Combine(roomRoot.gameObject);


    }

}
