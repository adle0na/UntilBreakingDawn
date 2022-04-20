using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController_CHG : MonoBehaviour
{
    //체력
    [SerializeField]
    public int hp;
    public int currentHp;

    // 스테미나
    [SerializeField]
    public int sp;
    public int currentSp;

    // 스테미나 증가량
    [SerializeField]
    public int spIncreaseSpeed;

    // 스테미나 재회복 딜레이
    [SerializeField]
    public int spRechargeTime;         //시간 지정하여
    private int currentSpRechargeTime;  //지정된 시간 이후에 회복

    // 스테미나 감소 여부
    private bool spUsed;

    // 방어력
    [SerializeField]
    public int dp;
    private int currentDp;

    // 배고픔
    [SerializeField]
    public int hungry;
    public int currentHungry;

    // 배고픔이 줄어드는 속도
    [SerializeField]
    public int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    // 목마름
    [SerializeField]
    public int thirsty;
    public int currentThirsty;

    // 배고픔이 줄어드는 속도
    [SerializeField]
    public int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    // 만족도
    [SerializeField]
    public int satisfy;
    private int currentSatisfy;

    [SerializeField]
    public Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;


    GameManager_CHG theGameManager;
    PlayerController_CHG thePlayerController;

    private void Awake()
    {
        theGameManager = GetComponent<GameManager_CHG>();
        thePlayerController = GameObject.Find("Player").GetComponent<PlayerController_CHG>();
    }

    private void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;

      
    }

    private void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();
        GaugeUpdate();
    }

    

    //배고픔 수치 떨어지는 함수
    private void Hungry()
    {
        if (currentHungry > 0)
        {
            //  1씩 증가하다가 hungryDecreaseTime 에 지정된 숫자가 넘으면 else로 이동
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;  // 재계산에 쓰이기 위해서 0으로 초기화
            }
        }
        // 배고픔 수치가 0이 됐을때
        else
        {
            Debug.Log("배고픔 수치가 0이 되었습니다.");
        }
    }


    //목마름 수치 떨어지는 함수
    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            //  1씩 증가하다가 thirstyDecreaseTime 에 지정된 숫자가 넘으면 else로 이동
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;  // 재계산에 쓰이기 위해서 0으로 초기화
            }
        }
        // 배고픔 수치가 0이 됐을때
        else
            Debug.Log("목마름 수치가 0이 되었습니다.");
    }

    //게이지 이미지 닳는거 보여주는 함수
    private void GaugeUpdate()
    {
        // 백분율하기 위하여 / 사용
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }


    // 스테미나 다시 차는 함수
    private void SPRechargeTime()
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
    }


   

    // 스테미나 다시 차는 함수
    private void SPRecover()
    {
        // spUsed 가 false 이거나 현재 sp가 최대 sp 보다 작을 때 실행
        if(!spUsed && currentSp < sp)
        {
            // 현재 sp가 spIncreaseSpeed 만큼 증가
            currentSp += spIncreaseSpeed;
        }
    }

    // HP가 찰 때
    public void IncreaseHP(int _count)
    {
        // 최대 hp (100) 보다 현재 hp와 파라미터안의 값과 더한 값이 작으면 피 회복 
        if (currentHp + _count < hp)
            currentHp += _count;
        else
            currentHp = hp;
    }

    // HP가 줄어들 때 (공격 혹은 잘못된 음식 섭취)
    public void DecreaseHP(int _count)
    {
        //HP가 깎이기 전에 DP 먼저 깎임
        if(currentDp > 0)
        {
            DecreaseDP(_count);
            return;
        }

        //파라미터의 데미지 받음
        currentHp -= _count;

        if (currentHp <= 0)
        {
            Debug.Log("HP가 0이 되었습니다.");
        }
    }

    // DP가 찰 때
    public void IncreaseDP(int _count)
    {
        // 최대 dp 보다 현재 dp와 파라미터안의 값과 더한 값이 작으면 방어 회복 
        if (currentDp + _count < dp)
            currentDp += _count;
        else
            currentDp = dp;
    }

    // DP가 줄어들 때 
    public void DecreaseDP(int _count)
    {
        //파라미터의 데미지 받음
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("방어력이 0이 되었습니다.");
    }

    // 배고픔이 찰 때
    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    // 배고픔이 줄어들 때 
    public void DecreaseHungry(int _count)
    {
        if(currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }

    // 갈증이 찰 때
    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    // 갈증이 날 때
    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }

    // 스테미나 떨어지는 함수
    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        // 0 보다 크게 하여 마이너스가 되지 않게 만듦
        // 0 보다 클 때는 넘어온 파라미터 값만큼 깎인다
        if (currentSp - _count > 0)
            currentSp -= _count;
        // 마이너스가 되면 0 으로 바뀐다 
        else
            currentSp = 0;
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }

    
}

