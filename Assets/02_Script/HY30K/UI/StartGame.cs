using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void GameStart()
    {
        StartGameText.Instance.GameStart();
        MonsterSpawnSystem.Instance.HandleSpawn();
        ClearTimeManager.Instance.TimerStarted = true;
    }
}
