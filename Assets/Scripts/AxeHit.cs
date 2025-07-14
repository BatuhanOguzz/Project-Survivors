using UnityEngine;

public class AxeHit : MonoBehaviour
{
    public GameObject bloodParticlePrefab;
    public int damage = 40;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Can azalt
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // Kan efekti oluþtur
            if (bloodParticlePrefab != null)
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                GameObject fx = Instantiate(bloodParticlePrefab, hitPoint, Quaternion.identity);
                Destroy(fx, 2f); // 2 saniye sonra sil
            }
        }
    }
}
