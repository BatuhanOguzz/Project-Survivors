using UnityEngine;

[CreateAssetMenu(menuName = "Card/Upgrade Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite cardSprite;
}
