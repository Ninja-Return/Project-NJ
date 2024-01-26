using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DebugSC : NetworkBehaviour
{

    void Start()
    {
        float time = 0f;
        float result = (Mathf.Sin(time - Mathf.PI / 2) + 1) / 2;
        Debug.Log("Result: " + result);
    }

}
