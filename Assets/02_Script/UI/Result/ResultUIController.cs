using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultUIController : MonoBehaviour
{
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
    }

    public void EscapeFail()
    {
        resuitPanel.color = Color.red;
        winnerText.color = Color.red;

        winText.text = "탈출 실패";

        //if (HostSingle.Instance.GameManager.gameMode == GameMode.Single)
        //    winText.text = "탈출 실패";
        //else
        //    winText.text = "마피아 승리";

    }

    public void EscapeClear()
    {
        Color skyColor = new Color(0, 0.6f, 1, 1);
        resuitPanel.color = skyColor;
        winnerText.color = skyColor;

        if (HostSingle.Instance.GameManager.gameMode == GameMode.Tutorial)
            winText.text = "튜토리얼 완수";
        else
            winText.text = "플레이 결과";

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
