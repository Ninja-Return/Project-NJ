using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


public class ColorManager : NetworkBehaviour
{
    [SerializeField]
    public NetworkVariable<Color> playerColor = new NetworkVariable<Color>();

    public override void OnNetworkSpawn()
    {
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
