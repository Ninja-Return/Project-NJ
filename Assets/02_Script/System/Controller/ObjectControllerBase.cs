using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectControllerBase : MonoBehaviour
{

    public abstract void Move(Vector3 pos);
    public abstract void Rotate(Vector3 dir);

}
