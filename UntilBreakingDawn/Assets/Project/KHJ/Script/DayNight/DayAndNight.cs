using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float dayChange = 10.0f;       // �� �� �ٲٱ� �ִ� �ð���

    private float ChangeDayCount = 1.0f;   // �� �� �ٲٱ� ���� �ð��� 
    private float RotateDay = 180f;        // 180���� �ٲ㼭 ���� ����

    private SceneManager Scene;             // �� �ٲٱ�

    private bool isNight = false;           // �� �� �ٲٱ��

    // Update is called once per frame
    void Update()
    {
        // ���� 100�� = ���� �ð� 1��
        // ������ ���� �ð����� ���
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
