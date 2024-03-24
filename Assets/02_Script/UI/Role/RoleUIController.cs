using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleUIController : MonoBehaviour
{

    [SerializeField] private Image rolePanel;
    [SerializeField] private TMP_Text roleText;
    [SerializeField] private Material mafiaMat, playerMat;

    public void SetRole(PlayerRole role)
    {

        SoundManager.Play2DSound("GameStart");

        roleText.text = GetText(role);
        rolePanel.material = role == PlayerRole.Mafia ? mafiaMat : playerMat;

        StartCoroutine(RoleSetCo());

    }

    private string GetText(PlayerRole role)
    {


        switch (role)
        {
            case PlayerRole.Survivor:
                return "당신의 역할은 생존자 입니다";
            case PlayerRole.Mafia:
                return "당신의 역할은 마피아 입니다";
            case PlayerRole.New:
                return "당신의 역할은 ??? 입니다"; 
        }

        return "치명적 에러";

    }

    private IEnumerator RoleSetCo()
    {

        rolePanel.transform.TVEffect(true);

        yield return new WaitForSeconds(3);

        rolePanel.transform.TVEffect(false);

    }

}
