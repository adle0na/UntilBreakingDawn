using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CloseWeapon_Base _Base = null;
    public CloseWeapon_Base Base
    {
        get
        {
            return _Base;
        }
        set
        {
            _Base = value;
        }
    }

    private static GameManager instance = null;
    public static GameManager Inst
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            instance.Initialize();
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Initialize()
    {
        _Base = FindObjectOfType<CloseWeapon_Base>();
    }
}
