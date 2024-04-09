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

                    Debug.Log("�����Ǵ� ����");

                    //���� ����
                    //var monster =
                    //    Instantiate(monsterPrefab, Random.insideUnitSphere * 3, Quaternion.identity);
                    //
                    //monster.NetworkObject.Spawn(true);
                    //monster.Spawn(item.transform);

                }
                else
                {

                    //�̻��� ���� ������

                    Debug.Log("������ ����");

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
