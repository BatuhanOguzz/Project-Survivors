using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [HideInInspector] public int xpAmount = 1;
    public float pickupRadius = 4f;          // Oyuncuya çekilmeye başlama mesafesi
    public float moveSpeed = 8f;             // Çekilme hızı

    Transform player;

    void Start()
    {
        // Oyuncuyu bul
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < pickupRadius)
        {
            // Oyuncuya doğru yaklaş
            Vector3 targetPos = player.position + Vector3.up * 0.5f; // Yarı yüksekliğe yaklaşsın
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerXP playerXP = other.GetComponent<PlayerXP>();
            if (playerXP != null)
            {
                playerXP.AddXP(xpAmount);
            }
            Destroy(gameObject);
        }
    }
}
