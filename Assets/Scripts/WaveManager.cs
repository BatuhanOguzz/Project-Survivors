using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public Transform player;
    public EnemyPoolManager poolManager;
    public LayerMask groundLayer;
    public float spawnRadius = 15f;
    public float waveDelay = 5f;

    public List<WaveData> waves;
    private int currentWaveIndex = 0;

    void Start()
    {
        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (currentWaveIndex < waves.Count)
        {
            WaveData wave = waves[currentWaveIndex];
            Debug.Log($"Wave {currentWaveIndex + 1} başladı");

            float waveTime = 0f;
            float[] spawnTimers = new float[wave.enemiesToSpawn.Length];

            while (waveTime < wave.waveDuration)
            {
                waveTime += Time.deltaTime;

                for (int i = 0; i < wave.enemiesToSpawn.Length; i++)
                {
                    EnemySpawnInfo info = wave.enemiesToSpawn[i];
                    spawnTimers[i] += Time.deltaTime;

                    float spawnInterval = 1f / info.spawnRatePerSecond;

                    if (spawnTimers[i] >= spawnInterval)
                    {
                        spawnTimers[i] = 0f;
                        SpawnEnemy(info.enemyPrefab);
                    }
                }

                yield return null;
            }

            Debug.Log($"Wave {currentWaveIndex + 1} bitti");
            currentWaveIndex++;
            yield return new WaitForSeconds(waveDelay);
        }

        Debug.Log("Tüm dalgalar bitti!");
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector2 randCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randCircle.x, 10, randCircle.y);

        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 100f, groundLayer))
        {
            spawnPos.y = hit.point.y;
            poolManager.GetFromPool(enemyPrefab.name, spawnPos, Quaternion.identity);
        }
    }
}
