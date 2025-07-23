using DG.Tweening;
using Synty.AnimationBaseLocomotion.Samples;
using System.Collections.Generic;
using UnityEngine;

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
        gameObject.SetActive(false); // Başlangıçta gizli tutmak daha iyi, istediğinde Show3RandomCards ile açılır
    }

    /// <summary>
    /// Seviye atlayınca çağrılır → Rastgele 3 kartı gösterir
    /// </summary>
    public void Show3RandomCards()
    {
        Debug.Log("Show3RandomCards çağrıldı!");

        if (cardOptions == null)
        {
            Debug.LogError("cardOptions NULL! Inspector'da referans verilmemiş.");
            return;
        }

        if (cardOptions.Count < 3)
        {
            Debug.LogWarning("Yeterli kart yok! cardOptions.Count: " + cardOptions.Count);
            return;
        }

        if (cardPrefab == null)
        {
            Debug.LogError("cardPrefab atanmadı! Inspector’da prefab sürüklenmemiş.");
            return;
        }

        if (parentCanvas == null)
        {
            Debug.LogError("parentCanvas atanmadı! Kartları hangi UI'ya ekleyeceğini bilmiyor.");
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
        Debug.Log("ShowCards çalıştı, kartlar gösteriliyor.");
        gameObject.SetActive(true); // Kart UI ekranını aktif et
        ClearCards();

        for (int i = 0; i < cards.Count; i++)
        {
            GameObject card = Instantiate(cardPrefab, parentCanvas);
            card.transform.localScale = Vector3.zero;

            // DOTween animasyonları unscaled time ile çalışsın!
            card.transform.DOLocalMove(positions[i], 0.5f)
                .SetDelay(0.2f * i)
                .SetUpdate(true);

            card.transform.DOScale(Vector3.one, 0.5f)
                .SetDelay(0.2f * i)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);

            var controller = card.GetComponent<CardUIController>();
            if (controller != null)
            {
                controller.Setup(cards[i], this);
                activeCards.Add(card);
            }
            else
            {
                Debug.LogError("Kart prefabında CardUIController component’i yok!");
            }
        }
    }

    public void OnCardSelected(CardData selectedCard)
    {
        Debug.Log("Seçilen kart: " + selectedCard.cardName);
        gameObject.SetActive(false);
        ClearCards();

        var playerHealth = Object.FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            if (selectedCard.cardType == CardType.MaxHealthUp ||
                selectedCard.cardType == CardType.HealOverTime)
            {
                playerHealth.ApplyCardUpgrade(selectedCard);
            }
        }

        if (selectedCard.cardType == CardType.DamageUp)
        {
            var axe = Object.FindFirstObjectByType<AxeHit>();
            if (axe != null)
                axe.SetDamage(axe.damage + selectedCard.value);
        }

        // HIZ ARTIŞI KARTI (SamplePlayerAnimationController'a özel!)
        if (selectedCard.cardType == CardType.Speed)
        {
            var animController = Object.FindFirstObjectByType<SamplePlayerAnimationController>();
            if (animController != null)
            {
                animController.IncreaseWalkSpeed(selectedCard.value);
                animController.IncreaseSprintSpeed(selectedCard.value);
                Debug.Log("SpeedUp uygulandı! Yeni walk/sprint speed arttı.");
            }
            else
            {
                Debug.LogWarning("SamplePlayerAnimationController bulunamadı!");
            }
        }

        var xpScript = Object.FindFirstObjectByType<PlayerXP>();
        if (xpScript != null)
            xpScript.ResumeGameAfterCardSelection();
    }



    private void ClearCards()
    {
        foreach (var card in activeCards)
            Destroy(card);
        activeCards.Clear();
    }

    // Test amaçlı: T tuşuna basınca kartları aç
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T tuşuna basıldı, kartlar gösteriliyor.");
            Show3RandomCards();
        }
    }
}
