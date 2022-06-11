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

    private NavMeshAgent nav;
    private Animator anim;
    private Rigidbody rigid;
    private CapsuleCollider collider;

    private IEnumerator AttackSave;
    private IEnumerator ExplosionSave;

    private void Awake()
    {
        status = FindObjectOfType<Status>();
        target = FindObjectOfType<PlayerControllerHSW>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        StartCoroutine(ChaseStart());

    }

    private void Start()
    {
        AttackSave = Attack();
        ExplosionSave = Explosion();
    }

    /*
    // 총알 프리팹 예시용
    private void OnTriggerEnter(Collider other)
    {
        // 총알에 맞았을 때 몬스터의 체력을 깎고 OnDamage 코루틴 시작 
        if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            StartCoroutine(OnDamage());
        }
    }
    */

    IEnumerator OnDamage()
    {
        // 맞았을 때 isHit 애니메이션 동작 
        anim.SetBool("isHit", true);
        nav.enabled = false;
        yield return new WaitForSeconds(0.1f);
        nav.enabled = true;
        anim.SetBool("isHit", false);

        // 체력이 0이 되었을 때 
        if (curHealth <= 0)
        {
            // 충돌을 없애기 위해 죽는 즉시 레이어를 변경해 모든 오브젝트와의 충돌을 막음 
            //gameObject.layer = 14;
            collider.enabled = false;
            nav.enabled = false;
            anim.SetTrigger("onDeath");
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
                targetRange = 30f;
                break;
        }
        // RayCastHit의 SphareCastAll을 사용해 범위 내의 모든 적 찾기 
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius,
                                    transform.forward, targetRange, LayerMask.GetMask("Player"));
        // 만약 범위 내에 몬스터가 있을 시 어택 코루틴 실행 
        if(rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine("Attack");
        }
        
    }

    IEnumerator Attack()
    { 
        isChase = false;
        isAttack = true;

        anim.SetBool("isAttack", true);

        // 타입 별로 공격 방식 차별화 
        switch (enemyType)
        {
            case Type.MeleeE:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(0.1f);

                break;

            case Type.BombE:
                // 사정거리 내에 들어왔으면 추격을 멈추고
                nav.enabled = false;
                yield return new WaitForSeconds(0.5f);
                
                // Explosion 코루틴 실행
                StartCoroutine("Explosion");
                break;

            case Type.RangedE:
                // 투사체를 지정해논 투사체 발사대에 생성한 뒤 힘을 주어 앞으로 던짐 
                yield return new WaitForSeconds(0.2f);
                GameObject instantFireBall = Instantiate(Boulder, shoter.position, shoter.rotation);
                Rigidbody rigidFire = instantFireBall.GetComponent<Rigidbody>();
                rigidFire.velocity = transform.forward * 20;
                
                yield return new WaitForSeconds(1f);
                break;
        }

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    IEnumerator Explosion()
    {
        anim.SetTrigger("onIdle");
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
        Effect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
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

}
