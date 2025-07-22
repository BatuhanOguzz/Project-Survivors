using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUIController : MonoBehaviour, IPointerClickHandler
{
    public Image cardImage;

    private CardData cardData;
    private CardSelectionUIController controller;
    private bool isInitialized = false;

    public void Setup(CardData data, CardSelectionUIController ctrl)
    {
        cardData = data;
        controller = ctrl;
        cardImage.sprite = data.cardSprite;
        isInitialized = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("CardUIController → Kart henüz initialize edilmedi, tıklama engellendi.");
            return;
        }

        controller.OnCardSelected(cardData);
    }
}
