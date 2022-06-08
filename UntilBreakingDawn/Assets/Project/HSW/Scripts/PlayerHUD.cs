using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class PlayerHUD : MonoBehaviour
{
    private WeaponBase       _weapon;             // 현재 무기 정보
    
    [Header("Components")]
    [SerializeField]
    private Status           _status;             // 플레이어 상태 수치
    
    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI  _textWeaponName;     // 무기 이름
    [SerializeField]
    private Image            _imageWeaponIcon;    // 무기 아이콘
    [SerializeField]
    private Sprite[]         _spriteWeaponIcons;  // 무기 아이콘 Sprite배열

    [SerializeField]
    private Vector2[]        _sizeWeaponIcons;

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI  _textAmmo;           // 현재/최대 탄 수 출력 Text

    [Header("Magazine")]
    [SerializeField]
    private GameObject       _magazineUIPrefab;   // 탄창 UI Prefab
    [SerializeField]
    private Transform        _magzineParent;      // 탄창 UI 배치되는 Panel
    [SerializeField]
    private int              _maxMagazineCount;
    
    private List<GameObject> _magazineList;       // 탄창 UI 리스트

    [Header("HP & BloodScreen UI")]
    [SerializeField]
    private TextMeshProUGUI  _textHP;
    [SerializeField]
    private TextMeshProUGUI  _textHungry;
    
    [SerializeField]
    private Image            _BloodScreen;
    [SerializeField]
    private AnimationCurve   _curveBloodScreen;
    private void Awake()
    {
        _status._onHPEvent.AddListener(UpdateHPHUD);
    }

    private void Update()
    {
        _status.DecreaseHungry(1);
    }

    public void SetupAllWeapons(WeaponBase[] weapons)
    {
        SetupMagazine();

        for (int i = 0; i < weapons.Length; ++i)
        {
            weapons[i]._onAmmoEvent.AddListener(UpdateAmmoHUD);
            weapons[i]._onMagazineEvent.AddListener(UpdateMagazineHUD);
        }
    }

    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        _weapon = newWeapon;

        SetupWeapon();
    }

    private void SetupWeapon()
    {
        _textWeaponName.text = _weapon.WeaponName.ToString();
        _imageWeaponIcon.sprite = _spriteWeaponIcons[(int) _weapon.WeaponName];
        _imageWeaponIcon.rectTransform.sizeDelta = _sizeWeaponIcons[(int) _weapon.WeaponName];
    }
    
    private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
    {
        _textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
    }

    private void SetupMagazine()
    {
        _magazineList = new List<GameObject>();
        for (int i = 0; i < _maxMagazineCount; ++i)
        {
            GameObject clone = Instantiate(_magazineUIPrefab);
            clone.transform.SetParent(_magzineParent);
            clone.SetActive(false);
            
            _magazineList.Add(clone);
        }
    }

    private void UpdateMagazineHUD(int currentMagazine)
    {
        for (int i = 0; i < _magazineList.Count; ++i)
        {
            _magazineList[i].SetActive(false);
        }

        for (int i = 0; i < currentMagazine; ++i)
        {
            _magazineList[i].SetActive(true);
        }
    }

    private Coroutine _coroutine;
    
    private void UpdateHPHUD(int previous, int current)
    {
        _textHP.text = "HP " + current;

        if (previous <= current) return;

        if (previous - current > 0)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(OnBloodScreen());
        }
    }
    
    private void UpdateHungryHUD(int previous, int current)
    {
        _textHungry.text = "Hungry " + current;

        if (previous <= current) return;

        if (previous - current > 0)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(OnBloodScreen());
        }
    }

    private IEnumerator OnBloodScreen()
    {
        _BloodScreen.color = Color.white;
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime;

            Color color        = _BloodScreen.color;
            color.a            = Mathf.Lerp(color.a, 0, _curveBloodScreen.Evaluate(percent));
            _BloodScreen.color = color;
            
            yield return new WaitForFixedUpdate();
        }

        _coroutine = null;
    }
}
