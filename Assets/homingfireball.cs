using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HomingFireball : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 12f;
    public float homingStrength = 10f;
    public float lifeTime = 10f;

    [Header("Combat")]
    public float damage = 10f;

    [Header("VFX")]
    public GameObject explosionVFXPrefab;

    [HideInInspector] public EnemyFireballShooter owner; // <<< Shooter referansı

    Rigidbody _rb;
    Transform _target;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime); // süre dolunca yok ol
    }

    public void SetTarget(Transform target) => _target = target;

    void FixedUpdate()
    {
        if (_target == null)
        {
            _rb.linearVelocity = transform.forward * speed;
            return;
        }

        Vector3 dir = (_target.position - transform.position).normalized;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, homingStrength * Time.fixedDeltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newDir);

        _rb.linearVelocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var hp = other.GetComponent<PlayerHealth>();
        if (hp != null) hp.TakeDamage(damage);

        Explode();
    }

    void Explode()
    {
        if (explosionVFXPrefab != null)
        {
            var vfx = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 5f);
        }
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        // <<< Yok olurken Shooter’a haber ver
        if (owner != null)
            owner.OnProjectileDestroyed(this);
    }
}
