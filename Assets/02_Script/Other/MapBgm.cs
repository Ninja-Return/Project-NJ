using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MapBgm : NetworkBehaviour
{
    public static MapBgm Instance;

    [SerializeField] private AudioSource gameBgm;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        New_GameManager.Instance.OnHardEvent += () => { GameBgm(false); };
    }

    public void GameBgm(bool value)
    {
        GameBgmClientRpc(value);
    }

    [ClientRpc]
    private void GameBgmClientRpc(bool value)
    {
        if (value)
        {
            gameBgm.Play();
        }
        else
        {
            gameBgm.Stop();
        }
    }
}
