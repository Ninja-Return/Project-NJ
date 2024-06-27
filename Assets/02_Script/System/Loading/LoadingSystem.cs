using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSystem : NetworkBehaviour
{
    [SerializeField] private List<string> adviceExtrs;
    [SerializeField] private TMP_Text adviceText;

    private int completeCount;
    private int joinCurrentScene;

    private void Start()
    {

        if (IsServer)
        {

            StartCoroutine(WaitCo());

        }

        CompleteSceneJoinServerRPC();

        adviceText.text = adviceExtrs.GetRandomListObject();

    }


    [ClientRpc]
    private void JoinChannelClientRPC()
    {

        Join();

    }

    private async void Join()
    {

        try
        {

            await NetworkController.Instance.vivox.Join3DChannel();

        }
        catch(System.Exception ex)
        {

            Debug.LogError(ex.Message);

        }
        finally
        {


            CompleteJoinServerRPC();

        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void CompleteJoinServerRPC()
    {

        completeCount++;

    }

    [ServerRpc(RequireOwnership = false)]
    private void CompleteSceneJoinServerRPC()
    {

        joinCurrentScene++;

    }
    private IEnumerator WaitCo()
    {

        yield return new WaitUntil(() =>
        joinCurrentScene == NetworkManager.ConnectedClients.Count);

        JoinChannelClientRPC();

        yield return StartCoroutine(CycleCo());


    }


    private IEnumerator CycleCo()
    {

        yield return new WaitUntil(() =>
        completeCount == NetworkManager.ConnectedClients.Count);

        string sceneName;
        sceneName = SceneSelectManager.Instance.sceneName;

        NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        WaitForSceneToLoad();
    }

    private IEnumerator WaitForSceneToLoad()
    {
        string sceneName;
        sceneName = HostSingle.Instance.GameManager.gameMode == GameMode.Single ? "Office_Map_Single" : "Office_Map";
        var asyncLoadMap = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoadMap.isDone)
        {
            yield return null;
        }
    }

}