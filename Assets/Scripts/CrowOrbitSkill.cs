using UnityEngine;
using System.Collections.Generic;

public class CrowOrbitSkill : MonoBehaviour
{
    [Header("Upgrade / Level")]
    [Tooltip("Kart kaç kere seçilebilir (seçim sayısı). 4 → toplam 1/2/3/4 balta.")]
    public int maxLevel = 4;

    [Tooltip("İlk seçimde kaç balta açılacak?")]
    public int baseAxesOnFirstPick = 1;

    [SerializeField, Tooltip("Kaç kere kart seçildi (1..maxLevel). 0 ise hiç alınmamış.")]
    private int currentLevel = 0;
    public int CurrentLevel => currentLevel;     // dışarıya sadece okuma
    public bool IsMaxed => currentLevel >= maxLevel;

    [Header("Orbit Settings")]
    public bool skillActive = false;
    public GameObject axePrefab;
    public float orbitRadius = 2f;
    public float orbitSpeed = 120f;   // derece/sn
    public float spinSpeed = 250f;    // Y ekseninde dönüş
    public float orbitHeight = 1.8f;  // baltalar yukarıda döner

    [Header("Combat")]
    public float damage = 10f;        // her baltanın hasarı

    private readonly List<GameObject> axes = new();
    private float angleOffset;

    void Update()
    {
        if (!skillActive) return;

        int targetCount = GetTargetAxeCountForLevel(currentLevel);
        if (targetCount <= 0) return;

        if (axes.Count != targetCount)
            RebuildAxes(targetCount);

        float deltaAngle = orbitSpeed * Time.deltaTime;
        angleOffset += deltaAngle;

        for (int i = 0; i < axes.Count; i++)
        {
            var axe = axes[i];
            if (axe == null) continue;

            float angle = angleOffset + i * (360f / axes.Count);
            float rad = angle * Mathf.Deg2Rad;

            Vector3 orbitPos = transform.position
                             + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * orbitRadius
                             + Vector3.up * orbitHeight;

            axe.transform.position = orbitPos;
            axe.transform.rotation = Quaternion.Euler(0f, spinSpeed * Time.time, 90f);

            var proj = axe.GetComponent<CrowProjectile>();
            if (proj != null) proj.damage = damage;
        }
    }

    /// <summary>
    /// Kart seçildiğinde çağır. damageAdd: kart value (damage yığmak istemezsen 0f gönder).
    public void ApplyCardUpgrade(float damageAdd)
    {
        if (IsMaxed) return; // max'ta sessizce çık

        currentLevel++;
        skillActive = true;

        damage += damageAdd;

        int targetCount = GetTargetAxeCountForLevel(currentLevel);
        RebuildAxes(targetCount);
        // isteğe bağlı logları kaldırdık
    }

    private int GetTargetAxeCountForLevel(int level)
    {
        if (level <= 0) return 0;
        return baseAxesOnFirstPick + (level - 1); // 1→base, 2→base+1, 3→base+2...
    }

    private void RebuildAxes(int targetCount)
    {
        if (axePrefab == null)
        {
            Debug.LogWarning("[CrowOrbitSkill] Axe prefabı atanmadı!");
            return;
        }

        // Fazlayı sil
        while (axes.Count > targetCount)
        {
            var go = axes[^1];
            axes.RemoveAt(axes.Count - 1);
            if (go) Destroy(go);
        }

        // Eksikse oluştur
        while (axes.Count < targetCount)
        {
            var go = Instantiate(axePrefab, transform.position, Quaternion.Euler(-90f, -90f, 0f));
            axes.Add(go);
        }

        angleOffset = 0f;
        ForcePlaceNow();
    }

    private void ForcePlaceNow()
    {
        if (axes.Count == 0) return;

        for (int i = 0; i < axes.Count; i++)
        {
            float angle = i * (360f / axes.Count);
            float rad = angle * Mathf.Deg2Rad;

            Vector3 orbitPos = transform.position
                             + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * orbitRadius
                             + Vector3.up * orbitHeight;

            axes[i].transform.position = orbitPos;
            axes[i].transform.rotation = Quaternion.Euler(0f, spinSpeed * Time.time, 90f);

            var proj = axes[i].GetComponent<CrowProjectile>();
            if (proj != null) proj.damage = damage;
        }
    }

    public void ResetSkill()
    {
        skillActive = false;
        currentLevel = 0;
        foreach (var go in axes) if (go) Destroy(go);
        axes.Clear();
    }
}
