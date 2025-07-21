using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public event Action onDeath;
    private bool isDead = false;

    [Header("XP Ayarları")]
    public GameObject xpOrbPrefab; // Inspector’dan ata
    public int xpAmount = 5;       // Bu düşmanın verdiği XP (farklı set edebilirsin)

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        onDeath?.Invoke();

        DropXP(); // <<<<<<<<<< XP orb bırakma

        // Animator'u kapat
        var ragdoll = GetComponent<EnemyRagdoll>();
        if (ragdoll != null)
            ragdoll.ActivateRagdoll();

        // EnemyAI gibi scriptleri devre dışı bırak
        var ai = GetComponent<EnemyAI>();
        if (ai != null)
            ai.enabled = false;

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
            agent.enabled = false;

        Destroy(gameObject, 4f); // Ragdoll birkaç sn sonra temizlenir
    }

    void DropXP()
    {
        if (xpOrbPrefab != null)
        {
            Vector3 orbPos = transform.position + Vector3.up; // 1 birim yukarı
            GameObject orb = Instantiate(xpOrbPrefab, orbPos, Quaternion.identity);
            XPOrb orbScript = orb.GetComponent<XPOrb>();
            if (orbScript != null)
            {
                orbScript.xpAmount = xpAmount;
            }
        }
    }
}
