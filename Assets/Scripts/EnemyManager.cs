using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [Header("Oyuncu Referansý")]
    public Transform player;

    [Header("Sahnedeki Düþmanlar")]
    public List<EnemyAI> enemies = new List<EnemyAI>();

    private void Awake()
    {
        // Singleton eriþimi
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Çift instance varsa sil
        }
    }

    /// <summary>
    /// Bir düþmaný listeye kaydeder.
    /// </summary>
    public void RegisterEnemy(EnemyAI enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    /// <summary>
    /// Bir düþmaný listeden çýkarýr.
    /// </summary>
    public void UnregisterEnemy(EnemyAI enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
}
