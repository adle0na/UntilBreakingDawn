using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Casing : MonoBehaviour
{
    [SerializeField] private float       _deactiveTime = 5.0f;
    [SerializeField] private float       _casingSpin = 1.0f;
    [SerializeField] private AudioClip[] _audioClips;

    private Rigidbody   _rigidbody3D;
    private AudioSource _audioSource;
    private MemoryPool  _memoryPool;

    public void Setup(MemoryPool pool, Vector3 direction)
    {
        _rigidbody3D = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _memoryPool  = pool;

        _rigidbody3D.velocity        = new Vector3(direction.x, 1.0f, direction.z);
        _rigidbody3D.angularVelocity = new Vector3(Random.Range(-_casingSpin, _casingSpin),
                                                   Random.Range(-_casingSpin, _casingSpin),
                                                   Random.Range(-_casingSpin, _casingSpin));

        StartCoroutine("DeactivateAfterTime");
    }

    private void OnCollisionEnter(Collision collision)
    {
        int index = Random.Range(0, _audioClips.Length);
        _audioSource.clip = _audioClips[index];
        _audioSource.Play();
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(_deactiveTime);
        
        _memoryPool.DeactivatePoolItem(this.gameObject);
    }
}
