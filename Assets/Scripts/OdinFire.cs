using UnityEngine;
using DG.Tweening;

public class OdinFireSkill : MonoBehaviour
{
    public bool skillActive = true;             // Sadece kart gelince true olacak
    public float fireInterval = 2f;              // Kaç saniyede bir atacak
    public GameObject molotovPrefab;             // DOTween ile uçacak prefab
    public float flightTime = 1f;              // Havada kalma süresi
    public float arcHeight = 2.5f;               // Yüksekliğin tepe noktası
    public float damage = 10f;

    public GameObject explosionPrefab; // Patlama efektini burada ata (Inspector’dan)
    public float explosionRadius = 2.5f;

    private float timer;

    void Update()
    {
        if (!skillActive) return;

        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            timer = 0f;
            FireMolotovAtNearestEnemy();
        }
    }

    void FireMolotovAtNearestEnemy()
    {
        GameObject target = FindNearestEnemy();
        if (target == null) return;

        Vector3 start = transform.position + Vector3.up * 1.2f; // Karakterin biraz üstü
        Vector3 end = target.transform.position + Vector3.up * 0.7f; // Düşmanın biraz üstü

        GameObject molotov = Instantiate(molotovPrefab, start, Quaternion.identity);

        // Arc için ara nokta (tepe noktası)
        Vector3 mid = Vector3.Lerp(start, end, 0.5f) + Vector3.up * arcHeight;

        // DOTween ile path animasyonu
        Sequence seq = DOTween.Sequence();
        seq.Append(molotov.transform.DOPath(new Vector3[] { mid, end }, flightTime, PathType.CatmullRom)
            .SetEase(Ease.OutQuad)
            .SetLookAt(0.01f));
        seq.OnComplete(() =>
        {
            // Patlama efekti
            if (explosionPrefab != null)
                Instantiate(explosionPrefab, molotov.transform.position, Quaternion.identity);

            // Alan hasarı
            Collider[] hits = Physics.OverlapSphere(molotov.transform.position, explosionRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    var enemyHealth = hit.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                        enemyHealth.TakeDamage((int)damage); // veya float damage uyumluysa direkt damage
                }
            }

            Destroy(molotov);
            Debug.Log("Molotov patladı ve alan hasarı verdi! Damage: " + damage);
        });
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return null;
        GameObject nearest = enemies[0];
        float minDist = Vector3.Distance(transform.position, nearest.transform.position);
        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = e;
            }
        }
        return nearest;
    }
}
