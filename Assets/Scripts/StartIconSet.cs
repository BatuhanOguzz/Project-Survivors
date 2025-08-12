using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatIconSet", menuName = "UI/Stat Icon Set")]
public class StatIconSet : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public StatType stat;
        public Sprite icon;
        public Color iconTint = Color.white;
        public Color barColor = new Color(0.2f, 0.7f, 0.4f);
    }

    public List<Entry> entries = new();

    private Dictionary<StatType, Entry> _map;
    void OnEnable()
    {
        _map = new Dictionary<StatType, Entry>();
        foreach (var e in entries) _map[e.stat] = e;
    }

    public bool TryGet(StatType type, out Entry entry)
    {
        if (_map == null) OnEnable();
        return _map.TryGetValue(type, out entry);
    }
}
