using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LightMonster : NetworkBehaviour
{
    public void Spawn(Transform targetTrm)
    {

        StartCoroutine(MovementCo(targetTrm));

    }

    private IEnumerator MovementCo(Transform trm)
    {

        var startPos = transform.position;

        float per = 0;

        while (per <= 1)
        {

            transform.position = Vector3.Lerp(startPos, trm.position, per);
            per += Time.deltaTime;

            yield return null;

        }

    }

}
