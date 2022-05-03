using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

public class WeaponAssultRifle : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent             _onAmmoEvent = new AmmoEvent();
    
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject           _muzzleFlashEffect;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform            _casingSpawnPoints;
    
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

    public WeaponName WeaponName => _weaponSetting._WeaponName;
    
    private void Awake()
    {
        _audioSource      = GetComponent<AudioSource>();
        _animator         = GetComponentInParent<PlayerAnimControlHSW>();
        _casingMemoryPool = GetComponent<CasingMemoryPool>();

        _weaponSetting._currentAmmo = _weaponSetting._maxAmmo;
    }

    private void OnEnable()
    {
        PlaySound(_audioClipTakeOutWeapon);
        _muzzleFlashEffect.SetActive(false);
        
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
        if (_isReload == true) return;
        
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

                _weaponSetting._currentAmmo = _weaponSetting._maxAmmo;
                _onAmmoEvent.Invoke(_weaponSetting._currentAmmo, _weaponSetting._maxAmmo);
                
                yield break;
            }

            yield return null;
        }
    }
    
    private void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
