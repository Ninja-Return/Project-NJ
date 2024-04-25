using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Dumbell : HandItemRoot
{
    [SerializeField] private float distance;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;

    public override void DoUse()
    {

        if(!isOwner) return;
        NetworkSoundManager.Play3DSound("DumbellShoot", transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);

        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector2 ScreenCenter = new Vector2(cam.pixelWidth / 2, cam.pixelHeight / 2);
        Ray ray = cam.ScreenPointToRay(ScreenCenter);
        PlayerCheck(ray);
        
    }

    void PlayerCheck(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, playerMask))
        {
            if (CheakObstacle(ray, hit.transform.position)) return;

            PlayerController playerId = hit.transform.GetComponent<PlayerController>();
            playerId.AddSpeed(-3f, 10f);
        }
    }

    //厘局拱 面倒贸府 
    bool CheakObstacle(Ray ray, Vector3 hitPos)
    {
        float checkDistance = Mathf.Abs(Vector3.Distance(ray.origin, hitPos));

        return Physics.Raycast(ray, checkDistance, obstacleMask);
    }
}
