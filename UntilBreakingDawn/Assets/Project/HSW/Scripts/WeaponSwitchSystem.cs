using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitchSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerControllerHSW _playerControllerHsw;
    [SerializeField]
    private PlayerHUD           _playerHUD;

    [SerializeField]
    private WeaponBase[]        weapons;

    private WeaponBase          _currentWeapon;
    private WeaponBase          _previousWeapon;

    private void Awake()
    {
        _playerHUD.SetupAllWeapons(weapons);

        for (int i = 0; i < weapons.Length; ++i)
        {
            if (weapons[i].gameObject != null)
                weapons[i].gameObject.SetActive(false);
        }

        SwitchingWeapon(WeaponType.Main);
    }

    private void Update()
    {
        UpdateSwitch();
    }

    private void UpdateSwitch()
    {
        if (!Input.anyKeyDown) return;

        int inputIndex = 0;
        if (int.TryParse(Input.inputString, out inputIndex) && (inputIndex > 0 && inputIndex < 5))
            SwitchingWeapon((WeaponType) (inputIndex - 1));
    }

    private void SwitchingWeapon(WeaponType weaponType)
    {
        if (weapons[(int) weaponType] == null)
            return;

        if (_currentWeapon != null)
            _previousWeapon = _currentWeapon;

        _currentWeapon = weapons[(int) weaponType];

        if (_currentWeapon == _previousWeapon)
            return;
        
        _playerControllerHsw.SwitchingWeapon(_currentWeapon);
        _playerHUD.SwitchingWeapon(_currentWeapon);
        
        if(_previousWeapon != null)
            _previousWeapon.gameObject.SetActive(false);
        _currentWeapon.gameObject.SetActive(true);
    }
}
