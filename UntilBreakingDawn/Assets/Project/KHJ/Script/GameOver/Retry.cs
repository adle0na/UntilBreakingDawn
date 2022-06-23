using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    private GameObject playerRetry;

    private void Awake()
    {
        playerRetry = GameObject.Find("Player");
    }

    public void RetryClick(int _TabNumber)
    {
        if (playerRetry.GetComponent<Status>().isPaused == false)
        {
            playerRetry.GetComponent<Status>().isPaused = true;
        }

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainScene");
    }
}
