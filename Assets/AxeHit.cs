using UnityEngine;

public class AxeHit : MonoBehaviour
{
    public GameObject bloodParticlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);

            if (bloodParticlePrefab != null)
            {
                GameObject fx = Instantiate(bloodParticlePrefab, hitPoint, Quaternion.identity);
                Destroy(fx, 8f); // 2 saniye sonra otomatik sil
            }
        }
    }
}
