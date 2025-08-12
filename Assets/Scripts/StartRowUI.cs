using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatRowUI : MonoBehaviour
{
    [Header("Refs")]
    public Image icon;
    public TMP_Text label;
    public TMP_Text valueText;
    public Slider bar;                 // opsiyonel (null olabilir)
    public GameObject barContainer;    // bar’ý kapatýp açmak için

    [Header("Style")]
    public float hiddenBarHeight = 0f; // bar yokken daha kompakt görünüm (opsiyonel)

    private float _min, _max;

    public void Setup(Sprite ic, Color iconTint, string labelText, bool showBar, Color barColor, float min = 0, float max = 1)
    {
        if (icon) { icon.sprite = ic; icon.color = iconTint; }
        if (label) label.text = labelText;

        _min = min; _max = Mathf.Max(min + 0.0001f, max);

        if (barContainer)
            barContainer.SetActive(showBar && bar != null);

        if (bar)
        {
            bar.minValue = 0f;
            bar.maxValue = 1f;

            // bar’ýn fill rengini deðiþtir
            var fill = bar.fillRect ? bar.fillRect.GetComponent<Image>() : null;
            if (fill) fill.color = barColor;
        }
    }

    public void SetValue(float rawValue, string formatted = null)
    {
        if (valueText)
            valueText.text = string.IsNullOrEmpty(formatted) ? rawValue.ToString("0.##") : formatted;

        if (bar)
        {
            float t = Mathf.InverseLerp(_min, _max, rawValue);
            bar.value = t;
        }
    }
}
