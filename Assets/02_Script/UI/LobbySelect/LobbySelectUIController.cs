using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;

public class LobbySelectUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField roomInputField;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private Transform lobbyPanelRoot;
    [SerializeField] private LobbyPanel lobbyPrefab;
    [SerializeField] private GameObject checkIcon;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TMP_Text loadingText;

    public bool roomPrivate { get; set; }
    private bool isCoolDown;
    private bool isRoomLook;
    private const float refecshCoolDown = 3f;

    private void Start()
    {

        Refresh();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public async void CreateRoom()
    {

        try
        {


            var str = roomInputField.text;
            if (str == string.Empty)
            {

                str = $"Luckey_{UnityEngine.Random.Range(0, 1000)}";

            }

            PopLoadingPanel("规 积己 吝...");

            bool isConnected = await AppController.Instance.StartHostAsync(PlayerPrefs.GetString("PlayerName"), str, roomPrivate);
            if (!isConnected)
            {
                ShutDownLoadingPanel();
            }

            HostSingle.Instance.GameManager.gameMode = GameMode.Mutli;
            NetworkManager.Singleton.SceneManager.LoadScene(SceneList.LobbyScene, LoadSceneMode.Single);


        }
        catch (Exception ex)
        {


            Debug.LogException(ex);

        }


    }

    public async void JoinRoom()
    {

        try
        {

            PopLoadingPanel("规 曼啊 吝...");

            bool isConnected = await AppController.Instance.StartClientAsync(PlayerPrefs.GetString("PlayerName"), joinCodeInputField.text);
            if (!isConnected)
            {
                ShutDownLoadingPanel();
            }

        }
        catch (Exception ex)
        {

            Debug.LogException(ex);

        }

    }

    public void CheckSeleteRoom()
    {
        isRoomLook = !isRoomLook;
        checkIcon.SetActive(isRoomLook);
    }

    private void Update()
    {

#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.L))
        {

            LoadDebugging();

        }

#endif

    }

    private async void LoadDebugging()
    {


        await AppController.Instance.StartHostAsync(PlayerPrefs.GetString("PlayerName"), System.Guid.NewGuid().ToString(), true);

        HostSingle.Instance.GameManager.gameMode = GameMode.Mutli;
        NetworkManager.Singleton.SceneManager.LoadScene(SceneList.TestScene, LoadSceneMode.Single);

    }

    public async void StartTutorial()
    {
        try
        {

            await AppController.Instance.StartHostAsync(PlayerPrefs.GetString("PlayerName"), roomInputField.text, true);

            NetworkManager.Singleton.SceneManager.LoadScene(SceneList.TutorialScene, LoadSceneMode.Single);

        }
        catch (Exception ex)
        {

            Debug.LogException(ex);

        }
    }

    public async void StartSinglePlay()
    {
        try
        {

            await AppController.Instance.StartHostAsync(PlayerPrefs.GetString("PlayerName"), roomInputField.text);

            isRoomLook = true;
            HostSingle.Instance.GameManager.gameMode = GameMode.Single;
            NetworkManager.Singleton.SceneManager.LoadScene(SceneList.LobbyScene, LoadSceneMode.Single);

        }
        catch (Exception ex)
        {

            Debug.LogException(ex);

        }
        //SceneManager.LoadScene(SceneList.SingleGameScene);
    }

    public void BackBtn()
    {
        SceneManager.LoadScene(SceneList.IntroScene);
    }

    public async void Refresh()
    {

        if (isCoolDown) return;

        StartCoroutine(CoolDownCo());

        var lobbys = await AppController.Instance.GetLobbyList();

        var childs = lobbyPanelRoot.GetComponentsInChildren<LobbyPanel>();

        foreach (var child in childs)
        {

            Destroy(child.gameObject);

        }

        foreach (var lobby in lobbys)
        {

            Instantiate(lobbyPrefab, lobbyPanelRoot).SetPanel(lobby, this);

        }

    }

    public void PopLoadingPanel(string extra)
    {
        loadingPanel.SetActive(true);
        loadingText.text = extra;
    }

    public void ShutDownLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }

    private IEnumerator CoolDownCo()
    {

        isCoolDown = true;
        yield return new WaitForSeconds(refecshCoolDown);
        isCoolDown = false;
    }

}
