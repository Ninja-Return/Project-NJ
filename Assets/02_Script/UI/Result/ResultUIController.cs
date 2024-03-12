using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ResultUIController : MonoBehaviour
{
    [SerializeField] private Image resuitPanel;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Transform players;
    [SerializeField] private GameObject playerPrefab;

    [Header("FeedbackText")]
    [SerializeField] private TextMeshProUGUI clearAreaText;
    [SerializeField] private TextMeshProUGUI mafiaKillText;
    [SerializeField] private TextMeshProUGUI monsterKillText;
    [SerializeField] private TMP_Text winText;

    public void MafiaWin()
    {
        resuitPanel.color = Color.red;
        winnerText.color = Color.red;

        winText.text = "���Ǿ� �¸�";

    }

    public void HumanWin()
    {
        Color skyColor = new Color(0, 0.6f, 1, 1);
        resuitPanel.color = skyColor;
        winnerText.color = skyColor;

        winText.text = "������ �¸�";

    }

    private void FeedbackSetting()
    {
        //players�� ��� �÷��̾� �ֱ�(playerPrefab)
        //����� ���Ǿƴ� ������, �����ڴ� ���, ������+��������� ȸ��

        //clearAreaText.text = ;
        //mafiaKillText.text = ;
        //monsterKillText.text = ;
    }
}
