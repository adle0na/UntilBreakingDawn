using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WeaponAssultRifle                _weapon;

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI                  _textWeaponName;
    [SerializeField] private Image           _imageWeaponIcon;
    [SerializeField] private Sprite[]        _spriteWeaponIcons;

    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI _textAmmo;

    private void Awake()
    {
        SetupWeapon();
        
        _weapon._onAmmoEvent.AddListener(UpdateAmmoHUD);
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
}
