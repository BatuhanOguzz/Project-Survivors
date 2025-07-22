using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public event Action onDeath;
    private bool isDead = false;

    [Header("XP Ayarları")]
    public GameObject xpOrbPrefab;
    public int xpAmount = 5;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        onDeath?.Invoke();

        DropXP();

        SetLayerRecursively(transform, "DeadEnemy"); // 💀 Cesedi player'dan ayır

        var ragdoll = GetComponent<EnemyRagdoll>();
        if (ragdoll != null)
            ragdoll.ActivateRagdoll();

        var ai = GetComponent<EnemyAI>();
        if (ai != null)
            ai.enabled = false;

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
            agent.enabled = false;

        Destroy(gameObject, 4f);
    }

    void DropXP()
    {
        if (xpOrbPrefab != null)
        {
            Vector3 orbPos = transform.position + Vector3.up;
            GameObject orb = Instantiate(xpOrbPrefab, orbPos, Quaternion.identity);
            XPOrb orbScript = orb.GetComponent<XPOrb>();
            if (orbScript != null)
                orbScript.xpAmount = xpAmount;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: xpOrbPrefab atanmadı! Inspector’dan prefab atamayı unutma.");
        }
    }

    // ✅ Tüm alt objelerin layer'ını değiştirir
    void SetLayerRecursively(Transform obj, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in obj.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = layer;
        }
    }
}
