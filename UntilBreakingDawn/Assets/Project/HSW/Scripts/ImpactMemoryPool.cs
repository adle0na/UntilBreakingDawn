using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImpactType {Default = 0, Wood = 1, Metal = 2, Stone = 3 }

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
        if (hit.transform.CompareTag("Dirt"))
        {
            OnSpawnImpack(ImpactType.Default, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (hit.transform.CompareTag("Wood"))
        {
            OnSpawnImpack(ImpactType.Wood, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (hit.transform.CompareTag("Metal"))
        {
            OnSpawnImpack(ImpactType.Metal, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (hit.transform.CompareTag("Stone"))
        {
            OnSpawnImpack(ImpactType.Stone, hit.point, Quaternion.LookRotation(hit.normal));
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
