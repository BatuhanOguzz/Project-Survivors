using UnityEngine;

public class KickAoESkill : MonoBehaviour
{
    public bool skillActive = false;
    public float aoeInterval = 5f;
    public float aoeRadius = 4f;
    public float damage = 20f;

    public GameObject aoeFXPrefab; // Inspector’dan FX prefabını ata
    public float fxDuration = 1.2f; // FX kaç sn sonra silinecek

    private float timer;

    void Update()
    {
        if (!skillActive) return;

        timer += Time.deltaTime;
        if (timer >= aoeInterval)
        {
            timer = 0f;
            DoAoEAttack();
        }
    }

    void DoAoEAttack()
    {
        Vector3 center = transform.position;

        // FX oluştur (yere basık, ayak hizasında)
        if (aoeFXPrefab != null)
        {
            GameObject fx = Instantiate(aoeFXPrefab, center + Vector3.up * 0.05f, Quaternion.identity);
            fx.transform.localScale = Vector3.one * aoeRadius * 0.5f; // Alanla orantılı büyüklük
            Destroy(fx, fxDuration);
        }

        Collider[] hits = Physics.OverlapSphere(center, aoeRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var enemyHealth = hit.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                        enemyHealth.TakeDamage((int)damage);
            }
        }

        Debug.Log("AoE Kick FX ile vuruldu! Alan: " + aoeRadius + ", Hasar: " + damage);
    }
}
