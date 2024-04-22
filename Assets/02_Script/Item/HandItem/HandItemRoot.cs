using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class HandItemRoot : MonoBehaviour
{

    [field:SerializeField] public Vector3 handRotation { get; protected set;}
    [field:SerializeField] public Vector3 handPivot { get; protected set; }
    [field:SerializeField] public bool isLocalUse {  get; protected set; }
    [field:SerializeField] public bool is1Use { get; protected set; }

    public string extraData { get; set; }

    public abstract void DoUse();

    public virtual void SetUpExtraData(string extraData) { this.extraData = extraData; }
    public virtual void DoRelease() { }

}
