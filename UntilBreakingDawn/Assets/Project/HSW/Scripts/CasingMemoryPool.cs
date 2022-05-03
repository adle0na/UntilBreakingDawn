using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CasingMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _casingPrefabs;
    private MemoryPool _memoryPool;

    private void Awake()
    {
        _memoryPool = new MemoryPool(_casingPrefabs);
    }

    public void SpawnCasing(Vector3 position, Vector3 direction)
    {
        GameObject item = _memoryPool.ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = Random.rotation;
        item.GetComponent<Casing>().Setup(_memoryPool, direction);
    }
}
