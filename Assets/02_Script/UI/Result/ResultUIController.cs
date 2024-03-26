using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultUIController : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] private Image resuitPanel;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Transform players;
    [SerializeField] private MeetingProfile playerPrefab;

    [Header("FeedbackText")]
    //[SerializeField] private TextMeshProUGUI clearAreaText;
    //[SerializeField] private TextMeshProUGUI mafiaKillText;
    //[SerializeField] private TextMeshProUGUI monsterKillText;
    [SerializeField] private TMP_Text winText;

    private void Start()
    {
        Cursor.visible = true;
        DOFadeResult();
    }

    private void DOFadeResult()
    {
        canvasGroup.DOFade(1, 2);
    }

    public void EscapeFail()
    {
        resuitPanel.color = Color.red;
        winnerText.color = Color.red;

        winText.text = "Ż�� ����";

        //if (HostSingle.Instance.GameManager.gameMode == GameMode.Single)
        //    winText.text = "Ż�� ����";
        //else
        //    winText.text = "���Ǿ� �¸�";

    }

    public void EscapeClear()
    {
        Color skyColor = new Color(0, 0.6f, 1, 1);
        resuitPanel.color = skyColor;
        winnerText.color = skyColor;

        if (HostSingle.Instance.GameManager.gameMode == GameMode.Tutorial)
            winText.text = "Ʃ�丮�� �ϼ�";
        else
            winText.text = "�÷��� ���";

    }

    public void SpawnPanel(ulong clientId, string userName, bool isOwner, bool isBreak)
    {
        var panel = Instantiate(playerPrefab, players);
        panel.Setting(clientId, userName, isOwner, false);

        if (isBreak)
            panel.ColorChange(Color.blue);
        else
            panel.ColorChange(Color.red);

    }

    public void BackMain()
    {
        SceneManager.LoadScene(SceneList.LobbySelectScene);
    }

}
