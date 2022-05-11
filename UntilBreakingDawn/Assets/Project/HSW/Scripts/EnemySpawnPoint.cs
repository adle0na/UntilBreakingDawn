using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField]
    private float        _fadeSpeed = 4;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine("OnFadeEffect");
    }

    private void OnDisable()
    {
        StopCoroutine("OnFadeEffect");
    }

    private IEnumerator OnFadeEffect()
    {
        while (true)
        {
            Color color = _meshRenderer.material.color;
            color.a     = Mathf.Lerp(1, 0, Mathf.PingPong(Time.time * _fadeSpeed, 1));
            _meshRenderer.material.color = color;

            yield return null;
        }
    }
}
