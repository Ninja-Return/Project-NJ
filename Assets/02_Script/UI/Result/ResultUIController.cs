using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class ResultUIController : NetworkBehaviour
{
    [SerializeField] private Image fadePanel;
    [SerializeField] private Image resuitPanel;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Transform players;
    [SerializeField] private MeetingProfile playerPrefab;

    [Header("FeedbackText")]
    [SerializeField] private TMP_Text escapePlayerText;
    [SerializeField] private TMP_Text failPlayerText;
    [SerializeField] private TMP_Text winText;

    private float escapePlayerCnt = 0f;
    private float failPlayerCnt = 0f;

    private void Start()
    {

        Cursor.visible = true;

        Sequence resultSequence = DOTween.Sequence();
        resultSequence.Append(fadePanel.DOFade(0, 1.5f))
            .OnComplete(() => fadePanel.gameObject.SetActive(false));

    }

    public void EscapeFail()
    {
        resuitPanel.color = Color.red;
        winnerText.color = Color.red;

        winText.text = "탈출 실패";
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
            PlayerCountServerRpc(1f, 0f);
            panel.ColorChange(Color.blue);
        }
        else
        {
            PlayerCountServerRpc(0f, 1f);
            panel.ColorChange(Color.red);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlayerCountServerRpc(float escapeCnt, float failCnt)
    {
        escapePlayerCnt += escapeCnt;
        failPlayerCnt += failCnt;

        escapePlayerText.text = $"탈출한 플레이어 : {escapeCnt}명";
        failPlayerText.text = $"죽은 플레이어 : {failCnt}명";
        FeedbackSettingClientRpc(escapePlayerCnt, failPlayerCnt);
    }

    [ClientRpc]
    private void FeedbackSettingClientRpc(float escapeCnt, float failCnt)
    {
        escapePlayerText.text = $"탈출한 플레이어 : {escapeCnt}명";
        failPlayerText.text = $"죽은 플레이어 : {failCnt}명";
    }

    public void BackMain()
    {
        if (IsHost)
        {
            HostSingle.Instance.GameManager.ShutdownAsync();
            SceneManager.LoadScene(SceneList.LobbySelectScene);
        }
        else
        {
            ClientSingle.Instance.GameManager.Disconnect();
        }
    }
}
