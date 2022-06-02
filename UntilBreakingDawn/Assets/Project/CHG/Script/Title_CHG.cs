using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_CHG : MonoBehaviour
{
    public string sceneName = "GameStage";

    //start 버튼
    public void ClickStart()
    {
        Debug.Log("시작");
        SceneManager.LoadScene(sceneName);
    }

    //Exit 버튼
    public void ClickExit()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }
}
