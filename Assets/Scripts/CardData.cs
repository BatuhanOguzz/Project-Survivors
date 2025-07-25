using UnityEngine;

public enum CardType
{
    MaxHealthUp,
    DamageUp,
    HealOverTime,
    Speed,
    AttackSpeedUp,
    XpBoost,
    OdinFire,
    SpearThrow,
    // Di�er kart tipleri eklenebilir
}

[CreateAssetMenu(menuName = "Card/Upgrade Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite cardSprite;
    public CardType cardType;    
    public float value;           
}
