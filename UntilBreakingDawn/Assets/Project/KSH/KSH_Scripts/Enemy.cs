using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : InteractionObject
{
    // 몬스터 타입을 enum으로 저장 
    public enum Type { MeleeE, BombE, RangedE};
    public Type enemyType;

    public int maxHealth;
    public int curHealth;
    public int exDamage;

    private PlayerControllerHSW target;
    public BoxCollider meleeArea;
    public bool isChase;
    public bool isAttack;

    private Status status;
    public GameObject BombEnemy;
    public GameObject Effect;
    public GameObject Boulder;
    public Transform shoter;
    public GameObject Player;

    private NavMeshAgent nav;
    private Animator anim;
    private Rigidbody rigid;
    private CapsuleCollider collider;


    private AudioSource Audio;

    public AudioClip enemyAttack;
    public AudioClip enemyHit;
    public AudioClip enemyDeath;
    public AudioClip enemyRangedAttack;
    public AudioClip enemyBombEx;
    public AudioClip enemyFoot;
    public AudioClip explosion;

    private void Awake()
    {
        Audio = GetComponent<AudioSource>();
        status = FindObjectOfType<Status>();
        target = FindObjectOfType<PlayerControllerHSW>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        StartCoroutine(ChaseStart());

    }

    IEnumerator OnDamage()
    {
        // 맞았을 때 isHit 애니메이션 동작 
        anim.SetBool("isHit", true);
        nav.enabled = false;
        PlaySE(enemyHit);
        yield return new WaitForSeconds(0.2f);
        nav.enabled = true;
        anim.SetBool("isHit", false);

        // 체력이 0이 되었을 때 
        if (curHealth <= 0)
        {
            collider.enabled = false;
            nav.enabled = false;
            anim.SetTrigger("onDeath");
            PlaySE(enemyDeath);
            // 몬스터의 타입 별로 죽을 때 해야하는 행동 실행, 죽었을 때 공격 방지
            switch (enemyType)
            {
                case Type.MeleeE:
                    StopCoroutine("Attack");
                    // meleeArea가 true일 때 코루틴이 멈추면 죽은 시체가 계속 공격하기 때문에 false로 변경 
                    //meleeArea.enabled = false;
                    Destroy(meleeArea);
                    break;
                case Type.BombE:
                    StopCoroutine("Explosion");
                    // 코루틴이 중지되어도 실행 중이었던 코루틴으로 인해 이펙트가 나오기 때문에 죽는 즉시 이펙트 비활성화 
                    Effect.gameObject.SetActive(false);
                    break;
                case Type.RangedE:
                    StopCoroutine("Attack");
                    break;
            }
            // 죽고 3초 뒤 몬스터 사라짐 
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }

    IEnumerator ChaseStart()
    {
        yield return new WaitForSeconds(2);
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    private void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(target.transform.position);
            nav.isStopped = !isChase;
        }
    }

    // 적을 타게팅 하는 함수 
    private void Targetting()
    {
        float targetRadius = 0;
        float targetRange = 0;

        // 각 타입별로 사거리와 정확도 차별화 
        switch (enemyType)
        {
            case Type.MeleeE:
                targetRadius = 1.0f;
                targetRange = 1.5f;
                break;
            case Type.BombE:
                targetRadius = 1f;
                targetRange = 3f;
                break;
            case Type.RangedE:
                targetRadius = 0.5f;
                targetRange = 10f;
                break;
        }
        // RayCastHit의 SphareCastAll을 사용해 범위 내의 모든 적 찾기 
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius,
                                    transform.forward, targetRange, LayerMask.GetMask("Player"));
        // 만약 범위 내에 몬스터가 있을 시 어택 코루틴 실행 
        if (!isAttack && rayHits.Length > 0)
        {
            StartCoroutine("Attack");
        }
        
    }

    IEnumerator Attack()
    { 
        isChase = false;
        isAttack = true;

        // 타입 별로 공격 방식 차별화 
        switch (enemyType)
        {
            case Type.MeleeE:

                anim.SetBool("isAttack", true);
                yield return new WaitForSeconds(0.2f);
                PlaySE(enemyAttack);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(0.1f);
                anim.SetBool("isAttack", false);
                break;

            case Type.BombE:
                // 사정거리 내에 들어왔으면 추격을 멈추고
                nav.enabled = false;
                yield return new WaitForSeconds(0.4f);
                
                // Explosion 코루틴 실행
                StartCoroutine("Explosion");
                break;

            case Type.RangedE:

                Player = GameObject.FindWithTag("Player");
                transform.LookAt(Player.transform);

                anim.SetBool("isAttack", true);
                // 투사체를 지정해논 투사체 발사대에 생성한 뒤 힘을 주어 앞으로 던짐
                PlaySE(enemyRangedAttack);
                yield return new WaitForSeconds(0.2f);
                GameObject instantFireBall = Instantiate(Boulder, shoter.position, shoter.rotation);
                Rigidbody rigidFire = instantFireBall.GetComponent<Rigidbody>();
                rigidFire.velocity = transform.forward * 20;

                yield return new WaitForSeconds(1f);

                anim.SetBool("isAttack", false);
                break;
        }

        isChase = true;
        isAttack = false;
    }

    IEnumerator Explosion()
    {
        anim.SetTrigger("onIdle");
        PlaySE(enemyBombEx);
        // 2초 대기한 뒤 
        yield return new WaitForSeconds(2.5f);


        // 범위 내의 적을 찾는다.
        RaycastHit[] exRayHits = Physics.SphereCastAll(transform.position, 7,
                                                        Vector3.up, 0f, LayerMask.GetMask("Player"));

        // 찾을 시 Player 스크립트에서 HitByExplosion 함수(범위 내의 적에게 데미지) 호출 
        if(exRayHits.Length > 0)
        {
            status.HitByExplosion(exDamage);
        }

        // 폭팔 이펙트 실행
        PlaySE(explosion);
        Effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        // 몬스터 소멸 
        BombEnemy.SetActive(false);
    }

    private void FixedUpdate()
    {
        Targetting();
    }

    public override void TakeDamage(int damage)
    {
        curHealth -= damage;

        StartCoroutine(OnDamage());   
    }

    private void PlaySE(AudioClip _clip)
    {
        Audio.clip = _clip;
        Audio.Play();
    }

    public void FootSE()
    {
        PlaySE(enemyFoot);
    }
}
