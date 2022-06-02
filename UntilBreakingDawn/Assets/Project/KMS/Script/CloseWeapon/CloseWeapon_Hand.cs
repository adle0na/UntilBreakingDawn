using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon_Hand : CloseWeapon_Base
{
    IHit hit = null;

    // 이후 Start를 OnEnable로 바꿀것
    private void Start()
    {
        if (isHand)
        {
            base.weaponRange = 3.0f;
            base.attackDelay = 1.7f;
            base.attackDelayA = 0.5f;
            base.attackDelayB = 0.1f;
        }
    }
    void Update()
    {
        if (isHand)
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
                switch (hitInfo.transform.name)
                {
                    case "Tree":
                        base.weaponDamage = 5;
                        Debug.Log($"{hitInfo.transform.name} : {weaponDamage}");
                        break;
                    case "Rock":
                        base.weaponDamage = 2;
                        Debug.Log($"{hitInfo.transform.name} : {weaponDamage}");
                        break;
                }
                // 공격받은 오브젝트 내부 OnHit함수 실행
                hit = hitInfo.collider.gameObject.GetComponent<IHit>();
                hit.OnHit();
            }
            // IHit 실행함수 작성
            yield return null;
        }
    }
}
