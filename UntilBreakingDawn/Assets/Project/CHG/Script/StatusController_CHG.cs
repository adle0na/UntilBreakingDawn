using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController_CHG : MonoBehaviour
{
    //ü��
    [SerializeField]
    public int hp;
    public int currentHp;

    // ���׹̳�
    [SerializeField]
    public int sp;
    public int currentSp;

    // ���׹̳� ������
    [SerializeField]
    public int spIncreaseSpeed;

    // ���׹̳� ��ȸ�� ������
    [SerializeField]
    public int spRechargeTime;         //�ð� �����Ͽ�
    private int currentSpRechargeTime;  //������ �ð� ���Ŀ� ȸ��

    // ���׹̳� ���� ����
    private bool spUsed;

    // ����
    [SerializeField]
    public int dp;
    private int currentDp;

    // �����
    [SerializeField]
    public int hungry;
    public int currentHungry;

    // ������� �پ��� �ӵ�
    [SerializeField]
    public int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    // �񸶸�
    [SerializeField]
    public int thirsty;
    public int currentThirsty;

    // ������� �پ��� �ӵ�
    [SerializeField]
    public int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    // ������
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

    

    //����� ��ġ �������� �Լ�
    private void Hungry()
    {
        if (currentHungry > 0)
        {
            //  1�� �����ϴٰ� hungryDecreaseTime �� ������ ���ڰ� ������ else�� �̵�
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;  // ���꿡 ���̱� ���ؼ� 0���� �ʱ�ȭ
            }
        }
        // ����� ��ġ�� 0�� ������
        else
        {
            Debug.Log("����� ��ġ�� 0�� �Ǿ����ϴ�.");
        }
    }


    //�񸶸� ��ġ �������� �Լ�
    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            //  1�� �����ϴٰ� thirstyDecreaseTime �� ������ ���ڰ� ������ else�� �̵�
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;  // ���꿡 ���̱� ���ؼ� 0���� �ʱ�ȭ
            }
        }
        // ����� ��ġ�� 0�� ������
        else
            Debug.Log("�񸶸� ��ġ�� 0�� �Ǿ����ϴ�.");
    }

    //������ �̹��� ��°� �����ִ� �Լ�
    private void GaugeUpdate()
    {
        // ������ϱ� ���Ͽ� / ���
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }


    // ���׹̳� �ٽ� ���� �Լ�
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


   

    // ���׹̳� �ٽ� ���� �Լ�
    private void SPRecover()
    {
        // spUsed �� false �̰ų� ���� sp�� �ִ� sp ���� ���� �� ����
        if(!spUsed && currentSp < sp)
        {
            // ���� sp�� spIncreaseSpeed ��ŭ ����
            currentSp += spIncreaseSpeed;
        }
    }

    // HP�� �� ��
    public void IncreaseHP(int _count)
    {
        // �ִ� hp (100) ���� ���� hp�� �Ķ���;��� ���� ���� ���� ������ �� ȸ�� 
        if (currentHp + _count < hp)
            currentHp += _count;
        else
            currentHp = hp;
    }

    // HP�� �پ�� �� (���� Ȥ�� �߸��� ���� ����)
    public void DecreaseHP(int _count)
    {
        //HP�� ���̱� ���� DP ���� ����
        if(currentDp > 0)
        {
            DecreaseDP(_count);
            return;
        }

        //�Ķ������ ������ ����
        currentHp -= _count;

        if (currentHp <= 0)
        {
            Debug.Log("HP�� 0�� �Ǿ����ϴ�.");
        }
    }

    // DP�� �� ��
    public void IncreaseDP(int _count)
    {
        // �ִ� dp ���� ���� dp�� �Ķ���;��� ���� ���� ���� ������ ��� ȸ�� 
        if (currentDp + _count < dp)
            currentDp += _count;
        else
            currentDp = dp;
    }

    // DP�� �پ�� �� 
    public void DecreaseDP(int _count)
    {
        //�Ķ������ ������ ����
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("������ 0�� �Ǿ����ϴ�.");
    }

    // ������� �� ��
    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    // ������� �پ�� �� 
    public void DecreaseHungry(int _count)
    {
        if(currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }

    // ������ �� ��
    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    // ������ �� ��
    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }

    // ���׹̳� �������� �Լ�
    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        // 0 ���� ũ�� �Ͽ� ���̳ʽ��� ���� �ʰ� ����
        // 0 ���� Ŭ ���� �Ѿ�� �Ķ���� ����ŭ ���δ�
        if (currentSp - _count > 0)
            currentSp -= _count;
        // ���̳ʽ��� �Ǹ� 0 ���� �ٲ�� 
        else
            currentSp = 0;
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }

    
}

