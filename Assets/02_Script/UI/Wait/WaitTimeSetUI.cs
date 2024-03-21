using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitTimeSetUI : MonoBehaviour
{

    [SerializeField] private TMP_Text text;

    private void Start()
    {

        New_GameManager.Instance.OnWaitTimeChanged += HandleTimeChanged;

    }

    private void HandleTimeChanged(int obj)
    {

        text.text = $"시작까지 : {obj}";

        if (obj == -1) text.text = "";

    }
}
