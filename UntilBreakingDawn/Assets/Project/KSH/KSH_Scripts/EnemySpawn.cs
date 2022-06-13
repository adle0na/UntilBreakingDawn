using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public enum spawnEnemyType { Enemy, Ranged, Bomb, Minotaur, Megusa};
    public spawnEnemyType enemyType;

    public GameObject enemy = null;
    public GameObject rangedEnemy = null;
    public GameObject bombEnemy = null;
    public GameObject minotaur = null;
    public GameObject medusa = null;

    public float waveInterval  = 3.0f;
    public int   spawnCount    = 3;
    public float spawnInterval = 1.5f;
    private float spawnRate    = 1f;

    WaitForSeconds waitSpawnInterval      = null;
    WaitForSeconds waitSpawnIntervalFirst = null;
    private void Start()
    {
        waitSpawnInterval = new WaitForSeconds(spawnInterval);
        waitSpawnIntervalFirst = new WaitForSeconds(spawnRate);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return waitSpawnInterval;
        for (int j = 0; j < waveInterval; j++)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                switch (enemyType) {
                    case spawnEnemyType.Enemy:
                        GameObject obj = Instantiate(enemy);
                        obj.transform.position = transform.position;
                        obj.transform.rotation = transform.rotation;
                        break;
                    case spawnEnemyType.Ranged:
                        GameObject obj1 = Instantiate(rangedEnemy);
                        obj1.transform.position = transform.position;
                        obj1.transform.rotation = transform.rotation;
                        break;
                    case spawnEnemyType.Bomb:
                        GameObject obj2 = Instantiate(bombEnemy);
                        obj2.transform.position = transform.position;
                        obj2.transform.rotation = transform.rotation;
                        break;
                    case spawnEnemyType.Minotaur:
                        GameObject obj3 = Instantiate(minotaur);
                        obj3.transform.position = transform.position;
                        obj3.transform.rotation = transform.rotation;
                        break;
                    case spawnEnemyType.Megusa:
                        GameObject obj4 = Instantiate(medusa);
                        obj4.transform.position = transform.position;
                        obj4.transform.rotation = transform.rotation;
                        break;


                }
                yield return waitSpawnIntervalFirst;
            }
        }
    }


}
