using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MapLaserDoor : NetworkBehaviour
{
    [Header("Ref")]
    [SerializeField] private Transform overlapTrs;
    [SerializeField] private TMP_Text personnalText;
    [SerializeField] private new Light light;
    [SerializeField] private GameObject laserObj;

    [Header("Value")]
    [SerializeField] private float overlapRange;
    [SerializeField] private int passPersonnel;
    [SerializeField] private LayerMask playerLayer;

    private bool isOpen;

    private void Update()
    {
        if (!IsServer) return;
        if (isOpen) return;

        if (CheckPlayers() || PlayerManager.Instance.alivePlayer.Count == 1)
        {
            isOpen = true;
            PassLazerClientRpc();
        }
    }

    private bool CheckPlayers()
    {
        var players = Physics.OverlapSphere(overlapTrs.position, overlapRange, playerLayer);
        int playerCnt = 0;

        foreach (var player in players)
        {
            if (player.TryGetComponent(out PlayerController controller))
            {
                playerCnt++;
            }
        }

        LazerTextClientRpc(passPersonnel - playerCnt);

        if (playerCnt >= passPersonnel)
            return true;
        return false;
    }

    [ClientRpc]
    private void LazerTextClientRpc(int cnt)
    {
        personnalText.text = cnt.ToString();
    }

    [ClientRpc]
    private void PassLazerClientRpc()
    {
        NetworkSoundManager.Play3DSound("MapLaser", overlapTrs.position, 0.1f, 15f, SoundType.SFX, AudioRolloffMode.Linear);

        light.color = Color.green;
        personnalText.text = "0";

        StartCoroutine(BlinkCo(false));
    }

    private IEnumerator BlinkCo(bool value)
    {

        for (int i = 1; i < 10; i++)
        {

            yield return new WaitForSeconds(0.5f / i);
            laserObj.SetActive(!laserObj.activeSelf);

        }

        laserObj.SetActive(value);

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(overlapTrs.position, overlapRange);
    }
#endif
}
