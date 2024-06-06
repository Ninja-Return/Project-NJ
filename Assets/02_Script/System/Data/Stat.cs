using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float DefaultValue
    {
        get
        {
            return value;
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

    public Stat Copy()
    {

        Stat stat = new Stat();
        stat.value = value;
        stat.modify = modify.ToList();

        return stat;

    }

}
