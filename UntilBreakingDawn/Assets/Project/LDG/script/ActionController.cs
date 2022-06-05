using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //아이템 습득이 가능한 최대 거리

    private bool pickupActivated = false; //아이템 습득 가능할시 true

    private RaycastHit hitInfo; //충돌체 정보 저장

    //특정 레이어를 가진 오브젝트에 대해서만 습득할수 있어야 함
    [SerializeField]
    private LayerMask layerMask;
    //행동을 보여 줄 텍스트
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theinventory;
   
    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }
    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickup();
        }
    }
    private void CanPickup()
    {
        if(pickupActivated)
        {
            if(hitInfo.transform !=null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "획득했습니다");
                theinventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickup>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }    
        }
    }
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
            InfoDisappear();
    }
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "획득" + "<color=yellow>" + "(E)" + "</color>";
    }
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
