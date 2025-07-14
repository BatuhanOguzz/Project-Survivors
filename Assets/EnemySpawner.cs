using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRadius = 10f;
    public float spawnInterval = 2f;
    public LayerMask groundLayer; // Yere ray atmak için

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randomCircle.x, 10, randomCircle.y);

        Ray ray = new Ray(spawnPos, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            spawnPos.y = hit.point.y;
            GameObject enemy = EnemyPool.Instance.GetEnemy(spawnPos);
            // EnemyAI zaten EnemyManager’a kaydoluyor, ekstra bir iþlem gerekmez
        }
        else
        {
            Debug.LogWarning("Enemy spawn edilmek istenen noktada zemin bulunamadý! Pozisyon: " + spawnPos);
        }
    }
}
