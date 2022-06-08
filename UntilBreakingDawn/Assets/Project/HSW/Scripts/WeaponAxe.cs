using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class WeaponAxe : WeaponBase
{
    // ���� ���� �Ǵ�
    protected bool isAttack = false;
    protected bool isSwing  = false;

    // Attack
    protected int weaponDamage = 5;

    protected float weaponRange;
    protected float attackDelay;
    protected float attackDelayA;
    protected float attackDelayB;

    // Animation
    public Animator anim = null;

    // Hit Target Info
    private RaycastHit hitInfo;

    IHit hit = null;

    public override void StartWeaponAction(int type = 0)
    {
        if(type == 0)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        isAttack = true;
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDelayA);
        isSwing = true;

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(attackDelay - attackDelayA - attackDelayB);
        isAttack = false;
    }

    private bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, weaponRange, LayerMask.GetMask("Resource")))
        {
            return true;
        }
        return false;
    }

    private IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;

                // Damage ����
                if (hitInfo.transform.CompareTag("Tree"))
                {
                    weaponDamage = 5;
                    Debug.Log($"{hitInfo.transform.name} : {weaponDamage}");
                }
                else if (hitInfo.transform.CompareTag("Rock"))
                {
                    weaponDamage = 10;
                    Debug.Log($"{hitInfo.transform.name} : {weaponDamage}");
                }
                else
                {
                    Debug.Log("Error");
                }
                // ���ݹ��� ������Ʈ ���� OnHit�Լ� ����
                hit = hitInfo.collider.gameObject.GetComponent<IHit>();
                hit.weaponDamage = weaponDamage;
                hit.OnHit();
            }
            // IHit �����Լ� �ۼ�
            yield return null;
        }
    }

    public override void StopWeaponAction(int type = 0)
    {

    }

    public override void StartReload()
    {

    }

    public override void IncreaseMagazineMain(int magazineMain)
    {

    }

    public override void IncreaseMagazineSub(int magazineSub)
    {

    }
}
