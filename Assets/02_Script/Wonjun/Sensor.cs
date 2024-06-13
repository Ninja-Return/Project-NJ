using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Sensor : HandItemRoot
{
    [SerializeField] private float sensorRange;
    public LayerMask EnemyLayer;
    Light sensorlight;

    [SerializeField] private NetworkObject SensorPrefab;
    [SerializeField] private float firePower;
    [SerializeField] private float spawnDistance = .5f; // 앞으로 스폰할 거리

    private bool enemyDetectedPreviously;

    public override void DoUse()
    {
        //폐기
        //Vector3 forwardDirection = Camera.main.transform.forward;

        //Vector3 spawnPosition = transform.position + forwardDirection * spawnDistance;
        //NetworkObject newSensor = Instantiate(SensorPrefab, spawnPosition, Quaternion.identity);
        //newSensor.Spawn(true);

        //Rigidbody seosorRigid = newSensor.GetComponent<Rigidbody>();
        //seosorRigid.AddForce(forwardDirection * firePower, ForceMode.Impulse);
    }

    private void Start()
    {
        sensorlight = gameObject.GetComponent<Light>();
        sensorlight.color = Color.green;
        enemyDetectedPreviously = false;
    }

    private void Update()
    {
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, sensorRange, EnemyLayer);

        if (detectedColliders.Length > 0)
        {
            if (!enemyDetectedPreviously)
            {
                NetworkSoundManager.Play3DSound("Sensor", transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);
                Debug.Log("소리");
                StartCoroutine(renderCH());
            }
            enemyDetectedPreviously = true;
        }
        else
        {
            enemyDetectedPreviously = false;
        }
                Debug.Log(enemyDetectedPreviously); 
    }

    IEnumerator renderCH()
    {
        sensorlight.color = Color.red;
        yield return new WaitForSeconds(4f);
        sensorlight.color = Color.green;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, sensorRange);
    }
}
