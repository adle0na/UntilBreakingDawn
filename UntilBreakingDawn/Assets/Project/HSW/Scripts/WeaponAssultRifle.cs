using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WeaponAssultRifle : WeaponBase
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject       _muzzleFlashEffect;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform        _casingSpawnPoints;
    [SerializeField]
    private Transform        _impactSpawnPoint;
    
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip        _audioClipTakeOutWeapon;
    [SerializeField]
    private AudioClip        _audioClipFire;
    [SerializeField]
    private AudioClip        _audioClipReload;

    [Header("Aim UI")]
    [SerializeField]
    private Image            _imageAim;
    
    private bool             _isModeChange   = false;
    private float            _defaultModeFOV = 60;
    private float            _aimModeFOV     = 30;
    
    private CasingMemoryPool _casingMemoryPool;
    private ImpactMemoryPool _impactMemoryPool;
    private Camera           _mainCamera;

    private void Awake()
    {
        base.Setup();

        _casingMemoryPool = GetComponent<CasingMemoryPool>();
        _impactMemoryPool = GetComponent<ImpactMemoryPool>();
        _mainCamera = Camera.main;

        _weaponSetting._currentMagazine = _weaponSetting._maxMagazine;
        _weaponSetting._currentAmmo = _weaponSetting._maxAmmo;

    }

    private void OnEnable()
    {
        PlaySound(_audioClipTakeOutWeapon);
        _muzzleFlashEffect.SetActive(false);
        
        _onMagazineEvent.Invoke(_weaponSetting._currentMagazine);
        _onAmmoEvent.Invoke(_weaponSetting._currentAmmo, _weaponSetting._maxAmmo);

        ResetVariables();
    }

    public override void StartWeaponAction(int type = 0)
    {
        // 재장전 중 무기 액션 X
        if (_isReload == true) return;
        // 모드 전환중 무기 액션 X
        if (_isModeChange == true) return;
        
        // 왼쪽 마우스 클릭시 공격
        if (type == 0)
        {
            // 연속 공격
            if (_weaponSetting._isAutomaticAttack == true)
            {
                _isAttack = true;
                StartCoroutine("OnAttackLoop");
            }
            // 단발 공격
            else
            {
                OnAttack();
            }
        }
        // 마우스 오른쪽 클릭시 모드 전환
        else
        {
            // 공격중일때 모드전환 X
            if (_isAttack == true) return;

            StartCoroutine("OnModeChange");
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
        if (type == 0)
        {
            _isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }

    public override void StartReload()
    {
        if (_isReload == true || _weaponSetting._currentMagazine <= 0) return;
        
        StopWeaponAction();

        StartCoroutine("OnReload");
    }
    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();

            yield return null;
        }
    }

    public void OnAttack()
    {
        if (Time.time - _lastAttackTime > _weaponSetting._attackRate)
        {
            if (_animator._MoveSpeed > 0.5f)
            {
                return;
            }

            _lastAttackTime = Time.time;

            if (_weaponSetting._currentAmmo <= 0)
            {
                return;
            }

            _weaponSetting._currentAmmo--;
            _onAmmoEvent.Invoke(_weaponSetting._currentAmmo, _weaponSetting._maxAmmo);

            // 무기 애니메이션 (모드에 따라 AimFire, Fire)
            string animation = _animator.AimModeIs == true ? "AimFire" : "Fire";
            _animator.Play(animation, -1, 0);

            if (_animator.AimModeIs == false) StartCoroutine("OnMuzzleFlashEffect");

            PlaySound(_audioClipFire);
            
            _casingMemoryPool.SpawnCasing(_casingSpawnPoints.position, transform.right);

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
        Debug.DrawRay(ray.origin, ray.direction * _weaponSetting._attackDistance, Color.red);

        Vector3 attackDirection = (targetPoint - _impactSpawnPoint.position).normalized;
        if (Physics.Raycast(_impactSpawnPoint.position, attackDirection, out hit, _weaponSetting._attackDistance))
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
        Debug.DrawRay(_impactSpawnPoint.position, attackDirection*_weaponSetting._attackDistance, Color.blue);
    }

    private IEnumerator OnModeChange()
    {
        float current = 0;
        float percent = 0;
        float time = 0.35f;

        _animator.AimModeIs = !_animator.AimModeIs;
        _imageAim.enabled   = !_imageAim.enabled;

        float start = _mainCamera.fieldOfView;
        float end = _animator.AimModeIs == true ? _aimModeFOV : _defaultModeFOV;

        _isModeChange = true;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            _mainCamera.fieldOfView = Mathf.Lerp(start, end, percent);

            yield return null;
        }
        _isModeChange = false;
    }

    private void ResetVariables()
    {
        _isReload     = false;
        _isAttack     = false;
        _isModeChange = false;
    }

    public void IncreaseMagazine(int magazine)
    {
        _weaponSetting._currentMagazine =
            CurrentMagazine + magazine > MaxMagazine ? MaxMagazine : CurrentMagazine + magazine;
        
        _onMagazineEvent.Invoke(CurrentMagazine);
    }
}
