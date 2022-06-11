using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerControllerHSW : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField] 
    private KeyCode _keyCodeRun    = KeyCode.LeftShift;
    [SerializeField] 
    private KeyCode _keyCodeJump   = KeyCode.Space;
    [SerializeField]
    private KeyCode _keyCodeReload = KeyCode.R;

    private KeyCode _inventory5    = KeyCode.Alpha5;
    private KeyCode _inventory6    = KeyCode.Alpha6;
    private KeyCode _inventory7    = KeyCode.Alpha7;
    private KeyCode _inventory8    = KeyCode.Alpha8;



    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _audioClipWalk;
    [SerializeField]
    private AudioClip _audioClipRun;

    private int               _keyNumber;
    private RotateToMouseHSW  _rotateToMouseHsw;
    private MovementHSW       _movement;
    private Status            _status;
    private AudioSource       _audioSource;
    private WeaponBase        _weapon;
    private Inventory         _inventory;

    public Status Status     => _status;
    public WeaponBase Weapon => _weapon;
    
    private void Awake()
    {
        Cursor.visible    = false;
        Cursor.lockState  = CursorLockMode.Locked;
        
        _rotateToMouseHsw = GetComponent<RotateToMouseHSW>();
        _movement         = GetComponent<MovementHSW>();
        _status           = GetComponent<Status>();
        _audioSource      = GetComponent<AudioSource>();;
        _weapon           = GetComponentInChildren<WeaponAssultRifle>();
        _inventory        = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateWeaponAction();
        UpdateInventoryUseCheck();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        _rotateToMouseHsw.UpdateRotate(mouseX, mouseY);
    }

    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
        {
            bool isRun = false;
            
            if (z > 0) isRun     = Input.GetKey(_keyCodeRun);
            
            _movement._MoveSpeed = isRun == true ? _status.RunSpeed : _status.WalkSpeed;
            _weapon.Animator._MoveSpeed = isRun == true ? 1 : 0.5f;
            _audioSource.clip    = isRun == true ? _audioClipRun : _audioClipWalk;
        }
        else
        {
            _movement._MoveSpeed        = 0;
            _weapon.Animator._MoveSpeed = 0;

            if (_audioSource.isPlaying == true)
            {
                _audioSource.Stop();
            }
        }
        
        _movement.MoveTo(new Vector3(x, 0, z));
    }

    private void UpdateJump()
    {
        if (Input.GetKeyDown(_keyCodeJump))
        {
            _movement.Jump();
        }
    }

    private void UpdateWeaponAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _weapon.StopWeaponAction();
        }
        if (Input.GetMouseButtonDown(1))
        {
            _weapon.StartWeaponAction(1);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _weapon.StopWeaponAction(1);
        }
        if (Input.GetKeyDown(_keyCodeReload))
        {
            _weapon.StartReload();
        }
    }

    private void UpdateInventoryUseCheck()
    {
        if (Input.GetKeyDown(_inventory5))
            _inventory.ItemUseCheck(5);
        if (Input.GetKeyDown(_inventory6))
            _inventory.ItemUseCheck(6);
        if (Input.GetKeyDown(_inventory7))
            _inventory.ItemUseCheck(7);
        if (Input.GetKeyDown(_inventory8))
            _inventory.ItemUseCheck(8);
    }
    
    public void TakeDamage(int damage)
    {
        bool isDie = _status.DecreaseHP(damage);

        if (isDie == true)
        {
            Debug.Log("GameOver");
        }
    }

    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        _weapon = newWeapon;
    }



}
