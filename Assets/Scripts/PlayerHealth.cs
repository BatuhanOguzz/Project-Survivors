using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        if (healthSlider != null)
            healthSlider.value = currentHealth;
        Debug.Log("Heal Over Time: +" + amount + " → Şu anki can: " + currentHealth);
    }

    public void ApplyCardUpgrade(CardData card)
    {
        switch (card.cardType)
        {
            case CardType.MaxHealthUp:
                maxHealth += card.value;
                currentHealth += card.value;
                if (healthSlider != null)
                {
                    healthSlider.maxValue = maxHealth;
                    healthSlider.value = currentHealth;
                }
                Debug.Log("Max Health kartı seçildi! Yeni maxHealth: " + maxHealth);
                break;

            case CardType.HealOverTime:
                StartCoroutine(HealOverTimeRoutine(card.value, 10, 1f));
                Debug.Log("Heal Over Time kartı seçildi! 10 saniye boyunca iyileşiyor.");
                break;

                // Burada DamageUp yok!
        }
    }

    private IEnumerator HealOverTimeRoutine(int healPerTick, int duration, float interval)
    {
        int elapsed = 0;
        while (elapsed < duration)
        {
            Heal(healPerTick);
            yield return new WaitForSeconds(interval);
            elapsed += Mathf.RoundToInt(interval);
        }
    }

    void Die()
    {
        Debug.Log("Player öldü!");
        // Ölüm animasyonu, oyun sonu vs. burada
    }
}
