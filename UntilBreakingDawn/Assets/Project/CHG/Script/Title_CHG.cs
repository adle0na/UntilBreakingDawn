using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_CHG : MonoBehaviour
{
    public string sceneName = "GameStage";

    public static Title_CHG instance;

    public GameObject mainScreen;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    //start ��ư
    public void ClickStart()
    {
        LoadingSceneController.LoadScene("MainScene");
        gameObject.SetActive(false);
    }

    //exit ��ư
    public void ClickExit()
    {
        Application.Quit();
    }
}
