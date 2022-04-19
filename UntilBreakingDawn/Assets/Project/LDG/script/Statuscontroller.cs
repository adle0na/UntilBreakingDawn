using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    //hp
    [SerializeField]
    private int hp;
    private int currentHp;

    //sp
    [SerializeField]
    private int sp;
    private int currentSp;

    //sp ??????
    [SerializeField]
    private int spIncreaseSpeed;

    //sp ?????? ??????
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;

    //sp ???? ????
    [SerializeField]
    private bool spUsed;

    //??????
    [SerializeField]
    private int dp;
    private int currentDp;

    //??????
    [SerializeField]
    private int hungry;
    private int currentHungry;

    //???????? ????????????
    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungeryDecreaseTime;

    //??????
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    //???????? ???????? ????
    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    //??????
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    //?????? ??????
    [SerializeField]
    private Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        GaugeUpdate();
    }
    private void Hungry()
    {
        if (currentHungry > 0)
        {
            if (currentHungeryDecreaseTime <= hungryDecreaseTime)
                currentHungeryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungeryDecreaseTime = 0;
            }
        }
        else
            Debug.Log("?????? ?????? 0?? ??????????");
    }
    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("?????? ?????? 0?? ??????????");
    }
    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }
}
