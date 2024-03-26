using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float value;
    public float Value
    {
        get
        {
            float returnValue = value;

            foreach (var mod in modify)
            {
                returnValue += mod;
            }

            return returnValue;
        }
    }

    private List<float> modify = new List<float>();

    public void SetValue(float newValue)
    {
        value = newValue;
    }

    public void AddMod(float modifier)
    {
        modify.Add(modifier);
    }

    public void RemoveMod(float modifier)
    {
        modify.Remove(modifier);
    }
}
