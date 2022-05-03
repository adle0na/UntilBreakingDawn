using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponAssultRifle : MonoBehaviour
{
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

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting        _weaponSetting;
    private float                _lastAttackTime = 0;
    
    private AudioSource          _audioSource;
    private PlayerAnimControlHSW _animator;
    private CasingMemoryPool     _casingMemoryPool;
    
    private void Awake()
    {
        _audioSource      = GetComponent<AudioSource>();
        _animator         = GetComponentInParent<PlayerAnimControlHSW>();
        _casingMemoryPool = GetComponent<CasingMemoryPool>();
    }

    private void OnEnable()
    {
        PlaySound(_audioClipTakeOutWeapon);
        _muzzleFlashEffect.SetActive(false);
    }

    public void StartWeaponAction(int type = 0)
    {
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
    
    private void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
