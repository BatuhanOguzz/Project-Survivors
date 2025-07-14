using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [Header("Oyuncu Referans�")]
    public Transform player;

    [Header("Sahnedeki D��manlar")]
    public List<EnemyAI> enemies = new List<EnemyAI>();

    private void Awake()
    {
        // Singleton eri�imi
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �ift instance varsa sil
        }
    }

    /// <summary>
    /// Bir d��man� listeye kaydeder.
    /// </summary>
    public void RegisterEnemy(EnemyAI enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    /// <summary>
    /// Bir d��man� listeden ��kar�r.
    /// </summary>
    public void UnregisterEnemy(EnemyAI enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
}
