using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRevolver : WeaponBase
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject       _muzzleFlashEffect; // 총구 이펙트
    
    [Header("Spawn Points")]
    [SerializeField]
    private Transform        _bulletSpawnPoint;  // 총알 생성 위치
    
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip        _audioClipFire;     // 발사 사운드
    [SerializeField]
    private AudioClip        _audioClipReload;   // 장전 사운드

    private ImpactMemoryPool _impactMemoryPool;  // 공격 효과 관리
    private Camera           _mainCamera;        // RayCast

    private void OnEnable()
    {
        _muzzleFlashEffect.SetActive(false);
        
        _onMagazineEvent.Invoke(_weaponSetting._currentMagazine);
        
        _onAmmoEvent.Invoke(_weaponSetting._currentAmmo, _weaponSetting._maxAmmo);

        ResetVariables();
    }

    private void Awake()
    {
        base.Setup();
        
        _impactMemoryPool               = GetComponent<ImpactMemoryPool>();
        _mainCamera                     = Camera.main;

        _weaponSetting._currentMagazine = _weaponSetting._maxMagazine;
        _weaponSetting._currentAmmo     = _weaponSetting._maxAmmo;
    }

    public override void StartWeaponAction(int type = 0)
    {
        if (type == 0 && _isAttack == false && _isReload == false)
            OnAttack();
    }

    public override void StopWeaponAction(int type = 0)
    {
        _isAttack = false;
    }

    public override void StartReload()
    {
        if (_isReload == true || _weaponSetting._currentMagazine <= 0) return;
        
        StopWeaponAction();

        StartCoroutine("OnReload");
    }
    
    public override void IncreaseMagazineSub(int magazineSub)
    {
        _weaponSetting._currentMagazine =
            CurrentMagazine + magazineSub > MaxMagazine ? MaxMagazine : CurrentMagazine + magazineSub;
        
        _onMagazineEvent.Invoke(CurrentMagazine);
    }

    public override void IncreaseMagazineMain(int magazineMain)
    {
        
    }

    private void OnAttack()
    {
        if (Time.time - _lastAttackTime > _weaponSetting._attackRate)
        {
            if (_animator._MoveSpeed > 0.5f) return;

            _lastAttackTime = Time.time;

            if (_weaponSetting._currentAmmo <= 0) return;
            
            _weaponSetting._currentAmmo--;
            _onAmmoEvent.Invoke(_weaponSetting._currentAmmo, _weaponSetting._maxAmmo);
            
            _animator.Play("Fire", -1, 0);

            StartCoroutine("OnMuzzleFlashEffect");
            
            PlaySound(_audioClipFire);

            TwoStepRaycast();
        }
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        _muzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(_weaponSetting._attackRate * 0.3f);
        
        _muzzleFlashEffect.SetActive(false);
    }

    private IEnumerator OnReload()
    {
        _isReload  = true;

        _animator.OnReload();
        PlaySound(_audioClipReload);

        while (true)
        {
            if (_audioSource.isPlaying == false && _animator.CurrentAnimationIs("Movement"))
            {
                _isReload  = false;

                _weaponSetting._currentMagazine--;
                _onMagazineEvent.Invoke(_weaponSetting._currentMagazine);
                
                _weaponSetting._currentAmmo = _weaponSetting._maxAmmo;
                _onAmmoEvent.Invoke(_weaponSetting._currentAmmo, _weaponSetting._maxAmmo);
                
                yield break;
            }

            yield return null;
        }
    }
    
    private void TwoStepRaycast()
    {
        Ray        ray;
        RaycastHit hit;
        Vector3    targetPoint = Vector3.zero;

        ray = _mainCamera.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out hit, _weaponSetting._attackDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * _weaponSetting._attackDistance;
        }

        Vector3 attackDirection = (targetPoint - _bulletSpawnPoint.position).normalized;
        if (Physics.Raycast(_bulletSpawnPoint.position, attackDirection, out hit, _weaponSetting._attackDistance))
        {
            _impactMemoryPool.SpawnImpack(hit);

            if (hit.transform.CompareTag("Enemy"))
            {
                //적 체력 스테이터스에 TakeDamege함수 넣을것
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(_weaponSetting._damage);
            }
            else if (hit.transform.CompareTag("ExplosiveBarrel"))
            {
                hit.transform.GetComponent<InteractionObject>().TakeDamage(_weaponSetting._damage);
            }
        }
    }
    
    private void ResetVariables()
    {
        _isReload = false;
        _isAttack = false;
    }
    
}
