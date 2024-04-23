using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryGun : HandItemRoot
{
    [SerializeField] private float distance;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask enemyMask;

    public override void DoUse()
    {
        NetworkSoundManager.Play3DSound("GunShot", transform.position, 0.01f, 30f, SoundType.SFX, AudioRolloffMode.Linear);

        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector2 ScreenCenter = new Vector2(cam.pixelWidth / 2, cam.pixelHeight / 2);
        Ray ray = cam.ScreenPointToRay(ScreenCenter);

        EnemyCheck(ray);
        PlayerCheck(ray);
    }

    void EnemyCheck(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, enemyMask))
        {
            if (CheakObstacle(ray, hit.transform.position)) return;

            hit.transform.GetComponent<IEnemyInterface>().Death();
        }
    }

    void PlayerCheck(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, playerMask))
        {
            if (CheakObstacle(ray, hit.transform.position)) return;

            ulong playerId = hit.transform.GetComponent<PlayerController>().OwnerClientId;
            PlayerManager.Instance.PlayerDie(EnumList.DeadType.Monster, playerId);
        }
    }

    //厘局拱 面倒贸府 
    bool CheakObstacle(Ray ray, Vector3 hitPos)
    {
        float checkDistance = Mathf.Abs(Vector3.Distance(ray.origin, hitPos));

        return Physics.Raycast(ray, checkDistance, obstacleMask);
    }
}
