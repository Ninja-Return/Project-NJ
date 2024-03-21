using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


public class ColorManager : NetworkBehaviour
{
    [SerializeField] private Color ownColor;
    public NetworkVariable<Color> playerColor = new NetworkVariable<Color>();

    private void Start()
    {

        return;

        ownColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        playerColor.Value = ownColor;
    }

    public override void OnNetworkSpawn()
    {

        return;

        if (IsServer)
        {
            UserData? data = HostSingle.Instance.GameManager.NetServer.GetUserDataByClientID(OwnerClientId);

            playerColor.Value = data.Value.color;
        }

        if (IsOwner)
        {

        }
    }

}
