using UnityEngine;

public class MoveSpeedStat : MonoBehaviour
{
    private static MoveSpeedStat _instance;
    public static MoveSpeedStat Instance
    {
        get
        {
            if (_instance == null) _instance = FindFirstObjectByType<MoveSpeedStat>();
            return _instance;
        }
    }

    [Header("Baselines (Inspector’dan set et)")]
    public float baselineWalk = 2.5f;
    public float baselineSprint = 4.5f;

    [Header("Toplam Eklenen Hýz (kartlarla artar)")]
    public float totalAdd = 0f;

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); return; }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Add(float v)
    {
        totalAdd += v;
    }

    public float WalkMultiplier()
    {
        if (baselineWalk <= 0f) return 1f;
        return (baselineWalk + totalAdd) / baselineWalk;
    }

    public float SprintMultiplier()
    {
        if (baselineSprint <= 0f) return 1f;
        return (baselineSprint + totalAdd) / baselineSprint;
    }

    // Ýstersen tek bir birleþik “hareket çarpaný”:
    public float AvgMultiplier()
    {
        return (WalkMultiplier() + SprintMultiplier()) * 0.5f;
    }
}
