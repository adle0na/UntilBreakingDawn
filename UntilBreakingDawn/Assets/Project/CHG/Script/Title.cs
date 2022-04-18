using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";

    public static Title instance;

    private SaveNLoad theSaveNLoae;

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

    //start 버튼
    public void ClickStart()
    {
        Debug.Log("로딩");
        SceneManager.LoadScene(sceneName);
    }

    //start 버튼
    public void ClickLoad()
    {
        Debug.Log("로드");

        StartCoroutine(LoadCoroutin());

       
        IEnumerator LoadCoroutin()
        {

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                yield return null;
            }

            theSaveNLoae = FindObjectOfType<SaveNLoad>();

            theSaveNLoae.LoadData();
            Destroy(gameObject);

        }
    }

    //start 버튼
    public void ClickExit()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }
}
