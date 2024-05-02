using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpulse : MonoBehaviour
{

    [SerializeField] private List<GameObject> impulseSorces;

    private Dictionary<string, CinemachineImpulseSource> impulseContainer = new();

    private void Awake()
    {
        
        foreach(var item in impulseSorces) 
        {
            
            impulseContainer.Add(item.name, item.GetComponent<CinemachineImpulseSource>());

        }

    }

    public void PlayImpulse(string name)
    {

        if(impulseContainer.TryGetValue(name, out var source))
        {

            source.GenerateImpulse();

        }

    }

}
