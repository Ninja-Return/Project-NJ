using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Lobbies.Models;

public class TestUIController : MonoBehaviour
{

    [SerializeField] private TMP_InputField roomCodeField;

    public async void CreateRoom()
    {

        await AppController.Instance.StartHostAsync("asdfasdfasdf", "asdfasdfasdfasdf");

        //NetworkManager.Singleton.SceneManager.LoadScene(SceneList.MultiGameScene, LoadSceneMode.Single);

    }

    public async void JoinRoom()
    {

        await AppController.Instance.StartClientAsync("asdfasdf", roomCodeField.text);

    }

}