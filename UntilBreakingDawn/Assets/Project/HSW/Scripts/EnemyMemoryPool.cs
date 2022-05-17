using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class EnemyMemoryPool : MonoBehaviour
{
    [SerializeField]
    private Transform  _target;
    [SerializeField]
    private GameObject _enemySpawnPointPrefab;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private float      _enemySpawnTime    = 1;
    [SerializeField]
    private float      _enemySpawnLatency = 1;

    private MemoryPool _spawnPointMemoryPool;
    private MemoryPool _enemyMemoryPool;

    private int        numberOfEnemiesSpawnedAtOnce = 1;
    private Vector2Int mapSize                      = new Vector2Int(100, 100);

    private void Awake()
    {
        _spawnPointMemoryPool = new MemoryPool(_enemySpawnPointPrefab);
        _enemyMemoryPool      = new MemoryPool(_enemyPrefab);
    }

    public IEnumerator SpawnTile()
    {
        int currentNumber = 0;
        int maximumNumber = 50;

        while (true)
        {
            for (int i = 0; i < numberOfEnemiesSpawnedAtOnce; ++i)
            {
                GameObject item = _spawnPointMemoryPool.ActivatePoolItem();

                item.transform.position = new Vector3(Random.Range(-mapSize.x * 0.49f, mapSize.x * 0.49f), 3,
                    Random.Range(-mapSize.y * 0.49f, mapSize.y * 0.49f));
                StartCoroutine("SpawnEnemy", item);
            }

            currentNumber++;

            if (currentNumber >= maximumNumber)
            {
                currentNumber = 0;
                numberOfEnemiesSpawnedAtOnce++;
            }

            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }

    public IEnumerator SpawnEnemy(GameObject point)
    {
        yield return new WaitForSeconds(_enemySpawnLatency);

        GameObject item = _enemyMemoryPool.ActivatePoolItem();
        item.transform.position = point.transform.position;

        item.GetComponent<EnemyFSM>().Setup(_target, this);

        _spawnPointMemoryPool.DeactivatePoolItem(point);
    }

    public void DeactivateEnemy(GameObject enemy)
    {
        _enemyMemoryPool.DeactivatePoolItem(enemy);
    }
}