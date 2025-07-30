using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;

    [Tooltip("Saniyede kaç kez spawn edilsin. Örn: 1 = her saniye 1 kez")]
    public float spawnRatePerSecond = 1f;
}

[CreateAssetMenu(menuName = "WaveData")]
public class WaveData : ScriptableObject
{
    [Tooltip("Bu wave'in süresi (saniye cinsinden)")]
    public float waveDuration = 30f;

    [Tooltip("Bu wave'de hangi düşmanlar ne sıklıkla spawn edilecek")]
    public EnemySpawnInfo[] enemiesToSpawn;
}
