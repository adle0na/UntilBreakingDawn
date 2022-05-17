using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public Slider     _timerSlider;
    public Text       _timerText;
    public float      _maxTime;
    public bool       _stopTimer;
    public GameObject _startMassage;
    public GameObject _warningMessage;

    private float _gameTime;
    private float _timer = 1;
    private float _time;
    private int   _minutes;
    private int   _seconds;
    
    private void Start()
    {
        _stopTimer = false;
        _timerSlider.maxValue = _gameTime;
        _timerSlider.value = _gameTime;
    }

    void Update()
    {
        _timer += Time.deltaTime;

        _time = _gameTime - _timer;
        _minutes = Mathf.FloorToInt(_time / 60);
        _seconds = Mathf.FloorToInt(_time - _minutes * 60f);
        
        string textTime = string.Format("{0:0}:{1:00}", _minutes, _seconds);
        
        if (_time <= 300 && _time > 270 )
        {
            _startMassage.SetActive(true);
        }
        else if (_time <= 270 && _time > 30)
        {
            _startMassage.SetActive(false);
            _warningMessage.SetActive(false);
        }
        else if (_time >= 40 && _time > 10)
        {
            _warningMessage.SetActive(true);
        }
        else if (_time <= 0)
        {
            _startMassage.SetActive(false);
            _warningMessage.SetActive(false);
            if (_stopTimer == false)
            {
                _stopTimer = true;
                StartCoroutine(GameObject.Find("EnemyMemoryPool").GetComponent<EnemyMemoryPool>().SpawnTile());
                return;
            }
        }
        else
        {
            _time = _maxTime;
        }

        if (_stopTimer == false)
        {
            _timerText.text = textTime;
            _timerSlider.value = _time;
        }
    }

}
