using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSic : MonoBehaviour
{

    [SerializeField] private Transform d1, d2;

    private void Start()
    {

        WaitRoomManager.Instance.IsRunningGame.OnValueChanged += HandleValueChanged;

    }

    private void HandleValueChanged(bool previousValue, bool newValue)
    {

        if (newValue)
        {

            Open();

        }
        else
        {

            Close();

        }

    }

    public void Open()
    {

        d1.DOLocalMoveZ(-2, 0.3f);
        d2.DOLocalMoveZ(-3.5f, 0.3f);

    }

    public void Close()
    {


        d1.DOLocalMoveZ(0, 0.3f);
        d2.DOLocalMoveZ(-1.3f, 0.3f);

    }

}
