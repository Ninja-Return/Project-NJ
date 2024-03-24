using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RoomData : NetworkBehaviour
{
    
    public enum Dir
    {

        Fwd,
        Bak,
        Rit,
        Lft

    }

    [System.Serializable]
    public struct CloseData
    {

        public Dir dir;
        public GameObject openObj;
        public GameObject closeObj;

    }

    [field:SerializeField] public List<Dir> dirs { get; private set; } = new();
    [field:SerializeField] public List<CloseData> closeDatas { get; private set; } = new();

    [ClientRpc]
    public void CloseClientRPC(Dir dir)
    {

        var data = closeDatas.Find(x => x.dir == dir);

        if (data.openObj == null) return;

        data.closeObj.SetActive(true);
        data.openObj.SetActive(false);

    }

    public void Close(Dir dir)
    {

        var data = closeDatas.Find(x => x.dir == dir);

        if (data.openObj == null) return;

        data.closeObj.SetActive(true);
        data.openObj.SetActive(false);

    }


    public static Vector3 GetDirVec(Dir dir)
    {

        return dir switch
        {

            Dir.Fwd => Vector3.forward,
            Dir.Bak => Vector3.back,
            Dir.Rit => Vector3.right,
            Dir.Lft => Vector3.left,
            _ => Vector3.zero

        };

    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {

        Gizmos.DrawWireCube(transform.position + new Vector3(0, 2, 0), new Vector3(12, 4, 12));

        foreach(var dir in dirs)
        {

            var dirvec = GetDirVec(dir) * 12;
            Gizmos.DrawWireSphere(transform.position + dirvec, 0.3f);

        }

    }

#endif

}
