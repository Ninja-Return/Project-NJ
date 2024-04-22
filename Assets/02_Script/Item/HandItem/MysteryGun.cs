using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryGun : HandItemRoot
{
    [SerializeField] private float distance;
    [SerializeField] private LayerMask enemyMask;

    public override void DoUse()
    {
        //NetworkSoundManager.Play3DSound("SodaOpen", transform.position, 0.01f, 15f); ÃÑ ½î´Â ¼Ò¸®

        Camera cam = Camera.main;
        Vector2 ScreenCenter = new Vector2(cam.pixelWidth / 2, cam.pixelHeight / 2);
        Ray ray = cam.ScreenPointToRay(ScreenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance, enemyMask))
        {
            hit.transform.GetComponent<IEnemyInterface>().Death();
        }
    }
}
