using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_CHG : MonoBehaviour
{
    public string sceneName = "GameStage";

    public static Title_CHG instance;

    private SaveNLoad_CHG theSaveNLoae;

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
        Debug.Log("�ε�");
        SceneManager.LoadScene(sceneName);
    }

    //start ��ư
    public void ClickLoad()
    {
        Debug.Log("�ε�");

        StartCoroutine(LoadCoroutin());

       
        IEnumerator LoadCoroutin()
        {

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                yield return null;
            }

            theSaveNLoae = FindObjectOfType<SaveNLoad_CHG>();

            theSaveNLoae.LoadData();
            Destroy(gameObject);

        }
    }

    //start ��ư
    public void ClickExit()
    {
        Debug.Log("��������");
        Application.Quit();
    }
}
