using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnCreditChange(int oldValue, int newValue, int addValue);

public class CreditSystem : MonoBehaviour
{

    private int credit;
    public int Credit 
    {

        get
        {

            return credit;

        }

        set
        {

            OnCreditChanged?.Invoke(credit, value, value - credit);
            credit = value;

        }
    
    }

    public event OnCreditChange OnCreditChanged;



}
