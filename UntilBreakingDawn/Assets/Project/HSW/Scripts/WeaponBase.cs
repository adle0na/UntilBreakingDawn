using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum WeaponType { Main=0, Sub, Axe, Pickaxe}

    [System.Serializable] // 탄수 이벤트 처리
    public class AmmoEvent: UnityEngine.Events.UnityEvent<int, int> { }
    [System.Serializable] // 탄창 이벤트 처리
    public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }

    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("WeaponBase")]
        [SerializeField]
        public WeaponType              _weaponType;

        [SerializeField]
        protected WeaponSetting        _weaponSetting;

        protected float                _lastAttackTime = 0;
        protected bool                 _isReload       = false;
        protected bool                 _isAttack       = false;
        protected AudioSource          _audioSource;
        protected PlayerAnimControlHSW _animator;
        public    bool                 _isInspecting;

        [HideInInspector]
        public AmmoEvent               _onAmmoEvent     = new AmmoEvent();
        [HideInInspector]
        public MagazineEvent           _onMagazineEvent = new MagazineEvent();
        public PlayerAnimControlHSW    Animator        => _animator;
        public WeaponName              WeaponName      => _weaponSetting._WeaponName;
        public int                     CurrentMagazine => _weaponSetting._currentMagazine;
        public int                     MaxMagazine     => _weaponSetting._maxMagazine;

        public abstract void StartWeaponAction(int type = 0);
        public abstract void StopWeaponAction(int type = 0);
        public abstract void IncreaseMagazineMain(int magazine);
        public abstract void IncreaseMagazineSub(int magazine);
        public abstract void StartReload();

        protected void PlaySound(AudioClip clip)
        {
            _audioSource.Stop();
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        protected void Setup()
        {
            _audioSource = GetComponent<AudioSource>();
            _animator    = GetComponent<PlayerAnimControlHSW>();
        }
    }

