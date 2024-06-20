using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unity.Netcode;

public class SprayItem : HandItemRoot
{
    [SerializeField] private Transform sprayTrs;
    [SerializeField] private ParticleSystem sprayParticle;
    [SerializeField] private NetworkObject decal;
    [SerializeField] private float sprayRange;
    [SerializeField] private float sprayTime;

    private AudioSource audioSource;

    readonly float decalFrame = 0.15f;
    bool isUse;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void DoUse()
    {
        if (!isUse)
        {
            isUse = true;

            sprayParticle.Play();
            audioSource.Play();
            StartCoroutine(DecalStamp());
        }
    }

    private IEnumerator DecalStamp()
    {
        float currentTime = 0f;
        float frameTime = sprayTime * decalFrame;

        while (currentTime < sprayTime)
        {
            yield return new WaitForSeconds(frameTime);

            RaycastHit hit;
            Ray ray = new Ray(sprayTrs.position, sprayTrs.forward);

            bool isRange = Physics.Raycast(ray, out hit, sprayRange);

            if (isRange)
            {
                Vector3 decalDirection = -hit.normal;
                Quaternion decalRotation = Quaternion.LookRotation(decalDirection);

                ObjectManager.Instance.SpawnObject("Spray", hit.point, decalRotation);
            }

            currentTime += frameTime;
        }

        sprayParticle.Stop();
        audioSource.Stop();
        Inventory.Instance.Deleteltem();
    }
}
