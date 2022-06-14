using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Timer : MonoBehaviour
{
    public Slider     _timerSlider;
    public Text       _timerText;
    public float      _maxTime;
    
    [HideInInspector]
    public bool       stopTimer;
    
    public GameObject _startMassage;
    public GameObject _warningMessage;
    public GameObject _warningBossMessage;

    [SerializeField]
    private float _gameTime;
    private float _timer = 0;
    private float _time;
    
    private int   _minutes;
    private int   _seconds;
    
    private void Start()
    {
        stopTimer = false;
        _timerSlider.maxValue = _gameTime;
        _timerSlider.value    = _gameTime;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        _time = _gameTime - _timer;
        _minutes = Mathf.FloorToInt(_time / 60);
        _seconds = Mathf.FloorToInt(_time - _minutes * 60f);
        
        string textTime = string.Format("{0:0}:{1:00}", _minutes, _seconds);
        
        if (_time <= 600 && _time > 580 )
        {
            _startMassage.SetActive(true);
            _warningMessage.SetActive(false);
            _warningBossMessage.SetActive(false);
        }
        else if (_time <= 580 && _time > 440)
        {
            _startMassage.SetActive(false);
        }
        else if (_time >= 440 && _time > 420)
        {
            _warningMessage.SetActive(true);
        }
        else if (_time >= 400 && _time > 300)
        {
            _warningMessage.SetActive(false);
        }
        else if (_time >= 300 && _time > 280)
        {
            _warningBossMessage.SetActive(true);
        }
        else if (_time >= 280 && _time > 200)
        {
            _warningBossMessage.SetActive(false);
        }
        else if (_time >= 200 && _time > 180)
        {
            _warningBossMessage.SetActive(true);
        }
        else if (_time >= 180 && _time > 0)
        {
            _warningBossMessage.SetActive(false);
        }
        else if (_time <= 0)
        {
            _startMassage.SetActive(false);
            _warningMessage.SetActive(false);
            if (stopTimer == false)
            {
                stopTimer = true;
                Debug.Log("소환테스트");
                return;
            }
        }
        else
        {
            _time = _maxTime;
        }

        if (stopTimer == false)
        {
            _timerText.text = textTime;
            _timerSlider.value = _time;
        }
    }

}
