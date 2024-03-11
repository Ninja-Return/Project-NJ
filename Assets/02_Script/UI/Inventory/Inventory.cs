using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EnumList;
using Unity.Netcode;
using Unity.Collections;

public delegate void SlotChange(string objKey, int idx);

public class Inventory : NetworkBehaviour
{
    public static Inventory Instance { get; private set; }

    public event SlotChange OnSlotClickEvt; //��ȣ�ۿ뿡�� ������ �տ���� �Լ� �־���� �����
    public event SlotChange OnSlotDropEvt; //��� ������ �����鼭 ������ �Լ� �־���

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI slotExpText;
    private SlotUI[] slots;
    private int slotIdx;

    [HideInInspector] public bool isShow = false;
    [HideInInspector] public bool isHold = false;

    [SerializeField] private List<ItemDataSO> firstItem = new();

    private void Start()
    {

        if (IsOwner)
        {

            Instance = this; //���߿� ���̱��� �ְ���?

            slots = GetComponentsInChildren<SlotUI>();
            for (int i = 0; i < slots.Length; i++) //�տ� �� ������ ���� ����� �ϴϱ�
            {
                slots[i].slotIndex = i;
            }

            inventoryPanel.SetActive(false);

            foreach(var item in firstItem)
            {

                ObtainItem(item);

            }

        }


    }

    public void SetActiveInventoryUI() //�÷��̾� ��ǲ�� ����
    {
        isShow = !isShow;

        Cursor.visible = isShow;
        Cursor.lockState = isShow ? CursorLockMode.None : CursorLockMode.Locked;

        if (isShow)
        {

            SoundManager.Play2DSound("InventoryOpen");

        }

        inventoryPanel.SetActive(isShow);
    }

    public void PopItemText(string ex)
    {
        slotExpText.text = $"\"{ex}\"";
    }

    public void ObtainItem(ItemDataSO data) //������ ������ �ҷ��� ��ǥ��
    {

        NetworkSoundManager.Play3DSound("GetItem", transform.position, 0.1f, 5);

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].slotData == null)
            {
                slots[i].InsertSlot(data);
                //slots[i].TouchSlot(); //���ڸ��� ���������� �̰ɷ�
                break;
            }
        }

        //���⼭�� ���� �ȸԾ����ٴ°� �˷��ټ��� �ְ�
        return;
    }

    public void HoldItem(string itemObj, int idx)
    {
        isHold = true;
        slotIdx = idx;

        OnSlotClickEvt?.Invoke(itemObj, idx); //������ ��� ���� pop�ϰ� ���ο� ������
    }

    public void DropItem(string itemObj, int idx)
    {

        NetworkSoundManager.Play3DSound("DropItem", transform.position, 0.1f, 5);

        slotIdx = idx;
        slots[slotIdx].ResetSlot();

        DropItemServerRPC(itemObj);

        OnSlotDropEvt?.Invoke(itemObj, idx); //�տ��� ������ ��ô
    }

    public void Deleteltem() //��ȸ�� ������ ������ ��ǥ�� ����ͼ� ����
    {
        if (!isHold) return;
        isHold = false;

        slots[slotIdx].ResetSlot();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DropItemServerRPC(FixedString128Bytes itemKey)
    {

        var item = Resources.Load<ItemRoot>($"ItemObj/{itemKey}");

        var trm = transform.root;

        var clone = Instantiate(item,
            trm.position + trm.forward, 
            Quaternion.identity);

        clone.NetworkObject.Spawn();

    }

    public string GetItemName(int idx)
    {

        if (idx == -1) return "";

        return slots[idx].data.itemName;

    }

}