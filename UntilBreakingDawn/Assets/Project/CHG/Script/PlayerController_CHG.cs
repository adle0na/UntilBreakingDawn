using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_CHG : MonoBehaviour
{
    //SerializeField 인스팩터 창에서 쉽게 수정가능

    [SerializeField]  //걷기 스피드
    private float walkSpeed;

    [SerializeField]  //달리기 스피드
    private float runSpeed;

    [SerializeField]  //앉은 
    private float crouchSpeed;
    
    public float applySpeed;  //상황에 따라 대입되는 값이 다름 (걷기와 뛰기)

    [SerializeField]   //점프
    private float jumpPower;  //점프 힘

    [SerializeField]   //대쉬
    private float dashSpeed;
    public float dashTime;
    
    [SerializeField]   //카메라
    private float lookSensitivity;    //카메라 민감도
    
    // 카메라 한계
    [SerializeField]
    private float cameraRotationLimit;         // 카메라 시선의 각도 제한 (고개가 돌아가는 범위)
    private float currentCameraRotationX;      // 현재 카메라 시선 위치 (0)

    [SerializeField]   //앉았을때 얼마나 앉을지 결정하는 변수
    private float crouchPosY;        // 숙인 값
    public float originPosY;        // 원래 값
    private float applyCrouchPosY;   // 위의 각각의 값을 넣어줄 변수 (applySpeed와 동일)
    
    //상태 변수
    private bool isGround = true;    // 땅에서 시작하니 true로 시작 
    private bool isRun = false;      // 안뛰고 있다
    private bool isCrouch = false;   // 안 앉고 있다
    private bool isDash = false;     // 대쉬 안하고 있음
    private bool isDead = false;     // 사망
    private bool isHungry = false;   // 배고프면
    
    // Rigidbody 선언
    // Collider로 충돌영역 설정, Rigidbody는 Collider에 물리학을 입히다
    // 사용하기 전에 선언이 필요
    private Rigidbody rigid;

    // CapsuleCollider 컴포넌트
    private CapsuleCollider capsuleCollider;
    
    [SerializeField]  //Camera 적용하기 위하여.. 컴포넌트에 없기 때문
    private Camera theCamera;

    private StatusController_CHG theStatusController;

    [SerializeField]  // 게임 오버
    private GameObject gameOver;

    GameManager_CHG theGameManager;

    Animator anim;

    [SerializeField]
    private string jump_Sound;


    void Start()
    {
        //rigidbody 갖고오기
        //Rigidbody 컴포넌트를 rigid에 넣는다는 뜻
        rigid = GetComponent<Rigidbody>();

        capsuleCollider = GetComponent<CapsuleCollider>();
        theStatusController = GameObject.Find("Status").GetComponent<StatusController_CHG>();
        theGameManager = FindObjectOfType<GameManager_CHG>();
        anim = GetComponent<Animator>();

        applySpeed = walkSpeed;   //기본값으로 걷는 스피드 대입
        originPosY = theCamera.transform.localPosition.y;  //카메라 로컬 좌표 사용(눈높이기 때문에)
        applyCrouchPosY = originPosY;  //기본값으로 지정된 originPosY
    }

    void Update()
    {
        IsGround();
        TryJump();
        //TryRun();
        TryCrouch();
        TryDash();
        Move();
        CameraRotation();
        CharacterRotation();
        Dead();

        DecreaseSpeed();
    }

    private void DecreaseSpeed()
    {
        if(theStatusController.currentHungry <= 0)
        {
            theStatusController.spIncreaseSpeed = 0;
            theStatusController.currentSp = 0;
            walkSpeed = 2;
        }
        if (theStatusController.currentThirsty <= 0)
        {
            theStatusController.spIncreaseSpeed = 0;
            theStatusController.currentSp = 0;
            walkSpeed = 2;
        }

    }


    private void Dead()
    {
        if (theStatusController.currentHp <= 0)
        {
            Time.timeScale = 0f;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-90.0f, 0, 0), Time.time * 0.01f); 

            theGameManager._goGameOverUI.SetActive(true);
        }
    }

    // 적에게 맞았을때
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            theStatusController.DecreaseHP(30);

           

            Vector3 pos = new Vector3(0, 0, -0.5f);
            transform.position = Vector3.Lerp(transform.position, pos, 0.03f);
        }
    }

    //private bool oneClick = false;
    //private double clickTimer = 0;
    //public float doubleClickSecond = 0.25f;
    //대쉬
    private void TryDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(Dash());
        }

        //if (oneClick && (Time.time - clickTimer) > doubleClickSecond)
        //{
        //    oneClick = false;
        //}

        //if (Input.GetKeyDown(KeyCode.W) && theStatusController.GetCurrentSP() > 0)
        //{
        //    if (!oneClick)
        //    {
        //        clickTimer = Time.time;
        //        oneClick = true;
                
        //    }
        //    else if (oneClick && ((Time.time - clickTimer) < doubleClickSecond))
        //    {
        //        oneClick = false;
        //        StartCoroutine(Dash());
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.A) && theStatusController.GetCurrentSP() > 0)
        //{
        //    if (!oneClick)
        //    {
        //        clickTimer = Time.time;
        //        oneClick = true;
        //    }
        //    else if (oneClick && ((Time.time - clickTimer) < doubleClickSecond))
        //    {
        //        oneClick = false;
        //        StartCoroutine(Dash());
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.S) && theStatusController.GetCurrentSP() > 0)
        //{
        //    if (!oneClick)
        //    {
        //        clickTimer = Time.time;
        //        oneClick = true;
        //    }
        //    else if (oneClick && ((Time.time - clickTimer) < doubleClickSecond))
        //    {
        //        oneClick = false;
        //        StartCoroutine(Dash());
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.D) && theStatusController.GetCurrentSP() > 0)
        //{
        //    if (!oneClick)
        //    {
        //        clickTimer = Time.time;
        //        oneClick = true;
        //    }
        //    else if (oneClick && ((Time.time - clickTimer) < doubleClickSecond))
        //    {
        //        oneClick = false;
        //        StartCoroutine(Dash());
        //    }
        //}
    }

    IEnumerator Dash()
    {
        isDash = true;

        applySpeed *= dashSpeed;

        theStatusController.DecreaseStamina(30);

        yield return new WaitForSeconds(dashTime);

        applySpeed = walkSpeed;

        isDash = false;
    }

    //앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Crouch();

        }
    }

    //앉기 동작
    private void Crouch()
    {
        isCrouch = !isCrouch;  //isCrouch가 true 면 false로, false 면 true로 바꿔주는 스위치 역할

        if (isCrouch) //isCrouch가 true일때
        {
            applySpeed = crouchSpeed;  //앉았을때 스피드 적용
            applyCrouchPosY = crouchPosY;  //앉았을때 시선 적용
            anim.SetBool("isCrouch", true);
        }
        else
        {
            applySpeed = walkSpeed;    //걷는 스피드
            applyCrouchPosY = originPosY; //평소 시선 적용
            anim.SetBool("isCrouch", false);
        }

        //카메라의 로칼 포지션 x, 적용되는 눈높이 포지션 y, 카메라의 로칼 포지션 z
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
        StartCoroutine(CrouchCoroutine()); //CrouchCoroutine 시작
    }

    IEnumerator CrouchCoroutine()  //앉는 행동을 부드럽게 하기 위하여 코루틴 사용
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)  //_posY가 원하는 값에 도래하면 break
        {
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)  //15번을 반복하다가 탈출
                break;
            yield return null;  //1프레임 마다 진행
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);

    }

    //지면 체크
    private void IsGround()
    {
        //Physics - 클래스
        //Raycast - 광선을 쏘는 메서드 (어느 위치에서 어느방향으로 얼마나)
        //transform을 사용하면 오브젝트의 좌표를 사용하기 때문에 Vector3 사용
        //capsuleCollider.bounds.extents.y - capsuleCollider의 영역의 반만큼 레이저를 쏘고 있다
        //0.1f를 더한 이유는 경사나 계단이 있을때 지면에 닿기 위하여
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y);
    }

    //점프시도

    //점프 동작
    private void Jump()
    { 
        //앉아있을때 점프하면 해제
        if (isCrouch)
            Crouch();

        theStatusController.DecreaseStamina(50);
        //jumpPower 만큼 trasform.up 방향으로 힘을 가하다
        rigid.velocity = transform.up * jumpPower;
    }

    //뛰는 시도
    //void TryRun()
    //{
    //    //왼쪽 쉬프트 누르면 뛸수 있음
    //    if (Input.GetKey(KeyCode.LeftShift) /*&& theStatusController.GetCurrentSP() > 0*/)
    //    {
    //        Running();
    //    }
    //    //왼쪽 쉬프트를 때면 뛰기 켄슬
    //    if (Input.GetKeyUp(KeyCode.LeftShift) /*|| theStatusController.GetCurrentSP() <= 0*/)
    //    {
    //        RunningCancel();
    //    }
    //}
    void TryJump()
    {
        //spacebar 누르면 점프
        //isGround 가 true 일때만 실행
        if (Input.GetKeyDown(KeyCode.Space) && isGround /*&& theStatusController.GetCurrentSP() > 0*/)
        {
            Jump();
            SoundManager_CHG.instance.PlaySE(jump_Sound);
        }
    }

    //달기기
    void Running()
    {
        if (isCrouch)
            Crouch();


        //true 면 기본값으로 applySpeed 에 있던 walkSpeed 가 runSpeed 로 바뀜
        isRun = true;
        theStatusController.DecreaseStamina(1);
        applySpeed = runSpeed;
    }

    //달리기 캔슬
    void RunningCancel()
    {
        //isRun이 false가 되면 applySpeed가 walkSpeed 로 바뀜
        isRun = false;
        applySpeed = walkSpeed;
    }

    //움직임
    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");  //좌우 이동키 변수화
        float moveDirZ = Input.GetAxisRaw("Vertical");    //앞뒤 이동키 변수화

        Vector3 moveHorizontal = transform.right * moveDirX;  //오른쪽 방향으로 나가겠다 변수처리
        Vector3 moveVertical = transform.forward * moveDirZ;  //전진하겠다 변수처리

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        //rigid 에 MovePosition 메서드를 사용하여 transform position (현재 위치)에서 velocity 만큼 더함
        rigid.MovePosition(transform.position + velocity * Time.deltaTime);

        anim.SetBool("isWalk", velocity != Vector3.zero);
    }

    private void CharacterRotation()  //좌우 캐릭터 회전
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        //_characterRotationY 값을 Quaternion 로 바꿔서 rotation 값과 곱하면 유니티 내부에서 회전
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    private void CameraRotation()  //상하 카메라 회전
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;  //카메라 민감도 만큼 빠르게 움직인다.
        currentCameraRotationX -= _cameraRotationX;

        //Clamp를 사용하여 currentCameraRotationX 을  cameraRotationLimit 만큼 고정으로 가둔다
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

    }

}