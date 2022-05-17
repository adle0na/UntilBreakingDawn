using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroyBytime : MonoBehaviour
{
    private ParticleSystem _particle;

    private void Awake()
    {
       _particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_particle.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}
