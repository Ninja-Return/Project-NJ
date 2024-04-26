using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void GameStart()
    {
        AlertText.Instance.GameStart();
        MapBgm.Instance.GameBgm(true);
        MonsterSpawnSystem.Instance.HandleSpawn();
        ClearTimeManager.Instance.TimerStarted = true;
    }
}
