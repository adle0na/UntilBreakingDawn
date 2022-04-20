using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_UI_CHG : MonoBehaviour
{
    [SerializeField]
    private GameObject go_BaseUi;
    [SerializeField]
    private SaveNLoad_CHG theSaveNLoad;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager_CHG.isPause)
        
                CallManu();
            
            else
          
                CloseManu();
       
        }
    }

    //�Ŵ� �θ���
    void CallManu()
    {
        GameManager_CHG.isPause = true;
        go_BaseUi.SetActive(true);
        Time.timeScale = 0f;   //�ð� ����
    }

    //�Ŵ� �ݱ�
    void CloseManu()
    {
        GameManager_CHG.isPause = false;
        go_BaseUi.SetActive(false);
        Time.timeScale = 1f;   //�ð� �ٽ� ����
    }

    //���� ���̺�
    public void ClickSave()
    {
        Debug.Log("���̺�");
        theSaveNLoad.SaveData();
    }

    //���� �ε�
    public void ClickLoad()
    {
        Debug.Log("�ε�");
        theSaveNLoad.LoadData();
    }

    //���� ������
    public void ClickExit()
    {
        Debug.Log("��������");
        Application.Quit();
    }
}
