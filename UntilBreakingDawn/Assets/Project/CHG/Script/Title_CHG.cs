using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_CHG : MonoBehaviour
{
    public string sceneName = "GameStage";

    //start ��ư
    public void ClickStart()
    {
        Debug.Log("����");
        SceneManager.LoadScene(sceneName);
    }

    //Exit ��ư
    public void ClickExit()
    {
        Debug.Log("��������");
        Application.Quit();
    }
}
