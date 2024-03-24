using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KickPanel : MonoBehaviour
{

    [SerializeField] private TMP_Text nameText;

    private ulong curId;

    public void SetUp(string str, ulong curId)
    {

        nameText.text = str;
        this.curId = curId;

    }

    public void Select()
    {



    }

}
