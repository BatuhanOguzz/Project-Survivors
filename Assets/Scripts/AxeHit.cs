using UnityEngine;

public class AxeHit : MonoBehaviour
{
    public GameObject bloodParticlePrefab;
    public int damage = 40;

    public void SetDamage(int newDamage)
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
                health.TakeDamage(damage);

            if (bloodParticlePrefab != null)
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                GameObject fx = Instantiate(bloodParticlePrefab, hitPoint, Quaternion.identity);
                Destroy(fx, 2f);
            }
        }
    }
}
