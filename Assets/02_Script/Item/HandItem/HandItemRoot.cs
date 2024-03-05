using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class HandItemRoot : MonoBehaviour
{

    [field:SerializeField] public Vector3 handRotation { get; protected set;}
    [field:SerializeField] public Vector3 handPivot { get; protected set; }

    public abstract void DoUse();

}
