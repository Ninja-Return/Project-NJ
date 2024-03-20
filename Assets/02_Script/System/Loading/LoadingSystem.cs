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

        await NetworkController.Instance.vivox.Join3DChannel();

        CompleteJoinServerRPC();

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

        NetworkManager.SceneManager.LoadScene(SceneList.GameScene, LoadSceneMode.Single);
        WaitForSceneToLoad();
    }

    private IEnumerator WaitForSceneToLoad()
    {
        var asyncLoadMap = SceneManager.LoadSceneAsync("Office_Map", LoadSceneMode.Single);
        while (!asyncLoadMap.isDone)
        {
            yield return null;
        }
    }

}