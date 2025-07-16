using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;
    public GameObject[] enemyPrefabs; // Birden fazla prefab
    public int poolSize = 20;

    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    void Awake()
    {
        Instance = this;
        foreach (var prefab in enemyPrefabs)
        {
            var queue = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            pools.Add(prefab, queue);
        }
    }

    public GameObject GetEnemy(GameObject prefab, Vector3 position)
    {
        if (!pools.ContainsKey(prefab))
        {
            // Prefab için pool yoksa oluþtur
            pools[prefab] = new Queue<GameObject>();
        }

        var pool = pools[prefab];
        GameObject enemy;

        if (pool.Count == 0)
        {
            // Pool boþsa yeni oluþtur
            enemy = Instantiate(prefab, position, Quaternion.identity);
        }
        else
        {
            enemy = pool.Dequeue();
            enemy.transform.position = position;
            enemy.SetActive(true);
        }
        return enemy;
    }

    public void ReturnEnemy(GameObject prefab, GameObject enemy)
    {
        enemy.SetActive(false);
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new Queue<GameObject>();
        }
        pools[prefab].Enqueue(enemy);
    }
}
