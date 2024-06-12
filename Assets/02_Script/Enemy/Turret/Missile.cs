using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Missile : NetworkBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    //[SerializeField] private LayerMask playerMask, enemyMask, obstacleMask;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem fireParticle;
    [SerializeField] private ParticleSystem bombParticle;
    [SerializeField] private NetworkAudioSource audioSource;
    //[SerializeField] private ParticleSystem frameParticle;

    private Rigidbody rb;
    private bool isBomb;

    private void Awake()
    {
        NetworkSoundManager.Play3DSound("MissileFly", transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!IsServer) return;
        if (isBomb) return;
        
        Collider[] cols = Physics.OverlapSphere(transform.position, range);
        if (cols.Length > 0)
        {
            isBomb = true;

            Transform target = cols[0].transform;

            if (target.transform.TryGetComponent(out IEnemyInterface enemy))
            {
                enemy.Death();
            }

            if (target.transform.TryGetComponent(out PlayerController player))
            {
                ulong playerId = player.OwnerClientId;
                PlayerManager.Instance.PlayerDie(EnumList.DeadType.Turret, playerId);
            }

            BombEffect();
        }
    }

    private void BombEffect()
    {
        NetworkSoundManager.Play3DSound("MissileBomb", transform.position, 0.1f, 40f, SoundType.SFX, AudioRolloffMode.Linear);

        rb.velocity = Vector3.zero;
        meshRenderer.enabled = false;

        fireParticle.Stop();
        bombParticle.Play();
        NetworkSoundManager.Play3DSound("Explosion", transform.position, 1, 60);
        audioSource.Stop();
        Invoke("MissileDestory", bombParticle.main.duration);
    }

    private void MissileDestory()
    {

        NetworkObject.Despawn();
    }

    public void FireMove(Vector3 dir)
    {
        rb.velocity = dir * speed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, range);
    }
}
