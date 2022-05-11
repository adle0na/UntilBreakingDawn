using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon_Knife : CloseWeapon_Base
{
    IHit hit = null;

    // ���� Start�� OnEnable�� �ٲܰ�
    private void Start()
    {
        if (isKnife)
        {
            base.weaponRange = 3.0f;
            base.attackDelay = 1.7f;
            base.attackDelayA = 0.5f;
            base.attackDelayB = 0.1f;
        }
    }
    void Update()
    {
        if (isKnife)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                // Damage ����
                switch (hitInfo.transform.name)
                {
                    case "Tree":
                        base.weaponDamage = 5;
                        Debug.Log($"{hitInfo.transform.name} : {weaponDamage}");
                        break;
                    case "TestRock":
                        base.weaponDamage = 10;
                        Debug.Log($"{hitInfo.transform.name} : {weaponDamage}");
                        break;
                }
                // ���ݹ��� ������Ʈ ���� OnHit�Լ� ����
                hit = hitInfo.collider.gameObject.GetComponent<IHit>();
                hit.OnHit();
            }
            // IHit �����Լ� �ۼ�
            yield return null;
        }
    }
}