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
    [SerializeField]
    private Transform        _grenadeSpawnPoint;

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
    private bool             _isInspecting;
    private bool             _isgrenadeThrowed;
    public  float            _knifeDelay = 1;
    private bool             _knifeUse;

    [Header("Prefabs")]
    public Transform         grenadePrefab;

    [Header("Grenade Settings")]
    public float mainGrenadeSpawnDelay = 0.35f;
    public float grenadeThrowDelay     = 5f;
    public int   grenadeCount          = 3;

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

    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Q) && !_knifeUse) 
        {
            _animator.Play ("Knife Attack 1", 0, 0f);
            StartCoroutine("KnifeDelay");
        }
        if (Input.GetKeyDown (KeyCode.F) && !_knifeUse) 
        {
            _animator.Play ("Knife Attack 2", 0, 0f);
            StartCoroutine("KnifeDelay");
        }
        if (Input.GetKeyDown (KeyCode.G) && !_isInspecting && grenadeCount > 0) 
        {
            StartCoroutine (GrenadeSpawnDelay ());
            _animator.Play("GrenadeThrow", 0, 0.0f);
        }
    }
    
    private IEnumerator KnifeDelay()
    {
        _knifeUse = true;
        yield return new WaitForSeconds(_knifeDelay);
        _knifeUse = false;
    }
    
    public override void StartWeaponAction(int type = 0)
    {
        // ?????? ?? ???? ???? X
        if (_isReload == true) return;
        // ???? ?????? ???? ???? X
        if (_isModeChange == true) return;

        // ???? ?????? ?????? ????
        if (type == 0)
        {
            // ???? ????
            if (_weaponSetting._isAutomaticAttack == true)
            {
                _isAttack = true;
                StartCoroutine("OnAttackLoop");
            }
            // ???? ????
            else
            {
                OnAttack();
            }
        }
        // ?????? ?????? ?????? ???? ????
        else
        {
            // ?????????? ???????? X
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
            if (_animator._MoveSpeed > 0.5f) return;

                _lastAttackTime = Time.time;

            if (_weaponSetting._currentAmmo <= 0) return;
            
            _weaponSetting._currentAmmo--;
            _onAmmoEvent.Invoke(_weaponSetting._currentAmmo, _weaponSetting._maxAmmo);

            // ???? ?????????? (?????? ???? AimFire, Fire)
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

        Vector3 attackDirection = (targetPoint - _impactSpawnPoint.position).normalized;
        if (Physics.Raycast(_impactSpawnPoint.position, attackDirection, out hit, _weaponSetting._attackDistance))
        {
            _impactMemoryPool.SpawnImpact(hit);

            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("?? ????");
                hit.transform.GetComponent<InteractionObject>().TakeDamage(_weaponSetting._damage);
            }
            else if (hit.transform.CompareTag("ExplosiveBarrel"))
            {
                hit.transform.GetComponent<InteractionObject>().TakeDamage(_weaponSetting._damage);
            }

            else if (hit.transform.CompareTag("Animals"))
            {
                hit.transform.GetComponent<InteractionObject>().TakeDamage(_weaponSetting._damage);
            }

            else if (hit.transform.CompareTag("Skill"))
            {
                hit.transform.GetComponent<InteractionObject>().TakeDamage(_weaponSetting._damage);
            }
        }
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

    private IEnumerator GrenadeSpawnDelay ()
    {
        _isInspecting = true;
        yield return new WaitForSeconds (mainGrenadeSpawnDelay);
        Instantiate(grenadePrefab, _grenadeSpawnPoint.transform.position, _grenadeSpawnPoint.rotation);
        grenadeCount--;
        yield return new WaitForSeconds (grenadeThrowDelay);
        _isInspecting = false;
    }
    private void ResetVariables()
    {
        _isReload         = false;
        _isAttack         = false;
        _isModeChange     = false;
        _isgrenadeThrowed = false;
    }


    
    public override void IncreaseMagazineMain(int magazineMain)
    {
        _weaponSetting._currentMagazine =
            CurrentMagazine + magazineMain > MaxMagazine ? MaxMagazine : CurrentMagazine + magazineMain;
        
        _onMagazineEvent.Invoke(CurrentMagazine);
    }
    
    public override void IncreaseMagazineSub(int magazineSub)
    {
        
    }
}
