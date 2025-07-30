using UnityEngine;
using System.Collections.Generic;

public class EnemyPoolManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int poolSizePerEnemy = 30;
    private Dictionary<string, Queue<GameObject>> poolDictionary = new();

    void Awake()
    {
        foreach (GameObject prefab in enemyPrefabs)
        {
            Queue<GameObject> enemyQueue = new();
            for (int i = 0; i < poolSizePerEnemy; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                enemyQueue.Enqueue(obj);
            }
            poolDictionary.Add(prefab.name, enemyQueue);
        }
    }

    public GameObject GetFromPool(string enemyName, Vector3 pos, Quaternion rot)
    {
        if (poolDictionary.TryGetValue(enemyName, out var queue))
        {
            GameObject enemy = queue.Dequeue();
            enemy.transform.SetPositionAndRotation(pos, rot);
            enemy.SetActive(true);
            queue.Enqueue(enemy); // sıraya geri koy
            return enemy;
        }
        return null;
    }
}
