using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImpactType {Normal = 0, obstacle, }

public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _impactPrefab;

    private MemoryPool[] _memoryPool;

    private void Awake()
    {
        _memoryPool = new MemoryPool[_impactPrefab.Length];
        for (int i = 0; i < _impactPrefab.Length; ++i)
        {
            _memoryPool[i] = new MemoryPool(_impactPrefab[i]);
        }
    }

    public void SpawnImpack(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Normal"))
        {
            OnSpawnImpack(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (hit.transform.CompareTag("Obstacle"))
        {
            OnSpawnImpack(ImpactType.obstacle, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    public void OnSpawnImpack(ImpactType type, Vector3 position, Quaternion rotation)
    {
        GameObject item = _memoryPool[(int) type].ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.GetComponent<Impact>().Setup(_memoryPool[(int)type]);
    }
}
