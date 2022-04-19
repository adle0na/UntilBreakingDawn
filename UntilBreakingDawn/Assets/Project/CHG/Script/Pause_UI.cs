using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Pause_UI : MonoBehaviour
{
    [SerializeField]
    private GameObject go_BaseUi;
    [SerializeField]
    private SaveNLoad theSaveNLoad;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause)
        
                CallManu();
            
            else
          
                CloseManu();
       
        }
    }

    //???? ??????
    void CallManu()
    {
        GameManager.isPause = true;
        go_BaseUi.SetActive(true);
        Time.timeScale = 0f;   //???? ????
    }

    //???? ????
    void CloseManu()
    {
        GameManager.isPause = false;
        go_BaseUi.SetActive(false);
        Time.timeScale = 1f;   //???? ???? ????
    }

    //???? ??????
    public void ClickSave()
    {
        Debug.Log("??????");
        theSaveNLoad.SaveData();
    }

    //???? ????
    public void ClickLoad()
    {
        Debug.Log("????");
        theSaveNLoad.LoadData();
    }

    //???? ??????
    public void ClickExit()
    {
        Debug.Log("????????");
        Application.Quit();
    }
}
*/