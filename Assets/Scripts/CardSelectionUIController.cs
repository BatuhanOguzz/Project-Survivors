using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardSelectionUIController : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform parentCanvas;
    public List<CardData> cardOptions;

    private List<GameObject> activeCards = new();

    private readonly Vector3[] positions = new Vector3[]
    {
        new Vector3(-500, 0, 0),
        new Vector3(0, 0, 0),
        new Vector3(500, 0, 0)
    };

    private void Start()
    {
        // Kart UI başlangıçta gizli olsun
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Button üzerinden çağrılır → Rastgele 3 kartı gösterir
    /// </summary>
    public void Show3RandomCards()
    {
        if (cardOptions == null || cardOptions.Count < 3)
        {
            Debug.LogWarning("Yeterli kart yok veya cardOptions atanmadı!");
            return;
        }

        var selected = new List<CardData>();
        while (selected.Count < 3)
        {
            var candidate = cardOptions[Random.Range(0, cardOptions.Count)];
            if (!selected.Contains(candidate)) selected.Add(candidate);
        }

        ShowCards(selected);
    }

    public void ShowCards(List<CardData> cards)
    {
        gameObject.SetActive(true);
        ClearCards();

        for (int i = 0; i < cards.Count; i++)
        {
            GameObject card = Instantiate(cardPrefab, parentCanvas);
            card.transform.localScale = Vector3.zero;

            card.transform.DOLocalMove(positions[i], 0.5f).SetDelay(0.2f * i);
            card.transform.DOScale(Vector3.one, 0.5f).SetDelay(0.2f * i).SetEase(Ease.OutBack);

            var controller = card.GetComponent<CardUIController>();
            if (controller != null)
            {
                controller.Setup(cards[i], this);
                activeCards.Add(card);
            }
            else
            {
                Debug.LogError("Kart prefabında CardUIController eksik!");
            }
        }
    }

    public void OnCardSelected(CardData selectedCard)
    {
        Debug.Log("Seçilen kart: " + selectedCard.cardName);
        gameObject.SetActive(false);
        ClearCards();
        // Seçilen kartın etkisini burada uygula
    }

    private void ClearCards()
    {
        foreach (var card in activeCards)
            Destroy(card);
        activeCards.Clear();
    }
}
