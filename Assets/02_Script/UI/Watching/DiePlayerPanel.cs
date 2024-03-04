using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiePlayerPanel : MonoBehaviour
{

    [SerializeField] private TMP_Text userNameText;
    

    public void Spawn(string name)
    {

        userNameText.text = name;

    }

}
