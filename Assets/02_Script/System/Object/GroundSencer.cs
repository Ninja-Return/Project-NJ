using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSencer : MonoBehaviour
{

    public bool IsGround { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        ///
        IsGround = !other.isTrigger;

    }

    private void OnTriggerStay(Collider other)
    {

        IsGround = !other.isTrigger;

    }

    private void OnTriggerExit(Collider other)
    {

        IsGround = false;

    }


}
