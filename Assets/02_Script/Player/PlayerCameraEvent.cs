using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCameraEvent : NetworkBehaviour
{

    [SerializeField] private Transform cam;
    [SerializeField] private LayerMask monsterLayer;
    [SerializeField] private float lenght = 10;
    [SerializeField] private float angle = 70;

    private PlayerImpulse impulse;

    private bool surpriseCoolDown;

    private void Awake()
    {
        
        impulse = GetComponent<PlayerImpulse>();


    }

    private void Update()
    {

        if (!IsOwner) return;

        var arr = Physics.OverlapSphere(cam.transform.position, lenght, monsterLayer);

        foreach(var item in arr)
        {

            var dist = Vector3.Distance(cam.position, item.transform.position);

            if(!Physics.Raycast(cam.position, item.transform.position - cam.position, dist, LayerMask.GetMask("Room")))
            {

                if (IsTargetInSight(item.transform) && !surpriseCoolDown)
                {

                    impulse.PlayImpulse("MonsterSurprise");
                    SoundManager.Play2DSound("MonsterSurprise");
                    StartCoroutine(SurpriseCoolDownCo());

                }

            }

        }

    }

    private bool IsTargetInSight(Transform target)
    { 

        Vector3 targetDir = (target.position - cam.position).normalized;
        float dot = Vector3.Dot(cam.forward, targetDir);
        
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;
        
        if (theta <= angle) return true;
        else return false;

    }

    private IEnumerator SurpriseCoolDownCo()
    {

        surpriseCoolDown = true;

        yield return new WaitForSeconds(30f);

        surpriseCoolDown = false;

    }

}
