using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public List<EnemyAI> enemies = new List<EnemyAI>();
    public Transform player;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        Vector3 currentPlayerPosition = player.position; // HER UPDATE'DE EN GÜNCEL POZÝSYON!
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.ManualUpdate(currentPlayerPosition);
        }
    }


    // Düþmanlar kendini buraya ekler
    public void RegisterEnemy(EnemyAI enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyAI enemy)
    {
        if (enemies.Contains(enemy))
            enemies.Remove(enemy);
    }
}
