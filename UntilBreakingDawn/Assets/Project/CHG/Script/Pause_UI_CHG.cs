using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_UI_CHG : MonoBehaviour
{
    [SerializeField]
    private GameObject go_BaseUi;
    [SerializeField]
    private GameObject status;

    public string sceneName = "GameStage";

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
        status.SetActive(false);
        Time.timeScale = 0f;   //�ð� ����
    }

    //�Ŵ� �ݱ�
    void CloseManu()
    {
        GameManager_CHG.isPause = false;
        go_BaseUi.SetActive(false);
        status.SetActive(true);
        Time.timeScale = 1f;   //�ð� �ٽ� ����
    }

    //���� ���̺�
    public void ClickContinue()
    {
        Debug.Log("�������");
        CloseManu();
    }

    //���� �ε�
    public void ClickRestart()
    {
        Debug.Log("�ٽý���");
        SceneManager.LoadScene(sceneName);
    }

    //���� ������
    public void ClickExit()
    {
        Debug.Log("��������");
        Application.Quit();
    }
}
