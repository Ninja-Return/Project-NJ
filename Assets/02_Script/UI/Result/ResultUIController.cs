using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class ResultUIController : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] private Image resuitPanel;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Transform players;
    [SerializeField] private MeetingProfile playerPrefab;

    [Header("FeedbackText")]
    [SerializeField] private TMP_Text escapePlayerText;
    [SerializeField] private TMP_Text failPlayerText;
    [SerializeField] private TMP_Text winText;

    private NetworkVariable<float> escapePlayerCnt = new NetworkVariable<float>();
    private NetworkVariable<float> failPlayerCnt = new NetworkVariable<float>();

    private void Start()
    {
        Cursor.visible = true;
        DOFadeResult();

        escapePlayerCnt.OnValueChanged += SetEscapeText;
        failPlayerCnt.OnValueChanged += SetFailText;
    }

    private void DOFadeResult()
    {
        canvasGroup.DOFade(1, 2);
    }

    public void EscapeFail()
    {
        resuitPanel.color = Color.red;
        winnerText.color = Color.red;

        winText.text = "탈출 실패";

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
            winText.text = "튜토리얼 완료";
        else
            winText.text = "플레이 결과";

    }

    public void SpawnPanel(ulong clientId, string userName, bool isOwner, bool isBreak)
    {
        var panel = Instantiate(playerPrefab, players);
        panel.Setting(clientId, userName, isOwner, false);

        if (isBreak)
        {
            escapePlayerCnt.Value++;
            panel.ColorChange(Color.blue);
        }
        else
        {
            failPlayerCnt.Value++;
            panel.ColorChange(Color.red);
        }
    }

    private void SetEscapeText(float oldCnt, float newCnt)
    {
        escapePlayerText.text = $"탈출한 플레이어 : {newCnt}명";
    }

    private void SetFailText(float oldCnt, float newCnt)
    {
        failPlayerText.text = $"죽은 플레이어 : {newCnt}명";
    }

    public void BackMain()
    {
        escapePlayerCnt.OnValueChanged -= SetEscapeText;
        failPlayerCnt.OnValueChanged -= SetFailText;

        SceneManager.LoadScene(SceneList.LobbySelectScene);
    }

}
