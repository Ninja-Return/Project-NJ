using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryGun : HandItemRoot
{
    [SerializeField] private float distance;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private ParticleSystem muzzle;

    private bool isUsed;

    public override void DoUse()
    {
        if (!isOwner || isUsed) return;

        isUsed = true;

        NetworkSoundManager.Play3DSound("GunShot", transform.position, 0.01f, 30f, SoundType.SFX, AudioRolloffMode.Linear);
        GetComponentInParent<PlayerImpulse>().PlayImpulse("GunShoot");
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector2 ScreenCenter = new Vector2(cam.pixelWidth / 2, cam.pixelHeight / 2);
        Ray ray = cam.ScreenPointToRay(ScreenCenter);
        muzzle.Play();

        EnemyCheck(ray);
        PlayerCheck(ray);

        StartCoroutine(DestroyCo());

    }

    void EnemyCheck(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, enemyMask))
        {
            if (CheakObstacle(ray, hit.transform.position)) return;

            if (hit.transform.TryGetComponent(out IEnemyInterface enemy))
                enemy.Death();
        }
    }

    void PlayerCheck(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, playerMask))
        {
            if (CheakObstacle(ray, hit.transform.position)) return;

            ulong playerId = hit.transform.GetComponent<PlayerController>().OwnerClientId;
            PlayerManager.Instance.PlayerDie(EnumList.DeadType.Gun, playerId);
        }
    }

    //厘局拱 面倒贸府 
    bool CheakObstacle(Ray ray, Vector3 hitPos)
    {
        float checkDistance = Mathf.Abs(Vector3.Distance(ray.origin, hitPos));

        return Physics.Raycast(ray, checkDistance, obstacleMask);
    }

    private IEnumerator DestroyCo()
    {

        yield return new WaitForSeconds(0.07f);
        Inventory.Instance.Deleteltem();

    }

}
