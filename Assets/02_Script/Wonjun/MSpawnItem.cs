using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MSpawnItem : HandItemRoot
{
    [SerializeField] private NetworkObject monsterPrefab;
    [SerializeField] private GameObject spawnIndicatorPrefab;
    [SerializeField] private float spawnDistance = 2f;
    private GameObject currentSpawnIndicator;
    Vector3 spawnMonsterPosition;
    Transform playerTransform;

    private void Start()
    {
        currentSpawnIndicator = Instantiate(spawnIndicatorPrefab);
        currentSpawnIndicator.SetActive(true);
    }

    public override void DoUse()
    {
        playerTransform = PlayerManager.Instance.localController.transform;
        Vector3 spawnPosition = playerTransform.position + playerTransform.forward * spawnDistance;

        RaycastHit hit;
        if (Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, spawnDistance))
        {
            spawnPosition = ((playerTransform.position - hit.point).normalized * .5f) + hit.point; 
        }

        currentSpawnIndicator.SetActive(false);

        MonsterSpawnSystem.Instance.SpawnMonsterServerRPC(spawnPosition, 2f);
    }

    private void Update()
    {
        playerTransform = PlayerManager.Instance.localController.transform;

        spawnMonsterPosition = playerTransform.position + playerTransform.forward * spawnDistance;

        RaycastHit hit;
        if (Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, spawnDistance))
        {
            spawnMonsterPosition = ((playerTransform.position - hit.point).normalized * .5f) + hit.point;

        }


        currentSpawnIndicator.transform.position = spawnMonsterPosition - new Vector3(0f, 1f,0f);
    }

    public void OnDisable()
    {
        currentSpawnIndicator.SetActive(false);
    }
}
