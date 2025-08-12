using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Synty.AnimationBaseLocomotion.Samples;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // (Eğer new Input System kullanıyorsan, opsiyonel)

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public Image fadeImage;
    public TextMeshProUGUI youDiedText;
    public Slider healthSlider;
    public TextMeshProUGUI extraInfoText;
    public Canvas UI;

    [Header("Regen / HoT (HUD için)")]
    public float healOverTime = 0f;   // saniyede iyileşme miktarı (HUD okur)

    private bool isDead = false; // 🛡 Ölüm tekrar etmesin

    void Start()
    {
        currentHealth = maxHealth;
        isDead = false;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        // Debug:
        // Debug.Log("Heal Over Time: +" + amount + " → Şu anki can: " + currentHealth);
    }

    public void ApplyCardUpgrade(CardData card)
    {
        if (isDead) return;

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
                // HUD’da göstermek için anlık HoT değerini set et
                healOverTime = card.value; // saniyede ne kadar iyileşeceği
                StartCoroutine(HealOverTimeRoutine(card.value, 10f, 1f));
                Debug.Log("Heal Over Time kartı seçildi! 10 saniye boyunca iyileşiyor.");
                break;
        }
    }

    private IEnumerator HealOverTimeRoutine(float healPerTick, float duration, float interval)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Heal(healPerTick);
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        // Süre bitince HUD’da bar/ değer sıfırlansın
        healOverTime = 0f;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player öldü!");
        if (UI != null) UI.gameObject.SetActive(false);

        // 1. Ragdoll aktif et
        PlayerRagdoll ragdoll = GetComponent<PlayerRagdoll>();
        if (ragdoll != null)
            ragdoll.EnableRagdoll(true);
        else
            Debug.LogWarning("PlayerRagdoll component'i bulunamadı!");

        // 2. Canvas'ı aç (YOU DIED yazan UI)
        GameObject youDiedCanvas = GameObject.Find("YouDiedCanvas");
        if (youDiedCanvas != null)
        {
            youDiedCanvas.SetActive(true);

            // 2.1 Fade başlat (hem image hem text)
            if (fadeImage != null && youDiedText != null)
                StartCoroutine(FadeInUI(fadeImage, youDiedText, 1.5f));
            else
                Debug.LogWarning("fadeImage ya da youDiedText atanmadı!");
        }
        else
        {
            Debug.LogWarning("YouDiedCanvas bulunamadı. İsmi doğru mu?");
        }

        // 3. Oyuncu kontrolünü kapat
        MonoBehaviour movementScript = GetComponent<SamplePlayerAnimationController>();
        if (movementScript != null)
            movementScript.enabled = false;
    }

    private IEnumerator FadeInUI(Image image, TextMeshProUGUI text, float duration)
    {
        float t = 0f;
        Color imgColor = image.color;
        Color txtColor = text.color;
        Color extraColor = extraInfoText != null ? extraInfoText.color : Color.clear;

        imgColor.a = 0f;
        txtColor.a = 0f;
        if (extraInfoText != null) extraColor.a = 0f;

        image.color = imgColor;
        text.color = txtColor;
        if (extraInfoText != null) extraInfoText.color = extraColor;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / duration);

            imgColor.a = Mathf.Lerp(0f, 1f, normalized);
            txtColor.a = Mathf.Lerp(0f, 1f, normalized);
            if (extraInfoText != null)
                extraColor.a = Mathf.Lerp(0f, 1f, normalized);

            image.color = imgColor;
            text.color = txtColor;
            if (extraInfoText != null)
                extraInfoText.color = extraColor;

            yield return null;
        }

        imgColor.a = 1f;
        txtColor.a = 1f;
        if (extraInfoText != null) extraColor.a = 1f;

        image.color = imgColor;
        text.color = txtColor;
        if (extraInfoText != null) extraInfoText.color = extraColor;
    }

    void Update()
    {
        if (!isDead) return;

        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("MainMenu"); // Ana Menü sahnesinin adı
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Mevcut sahneyi yeniden yükle
        }
    }
}
