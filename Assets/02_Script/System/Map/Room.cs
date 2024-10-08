using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using WebSocketSharp;

public enum Diractions
{

    Fwd,
    Bak,
    Rit,
    Lft

}

[System.Serializable]
public struct CloseData
{

    public Diractions dir;
    public GameObject openObject;
    public GameObject closeObject;

    public void Close()
    {

        if (openObject == null || closeObject == null) return;

        openObject.SetActive(false);
        closeObject.SetActive(true);

    }

}

[System.Serializable]
public struct DirLinks
{

    public Diractions dir;

}

public class Room : NetworkBehaviour
{
    
    [field:SerializeField] public DirationType dir { get; private set; }
    [SerializeField] private List<CloseData> closeDatas;


    public void Close(List<Diractions> closeDirs)
    {

        foreach(var item in closeDirs) 
        {

            var eqT = GetEqType(item);

            CloseClientRPC(eqT);
        
        }

    }

    [ClientRpc]
    private void CloseClientRPC(Diractions eqT)
    {

        closeDatas.Find(x => x.dir == eqT).Close();

    }

    public override void OnNetworkSpawn()
    {

        ChunkRenderingSystem.Instance.AddChunk(transform.position, GetComponentsInChildren<Renderer>());

    }

    private Diractions GetEqType(Diractions item)
    {

        if (transform.eulerAngles.y == 0) return item;
        else if (transform.eulerAngles.y == 90) return item switch
        {
            Diractions.Fwd => Diractions.Lft,
            Diractions.Bak => Diractions.Rit,
            Diractions.Rit => Diractions.Fwd,
            Diractions.Lft => Diractions.Bak,
            _ => Diractions.Fwd
        };
        else if(transform.eulerAngles.y == 180) return item switch
        {
            Diractions.Fwd => Diractions.Bak,
            Diractions.Bak => Diractions.Fwd,
            Diractions.Rit => Diractions.Lft,
            Diractions.Lft => Diractions.Rit,
            _ => Diractions.Fwd
        };
        else return item switch
        {
            Diractions.Fwd => Diractions.Rit,
            Diractions.Bak => Diractions.Lft,
            Diractions.Rit => Diractions.Bak,
            Diractions.Lft => Diractions.Fwd,
            _ => Diractions.Fwd
        };

    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {

        if(dir == DirationType.Dir1)
        {

            Gizmos.DrawLine(transform.position, transform.position + (-transform.right * 20));

        }
        if(dir == DirationType.Dir2_T_I)
        {

            Gizmos.DrawLine(transform.position, transform.position + (-transform.right * 20));
            Gizmos.DrawLine(transform.position, transform.position + (transform.right * 20));

        }
        if(dir == DirationType.Dir2_T_L)
        {

            Gizmos.DrawLine(transform.position, transform.position + (-transform.right * 20));
            Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 20));

        }
        if(dir == DirationType.Dir3)
        {

            Gizmos.DrawLine(transform.position, transform.position + (-transform.right * 20));
            Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 20));
            Gizmos.DrawLine(transform.position, transform.position + (transform.right * 20));

        }
        if(dir == DirationType.Dir4)
        {

            Gizmos.DrawLine(transform.position, transform.position + (-transform.right * 20));
            Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 20));
            Gizmos.DrawLine(transform.position, transform.position + (transform.right * 20));
            Gizmos.DrawLine(transform.position, transform.position + (-transform.forward * 20));

        }
        if(dir == DirationType.Room)
        {
            Gizmos.DrawLine(transform.position, transform.position + (-transform.right * 20));

        }
            Gizmos.DrawWireCube(transform.position, new Vector3(15, 6, 15));



    }

#endif

}
