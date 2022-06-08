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

    //매뉴 부르기
    void CallManu()
    {
        GameManager_CHG.isPause = true;
        go_BaseUi.SetActive(true);
        status.SetActive(false);
        Time.timeScale = 0f;   //시간 멈춤
    }

    //매뉴 닫기
    void CloseManu()
    {
        GameManager_CHG.isPause = false;
        go_BaseUi.SetActive(false);
        status.SetActive(true);
        Time.timeScale = 1f;   //시간 다시 시작
    }

    //게임 세이브
    public void ClickContinue()
    {
        Debug.Log("계속진행");
        CloseManu();
    }

    //게임 로드
    public void ClickRestart()
    {
        Debug.Log("다시시작");
        SceneManager.LoadScene(sceneName);
    }

    //게임 나가기
    public void ClickExit()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }
}
