using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class PlayerHUD : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WeaponAssultRifle _weapon;            // 현재 정보가 출력되는 무기
    [SerializeField]
    private Status            _status;
    
    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI   _textWeaponName;     // 무기 이름
    [SerializeField]
    private Image             _imageWeaponIcon;    // 무기 아이콘
    [SerializeField]
    private Sprite[]          _spriteWeaponIcons;  // 무기 아이콘 Sprite배열 

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI  _textAmmo;           // 현재/최대 탄 수 출력 Text

    [Header("Magazine")]
    [SerializeField]
    private GameObject       _magazineUIPrefab;   // 탄창 UI Prefab
    [SerializeField]
    private Transform        _magzineParent;      // 탄창 UI 배치되는 Panel
    
    private List<GameObject> _magazineList;       // 탄창 UI 리스트

    [Header("HP & BloodScreen UI")]
    [SerializeField]
    private TextMeshProUGUI  _textHP;
    [SerializeField]
    private Image            _BloodScreen;
    [SerializeField]
    private AnimationCurve   _curveBloodScreen;
    private void Awake()
    {
        SetupWeapon();
        SetupMagazine();
        
        _weapon._onAmmoEvent.AddListener(UpdateAmmoHUD);
        _weapon._onMagazineEvent.AddListener(UpdateMagazineHUD);
        _status._onHPEvent.AddListener(UpdateHPHUD);
    }

    private void SetupWeapon()
    {
        _textWeaponName.text = _weapon.WeaponName.ToString();
        _imageWeaponIcon.sprite = _spriteWeaponIcons[(int)_weapon.WeaponName];
    }

    private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
    {
        _textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
    }

    private void SetupMagazine()
    {
        _magazineList = new List<GameObject>();
        for (int i = 0; i < _weapon.MaxMagazine; ++i)
        {
            GameObject clone = Instantiate(_magazineUIPrefab);
            clone.transform.SetParent(_magzineParent);
            clone.SetActive(false);
            
            _magazineList.Add(clone);
        }

        for (int i = 0; i < _weapon.CurrentMagazine; ++i)
        {
            _magazineList[i].SetActive(true);
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
