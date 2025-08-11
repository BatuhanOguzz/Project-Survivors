using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillLevelRegistry : MonoBehaviour
{
    public static SkillLevelRegistry Instance { get; private set; }

    [Serializable]
    public class Entry
    {
        public SkillType type;
        public string label = "Skill";
        public int maxLevel = 4;          // CrowOrbit i�in CrowOrbitSkill.maxLevel ile ayn� olsun
        public Sprite icon;               // (opsiyonel) HUD�da ikon
    }

    [Header("Tan�mlar (Inspector�dan doldur)")]
    public List<Entry> entries = new();

    public event Action OnChanged;

    private readonly Dictionary<SkillType, Entry> _defs = new();
    private readonly Dictionary<SkillType, int> _levels = new();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _defs.Clear();
        foreach (var e in entries)
        {
            if (!_defs.ContainsKey(e.type)) _defs.Add(e.type, e);
        }
    }

    public int GetLevel(SkillType t) => _levels.TryGetValue(t, out var lv) ? lv : 0;
    public int GetMax(SkillType t) => _defs.TryGetValue(t, out var e) ? Mathf.Max(1, e.maxLevel) : 1;
    public string GetLabel(SkillType t) => _defs.TryGetValue(t, out var e) ? e.label : t.ToString();
    public Sprite GetIcon(SkillType t) => _defs.TryGetValue(t, out var e) ? e.icon : null;
    public bool IsMaxed(SkillType t) => GetLevel(t) >= GetMax(t);

    public void Increment(SkillType t)
    {
        int lv = GetLevel(t) + 1;
        int max = GetMax(t);
        _levels[t] = Mathf.Min(lv, max);
        OnChanged?.Invoke();
    }

    public void SetLevel(SkillType t, int level, int? maxOverride = null)
    {
        if (maxOverride.HasValue && _defs.TryGetValue(t, out var e))
            e.maxLevel = maxOverride.Value;

        int max = GetMax(t);
        _levels[t] = Mathf.Clamp(level, 0, max);
        OnChanged?.Invoke();
    }

    // �stersen oyuna ba�larken hepsini s�f�rlamak i�in:
    public void ResetAll()
    {
        _levels.Clear();
        OnChanged?.Invoke();
    }
}
