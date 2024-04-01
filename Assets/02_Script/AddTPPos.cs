using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTPPos : MonoBehaviour
{

    [SerializeField] private string key;

    private void Start()
    {

        if (New_GameManager.Instance == null) return;

        New_GameManager.Instance.AddPos(key, transform);

    }

}
