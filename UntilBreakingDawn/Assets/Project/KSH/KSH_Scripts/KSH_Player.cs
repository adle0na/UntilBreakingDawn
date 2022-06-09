using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KSH_Player : MonoBehaviour
{
    public float moveSpeed;
    private float hor;
    private float ver;
    private bool attack = false;


    public int maxHealth;
    public int health;
    public GameObject bullet;
    public Transform bulletShot;


    Vector3 moveVec;

    Rigidbody rigid;
    Material mat;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        mat = GetComponentInChildren<MeshRenderer>().material;
    }

    // 공격 함수 
    private void Attack()
    {
        if (attack)
        {
            StartCoroutine(Shot());
        }
    }

    IEnumerator Shot()
    {
        GameObject instantBullet = Instantiate(bullet, bulletShot.position, bulletShot.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletShot.forward * 20;

        yield return new WaitForSeconds(0.5f);
        
    }

    private void Update()
    {
        attack = Input.GetButtonDown("Fire1");
        hor = Input.GetAxisRaw("Horizontal");
        ver = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hor, 0, ver).normalized;

        transform.position += moveVec * moveSpeed * Time.deltaTime;


        Attack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Damage melee = other.GetComponent<Damage>();
            health -= melee.damage;
            StartCoroutine(OnDamage());
        }
        else if (other.tag == "Boulder")
        {
            Boulder boulder = other.GetComponent<Boulder>();
            health -= boulder.damage;
            StartCoroutine(OnDamage());
        }
    }
    
    IEnumerator OnDamage()
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (health > 0)
        {
            mat.color = Color.black;
        }
        else
        {
            mat.color = Color.gray;
            Destroy(gameObject, 1);
        }
    }

    // 폭팔 시 호출되는 함수 
    public void HitByExplosion(int exDamage)
    {
        health -= exDamage;
        StartCoroutine(OnDamage());
    }

    public void BossSkill(int skill1Damage)
    {
        health -= skill1Damage;
        StartCoroutine(OnDamage());
    }

    public void FireSkill(int fireDamage)
    {
        health -= fireDamage;
        StartCoroutine(OnDamage());
    }
}
