using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyExploder : MonoBehaviour
{
    [Header("Patlama Koþulu")]
    public float explodeDistance = 1.25f;   // Oyuncuya bu mesafe veya daha yakýnsa patla
    public float armDelay = 0.05f;          // Ayný frame’de birden fazla tetiklenmeyi önlemek için

    [Header("Hasar Ayarlarý")]
    public float explosionDamage = 80f;     // Yüksek hasar (float)
    public float explosionRadius = 2.5f;    // Alan yarýçapý
    public LayerMask damageLayers;          // Player’ýn layer’ýný dahil et

    [Header("VFX / SFX")]
    public GameObject explosionVfxPrefab;   // Büyük patlama VFX
    public GameObject shatterVfxPrefab;     // Parçalanma VFX
    public float vfxLifetime = 3f;
    public AudioClip explosionSfx;
    public float sfxVolume = 0.85f;

    [Header("Kamera Sarsma")]
    public bool cameraShake = true;
    public float shakeDuration = 0.2f;
    public float shakeIntensity = 0.35f;

    [Header("Opsiyonel")]
    public bool disableRagdollOnExplode = true;

    // Dahili
    Transform player;
    EnemyHealth health;
    EnemyAI ai;
    NavMeshAgent agent;
    Animator anim;

    bool exploded = false;
    float lastArmTime = -999f;

    void Awake()
    {
        health = GetComponent<EnemyHealth>();
        ai = GetComponent<EnemyAI>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        exploded = false;
        lastArmTime = -999f;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        if (health != null) health.onDeath += OnEnemyDeath;
    }

    void OnDisable()
    {
        if (health != null) health.onDeath -= OnEnemyDeath;
    }

    void Update()
    {
        if (exploded || player == null || health == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= explodeDistance && Time.time - lastArmTime > armDelay)
        {
            lastArmTime = Time.time;
            Explode();
        }
    }

    void Explode()
    {
        if (exploded) return;
        exploded = true;

        // AI/Agent durdur
        if (ai != null) ai.enabled = false;
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }
        if (anim != null)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);
        }

        // Patlamada ragdoll istemiyorsan kapat
        if (disableRagdollOnExplode)
        {
            var rag = GetComponent<EnemyRagdoll>();
            if (rag) rag.enabled = false;
        }

        // VFX
        SpawnVfx(shatterVfxPrefab, transform.position);
        SpawnVfx(explosionVfxPrefab, transform.position);

        // SFX
        if (explosionSfx)
            AudioSource.PlayClipAtPoint(explosionSfx, transform.position, sfxVolume);

        // Kamera sarsma
        if (cameraShake && Camera.main)
        {
            var shaker = Camera.main.GetComponent<SimpleCameraShaker>();
            if (shaker) shaker.Shake(shakeDuration, shakeIntensity);
        }

        // Alan hasarý (IDamageable ve/veya PlayerHealth)
        ApplyAreaDamage();

        // Kendini öldür (EnemyHealth normal ölüm akýþý)
        health.TakeDamage(int.MaxValue);
    }

    void ApplyAreaDamage()
    {
        if (explosionRadius <= 0f || explosionDamage <= 0f) return;

        Collider[] hits = Physics.OverlapSphere(
            transform.position, explosionRadius, damageLayers, QueryTriggerInteraction.Ignore);

        HashSet<object> damaged = new HashSet<object>();

        foreach (var hit in hits)
        {
            // 1) IDamageable varsa onu tercih et (genel çözüm)
            var dmg = hit.GetComponentInParent<IDamageable>();
            if (dmg != null && !damaged.Contains(dmg))
            {
                dmg.TakeDamage(explosionDamage);
                damaged.Add(dmg);
                continue;
            }

            // 2) Emniyet için PlayerHealth’e özel yol (IDamageable yoksa da çalýþsýn)
            var ph = hit.GetComponentInParent<PlayerHealth>();
            if (ph != null && !damaged.Contains(ph))
            {
                ph.TakeDamage(explosionDamage);
                damaged.Add(ph);
            }
        }
    }

    void SpawnVfx(GameObject prefab, Vector3 pos)
    {
        if (!prefab) return;
        var go = Instantiate(prefab, pos, Quaternion.identity);
        if (vfxLifetime > 0f) Destroy(go, vfxLifetime);
    }

    void OnEnemyDeath()
    {
        // exploded == false ise patlama olmadan öldü: ekstra VFX/Shake yok.
        // exploded == true ise VFX zaten spawn edildi.
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.25f);
        Gizmos.DrawSphere(transform.position, explosionRadius);

        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, explodeDistance);
    }
#endif
}
