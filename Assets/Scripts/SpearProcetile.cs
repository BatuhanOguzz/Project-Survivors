using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpearProjectile : MonoBehaviour
{
    [Header("Motion")]
    public float speed = 20f;
    public float lifeTime = 5f;

    [Tooltip("Balistik (yay) istiyorsan true yap. Rigidbody.gravity açık olmalı.")]
    public bool useBallistic = false;

    [Header("Combat")]
    public float damage = 10f;
    public LayerMask hitLayers = ~0;
    public bool destroyOnHit = true;
    public int pierceCount = 0;

    [Header("FX (opsiyonel)")]
    public GameObject hitEffectPrefab;

    [Header("Model Alignment")]
    [Tooltip("Sadece görsel child (mesh). Root hareketi/çarpışma için kalır.")]
    public Transform model;
    [Tooltip("Modelin ucunu +Z ile hizalamak için düzeltme (ör. 0,0,90).")]
    public Vector3 modelForwardFixEuler = Vector3.zero;

    private GameObject owner;
    private Collider projCol;
    private Rigidbody rb;
    private int remainingPierces;

    public void Init(GameObject owner, float damage, float speed)
    {
        this.owner = owner;
        this.damage = damage;
        this.speed = speed;

        remainingPierces = pierceCount;

        // Owner çarpışmasını kapat
        IgnoreOwnerCollisions(true);

        // Başlangıç hızı
        if (rb != null)
        {
            if (useBallistic)
            {
                rb.useGravity = true; // balistikte gravity açık olmalı
                rb.linearVelocity = transform.forward * this.speed;
            }
            else
            {
                rb.useGravity = false;
                rb.linearVelocity = transform.forward * this.speed;
            }
        }

        AlignModelToForward();
    }

    void Awake()
    {
        projCol = GetComponent<Collider>();
        projCol.isTrigger = true; // trigger sürümü

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }

    void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(Despawn), lifeTime);
        remainingPierces = pierceCount;

        if (rb != null)
        {
            if (useBallistic)
            {
                rb.useGravity = true;
                rb.linearVelocity = transform.forward * speed;
            }
            else
            {
                rb.useGravity = false;
                rb.linearVelocity = transform.forward * speed;
            }
        }

        AlignModelToForward();
    }

    void OnDisable()
    {
        IgnoreOwnerCollisions(false);
        owner = null;
        if (rb != null) rb.linearVelocity = Vector3.zero;
    }

    void Update()
    {
        // Düz çizgi modunda RB yoksa manuel ilerlet
        if (!useBallistic)
        {
            if (rb == null)
                transform.position += transform.forward * speed * Time.deltaTime;
            else
                rb.linearVelocity = transform.forward * speed; // sabit tut
        }
        // Balistikte RB+gravity işi yapar, elle hız verme!
        AlignModelToForward();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((hitLayers.value & (1 << other.gameObject.layer)) == 0) return;
        if (owner != null && IsOwnersCollider(other)) return;

        if (other.CompareTag("Enemy"))
        {
            var enemyHealth = other.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Mathf.RoundToInt(damage));
                if (hitEffectPrefab != null)
                {
                    Vector3 p = other.ClosestPoint(transform.position);
                    Instantiate(hitEffectPrefab, p, Quaternion.identity);
                }

                if (destroyOnHit && remainingPierces <= 0) { Despawn(); return; }
                else if (!destroyOnHit && remainingPierces > 0) { remainingPierces--; }
                else if (!destroyOnHit && pierceCount == 0) { Despawn(); return; }
            }
        }
        // İstersen başka şeye değince de yok et:
        // else { Despawn(); }
    }

    // --- Helpers ---
    void AlignModelToForward()
    {
        if (model == null) return;

        // Modeli mızrağın ileri yönüne çevir + düzeltme açısı uygula
        var baseRot = Quaternion.LookRotation(transform.forward, Vector3.up);
        model.rotation = baseRot * Quaternion.Euler(modelForwardFixEuler);
    }

    bool IsOwnersCollider(Collider c)
    {
        if (owner == null || c == null) return false;
        var oc = owner.GetComponent<Collider>();
        if (oc != null && c == oc) return true;
        var ocs = owner.GetComponentsInChildren<Collider>();
        foreach (var col in ocs) if (c == col) return true;
        return false;
    }

    void IgnoreOwnerCollisions(bool ignore)
    {
        if (projCol == null || owner == null) return;
        var ownerCols = owner.GetComponentsInChildren<Collider>();
        foreach (var oc in ownerCols)
            if (oc != null) Physics.IgnoreCollision(projCol, oc, ignore);
    }

    void Despawn()
    {
        Destroy(gameObject);
    }
}
