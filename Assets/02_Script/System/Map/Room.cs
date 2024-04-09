using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Room : MonoBehaviour
{
    
    [field:SerializeField] public DirationType dir { get; private set; }


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

            Gizmos.DrawWireCube(transform.position, new Vector3(30, 3, 30));
            Gizmos.DrawLine(transform.position, transform.position + (-transform.right * 20));

        }

    }

#endif

}
