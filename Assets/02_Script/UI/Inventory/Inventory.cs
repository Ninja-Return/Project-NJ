using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EnumList;
using Unity.Netcode;
using Unity.Collections;
using DG.Tweening;

public delegate void SlotChange(string objKey, int idx, string extraData);

public class Inventory : NetworkBehaviour
{
    public static Inventory Instance { get; private set; }

    public event SlotChange OnSlotClickEvt; //��ȣ�ۿ뿡�� ������ �տ����?�Լ� �־����?�����?
    public event SlotChange OnSlotDropEvt; //���?������ �����鼭 ������ �Լ� �־���
    public event SlotChange OnSlotRemove;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI slotExpText;

    private PlayerController playerController;
    private SlotUI[] slots;
    private int slotIdx;

    public bool isShow = false;
    public bool showingDelay = false;
    [HideInInspector] public bool isHold = false;
    public int getItemCount;

    [SerializeField] private List<ItemDataSO> firstItem = new();

    private void Start()
    {

        if (IsOwner)
        {

            Instance = this; //���߿� ���̱��� �ְ���?

            playerController = GetComponent<PlayerController>();
            slots = GetComponentsInChildren<SlotUI>();

            for (int i = 0; i < slots.Length; i++) //�տ� �� ������ ���� �����?�ϴϱ�
            {
                slots[i].slotIndex = i;
            }

            inventoryPanel.transform.localScale = Vector3.zero;

            foreach (var item in firstItem)
            {

                ObtainItem(item, "");

            }

        }


    }

    public void SetActiveInventoryUI(bool notPlayerActiveChange = false) //�÷��̾� ��ǲ�� ����
    {


        isShow = !isShow;

        showingDelay = true;
        StartCoroutine(ShowDelay());

        if (!notPlayerActiveChange)
        {

            Support.SettingCursorVisable(isShow);

            if (PlayerManager.Instance == null)
                playerController.Active(!isShow);
            else
                PlayerManager.Instance.localController.Active(!isShow);

            if (isShow)
            {

                SoundManager.Play2DSound("InventoryOpen");

            }

        }

        if (isShow)
            inventoryPanel.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
        else if (!isShow)
            inventoryPanel.transform.DOScale(new Vector3(0, 0, 0), 0.2f);
    }

    public void PopItemText(string ex)
    {
        slotExpText.text = $"\"{ex}\"";
    }

    public bool ObtainItem(ItemDataSO data, string extraData) //������ ������ �ҷ��� ��ǥ��
    {


        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].slotData == null)
            {
                getItemCount++;
                slots[i].InsertSlot(data, extraData);

                NetworkSoundManager.Play3DSound("GetItem", transform.position, 0.1f, 5);
                //slots[i].TouchSlot(); //���ڸ��� ���������� �̰ɷ�
                return true;
            }
        }

        //���⼭�� ���� �ȸԾ����ٴ°� �˷��ټ��� �ְ�
        return false;
    }

    public void HoldItem(string itemObj, int idx, string extraData)
    {
        isHold = true;
        slotIdx = idx;

        OnSlotClickEvt?.Invoke(itemObj, idx, extraData); //������ ���?���� pop�ϰ� ���ο� ������
    }

    public void DropItem(string itemObj, int idx, string extraData)
    {

        NetworkSoundManager.Play3DSound("DropItem", transform.position, 0.1f, 5);

        slotIdx = idx;
        slots[slotIdx].ResetSlot();

        getItemCount--;

        if (extraData == null) extraData = " ";

        DropItemServerRPC(itemObj, extraData);

        OnSlotDropEvt?.Invoke(itemObj, idx, extraData); //�տ��� ������ ��ô
    }

    public void Deleteltem() //��ȸ�� ������ ������ ��ǥ�� �����ͼ� ����
    {
        if (!isHold) return;
        isHold = false;

        getItemCount--;
        OnSlotRemove?.Invoke("", slotIdx, "");
        slots[slotIdx].ResetSlot();

    }

    [ServerRpc(RequireOwnership = false)]
    private void DropItemServerRPC(FixedString128Bytes itemKey, FixedString32Bytes extraData)
    {

        var item = Resources.Load<ItemRoot>($"ItemObj/{itemKey}");

        var trm = transform.root;

        var clone = Instantiate(item,
            trm.position + trm.forward,
            Quaternion.identity);

        clone.NetworkObject.Spawn();
        //clone.SetUpExtraDataServerRPC(extraData);

    }

    public string GetItemName(int idx)
    {

        if (idx == -1) return "";

        return slots[idx].data.itemName;

    }

    public void DropAllItem()
    {

        foreach (var item in slots)
        {

            if (item.slotData == null) continue;
            var trm = transform.root;
            ItemSpawnManager.Instance.SpawningItem(trm.position + transform.forward, item.slotData.poolingName);

        }

    }

    private IEnumerator ShowDelay()
    {

        yield return new WaitForSeconds(0.1f);
        showingDelay = false;

    }

}