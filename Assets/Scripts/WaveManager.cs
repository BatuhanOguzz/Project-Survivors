using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public Transform player;

    // Burada prefab dizisi kullan
    public GameObject[] enemyPrefabs;
    public int enemyCount = 5;
    public float spawnRadius = 10f;
    public LayerMask groundLayer;

    private int enemiesAlive = 0;

    void Start()
    {
        StartWave();
    }

    void StartWave()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPos = player.position + new Vector3(randomCircle.x, 10, randomCircle.y);

            Ray ray = new Ray(spawnPos, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                spawnPos.y = hit.point.y;

                // Burada random veya sýralý prefab seç
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);

                enemy.GetComponent<EnemyHealth>().onDeath += OnEnemyDeath;
                enemiesAlive++;
            }
        }
    }

    void OnEnemyDeath()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            Debug.Log("Wave bitti!");
            // Sonraki wave’i burada baþlatabilirsin
        }
    }
}
