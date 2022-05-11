using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public Slider _timerSlider;
    public Text   _timerText;
    public float  _gameTime;
    public bool  _stopTimer;

    private void Start()
    {
        _stopTimer = false;
        _timerSlider.maxValue = _gameTime;
        _timerSlider.value = _gameTime;
    }

    void Update()
    {
        float time = _gameTime - Time.time;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60f);

        string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        if (time <= 0)
        {
            if (_stopTimer == false)
            {
                _stopTimer = true;
                StartCoroutine(GameObject.Find("EnemyMemoryPool").GetComponent<EnemyMemoryPool>().SpawnTile());
            }
        }
        else
        {
            _stopTimer = false;
        }

        if (_stopTimer == false)
        {
            _timerText.text = textTime;
            _timerSlider.value = time;
        }
    }

}
