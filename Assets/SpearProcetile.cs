using UnityEngine;

public class SpearProjectile : MonoBehaviour
{
    public float damage = 30f;
    public float speed = 20f;
    public float lifeTime = 4f; // Kaç saniye sonra otomatik yok olur

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Düz ileri git
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Spear çarptı: " + other.name); // ← Eklersen, çarptığında log göreceksin

        if (other.CompareTag("Enemy"))
        {
            var enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage((int)damage);
                Debug.Log("Enemy'ye hasar verildi: " + damage);
            }
        }
    }
}
