using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }

public class WeaponAssultRifle : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent             _onAmmoEvent     = new AmmoEvent();

    [HideInInspector]
    public MagazineEvent         _OnMagazineEvent = new MagazineEvent();
    
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject           _muzzleFlashEffect;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform            _casingSpawnPoints;
    [SerializeField]
    private Transform            _impactSpawnPoint;
    
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip            _audioClipTakeOutWeapon;
    [SerializeField]
    private AudioClip            _audioClipFire;

    [SerializeField]
    private AudioClip            _audioClipReload;

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting        _weaponSetting;
    
    private float                _lastAttackTime = 0;
    private bool                 _isReload = false;
    
    private AudioSource          _audioSource;
    private PlayerAnimControlHSW _animator;
    private CasingMemoryPool     _casingMemoryPool;
    private ImpactMemoryPool     _impactMemoryPool;
    private Camera               _mainCamera;

    public WeaponName WeaponName      => _weaponSetting._WeaponName;
    public int        CurrentMagazine => _weaponSetting._currentMagazine;
    public int        maxMagazine     => _weaponSetting._maxMagazine;

    private void Awake()
    {
        _audioSource      = GetComponent<AudioSource>();
        _animator         = GetComponentInParent<PlayerAnimControlHSW>();
        _casingMemoryPool = GetComponent<CasingMemoryPool>();
        _impactMemoryPool = GetComponent<ImpactMemoryPool>();
        _mainCamera       = Camera.main;

        _weaponSetting._currentMagazine = _weaponSetting._maxMagazine;
        _weaponSetting._currentAmmo = _weaponSetting._maxAmmo;
    }

    private void OnEnable()
    {
        PlaySound(_audioClipTakeOutWeapon);
        _muzzleFlashEffect.SetActive(false);
        
        _OnMagazineEvent.Invoke(_weaponSetting._currentMagazine);
        _onAmmoEvent.Invoke(_weaponSetting._currentAmmo, _weaponSetting._maxAmmo);
    }

    public void StartWeaponAction(int type = 0)
    {
        if (_isReload == true) return;
        
        if (type == 0)
        {
            if (_weaponSetting._isAutomaticAttack == true)
            {
                StartCoroutine("OnAttackLoop");
            }
            else
            {
                OnAttack();
            }
        }
    }

    public void StopWeaponAction(int type = 0)
    {
        if (type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
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

            _animator.Play("Fire", -1, 0);

            StartCoroutine("OnMuzzleFlashEffect");
            
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
        _isReload = true;
        
        _animator.OnReload();
        PlaySound(_audioClipReload);

        while (true)
        {
            if (_audioSource.isPlaying == false && _animator.CurrentAnimationIs("Movement"))
            {
                _isReload = false;

                _weaponSetting._currentMagazine--;
                _OnMagazineEvent.Invoke(_weaponSetting._currentMagazine);
                
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
        }
        Debug.DrawRay(_impactSpawnPoint.position, attackDirection*_weaponSetting._attackDistance, Color.blue);
    }
    
    private void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
