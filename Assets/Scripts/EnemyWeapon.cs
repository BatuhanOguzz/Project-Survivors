using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int damage = 10; // Vuruş başına hasar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"[EnemyWeapon] Player hasar aldı: {damage} dmg, yeni canı: {playerHealth.currentHealth}");
            }
        }
    }
}
