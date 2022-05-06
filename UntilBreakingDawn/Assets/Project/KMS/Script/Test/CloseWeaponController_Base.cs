using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController_Base : MonoBehaviour
{
    // 현재 장착된 Hand형 타입 무기.
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // 공격중??
    protected bool isAttack = false;
    protected bool isSwing = false;

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
        currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;


        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }


    // 미완성 = 추상 코루틴.
    protected abstract IEnumerator HitCoroutine();


    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }

    // 완성 함수이지만, 추가 편집한 함수.
    //public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    //{
    //    if (WeaponManager.currentWeapon != null)
    //        WeaponManager.currentWeapon.gameObject.SetActive(false);

    //    currentCloseWeapon = _closeWeapon;
    //    WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
    //    WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

    //    currentCloseWeapon.transform.localPosition = Vector3.zero;
    //    currentCloseWeapon.gameObject.SetActive(true);
    //}
}
