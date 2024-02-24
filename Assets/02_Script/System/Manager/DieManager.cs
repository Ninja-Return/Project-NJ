using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DieManager : MonoBehaviour
{

    private List<CinemachineVirtualCamera> cameras = new();

    public static DieManager Instance { get; private set; }

    private void Awake()
    {
        
        Instance = this;

    }

    public void Die()
    {

        cameras = 
            FindObjectsOfType<PlayerController>()
            .Select(x =>  x.transform.GetComponentInChildren<CinemachineVirtualCamera>())
            .ToList();


        cameras[0].Priority = 100;

    }

}