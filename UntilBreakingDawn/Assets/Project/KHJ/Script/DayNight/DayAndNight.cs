using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float ChangeDayCount = 1.0f;   // ³· ¹ã ¹Ù²Ù±â ½ÃÀÛ ½Ã°£ÃÊ 
    [SerializeField] private float dayChange = 10.0f;       // ³· ¹ã ¹Ù²Ù±â ÃÖ´ë ½Ã°£ÃÊ
    [SerializeField] private float RotateDay = 0.1f;        // °ÔÀÓ 100ÃÊ = Çö½Ç 1ÃÊ

    private SceneManager Scene;             // ¾À ¹Ù²Ù±â

    private bool isNight = false;           // ³· ¹ã ¹Ù²Ù±â¿ë

    // Update is called once per frame
    void Update()
    {
        ChangeDayCount += Time.deltaTime;

        if (ChangeDayCount >= dayChange)
        {
            transform.Rotate(Vector3.right, RotateDay * Time.deltaTime);

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
