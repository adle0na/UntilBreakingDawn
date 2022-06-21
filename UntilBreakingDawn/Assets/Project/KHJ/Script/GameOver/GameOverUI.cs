using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject retry;
    public GameObject exit;

    private void Awake()
    {
        retry.SetActive(true);
        exit.SetActive(true);
    }
}
