using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintObject_Hand : HandItemRoot
{

    [SerializeField] private TMP_Text panelText;

    public override void DoUse()
    {
    }

    public override void SetUpExtraData(string extraData)
    {

        base.SetUpExtraData(extraData);
        panelText.text = extraData;

    }

}
