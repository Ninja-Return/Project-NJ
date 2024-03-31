using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTPPos : MonoBehaviour
{

    [SerializeField] private string key;

    private void Start()
    {

        New_GameManager.Instance.AddPos(key, transform);

    }

}
