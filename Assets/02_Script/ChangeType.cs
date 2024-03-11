using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeType : MonoBehaviour
{
    
    public void Change()
    {

        foreach(var item in GetComponentsInChildren<MeshRenderer>())
        {

            item.AddComponent<MeshCollider>();

        }

    }

}
