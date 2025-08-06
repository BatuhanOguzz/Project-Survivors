using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AxeHit : MonoBehaviour
{
    public GameObject bloodParticlePrefab;
    public float damage = 40f;

    [Header("Sound")]
    public AudioClip hitSound;
    public float hitVolume = 1f;

    private AudioSource audioSource;
    private Collider weaponCollider;

    private bool isHitboxActive = false;

    void Awake()
    {
        weaponCollider = GetComponent<Collider>();
        weaponCollider.isTrigger = true;
        weaponCollider.enabled = false; // Baþlangýçta kapalý

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
        Debug.Log("Axe damage güncellendi: " + damage);
    }

    /// <summary>
    /// Animasyon Event ile çaðrýlacak
    /// </summary>
    public void EnableHitbox()
    {
        isHitboxActive = true;
        weaponCollider.enabled = true;
        Debug.Log("Hitbox ENABLED");
    }

    /// <summary>
    /// Animasyon Event ile çaðrýlacak
    /// </summary>
    public void DisableHitbox()
    {
        isHitboxActive = false;
        weaponCollider.enabled = false;
        Debug.Log("Hitbox DISABLED");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isHitboxActive) return; // Sadece aktifken hasar uygula

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
                health.TakeDamage(Mathf.RoundToInt(damage));

            if (bloodParticlePrefab != null)
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                GameObject fx = Instantiate(bloodParticlePrefab, hitPoint, Quaternion.identity);
                Destroy(fx, 2f);
            }

            if (hitSound != null && audioSource != null)
                audioSource.PlayOneShot(hitSound, hitVolume);
        }
    }
}
