using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RoomCeil
{

    public Vector3 offset;
    public CellData cell;

}

public class Room : MonoBehaviour
{
    
    [field:SerializeField] public List<RoomCeil> cells { get; private set; }

    public static Vector3 GetDirByVector(DirationType dir)
    {

        return dir switch
        {
            DirationType.Left => Vector3.left,
            DirationType.Right => Vector3.right,
            DirationType.Forward => Vector3.forward,
            DirationType.Back => Vector3.back,
            _ => Vector3.zero
        };

    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {

        if (cells == null || cells.Count == 0) return;

        foreach(var cell in cells)
        {

            var origin = transform.position + new Vector3(cell.offset.x * 12, cell.offset.y * 3, cell.offset.z * 12);

            Gizmos.DrawWireCube(origin,
                new Vector3(12, 3, 12));

            foreach(var item in cell.cell.doorData)
            {

                var vec = GetDirByVector(item);
                Gizmos.DrawWireSphere(origin + (vec * 6), 0.5f);

            }

        }

    }

#endif

}
