using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LightMonsterSpawnSystem : NetworkBehaviour
{

    [SerializeField] private LightMonster monsterPrefab;

    private void Start()
    {

        if (!IsServer) return;

        New_GameManager.Instance.OnGameStarted += HandleGameStart;

    }

    private void HandleGameStart()
    {

        StartCoroutine(CheckSpawnCo());

    }

    private void CheckPlayerEvent()
    {

        foreach(var item in PlayerManager.Instance.players)
        {

            if(item.psychosisValue.Value < Random.value)
            {

                if(1f < Random.value)
                {

                    Debug.Log("스폰되다 괴물");

                    //괴물 스폰
                    //var monster =
                    //    Instantiate(monsterPrefab, Random.insideUnitSphere * 3, Quaternion.identity);
                    //
                    //monster.NetworkObject.Spawn(true);
                    //monster.Spawn(item.transform);

                }
                else
                {

                    //이상한 사운드 나오게

                    Debug.Log("나오다 사운드");

                }

            }

        }

    }

    private IEnumerator CheckSpawnCo()
    {

        var sec = new WaitForSeconds(15f);

        while (true)
        {

            yield return sec;
            
            CheckPlayerEvent();

        }

    }

}
