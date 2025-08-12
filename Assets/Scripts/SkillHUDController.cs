using System.Collections.Generic;
using UnityEngine;

public class SkillsHUDController : MonoBehaviour
{
    [Header("Setup")]
    public Transform listParent;
    public GameObject rowPrefab;
    public SkillLevelRegistry registry;

    private readonly Dictionary<SkillType, SkillRowUI> _rows = new();

    void Awake()
    {
        if (!registry) registry = SkillLevelRegistry.Instance;
        Build();
    }

    void OnEnable()
    {
        if (!registry) registry = SkillLevelRegistry.Instance;
        if (registry != null)
        {
            registry.OnChanged += RefreshAll;
            RefreshAll();
        }
    }

    void OnDisable()
    {
        if (registry != null)
            registry.OnChanged -= RefreshAll;
    }

    public void Build()
    {
        if (!registry || !listParent || !rowPrefab) return;

        for (int i = listParent.childCount - 1; i >= 0; i--)
            Destroy(listParent.GetChild(i).gameObject);

        _rows.Clear();

        foreach (var def in registry.entries)
        {
            var go = Instantiate(rowPrefab, listParent);
            var row = go.GetComponent<SkillRowUI>();
            if (!row) { Debug.LogError("rowPrefab üzerinde SkillRowUI yok!"); Destroy(go); continue; }

            // ÝKON & LABEL'ý entry'den al
            row.Setup(def.icon, def.label);

            _rows[def.type] = row;
            go.SetActive(false); // seviye > 0 olunca açacaðýz
        }
    }

    public void RefreshAll()
    {
        if (!registry) return;

        foreach (var def in registry.entries)
        {
            if (!_rows.TryGetValue(def.type, out var row) || row == null) continue;
            int lv = registry.GetLevel(def.type);
            int max = registry.GetMax(def.type);
            row.SetLevel(lv, max);
        }
    }
}
