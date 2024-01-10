using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{

    [SerializeField] private float value;
    public float Value 
    { 

        get
        {

            float returnVel = value;

            foreach(var v in modify)
            {

                returnVel += v;

            }

            return returnVel;

        }

    }

    private List<float> modify = new();

    public void AddMod(float value)
    {

        modify.Add(value);

    }

    public void RemoveMod(float value)
    {

        modify.Remove(value);

    }

}