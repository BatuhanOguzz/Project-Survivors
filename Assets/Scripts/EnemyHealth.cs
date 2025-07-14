using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public event Action onDeath;
    private bool isDead = false;

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

        // Animator'u kapat
        var ragdoll = GetComponent<EnemyRagdoll>();
        if (ragdoll != null)
            ragdoll.ActivateRagdoll();

        // EnemyAI gibi scriptleri devre dýþý býrak
        var ai = GetComponent<EnemyAI>();
        if (ai != null)
            ai.enabled = false;

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
            agent.enabled = false;

        Destroy(gameObject, 4f); // Ragdoll birkaç sn sonra temizlenir
    }
}
