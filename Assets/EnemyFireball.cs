using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyFireballShooter : MonoBehaviour
{
    [Header("Refs")]
    public Transform firePoint;
    public GameObject fireballPrefab;
    public GameObject explosionVFXPrefab;

    [Header("Tuning")]
    public float shootCooldown = 1.0f;
    public float projectileSpeed = 6f;
    public float homingStrength = 3f;
    public float projectileLifeTime = 10f;
    public float projectileDamage = 10f;

    [Header("Animator")]
    public string attackBoolName = "isAttacking";

    [Header("Rules")]
    public bool singleActiveProjectile = true; // <<< SADECE 1 aktif mermi olsun

    Animator _anim;
    float _nextShootTime;
    bool _prevIsAttacking;
    GameObject _activeProjectile;            // <<< þu anki mermi referansý

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        bool isAttacking = _anim != null && _anim.GetBool(attackBoolName);

        // Attack baþladýðý an bir kez dene (Animation Event kullanýyorsan Update kýsmýný kapatabilirsin)
        if (isAttacking && !_prevIsAttacking)
        {
            TryFire();
        }

        _prevIsAttacking = isAttacking;
    }

    // Attack klibine event eklersen buradan çaðýr
    public void AnimationEvent_Fire()
    {
        TryFire();
    }

    void TryFire()
    {
        if (Time.time < _nextShootTime) return;
        if (fireballPrefab == null || firePoint == null) return;

        // <<< Tek aktif mermi kuralý
        if (singleActiveProjectile && _activeProjectile != null) return;

        var go = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
        _activeProjectile = go; // <<< kilitle

        var fb = go.GetComponent<HomingFireball>();
        if (fb != null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) fb.SetTarget(player.transform);

            fb.speed = projectileSpeed;
            fb.homingStrength = homingStrength;
            fb.lifeTime = projectileLifeTime;
            fb.damage = projectileDamage;
            fb.explosionVFXPrefab = explosionVFXPrefab;

            // <<< Shooter referansýný ver ki mermi yok olunca kilidi açsýn
            fb.owner = this;
        }

        _nextShootTime = Time.time + shootCooldown;
    }

    // <<< Mermi yok olduðunda çaðrýlacak
    public void OnProjectileDestroyed(HomingFireball projectile)
    {
        if (_activeProjectile == null) return;
        // güvenlik: ayný mermi mi kontrol et
        if (projectile != null && projectile.gameObject == _activeProjectile)
        {
            _activeProjectile = null; // kilidi aç
        }
        else
        {
            // fallback
            _activeProjectile = null;
        }
    }
}
