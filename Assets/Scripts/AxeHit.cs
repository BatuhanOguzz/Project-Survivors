using UnityEngine;

public class AxeHit : MonoBehaviour
{
    public GameObject bloodParticlePrefab;
    public float damage = 40f;

    [Header("Sound")]
    public AudioClip hitSound;
    public float hitVolume = 1f;

    private AudioSource audioSource;

    void Awake()
    {
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

    private void OnTriggerEnter(Collider other)
    {
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

            // 🎵 Hit Sound
            if (hitSound != null && audioSource != null)
                audioSource.PlayOneShot(hitSound, hitVolume);
        }
    }
}
