using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_K : MonoBehaviour
{
    public static SoundManager_K instance;

    [Header("SondManager_K")]
    public SoundManager_KSH soundManager_KSH;

    private void Awake()
    {
        if (instance != this)
            instance = this;
    }
}
