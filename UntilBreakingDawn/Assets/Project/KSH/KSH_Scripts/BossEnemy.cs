using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour
{
    public int Health;
    public Transform target;
    public BoxCollider meleeArea;
    public bool isChase;
    public bool isAttack;



    Rigidbody rigid;

    NavMeshAgent nav;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        if (nav.enabled)
        {
            nav.SetDestination(target.position);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            Health -= bullet.damage;
            StartCoroutine(OnDamage());
        }
    }

    IEnumerator OnDamage()
    {
        if (Health <= 0)
        {
            gameObject.layer = 7;
            nav.enabled = false;
           
            yield return new WaitForSeconds(3f);
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        Targetting();
    }

    private void Targetting()
    {
        float targetRadius = 1.5f;
        float targetRange = 1.5f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius,
                                    transform.forward, targetRange, LayerMask.GetMask("Player"));
        
        if (rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack() {

        isAttack = true;

        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.5f);

        isAttack = false;
    }
}
