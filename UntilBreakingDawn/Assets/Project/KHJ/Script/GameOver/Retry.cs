using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public void RetryClick(int _TabNumber)
    {
        SceneManager.LoadScene("MainScene");
    }
}
