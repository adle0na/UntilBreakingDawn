using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;

    private Slot[] slots;

    [SerializeField]
    private PlayerControllerHSW _playerSet;

    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }
    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }
    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }
    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }
    public void AcquireItem(Item _Item, int _count = 1)
    {
        if (Item.ItemType.Equipment != _Item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _Item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_Item, _count);
                return;
            }
        }
    }

    public void ItemUseCheck(int keytype)
    {
        switch (keytype)
        {
            case 5:
                ItemTypeCheck(0);
                break;
            case 6:
                ItemTypeCheck(1);
                break;
            case 7:
                ItemTypeCheck(2);
                break;
            case 8:
                ItemTypeCheck(3);
                break;
            case 9:
                ItemTypeCheck(4);
                break;
            case 0:
                ItemTypeCheck(5);
                break;
        }
    }

    private void ItemTypeCheck(int inputslot)
    {
        Slot slot = slots[inputslot];

        switch (slot.GetItemType())
        {
            case Item.ItemType.Potion:
                _playerSet.Status.IncreaseHP(slot.item.editableValue);
                slot.UseSlot();
                break;

            case Item.ItemType.MagazineMain:
                if (_playerSet.Weapon._weaponType == WeaponType.Main)
                {
                    _playerSet.Weapon.IncreaseMagazineMain(slot.item.editableValue);
                    slot.UseSlot();
                }
                else return;
                break;

            case Item.ItemType.MagazineSub:
                if (_playerSet.Weapon._weaponType == WeaponType.Sub)
                {
                    _playerSet.Weapon.IncreaseMagazineSub(slot.item.editableValue);
                    slot.UseSlot();
                }
                else return;
                break;

            default:
                Debug.Log("사용 아이템이 아닙니다");
                break;
        }
    }
    //시험삼아 만든 소모 아이템 스크립트
    public int GetItemCount(string _itemName)
    {
        int temp = SearchSlotItem(slots, _itemName);
        return temp != 0 ? temp : SearchSlotItem(slots, _itemName);
    }

    private int SearchSlotItem(Slot[] _slots, string _itemName)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item != null)
            {
                if (_itemName == _slots[i].item.itemName)
                    return _slots[i].itemCount;
            }
        }

        return 0;
    }

    public void SetItemCount(string _itemName, int _itemCount)
    {
        if (!ItemCountAdjust(slots, _itemName, _itemCount))
            ItemCountAdjust(slots, _itemName, _itemCount);
    }

    private bool ItemCountAdjust(Slot[] _slots, string _itemName, int _itemCount)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item != null)
            {
                if (_itemName == _slots[i].item.itemName)
                {
                    _slots[i].SetSlotCount(-_itemCount);
                    return true;
                }
            }
        }
        return false;
    }
}