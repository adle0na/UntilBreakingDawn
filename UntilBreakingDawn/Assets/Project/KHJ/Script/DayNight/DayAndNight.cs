using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float dayChange = 10.0f;       // 낮 밤 바꾸기 최대 시간초

    private float ChangeDayCount = 1.0f;   // 낮 밤 바꾸기 시작 시간초 
    private float RotateDay = 180f;        // 180도로 바꿔서 낮밤 유지

    private SceneManager Scene;             // 씬 바꾸기

    private bool isNight = false;           // 낮 밤 바꾸기용

    // Update is called once per frame
    void Update()
    {
        // 게임 100초 = 현실 시간 1초
        // 지금은 현실 시간으로 계산
        ChangeDayCount += Time.deltaTime;
        Debug.Log(ChangeDayCount);
        if (ChangeDayCount >= dayChange)
        {
            transform.Rotate(Vector3.right, RotateDay);

            if (!isNight)
            {
                isNight = true;
                ChangeDayCount = 0;
            }
            else
            {
                isNight = false;
                ChangeDayCount = 0;
            }
        }

    }
}
