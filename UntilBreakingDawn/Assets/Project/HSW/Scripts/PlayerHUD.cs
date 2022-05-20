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
    private WeaponAssultRifle _weapon;            // ���� ������ ��µǴ� ����
    [SerializeField]
    private Status            _status;
    
    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI   _textWeaponName;     // ���� �̸�
    [SerializeField]
    private Image             _imageWeaponIcon;    // ���� ������
    [SerializeField]
    private Sprite[]          _spriteWeaponIcons;  // ���� ������ Sprite�迭 

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI  _textAmmo;           // ����/�ִ� ź �� ��� Text

    [Header("Magazine")]
    [SerializeField]
    private GameObject       _magazineUIPrefab;   // źâ UI Prefab
    [SerializeField]
    private Transform        _magzineParent;      // źâ UI ��ġ�Ǵ� Panel
    
    private List<GameObject> _magazineList;       // źâ UI ����Ʈ

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
