using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //������ ������ ������ �ִ� �Ÿ�

    private bool pickupActivated = false; //������ ���� �����ҽ� true

    private RaycastHit hitInfo; //�浹ü ���� ����

    //Ư�� ���̾ ���� ������Ʈ�� ���ؼ��� �����Ҽ� �־�� ��
    [SerializeField]
    private LayerMask layerMask;
    //�ൿ�� ���� �� �ؽ�Ʈ
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
                Debug.Log(hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "ȹ���߽��ϴ�");
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
        actionText.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "ȹ��" + "<color=yellow>" + "(E)" + "</color>";
    }
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
