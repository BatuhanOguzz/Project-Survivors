using UnityEngine;

public class StatsHUDBinder : MonoBehaviour
{
    [Header("HUD")]
    public StatsHUDController hud;

    [Header("World Refs (opsiyonel; boşsa otomatik bulunur)")]
    public PlayerHealth playerHealth;        // currentHealth, maxHealth, healOverTime
    public PlayerXP playerXP;                // xpMultiplier
    public AxeHit axeHit;                    // damage (temel silah)
    public CrowOrbitSkill crow;              // (opsiyonel)
    public OdinFireSkill odin;               // (opsiyonel)
    public SpearThrowSkill spear;            // (opsiyonel)
    public KickAoESkill aoe;                 // (opsiyonel)
    public MoveSpeedStat moveSpeedStat;      // WALK/SPRINT çarpanı

    [Header("Attack Speed")]
    public Animator animator;                // AttackSpeed için float param
    public string attackSpeedParam = "AttackSpeed";

    [Header("Güncelleme")]
    public float updateInterval = 0.25f;

    [Header("Debug")]
    public bool debugLogs = true;

    private float _elapsed;

    void Awake()
    {
        // Otomatik referanslar
        if (!hud) hud = FindFirstObjectByType<StatsHUDController>();
        if (!playerHealth) playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (!playerXP) playerXP = FindFirstObjectByType<PlayerXP>();
        if (!axeHit) axeHit = FindFirstObjectByType<AxeHit>();
        if (!crow) crow = FindFirstObjectByType<CrowOrbitSkill>();
        if (!odin) odin = FindFirstObjectByType<OdinFireSkill>();
        if (!spear) spear = FindFirstObjectByType<SpearThrowSkill>();
        if (!aoe) aoe = FindFirstObjectByType<KickAoESkill>();
        if (!moveSpeedStat) moveSpeedStat = FindFirstObjectByType<MoveSpeedStat>();
        if (!animator) animator = FindFirstObjectByType<Animator>();

        if (debugLogs)
        {
            Debug.Log($"[Binder] hud: {(hud ? "OK" : "NULL")}");
            Debug.Log($"[Binder] playerHealth: {(playerHealth ? "OK" : "NULL")}, playerXP: {(playerXP ? "OK" : "NULL")}, axeHit: {(axeHit ? "OK" : "NULL")}");
            Debug.Log($"[Binder] moveSpeedStat: {(moveSpeedStat ? "OK" : "NULL")}, animator: {(animator ? "OK" : "NULL")}");
        }
    }

    void Update()
    {
        if (!hud) return;

        _elapsed += Time.deltaTime;
        if (_elapsed < updateInterval) return;
        _elapsed = 0f;

        PushStatsToHUD();
    }

    private void PushStatsToHUD()
    {
        // --- Attack Speed ---
        float attackSpeed = 1f;
        if (animator != null && !string.IsNullOrEmpty(attackSpeedParam))
        {
            var pars = animator.parameters;
            for (int i = 0; i < pars.Length; i++)
            {
                if (pars[i].type == AnimatorControllerParameterType.Float &&
                    pars[i].name == attackSpeedParam)
                {
                    attackSpeed = animator.GetFloat(attackSpeedParam);
                    break;
                }
            }
        }
        hud.UpdateStat(StatType.AttackSpeed, attackSpeed);
        if (debugLogs) Debug.Log($"[Binder] AttackSpeed -> {attackSpeed:0.00}");

        // --- Move Speed (multiplier) ---
        float moveMult = moveSpeedStat ? moveSpeedStat.AvgMultiplier() : 1f;
        hud.UpdateStat(StatType.MoveSpeed, moveMult);
        if (debugLogs) Debug.Log($"[Binder] Move x -> {moveMult:0.00}");

        // --- Heal Over Time ---
        if (playerHealth)
        {
            hud.UpdateStat(StatType.HealOverTime, playerHealth.healOverTime);
            if (debugLogs) Debug.Log($"[Binder] HoT -> {playerHealth.healOverTime:0.00}/s");
        }
        else if (debugLogs) Debug.LogWarning("[Binder] playerHealth NULL (HoT/HP güncellenmez).");

        // --- Damage (ana silah) ---
        if (axeHit)
        {
            hud.UpdateStat(StatType.Damage, axeHit.damage);
            if (debugLogs) Debug.Log($"[Binder] DMG -> {axeHit.damage:0.0}");
        }
        else if (debugLogs) Debug.LogWarning("[Binder] axeHit NULL (Damage güncellenmez).");

        // --- Health ---
        if (playerHealth)
        {
            hud.UpdateStat(StatType.Health, playerHealth.currentHealth, playerHealth.maxHealth);
            if (debugLogs) Debug.Log($"[Binder] HP -> {playerHealth.currentHealth:0}/{playerHealth.maxHealth:0}");
        }

        // --- XP Multiplier ---
        if (playerXP)
        {
            hud.UpdateStat(StatType.XPMultiplier, playerXP.xpMultiplier);
            if (debugLogs) Debug.Log($"[Binder] XP x -> {playerXP.xpMultiplier:0.00}");
        }
        else if (debugLogs) Debug.LogWarning("[Binder] playerXP NULL (XP güncellenmez).");
    }
}
