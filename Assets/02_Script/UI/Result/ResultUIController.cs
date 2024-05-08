using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Michsky.UI.Dark;

public class ResultUIController : NetworkBehaviour
{
    public UIDissolveEffect dissolveEffect;
    [SerializeField] private Image upImg;
    [SerializeField] private Image downImg;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Transform players;
    [SerializeField] private MeetingProfile playerPrefab;

    [Header("FeedbackText")]
    [SerializeField] private TMP_Text escapePlayerText;
    [SerializeField] private TMP_Text failPlayerText;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private TMP_Text[] text;
    public NetworkVariable<float> playerTime = new NetworkVariable<float>();

    private float escapePlayerCnt = 0f;
    private float failPlayerCnt = 0f;
    private bool isDead = false;

    private void Start()
    {

        Cursor.visible = true;

        dissolveEffect.DissolveOut();

        if (IsServer) EscapeTimer();

    }

    public void EscapeFail()
    {
        upImg.color = Color.red;
        downImg.color = Color.red;
        winnerText.color = Color.red;
        isDead = true;


        EscapeTimer();

        winText.text = "탈출 실패";
    }

    public void EscapeClear()
    {
        Color skyColor = new Color(0, 0.6f, 1, 1);
        upImg.color = skyColor;
        downImg.color = skyColor;
        winnerText.color = skyColor;

        EscapeTimer();

        winText.text = "플레이 결과";

    }

    public void EscapeTimer()
    {
        foreach (var item in NetworkManager.ConnectedClientsIds)
        {
            TimerClientRpc(item, HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(item).Value.clearTime);
        }
    }

    [ClientRpc]
    private void TimerClientRpc(ulong clientId, float clearTime)
    {
        if (clientId != NetworkManager.LocalClientId) return;

        text[0].text = ((int)clearTime / 60 % 60).ToString() + " 분";
        text[1].text = ((int)clearTime % 60).ToString() + " 초";

        if (!isDead)
        {
            if (clearTime < 180 && !isDead)
                text[2].text = "S";
            else if (clearTime >= 180 && clearTime < 300 && !isDead)
                text[2].text = "A";
            else if (clearTime >= 300 && clearTime < 420 && !isDead)
                text[2].text = "B";
            else if (clearTime >= 420 && clearTime < 540 && !isDead)
                text[2].text = "C";
            else
                text[2].text = "D";
        }
        else
            text[2].text = "E";

    }

    public void SpawnPanel(ulong clientId, string userName, bool isOwner, bool isBreak)
    {
        var panel = Instantiate(playerPrefab, players);
        panel.Setting(clientId, userName, isOwner, false);

        if (!isOwner) return;

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

        escapePlayerText.text = $"탈출한 플레이어 : {escapePlayerCnt}명";
        failPlayerText.text = $"죽은 플레이어 : {failPlayerCnt}명";
        PlayerCountClientRpc(escapePlayerCnt, failPlayerCnt);
    }

    [ClientRpc]
    private void PlayerCountClientRpc(float escapeCnt, float failCnt)
    {
        escapePlayerText.text = $"탈출한 플레이어 : {escapeCnt}명";
        failPlayerText.text = $"죽은 플레이어 : {failCnt}명";
    }

    public async void BackMain()
    {
        if (IsHost)
        {

            await HostSingle.Instance.GameManager.ShutdownAsync();

            SceneManager.LoadScene(SceneList.LobbySelectScene);
        }
        else
        {
            ClientSingle.Instance.GameManager.Disconnect();
        }
    }
}
