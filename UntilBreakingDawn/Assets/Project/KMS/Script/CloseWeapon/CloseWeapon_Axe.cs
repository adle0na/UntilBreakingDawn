using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon_Axe : CloseWeapon_Base
{
    IHit hit = null;
    // 이후 Start를 OnEnable로 바꿀것
    private void OnEnable()
    {
        if (isAxe)
        {
            base.weaponRange = 3.0f;
            base.attackDelay = 1.0f;
            base.attackDelayA = 0.2f;
            base.attackDelayB = 0.2f;
        }
    }
    void Update()
    {
        if (isAxe)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                // Damage 변경
                if (hitInfo.transform.CompareTag("Tree"))
                {
                    base.weaponDamage = 10;
                    Debug.Log($"{hitInfo.transform.name} : {weaponDamage}");
                }
                else if(hitInfo.transform.CompareTag("Rock"))
                {
                    base.weaponDamage = 5;
                    Debug.Log($"{hitInfo.transform.name} : {weaponDamage}");
                }
                else
                {
                    Debug.Log("Error");
                }
                // 공격받은 오브젝트 내부 OnHit함수 실행
                hit = hitInfo.collider.gameObject.GetComponent<IHit>();
                hit.weaponDamage = weaponDamage;
                hit.OnHit();
            }
            // IHit 실행함수 작성
            yield return null;
        }
    }
}
