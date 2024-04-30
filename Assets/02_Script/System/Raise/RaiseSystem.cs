using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RaiseSystem : NetworkBehaviour
{
    public static RaiseSystem Instance;

    public Dictionary<string, int> raiseItem { get; private set; } = new();

    private void Awake()
    {
        Instance = this;
    }

    public void RaiseUp(string itemName, int raise)
    {
        if (raiseItem.TryGetValue(itemName, out int value))
        {
            SetRaiseItemServerRpc(itemName, raise);
        }
        else
        {
            SetRaiseItemServerRpc(itemName, raise, true);
        }
    }

    /// <param name="raise">값은 무조건 움수만</param>
    public void RaiseDown(string itemName, int raise)
    {
        if (raiseItem.TryGetValue(itemName, out int value))
        {
            SetRaiseItemServerRpc(itemName, raise);
        }
    }

    #region ServerRpc

    [ServerRpc(RequireOwnership = false)]
    public void StorePanelRefreshServerRpc(string itemName)
    {
        StorePanelRefreshClientRpc(itemName);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetRaiseItemServerRpc(string itemName, int value, bool isEmpty = false)
    {
        SetRaiseItemClientRpc(itemName, value, isEmpty);
    }

    #endregion

    #region ClientRpc

    [ClientRpc]
    private void StorePanelRefreshClientRpc(string itemName)
    {
        foreach (var item in FindObjectsOfType<StoreUIController>())
        {
            item.StorePanelRefresh(itemName);
        }
    }

    [ClientRpc]
    private void SetRaiseItemClientRpc(string itemName, int value, bool isEmpty = false)
    {
        if (isEmpty)
        {
            raiseItem[itemName] = value;
        }
        else
        {
            raiseItem[itemName] += value;
        }
    }

    #endregion
}
