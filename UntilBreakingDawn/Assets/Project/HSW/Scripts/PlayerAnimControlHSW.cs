using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerAnimControlHSW : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public float _MoveSpeed
    {
        set => _animator.SetFloat("movementSpeed", value);
        get => _animator.GetFloat("movementSpeed");
    }

    public void OnReload()
    {
        _animator.SetTrigger("onReload");
    }

    public bool AimModeIs
    {
        set => _animator.SetBool("isAimMode", value);
        get => _animator.GetBool("isAimMode");
    }
    
    public void Play(string stateName, int layer, float normalizedTime)
    {
        _animator.Play(stateName, layer, normalizedTime);
    }

    public bool CurrentAnimationIs(string name)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
