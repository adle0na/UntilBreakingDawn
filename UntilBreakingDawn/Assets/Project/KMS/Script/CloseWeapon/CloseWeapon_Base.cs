using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon_Base : MonoBehaviour
{
    // 무기 활성화 종류(무기 교체시 사용)
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;
    public bool isKnife;

    // 공격 여부 판단
    protected bool isAttack = false;
    protected bool isSwing = false;

    // Attack
    protected int weaponDamage = 5;

    protected float weaponRange;
    protected float attackDelay;
    protected float attackDelayA;
    protected float attackDelayB;

    // Animation
    public Animator anim = null;

    protected RaycastHit hitInfo;

    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }
    protected IEnumerator AttackCoroutine()
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
    protected virtual IEnumerator HitCoroutine()
    {
        yield return null;
    }

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, weaponRange, LayerMask.GetMask("Resource")))
        {
            return true;
        }
        return false;
    }
}
