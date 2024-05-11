using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Sensor : HandItemRoot
{
    [SerializeField] private float sensorRange;
    public LayerMask EnemyLayer;
    Renderer sensorRen;

    [SerializeField] private NetworkObject SensorPrefab;
    [SerializeField] private float firePower;
    [SerializeField] private float spawnDistance = .5f; // ������ ������ �Ÿ�
    [SerializeField] private bool OnSensor; 

    public override void DoUse()
    {

        //���
        //Vector3 forwardDirection = Camera.main.transform.forward;

        //Vector3 spawnPosition = transform.position + forwardDirection * spawnDistance;
        //NetworkObject newSensor = Instantiate(SensorPrefab, spawnPosition, Quaternion.identity);
        //newSensor.Spawn(true);


        //Rigidbody seosorRigid = newSensor.GetComponent<Rigidbody>();
        //seosorRigid.AddForce(forwardDirection * firePower, ForceMode.Impulse);
    }

    private void Start()
    {
        sensorRen = gameObject.GetComponent<Renderer>();
        sensorRen.material.color = Color.green;
        OnSensor = false;
    }

    private void Update()
    {

        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, sensorRange, EnemyLayer);

        if (detectedColliders.Length > 0 && !OnSensor)
        {
            NetworkSoundManager.Play3DSound("Sensor", transform.position, 0.1f, 30f, SoundType.SFX, AudioRolloffMode.Linear);
            Debug.Log("�Ҹ�");
            StartCoroutine(renderCH());
            OnSensor = true;
        }


    }


    IEnumerator renderCH()
    {
        sensorRen.material.color = Color.red; // ���� ������ ���� ����
        yield return new WaitForSeconds(3f);
        sensorRen.material.color = Color.green; // ���� ������ ���� ����
        OnSensor = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, sensorRange);
    }
}
