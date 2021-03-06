using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler
{

    private Vector3 originpos;

    public Item item; //ȹ???? ??????
    public int itemCount; //ȹ???? ???????? ????
    public Image itemImage; //???????? ?̹???

    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    void Start()
    {
        originpos = transform.position;
    }
    //?̹??? ?????? ????
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
    //?????? ȹ??
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.ItemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);
    }
    //?????? ???? ????
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        Debug.Log("???? ?????? ī??Ʈ : " + itemCount);

        if (itemCount <= 0)
            ClearSlot();
    }
    
    // ???? ?????? ????
    public void UseSlot()
    {
        itemCount--;
        text_Count.text = itemCount.ToString();
        
        if (itemCount <= 0)
            ClearSlot();
    }
    
    //???? ?ʱ?ȭ
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button==PointerEventData.InputButton.Right)
        {
            if(item !=null)
            {
                if(item.itemType==Item.ItemType.Equipment)
                {
                    //????
                }
                {
                    //?Ҹ?
                    Debug.Log(item.itemName + "?? ?????߽??ϴ?");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag ȣ????");
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop ȣ????");
        if (DragSlot.instance.dragSlot != null)
        ChangeSlot();
    }
    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if(_tempItem !=null)
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        else
            DragSlot.instance.dragSlot.ClearSlot();

    }

    public Item.ItemType GetItemType()
    {
        return item != null ? item.itemType : Item.ItemType.None;
    }
}