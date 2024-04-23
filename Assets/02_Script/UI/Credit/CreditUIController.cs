using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditUIController : MonoBehaviour
{

    [SerializeField] private CreditSystem creditSystem;
    [SerializeField] private TMP_Text creditText;

    private void Awake()
    {

        creditSystem.OnCreditChanged += HandleCreditChanged;

    }

    private void HandleCreditChanged(int oldValue, int newValue, int addValue)
    {

        creditText.text = "º“¿Ø±›: " + newValue.ToString();

    }

}
