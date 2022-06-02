using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_CHG : MonoBehaviour
{
    //SerializeField �ν����� â���� ���� ��������

    [SerializeField]  //�ȱ� ���ǵ�
    private float walkSpeed;

    [SerializeField]  //�޸��� ���ǵ�
    private float runSpeed;

    [SerializeField]  //���� 
    private float crouchSpeed;
    
    public float applySpeed;  //��Ȳ�� ���� ���ԵǴ� ���� �ٸ� (�ȱ�� �ٱ�)

    [SerializeField]   //����
    private float jumpPower;  //���� ��

    [SerializeField]   //�뽬
    private float dashSpeed;
    public float dashTime;
    
    [SerializeField]   //ī�޶�
    private float lookSensitivity;    //ī�޶� �ΰ���
    
    // ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit;         // ī�޶� �ü��� ���� ���� (���� ���ư��� ����)
    private float currentCameraRotationX;      // ���� ī�޶� �ü� ��ġ (0)

    [SerializeField]   //�ɾ����� �󸶳� ������ �����ϴ� ����
    private float crouchPosY;        // ���� ��
    public float originPosY;        // ���� ��
    private float applyCrouchPosY;   // ���� ������ ���� �־��� ���� (applySpeed�� ����)
    
    //���� ����
    private bool isGround = true;    // ������ �����ϴ� true�� ���� 
    private bool isRun = false;      // �ȶٰ� �ִ�
    private bool isCrouch = false;   // �� �ɰ� �ִ�
    private bool isDash = false;     // �뽬 ���ϰ� ����
    private bool isDead = false;     // ���
    private bool isHungry = false;   // �������
    
    // Rigidbody ����
    // Collider�� �浹���� ����, Rigidbody�� Collider�� �������� ������
    // ����ϱ� ���� ������ �ʿ�
    private Rigidbody rigid;

    // CapsuleCollider ������Ʈ
    private CapsuleCollider capsuleCollider;
    
    [SerializeField]  //Camera �����ϱ� ���Ͽ�.. ������Ʈ�� ���� ����
    private Camera theCamera;

    private StatusController_CHG theStatusController;

    [SerializeField]  // ���� ����
    private GameObject gameOver;

    GameManager_CHG theGameManager;

    Animator anim;

    [SerializeField]
    private string jump_Sound;


    void Start()
    {
        //rigidbody �������
        //Rigidbody ������Ʈ�� rigid�� �ִ´ٴ� ��
        rigid = GetComponent<Rigidbody>();

        capsuleCollider = GetComponent<CapsuleCollider>();
        theStatusController = GameObject.Find("Status").GetComponent<StatusController_CHG>();
        theGameManager = FindObjectOfType<GameManager_CHG>();
        anim = GetComponent<Animator>();

        applySpeed = walkSpeed;   //�⺻������ �ȴ� ���ǵ� ����
        originPosY = theCamera.transform.localPosition.y;  //ī�޶� ���� ��ǥ ���(�����̱� ������)
        applyCrouchPosY = originPosY;  //�⺻������ ������ originPosY
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

    // ������ �¾�����
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
    //�뽬
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

    //�ɱ� �õ�
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Crouch();

        }
    }

    //�ɱ� ����
    private void Crouch()
    {
        isCrouch = !isCrouch;  //isCrouch�� true �� false��, false �� true�� �ٲ��ִ� ����ġ ����

        if (isCrouch) //isCrouch�� true�϶�
        {
            applySpeed = crouchSpeed;  //�ɾ����� ���ǵ� ����
            applyCrouchPosY = crouchPosY;  //�ɾ����� �ü� ����
            anim.SetBool("isCrouch", true);
        }
        else
        {
            applySpeed = walkSpeed;    //�ȴ� ���ǵ�
            applyCrouchPosY = originPosY; //��� �ü� ����
            anim.SetBool("isCrouch", false);
        }

        //ī�޶��� ��Į ������ x, ����Ǵ� ������ ������ y, ī�޶��� ��Į ������ z
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
        StartCoroutine(CrouchCoroutine()); //CrouchCoroutine ����
    }

    IEnumerator CrouchCoroutine()  //�ɴ� �ൿ�� �ε巴�� �ϱ� ���Ͽ� �ڷ�ƾ ���
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)  //_posY�� ���ϴ� ���� �����ϸ� break
        {
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)  //15���� �ݺ��ϴٰ� Ż��
                break;
            yield return null;  //1������ ���� ����
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);

    }

    //���� üũ
    private void IsGround()
    {
        //Physics - Ŭ����
        //Raycast - ������ ��� �޼��� (��� ��ġ���� ����������� �󸶳�)
        //transform�� ����ϸ� ������Ʈ�� ��ǥ�� ����ϱ� ������ Vector3 ���
        //capsuleCollider.bounds.extents.y - capsuleCollider�� ������ �ݸ�ŭ �������� ��� �ִ�
        //0.1f�� ���� ������ ��糪 ����� ������ ���鿡 ��� ���Ͽ�
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y);
    }

    //�����õ�

    //���� ����
    private void Jump()
    { 
        //�ɾ������� �����ϸ� ����
        if (isCrouch)
            Crouch();

        theStatusController.DecreaseStamina(50);
        //jumpPower ��ŭ trasform.up �������� ���� ���ϴ�
        rigid.velocity = transform.up * jumpPower;
    }

    //�ٴ� �õ�
    //void TryRun()
    //{
    //    //���� ����Ʈ ������ �ۼ� ����
    //    if (Input.GetKey(KeyCode.LeftShift) /*&& theStatusController.GetCurrentSP() > 0*/)
    //    {
    //        Running();
    //    }
    //    //���� ����Ʈ�� ���� �ٱ� �˽�
    //    if (Input.GetKeyUp(KeyCode.LeftShift) /*|| theStatusController.GetCurrentSP() <= 0*/)
    //    {
    //        RunningCancel();
    //    }
    //}
    void TryJump()
    {
        //spacebar ������ ����
        //isGround �� true �϶��� ����
        if (Input.GetKeyDown(KeyCode.Space) && isGround /*&& theStatusController.GetCurrentSP() > 0*/)
        {
            Jump();
            SoundManager_CHG.instance.PlaySE(jump_Sound);
        }
    }

    //�ޱ��
    void Running()
    {
        if (isCrouch)
            Crouch();


        //true �� �⺻������ applySpeed �� �ִ� walkSpeed �� runSpeed �� �ٲ�
        isRun = true;
        theStatusController.DecreaseStamina(1);
        applySpeed = runSpeed;
    }

    //�޸��� ĵ��
    void RunningCancel()
    {
        //isRun�� false�� �Ǹ� applySpeed�� walkSpeed �� �ٲ�
        isRun = false;
        applySpeed = walkSpeed;
    }

    //������
    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");  //�¿� �̵�Ű ����ȭ
        float moveDirZ = Input.GetAxisRaw("Vertical");    //�յ� �̵�Ű ����ȭ

        Vector3 moveHorizontal = transform.right * moveDirX;  //������ �������� �����ڴ� ����ó��
        Vector3 moveVertical = transform.forward * moveDirZ;  //�����ϰڴ� ����ó��

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        //rigid �� MovePosition �޼��带 ����Ͽ� transform position (���� ��ġ)���� velocity ��ŭ ����
        rigid.MovePosition(transform.position + velocity * Time.deltaTime);

        anim.SetBool("isWalk", velocity != Vector3.zero);
    }

    private void CharacterRotation()  //�¿� ĳ���� ȸ��
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        //_characterRotationY ���� Quaternion �� �ٲ㼭 rotation ���� ���ϸ� ����Ƽ ���ο��� ȸ��
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    private void CameraRotation()  //���� ī�޶� ȸ��
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;  //ī�޶� �ΰ��� ��ŭ ������ �����δ�.
        currentCameraRotationX -= _cameraRotationX;

        //Clamp�� ����Ͽ� currentCameraRotationX ��  cameraRotationLimit ��ŭ �������� ���д�
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

    }

}