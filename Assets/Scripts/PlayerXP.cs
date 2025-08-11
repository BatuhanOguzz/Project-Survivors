using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerXP : MonoBehaviour
{
    [Header("XP / Level")]
    public int currentXP = 0;
    public int level = 1;
    public int xpToLevel = 100;        // 1 → 2 için başlangıç gereksinimi
    public float xpMultiplier = 1f;    // XP Boost kartları için çarpan

    [Header("UI")]
    public Slider xpSlider;                 // Inspector’dan ata
    public TMP_Text levelText;              // Inspector’dan ata (TextMeshPro)
    public CardSelectionUIController cardSelectionUI; // Kart UI referansı

    [Header("Level Zorluk (Düz Artış)")]
    [Tooltip("Her level up'tan sonra gereken XP'ye eklenecek sabit miktar.")]
    public int xpGrowthPerLevel = 30;       // ← İSTEDİĞİN: her seviye +30

    void Start()
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToLevel;
            xpSlider.value = currentXP;
            Debug.Log("Slider referansı başarıyla bulundu!");
        }
        else
        {
            Debug.LogWarning("XP Slider referansı atanmadı!");
        }

        if (levelText != null)
            levelText.text = "Level: " + level;
    }

    public void AddXP(int amount)
    {
        int finalXP = Mathf.RoundToInt(amount * xpMultiplier);
        currentXP += finalXP;
        Debug.Log($"XP eklendi! (Çarpan: {xpMultiplier}) Eklenecek XP: {finalXP} → Yeni toplam: {currentXP}");

        // Tek seferde 1’den fazla seviye atlamayı istemiyorsan if bırak (mevcut davranış).
        if (currentXP >= xpToLevel)
            LevelUp();

        if (xpSlider != null)
            xpSlider.value = currentXP;
        else
            Debug.LogWarning("XP Slider bağlı değil!");

        if (levelText != null)
            levelText.text = "Level: " + level;
    }

    void LevelUp()
    {
        level++;
        currentXP -= xpToLevel;

        // === DÜZ ARTAN ZORLUK ===
        // Her level up'ta gereken XP'ye sabit +30 ekle
        xpToLevel += Mathf.Max(0, xpGrowthPerLevel);

        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToLevel;
            xpSlider.value = currentXP;
        }

        if (levelText != null)
            levelText.text = "Level: " + level;

        Debug.Log($"LEVEL UP! Yeni seviye: {level} | Sonraki level için XP: {xpToLevel}");

        // Kart seçim ekranını aç
        if (cardSelectionUI != null)
        {
            Time.timeScale = 0f; // Oyunu durdur
            cardSelectionUI.Show3RandomCards();
        }
        else
        {
            Debug.LogWarning("CardSelectionUI atanmadı!");
        }
    }

    // Kart seçildikten sonra çağrılacak (kart UI'dan tetiklenecek)
    public void ResumeGameAfterCardSelection()
    {
        Time.timeScale = 1f;
    }

    // İsteğe bağlı: n → n+1 için gereken XP'yi hesaplamak (örn. 29→30)
    public int GetXpNeededFromLevel(int fromLevel, int startXpToLevel = 100)
    {
        // Düz artış: x_n = start + (fromLevel-1) * xpGrowthPerLevel
        return startXpToLevel + Mathf.Max(0, fromLevel - 1) * Mathf.Max(0, xpGrowthPerLevel);
    }
}
