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
        // Ragdoll aç
        GetComponent<EnemyRagdoll>().ActivateRagdoll();
        // EnemyAI gibi scriptleri devre dýþý býrakabilirsin
        Destroy(gameObject, 4f); // Ragdoll birkaç sn sonra temizlenir
    }
}
