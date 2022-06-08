using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : MonoBehaviour, IHit
{
    private static ResourceObject instance = null;

    // 0 = null, 1 = tree, 2 = rock
    private int _type = 0;
    public int Type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            Debug.Log(_type);
        }
    }
    private void Awake()
    {
        instance = this;
    }

    public static ResourceObject Inst
    {
        get
        {
            return instance;
        }
    }

    public int weaponDamage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void OnHit()
    {
        throw new System.NotImplementedException();
    }
}
