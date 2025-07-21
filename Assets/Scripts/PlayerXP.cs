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

        // Level Text'i güncelle (seviye atlama olmadan da çağırmak istersen)
        if (levelText != null)
        {
            levelText.text = level.ToString();
        }
    }

    void LevelUp()
    {
        level++;
        currentXP -= xpToLevel; // Arta kalan XP devreder

        // İstersen seviye başına gereken XP'yi artırabilirsin:
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
            levelText.text = level.ToString();
        }

        Debug.Log("LEVEL UP! Yeni seviye: " + level);
    }
}
