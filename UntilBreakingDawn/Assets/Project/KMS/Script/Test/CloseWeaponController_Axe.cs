using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeaponController_Axe : CloseWeaponController_Base
{
    // 활성화 여부.
    public static bool isThisWeaponActivate = true;

    void Update()
    {
        if (isThisWeaponActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
                switch (hitInfo.transform.name)
                {
                    case "TestTree":
                        currentCloseWeapon.damage = 10;
                        break;
                    case "TestRock":
                        currentCloseWeapon.damage = 2;
                        break;
                }
            }
            yield return null;
        }
    }

    //public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    //{
    //    base.CloseWeaponChange(_closeWeapon);
    //    isActivate = true;
    //}
}
