using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSystem : NetworkBehaviour
{

    private int completeCount;

    private void Start()
    {

        if (IsServer)
        {

            StartCoroutine(CycleCo());

            JoinChannelClientRPC();

        }

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

    private IEnumerator CycleCo()
    {

        yield return new WaitUntil(() =>
        completeCount == NetworkManager.ConnectedClients.Count);

        string sceneName;
        sceneName = SceneSelectManager.Instance.sceneName;
        Debug.Log($"Ω√πﬂ æ¿¿Ã∏ß{sceneName}");

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