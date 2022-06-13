using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillManager : MonoBehaviour
{
    [Header("Skill Health")]
    [SerializeField]
    protected int _maxHP = 100;
    protected int currentHP;

    private void Awake()
    {
        currentHP = _maxHP;
    }

    public abstract void TakeDamage2(int damage);

}