using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySelectUIController : MonoBehaviour
{

    [SerializeField] private TMP_InputField roomInputField;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private Transform lobbyPanelRoot;
    [SerializeField] private LobbyPanel lobbyPrefab;

    private void Start()
    {

        StartCoroutine(RefreshLobby());

    }

    public async void CreateRoom()
    {

        try
        {

            await AppController.Instance.StartHostAsync(Guid.NewGuid().ToString(), roomInputField.text);

            NetworkManager.Singleton.SceneManager.LoadScene(SceneList.LobbyScene, LoadSceneMode.Single);

        }
        catch(Exception ex)
        {

            Debug.LogException(ex);

        }


    }

    public async void JoinRoom()
    {

        try
        {

            await AppController.Instance.StartClientAsync(Guid.NewGuid().ToString(), joinCodeInputField.text);

        }
        catch (Exception ex)
        {

            Debug.LogException(ex);

        }

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

            yield return new WaitForSeconds(2f);

        }

    }

}
