using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillRowUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text label;
    public TMP_Text valueText;

    public void Setup(Sprite ic, string labelText)
    {
        if (icon) icon.sprite = ic;
        if (label) label.text = labelText;
    }

    public void SetLevel(int level, int max)
    {
        if (level <= 0)
        {
            gameObject.SetActive(false); // aktif deðilse gizle
            return;
        }

        gameObject.SetActive(true);
        if (valueText)
            valueText.text = (level >= max) ? "MAX" : $"Level {level}";
    }
}
