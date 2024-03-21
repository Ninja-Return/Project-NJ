using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Vivox;
using Unity.Netcode;

public class AppController : MonoBehaviour
{

    [SerializeField] private ClientSingle clientPrefab;
    [SerializeField] private HostSingle hostPrefab;

    public static AppController Instance;

    private void Awake()
    {
        
        Instance = this;

    }

    private async void Start()
    {

        DontDestroyOnLoad(gameObject);

        await UnityServices.InitializeAsync();

        var state = await AuthenticationWrapper.DoAuth(3);

        if(state != AuthState.Authenticated)
        {

            Debug.LogError("인증 실패");
            return;

        }

        await VivoxService.Instance.InitializeAsync();

        HostSingle host = Instantiate(hostPrefab, transform);
        host.CreateHost();

        ClientSingle client = Instantiate(clientPrefab, transform);
        client.CreateClient();

        OnInitComplete();

    }

    private void OnInitComplete()
    {

#if UNITY_EDITOR


        SceneManager.LoadScene(SceneList.LobbySelectScene);

#elif UNITY_STANDALONE_WIN

        SceneManager.LoadScene(SceneList.IntroScene);

#endif

    }

    public async Task<bool> StartHostAsync(string username, string lobbyName)
    {

        return await HostSingle.Instance.GameManager.StartHostAsync(lobbyName, GetUserData(username));

    }

    public async Task StartClientAsync(string username, string joinCode)
    {

        await ClientSingle.Instance.GameManager.StartClientAsync(joinCode, GetUserData(username));

    }

    private UserData GetUserData(string username)
    {

        return new UserData
        {

            nickName = username,
            authId = AuthenticationService.Instance.PlayerId

        };

    }

    public async Task<List<Lobby>> GetLobbyList()
    {

        try
        {

            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 20;
            options.Filters = new List<QueryFilter>()
            {

                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0"),
                new QueryFilter(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0"),

            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
            return lobbies.Results;

        }
        catch (LobbyServiceException ex)
        {

            Debug.LogError(ex);
            return new List<Lobby>();

        }

    }

}
