using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void GameStart()
    {
        StartGameText.Instance.GameStart();
        ClearTimeManager.Instance.TimerStarted = true;
    }
}
