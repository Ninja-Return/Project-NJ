using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OccaSoftware.Buto.Runtime.ButoLight;

public class PlayerCameraEvent : MonoBehaviour
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

        var arr = Physics.OverlapSphere(cam.transform.position, lenght, monsterLayer);

        foreach(var item in arr)
        {

            var dist = Vector3.Distance(cam.position, item.transform.position);

            if(!Physics.Raycast(cam.position, item.transform.position - cam.position, dist, ~monsterLayer))
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
