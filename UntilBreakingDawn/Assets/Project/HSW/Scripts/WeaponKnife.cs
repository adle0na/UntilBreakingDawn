using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnife : WeaponBase
{
    [SerializeField] private WeaponKnifeCollider _weaponKnifeCollider;

    private void OnEnable()
    {
        _isAttack = false;
        
        _onMagazineEvent.Invoke(_weaponSetting._currentMagazine);
        _onAmmoEvent.Invoke(_weaponSetting._currentAmmo, _weaponSetting._maxAmmo);
    }

    private void Awake()
    {
        base.Setup();

        _weaponSetting._currentMagazine = _weaponSetting._maxMagazine;
        _weaponSetting._currentAmmo     = _weaponSetting._maxAmmo;
    }

    public override void StartWeaponAction(int type = 0)
    {
        if (_isAttack == true) return;

        if (_weaponSetting._isAutomaticAttack == true)
        {
            StartCoroutine("OnAttackLoop", type);
        }
        else
        {
            StartCoroutine("OnAttack", type);
        }
    }
    
    
    public override void StopWeaponAction(int type = 0)
    {
        _isAttack = false;
        StopCoroutine("OnAttackLoop");
    }
    
    private IEnumerator OnAttackLoop(int type)
    {
        while (true)
        {
            yield return StartCoroutine("OnAttack", type);
        }
    }

    private IEnumerator OnAttack(int type)
    {
        _isAttack = true;
        
        _animator.SetFlaot("attackType", type);
        _animator.Play("Fire", -1, 0);

        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (_animator.CurrentAnimationIs("Movement"))
            {
                _isAttack = false;
                yield break;
            }

            yield return null;
        }
    }

    public void StartWeaponKnifeCollider()
    {
        _weaponKnifeCollider.StartCollider(_weaponSetting._damage);
    }
    
    
    // 사용 안된 메서드
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
