using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsHUDController : MonoBehaviour
{
    [Serializable]
    public class RowConfig
    {
        public StatType stat;
        public string label = "Label";
        public bool showBar = false;
        public float barMin = 0;
        public float barMax = 1;
        public string valueFormat = ""; // örn: "x{0:0.00}" veya "{0:0}/{1:0}"
    }

    [Header("Setup")]
    public Transform listParent;       // Vertical Layout Group olan container
    public GameObject rowPrefab;       // StatRowUI prefab
    public StatIconSet iconSet;        // ScriptableObject

    [Header("Rows")]
    public List<RowConfig> rows = new();

    [Header("Debug")]
    public bool debugLogs = true;

    private readonly Dictionary<StatType, StatRowUI> _rowMap = new();

    void Awake()
    {
        Build();
    }

    public void Build()
    {
        if (!listParent || !rowPrefab)
        {
            Debug.LogError("[HUD] listParent/rowPrefab eksik!");
            return;
        }

        for (int i = listParent.childCount - 1; i >= 0; i--)
            Destroy(listParent.GetChild(i).gameObject);

        _rowMap.Clear();

        foreach (var cfg in rows)
        {
            var go = Instantiate(rowPrefab, listParent);
            var row = go.GetComponent<StatRowUI>();
            if (!row)
            {
                Debug.LogError("[HUD] rowPrefab üzerinde StatRowUI yok!");
                continue;
            }

            Sprite sprite = null; Color tint = Color.white; Color barColor = new Color(0.3f, 0.6f, 1f);
            if (iconSet && iconSet.TryGet(cfg.stat, out var e))
            {
                sprite = e.icon; tint = e.iconTint; barColor = e.barColor;
            }

            row.Setup(sprite, tint, cfg.label, cfg.showBar, barColor, cfg.barMin, cfg.barMax);
            _rowMap[cfg.stat] = row;

            if (debugLogs) Debug.Log($"[HUD] Row created: {cfg.stat} label='{cfg.label}' showBar={cfg.showBar}");
        }
    }

    public void UpdateStat(StatType stat, float value, float? maxForFormat = null)
    {
        if (!_rowMap.TryGetValue(stat, out var row))
        {
            if (debugLogs) Debug.LogWarning($"[HUD] UpdateStat: Row not found for {stat}. Rows’ta bu StatType ekli mi?");
            return;
        }

        var cfg = rows.Find(r => r.stat == stat);
        if (cfg == null)
        {
            row.SetValue(value, value.ToString("0.##"));
            return;
        }

        if (!string.IsNullOrEmpty(cfg.valueFormat))
        {
            string formatted = maxForFormat.HasValue
                ? string.Format(cfg.valueFormat, value, maxForFormat.Value)
                : string.Format(cfg.valueFormat, value);
            row.SetValue(value, formatted);
        }
        else
        {
            row.SetValue(value);
        }
    }
}
