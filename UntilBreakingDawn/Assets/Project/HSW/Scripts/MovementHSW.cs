using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementHSW : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private Vector3                _moveForce;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;

    public float _MoveSpeed
    {
        set => _moveSpeed = Mathf.Max(0, value);
        get => _moveSpeed;
    }
    
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!_characterController.isGrounded)
        {
            _moveForce.y += _gravity * Time.deltaTime;
        }
        
        _characterController.Move(_moveForce * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        _moveForce = new Vector3(direction.x * _moveSpeed, _moveForce.y, direction.z * _moveSpeed);
    }

    public void Jump()
    {
        if (_characterController.isGrounded)
        {
            _moveForce.y = _jumpForce;
        }
    }
}
