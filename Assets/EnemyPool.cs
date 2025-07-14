using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;
    public GameObject enemyPrefab;
    public int poolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetEnemy(Vector3 position)
    {
        if (pool.Count == 0)
        {
            // Pool bitti, yeni oluþtur (veya yok say)
            GameObject obj = Instantiate(enemyPrefab, position, Quaternion.identity);
            return obj;
        }
        GameObject enemy = pool.Dequeue();
        enemy.transform.position = position;
        enemy.SetActive(true);
        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        pool.Enqueue(enemy);
    }
}
