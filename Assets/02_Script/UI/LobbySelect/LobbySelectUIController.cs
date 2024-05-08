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

    private bool isRoomLook;

    private void Start()
    {

        StartCoroutine(RefreshLobby());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public async void CreateRoom()
    {

        try
        {

            await AppController.Instance.StartHostAsync(PlayerPrefs.GetString("PlayerName"), roomInputField.text);

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

            await AppController.Instance.StartClientAsync(PlayerPrefs.GetString("PlayerName"), joinCodeInputField.text);

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

    public async void StartTutorial()
    {
        try
        {

            await AppController.Instance.StartHostAsync(PlayerPrefs.GetString("PlayerName"), roomInputField.text);

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

    private IEnumerator RefreshLobby()
    {

        while (true)
        {


            var lobbys = AppController.Instance.GetLobbyList();

            yield return new WaitUntil(() => lobbys.IsCompleted);

            var childs = lobbyPanelRoot.GetComponentsInChildren<LobbyPanel>();

            foreach (var child in childs)
            {

                Destroy(child.gameObject);

            }

            foreach (var lobby in lobbys.Result)
            {

                Instantiate(lobbyPrefab, lobbyPanelRoot).SetPanel(lobby);

            }

            yield return new WaitForSeconds(5f);

        }

    }

}
