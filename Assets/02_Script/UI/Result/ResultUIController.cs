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
    [SerializeField] private TextMeshProUGUI clearAreaText;
    [SerializeField] private TextMeshProUGUI mafiaKillText;
    [SerializeField] private TextMeshProUGUI monsterKillText;
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

    public void MafiaWin()
    {
        resuitPanel.color = Color.red;
        winnerText.color = Color.red;

        if (HostSingle.Instance.GameManager.gameMode == GameMode.Single)
            winText.text = "Ż�� ����";
        else
            winText.text = "���Ǿ� �¸�";

        ModeSet();

    }

    public void HumanWin()
    {
        Color skyColor = new Color(0, 0.6f, 1, 1);
        resuitPanel.color = skyColor;
        winnerText.color = skyColor;

        if (HostSingle.Instance.GameManager.gameMode == GameMode.Tutorial)
            winText.text = "Ʃ�丮�� �ϼ�";
        else if (HostSingle.Instance.GameManager.gameMode == GameMode.Single)
            winText.text = "Ż�� ����";
        else
            winText.text = "������ �¸�";

        ModeSet();

    }

    private void ModeSet()
    {
        if (HostSingle.Instance.GameManager.gameMode == GameMode.Single)
        {
            mafiaKillText.gameObject.SetActive(false);
            monsterKillText.gameObject.SetActive(false);
        }
    }

    public void SpawnPanel(ulong clientId, string userName, bool isOwner)
    {
        var panel = Instantiate(playerPrefab, players);

        panel.Setting(clientId, userName, isOwner, false);

        if (userName == PlayerPrefs.GetString("MafiaNickName"))
            panel.ColorChange(Color.red);
        else
            panel.ColorChange(Color.blue);

    }

    public void BackMain()
    {
        SceneManager.LoadScene(SceneList.LobbySelectScene);
    }

}
