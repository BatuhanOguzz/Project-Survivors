using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerXP : MonoBehaviour
{
    public int currentXP = 0;
    public int level = 1;
    public int xpToLevel = 100;

    public Slider xpSlider;        // Inspector’dan ata
    public TMP_Text levelText;     // Inspector’dan ata (TextMeshPro)
    public CardSelectionUIController cardSelectionUI; // Kart UI referansı

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

        // Level text'i başta güncelle
        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log("XP eklendi! Yeni toplam: " + currentXP);

        // Level atlama kontrolü
        if (currentXP >= xpToLevel)
        {
            LevelUp();
        }

        // Slider'ı güncelle
        if (xpSlider != null)
        {
            xpSlider.value = currentXP;
        }
        else
        {
            Debug.LogWarning("XP Slider bağlı değil!");
        }

        // Level Text'i güncelle
        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }
    }

    void LevelUp()
    {
        level++;
        currentXP -= xpToLevel;

        // Seviye başına XP artırma istersen:
        // xpToLevel = Mathf.RoundToInt(xpToLevel * 1.2f);

        // Slider'ı güncelle
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToLevel;
            xpSlider.value = currentXP;
        }

        // Level Text'i güncelle
        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }

        Debug.Log("LEVEL UP! Yeni seviye: " + level);

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
}
