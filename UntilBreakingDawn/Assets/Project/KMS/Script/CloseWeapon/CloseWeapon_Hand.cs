using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon_Hand : CloseWeapon_Base
{
    IHit hit = null;

    // ���� Start�� OnEnable�� �ٲܰ�
    private void OnEnable()
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
                // Damage ����
                if (hitInfo.transform.CompareTag("Tree"))
                {
                    base.weaponDamage = 2;
                    Debug.Log($"{hitInfo.transform.name} : {weaponDamage}");
                }
                else if (hitInfo.transform.CompareTag("Rock"))
                {
                    base.weaponDamage = 2;
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
}
