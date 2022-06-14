using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<float, float> { }
public class HungryEvent : UnityEngine.Events.UnityEvent<float, float> { }
public class Status : MonoBehaviour
{
    [HideInInspector]
    public HPEvent     _onHPEvent   = new HPEvent();

    [HideInInspector]
    public HungryEvent _hungryEvent = new HungryEvent();
    
    [Header("Walk, Run Speed")]
    [SerializeField]
    private float  _walkSpeed;
    [SerializeField]
    public float   _runSpeed;

    [Header("HP")]
    [SerializeField]
    private float  _maxHP = 100;
    [SerializeField]
    private float  _currentHP;
    public float WalkSpeed => _walkSpeed;
    public float RunSpeed  => _runSpeed;

    [Header("Hungry")]
    [SerializeField]
    private float _maxHungry = 100;
    public float _currentHungry;

    private void Awake()
    {
        _currentHP     = _maxHP;
        _currentHungry = _maxHungry;
    }

    private void Update()
    {
        if (_currentHungry >= 0)
        {
            DecreaseHungry(0.5f*Time.deltaTime);
        }
        
        if (_currentHungry == 0)
        {
            DecreaseHP(0.5f*Time.deltaTime);
        }
    }

    public bool DecreaseHP(float damage)
    {
        float previousHP = _currentHP;

        _currentHP = _currentHP - damage > 0 ? _currentHP - damage : 0;
        
        _onHPEvent.Invoke(previousHP, _currentHP);

        if (_currentHP == 0)
        {
            return true;
        }

        return false;
    }
    
    public bool DecreaseHungry(float time)
    {
        float previousHungry = _currentHungry;

        _currentHungry = _currentHungry - time > 0 ? _currentHungry - time : 0;
        
        _hungryEvent.Invoke(previousHungry, _currentHungry);

        if (_currentHungry == 0)
        {
            return true;
        }

        return false;
    }
    

    public void IncreaseHP(float hp)
    {
        float previousHP = _currentHP;
        
        _currentHP = _currentHP + hp > _maxHP ? _maxHP : _currentHP + hp;
        
        _onHPEvent.Invoke(previousHP, _currentHP);
    }
    
    public void IncreaseHungry(int hungry)
    {
        float previousHungry = _currentHungry;
        
        _currentHungry = _currentHungry + hungry > _maxHungry ? _maxHungry : _currentHungry + hungry;
        
        _hungryEvent.Invoke(previousHungry, _currentHungry);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Debug.Log("??????");
            DecreaseHP(50);
        }
        else if (other.tag == "Boulder")
        {
            DecreaseHP(50);
        }
    }
    public void HitByExplosion(int exDamage)
    {
        DecreaseHP(exDamage);
    }

    public void BossSkill()
    {
        DecreaseHP(100);
    }

    public void FireSkill(int fireDamage)
    {
        DecreaseHP(fireDamage);
    }
}
