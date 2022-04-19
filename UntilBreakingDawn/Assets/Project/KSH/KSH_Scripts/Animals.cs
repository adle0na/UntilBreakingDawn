using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animals : MonoBehaviour
{
    public enum Type { Boar, Caw, Chicken, Bear};
    public Type animals;

    public BoxCollider meleeArea;
    public Transform target;

    public int aniHealth;
    public float walkSpeed;
    public float walkTime;
    public float waitTime;
    public float runTime = 5;
    private float runSpeed = 15;
    private float applySpeed;
    
    private float currentTime;

    private Vector3 direction;

    private bool isAttack;
    private bool isAction;
    private bool isWalk;
    private bool isEat;
    private bool isRun;
    private bool isDead;
    private bool isHit;
    private bool isStop = true;
    private bool isAttackRun = true;

    private Animator anim;
    private Rigidbody rigid;
    private BoxCollider boxCollider;
    private NavMeshAgent nav;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentTime = waitTime;
        isAction = true;
    }

    private void Update()
    {
        if (animals == Type.Bear)
        {
            if (nav.enabled)
            {
                nav.SetDestination(target.position);
                anim.SetBool("isAttackRun", isAttackRun);
                nav.isStopped = !isStop;
            }
        }
        Move();
        Rotation();
        ElapseTime();
    }

    private void FixedUpdate()
    {
        if(isHit)
            Tagetting();
        
    }

    private void Move()
    {
        if (isWalk || isRun)
        {
            rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        }
    }

    private void Rotation()
    {
        if (isWalk || isRun)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                stateReset();
            }
        }
    }

    private void stateReset()
    {
        isHit = false;
        isRun = false;
        if (animals == Type.Bear)
            nav.enabled = false;
        if(animals != Type.Bear)
            anim.SetBool("isRun", isRun);
        isWalk = false;
        isAction = true;
        applySpeed = walkSpeed;
        anim.SetBool("isWalk", isWalk);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        stateEat();
    }


    private void stateEat()
    {
        isEat = false;
        isAction = true;
        anim.SetBool("isEat", isEat);
        RandAction();
    }

    private void RandAction()
    {
        isAction = true;

        int _random = Random.Range(0, 3); 

        if(_random == 0)
        {
            Wait();
        }
        else if(_random == 1)
        {
            TryWalk();
        }
        else if (_random == 2)
        {
            Eat();
        }

    }

    private void Wait()
    {
        currentTime = waitTime;
    }

    private void Eat()
    {
        isEat = true;
        anim.SetBool("isEat", isEat);
        currentTime = walkTime;
    }

    private void TryWalk()
    {
        isWalk = true;
        anim.SetBool("isWalk", isWalk);
        currentTime = walkTime;
        applySpeed = walkSpeed;
    }

    private void RunOrAttack()
    {
        if (aniHealth > 0)
        {
            if (animals == Type.Bear)
            { 
                currentTime = runTime;
                isHit = true;
                nav.enabled = true;
            }
            else
            {
                applySpeed = runSpeed;
                currentTime = runTime;
                isWalk = false;
                isEat = false;
                isRun = true;
                anim.SetBool("isRun", isRun);
            }
        }
        else
            StartCoroutine(Dead());
    }

    IEnumerator Attack()
    {
        isStop = false;
        isWalk = false;
        isEat = false;
        isAttack = true;
        isAttackRun = false;
        anim.SetBool("isAttack", isAttack);

        yield return new WaitForSeconds(0.9f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.1f);

        isAttack = false;
        isStop = true;
        isAttackRun = true;
        anim.SetBool("isAttack", isAttack);
    }

    IEnumerator Dead()
    {
        isAttack = false;
        isWalk = false;
        isRun = false;
        gameObject.layer = 7;

        anim.SetTrigger("onDead");

        yield return new WaitForSeconds(3f);

        Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            aniHealth -= bullet.damage;
            StartCoroutine(OnDamage());
        }
    }

    IEnumerator OnDamage()
    {
        RunOrAttack();
        yield return new WaitForSeconds(0.1f);
    }

    private void Tagetting()
    {
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, 1.5f,
                                    transform.forward, 3f, LayerMask.GetMask("Player"));
       
        if (rayHits.Length > 0 && !isAttack)
        {

            StartCoroutine(Attack());
        }
    }
}
    
