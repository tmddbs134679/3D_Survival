using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventroy : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPannel;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDes;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;

    public GameObject useBtn;
    public GameObject equipBtn;
    public GameObject unEquipBtn;
    public GameObject dropBtn;

    private PlayerController playerController;
    private PlayerCondition playerCondition;
    public Transform dropPos;

    ItemData selectedItem;
    int selectItemIdx;
    // Start is called before the first frame update
    void Start()
    {
        playerController = CharacterManager.Inst.Player.controller;
        playerCondition = CharacterManager.Inst.Player.condition;
        playerController.inventroy += Toggle;
        CharacterManager.Inst.Player.addItem += AddItem;

        dropPos = CharacterManager.Inst.Player.dropPos;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPannel.childCount];

        for(int i=0; i <slotPannel.childCount; i++)
        {
            slots[i] = slotPannel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].idx = i;
            slots[i].inventroy = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearSelectedWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDes.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useBtn.SetActive(false);
        equipBtn.SetActive(false);
        unEquipBtn.SetActive(false);
        dropBtn.SetActive(false);

    }

    public void Toggle()
    {
        if(IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Inst.Player.data;

        if(data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            UpdataUI();

            CharacterManager.Inst.Player.data = null;
            return;
        }

        ItemSlot emptySlot = GetEmptySlot();

        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdataUI();
            CharacterManager.Inst.Player.data = null;
            return;
        }

        ThrowItem(data);
        CharacterManager.Inst.Player.data = null;
    }

    void UpdataUI()
    {
        for(int i =0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }


    ItemSlot GetItemStack(ItemData data)
    {
       for(int i=0; i < slots.Length; i++)
        {
            if (slots[i].item == slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
       return null;
    }

    ItemSlot GetEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        Instantiate(data.prefab, dropPos.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int idx)
    {
        if (slots[idx].item == null) return;

        selectedItem = slots[idx].item;
        selectItemIdx = idx;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDes.text = selectedItem.des;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for(int i=0; i < selectedItem.consumables.Length; i++)
        {

            selectedItemName.text = selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text = selectedItem.consumables[i].value.ToString() + "\n";   
        }

        useBtn.SetActive(selectedItem.type == ItemType.Consumable);
        equipBtn.SetActive(selectedItem.type == ItemType.Eqipable && !slots[idx].equpped);
        unEquipBtn.SetActive(selectedItem.type == ItemType.Eqipable && slots[idx].equpped);
        dropBtn.SetActive(true);
    }

    public void OnUseBtn()
    {
        if(selectedItem.type == ItemType.Consumable)
        {
            for(int i =0; i < selectedItem.consumables.Length;i++)
            {
                switch(selectedItem.consumables[i].type)
                {
                    case ConsumableType.Health:
                        playerCondition.Heal(selectedItem.consumables[i].value);
                        break;

                    case ConsumableType.Hunger:
                        playerCondition.Eat(selectedItem.consumables[i].value);
                        break;
                }
            }
        }

        RemoveSelectedItem();

    }

    public void OnDropBtn()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        slots[selectItemIdx].quantity--;

        if (slots[selectItemIdx].quantity <= 0)
        {
            selectedItem = null;
            slots[selectItemIdx].item = null;
            selectItemIdx = -1;
            ClearSelectedWindow();
        }

        UpdataUI();
    }
 
}
