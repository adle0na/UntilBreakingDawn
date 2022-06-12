using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : InteractionObject
{
    public enum BossType { minotaur, medusa};
    public BossType Type;

    public float curHealth = 2000.0f;
    public float maxHealth = 2000.0f;
    public float chargingSkillDelay = 15.0f;
    //public int skill1Damage = 500;
    public int fireDamage = 100;

    //private float brokenDamage = 1.2f;
    public float fireSkillDelay = 20.0f;

    private PlayerControllerHSW target;
    //public GameObject bossSkill1;
    private Status status;
    //public GameObject PlayerSc;

    //미노타우르스 
    public BoxCollider meleeArea;
    //public GameObject effect;
    public GameObject player;
    public GameObject mail;
    public GameObject fireSkill;

    //메두사
    public GameObject fireBall;
    public Transform shoter;

    private bool isChase;
    private bool isAttack;
    private bool isBroken = true;
    //private bool skill1 = true;
    //public bool charging;

    Animator anim;
    Rigidbody rigid;
    BoxCollider collider;
    NavMeshAgent nav;

    private void Awake()
    {
        status = FindObjectOfType<Status>();
        target = FindObjectOfType<PlayerControllerHSW>();
        nav = GetComponent<NavMeshAgent>();
        collider = GetComponent<BoxCollider>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        StartCoroutine(ChaseStart());
    }

    IEnumerator ChaseStart()
    {
        yield return new WaitForSeconds(1);
        isChase = true;

        anim.SetBool("isWalk", true);
    }

    private void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(target.transform.position);
            nav.isStopped = !isChase;

            if (isBroken)
            {
                StartCoroutine(BrokenMail());
            }
        }
        FireSkill();
        //if (skill1)
        //{
        //    ChargingSkill();
        //}

    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!charging)
    //    {
    //        if (other.tag == "Bullet")
    //        {
    //            Bullet bullet = other.GetComponent<Bullet>();

    //            if (isBroken)
    //            {
    //                curHealth -= bullet.damage * brokenDamage;
    //                StartCoroutine(OnDamage());

    //            }
    //            else
    //            {
    //                curHealth -= bullet.damage;
    //                StartCoroutine(OnDamage());
    //            }
    //        }
    //    }
    //}

    IEnumerator OnDamage()
    { 
        if (curHealth <= 0)
        {
            collider.enabled = false;
            nav.enabled = false;
            anim.SetTrigger("onDeath");
            StopCoroutine("Attack");
            yield return new WaitForSeconds(2.15f);
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        Targetting();
    }

    private void Targetting()
    {
        float targetRadius = 0.0f;
        float targetRange = 0.0f;

        switch (Type) {
            case BossType.minotaur:
                targetRadius = 1.0f;
                targetRange = 2.0f;
                break;

            case BossType.medusa:
                targetRadius = 0.5f;
                targetRange = 20.0f;
                break;
        }

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                                    transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        nav.enabled = false;

        switch (Type)
        {
            case BossType.minotaur:

                anim.SetBool("isAttack", true);

                meleeArea.enabled = true;
                yield return new WaitForSeconds(2f);
                meleeArea.enabled = false;  
                yield return new WaitForSeconds(0.5f);

                anim.SetBool("isAttack", false);
                break;

            case BossType.medusa:

                anim.SetBool("isAttack", true);

                yield return new WaitForSeconds(2.45f);

                //yield return new WaitForSeconds(0.8f);

                //GameObject instantFireBall = Instantiate(fireBall, shoter.position, shoter.rotation);
                //Rigidbody rigidFireBall = instantFireBall.GetComponent<Rigidbody>();
                //rigidFireBall.velocity = transform.forward * 20;

                //GameObject instantFireBall2 = Instantiate(fireBall, shoter.position, shoter.rotation);
                //Rigidbody rigidFireBall2 = instantFireBall2.GetComponent<Rigidbody>();
                //rigidFireBall2.velocity = transform.TransformDirection (new Vector3(1, 0, 1).normalized) * 20;

                //GameObject instantFireBall3 = Instantiate(fireBall, shoter.position, shoter.rotation);
                //Rigidbody rigidFireBall3 = instantFireBall3.GetComponent<Rigidbody>();
                //rigidFireBall3.velocity = transform.TransformDirection (new Vector3(-1, 0, 1).normalized) * 20;

                //GameObject instantFireBall4 = Instantiate(fireBall, shoter.position, shoter.rotation);
                //Rigidbody rigidFireBall4 = instantFireBall4.GetComponent<Rigidbody>();
                //rigidFireBall4.velocity = transform.TransformDirection(new Vector3(1, 0, 2).normalized) * 20;

                //GameObject instantFireBall5 = Instantiate(fireBall, shoter.position, shoter.rotation);
                //Rigidbody rigidFireBall5 = instantFireBall5.GetComponent<Rigidbody>();
                //rigidFireBall5.velocity = transform.TransformDirection(new Vector3(-1, 0, 2).normalized) * 20;

                //yield return new WaitForSeconds(1.65f);

                anim.SetBool("isAttack", false);
                break;
        }

        isChase = true;
        nav.enabled = true;
        isAttack = false;
    }

    //private void ChargingSkill()
    //{
    //    if (curHealth < 3000)
    //    {
    //        charging = true;
    //        StopCoroutine("Attack");
    //        nav.enabled = false;
    //        bossSkill1.SetActive(true);
    //        anim.SetBool("isCharging", true);

    //        fireSkillDelay = 100.0f;
    //        chargingSkillDelay -= Time.deltaTime;

    //        switch (Type)
    //        {
    //            case BossType.minotaur:
    //                if (bossSkill1.GetComponent<BossSkill>().skillHealth < 0)
    //                {
    //                    anim.SetBool("isCharging", false);
    //                    nav.enabled = true;
    //                    skill1 = false;
    //                    charging = false;
    //                    Destroy(bossSkill1);

    //                    fireSkillDelay = 10.0f;
    //                }
    //                break;
    //            case BossType.medusa:
    //                if (bossSkill1.GetComponent<BossSkill_Medusa>().allDestroy == true)
    //                {
    //                    anim.SetBool("isCharging", false);
    //                    nav.enabled = true;
    //                    skill1 = false;
    //                    Destroy(bossSkill1);
    //                    charging = false;
    //                    fireSkillDelay = 5.0f;
    //                }
    //                break;
    //        }
    //        if(chargingSkillDelay < 0)
    //        {
    //            effect.SetActive(true);

    //            switch (Type)
    //            {
    //                case BossType.minotaur:
    //                    Destroy(bossSkill1.GetComponent<BossSkill>().gameObject);
    //                    break;
    //                case BossType.medusa:
    //                    Destroy(bossSkill1.GetComponent<BossSkill_Medusa>().gameObject);
    //                    break;
    //            }

    //            anim.SetBool("isCharging", false);
    //            nav.enabled = true;
    //            skill1 = false;
    //            charging = false;
    //            Destroy(effect, 1);

    //            RaycastHit[] skillRayHits = Physics.SphereCastAll(transform.position, 7,
    //                                                    Vector3.up, 0f, LayerMask.GetMask("Player"));

    //            if (skillRayHits.Length > 0)
    //            {
    //                PlayerSc.GetComponent<KSH_Player>().BossSkill(skill1Damage);
    //            }
    //        }
    //    }
    //}

    IEnumerator BrokenMail()
    {
        
        if (curHealth < maxHealth/2)
        {
            
            anim.SetTrigger("onHit");
            mail.SetActive(false);
            nav.enabled = false;
            yield return new WaitForSeconds(1.2f);
            nav.enabled = true;
            isBroken = false;
            
        }

    }

    private void FireSkill()
    {
        switch (Type)
        {
            case BossType.minotaur:
            fireSkillDelay -= Time.deltaTime;

            if (fireSkillDelay < 0)
            {
               anim.SetTrigger("onFireSkill");
               StartCoroutine(FireSkillEffect());
               fireSkillDelay = 10.0f;
            }
                break;
        }
    }

    IEnumerator FireSkillEffect()
    {
        nav.enabled = false;

        yield return new WaitForSeconds(1.3f);
        fireSkill.SetActive(true);

        RaycastHit[] fireRayHits = Physics.SphereCastAll(transform.position, 7,
                                                            Vector3.up, 0f, LayerMask.GetMask("Player"));
        if (fireRayHits.Length > 0)
        {
            status.FireSkill(fireDamage);
        }

        yield return new WaitForSeconds(1.8f);

        fireSkill.SetActive(false);
             
        
        nav.enabled = true;
    }

    public override void TakeDamage(int damage)
    {
        curHealth -= damage;

        StartCoroutine(OnDamage());

    }

    public void MedusaAttack()
    {

        GameObject instantFireBall = Instantiate(fireBall, shoter.position, shoter.rotation);
        Rigidbody rigidFireBall = instantFireBall.GetComponent<Rigidbody>();
        rigidFireBall.velocity = transform.forward * 20;

        GameObject instantFireBall2 = Instantiate(fireBall, shoter.position, shoter.rotation);
        Rigidbody rigidFireBall2 = instantFireBall2.GetComponent<Rigidbody>();
        rigidFireBall2.velocity = transform.TransformDirection(new Vector3(1, 0, 1).normalized) * 20;

        GameObject instantFireBall3 = Instantiate(fireBall, shoter.position, shoter.rotation);
        Rigidbody rigidFireBall3 = instantFireBall3.GetComponent<Rigidbody>();
        rigidFireBall3.velocity = transform.TransformDirection(new Vector3(-1, 0, 1).normalized) * 20;

        GameObject instantFireBall4 = Instantiate(fireBall, shoter.position, shoter.rotation);
        Rigidbody rigidFireBall4 = instantFireBall4.GetComponent<Rigidbody>();
        rigidFireBall4.velocity = transform.TransformDirection(new Vector3(1, 0, 2).normalized) * 20;

        GameObject instantFireBall5 = Instantiate(fireBall, shoter.position, shoter.rotation);
        Rigidbody rigidFireBall5 = instantFireBall5.GetComponent<Rigidbody>();
        rigidFireBall5.velocity = transform.TransformDirection(new Vector3(-1, 0, 2).normalized) * 20;

    }

}
