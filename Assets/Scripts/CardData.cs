using UnityEngine;

public enum CardType
{
    MaxHealthUp,
    DamageUp,
    HealOverTime,
    Speed,
    AttackSpeedUp,
    XpBoost
    // Diðer kart tipleri eklenebilir
}

[CreateAssetMenu(menuName = "Card/Upgrade Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite cardSprite;
    public CardType cardType;    // Kartýn tipi (Inspector’dan seçilecek)
    public float value;            // Artýþ miktarý
}
