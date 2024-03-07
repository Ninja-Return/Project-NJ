using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttachedItemUIController : MonoBehaviour
{

    [SerializeField] private TMP_Text text_1, text_2;

    public void Init(AttachedItemRPCData data)
    {

        text_1.text = data.item_1.ToString();
        text_2.text = data.item_2.ToString();

    }

}
