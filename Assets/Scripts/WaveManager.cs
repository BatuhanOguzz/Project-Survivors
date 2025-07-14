using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public Transform player;
    public GameObject enemyPrefab;
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
                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                enemy.GetComponent<EnemyHealth>().onDeath += OnEnemyDeath; // Ölünce haber almak için
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
            // Sonraki wave’i burada baþlatabilirsin (ileride)
        }
    }
}
