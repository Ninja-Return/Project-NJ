using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] private string targetScene;

    public void LoadScene() {

        SceneManager.LoadScene(targetScene);
    
    }

    public async void LoadGameScene() {

        await AppController.Instance.StartHostAsync(PlayerPrefs.GetString("PlayerName"), System.Guid.NewGuid().ToString(), true);

        HostSingle.Instance.GameManager.gameMode = GameMode.Mutli;
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene, LoadSceneMode.Single);

    }

}
