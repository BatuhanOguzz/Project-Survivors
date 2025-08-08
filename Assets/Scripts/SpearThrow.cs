using UnityEngine;

public class SpearThrowSkill : MonoBehaviour
{
    [Header("Skill")]
    public bool skillActive = false;
    public float throwInterval = 2f;

    [Header("Projectile Prefab")]
    public GameObject spearPrefab;
    public float spearDamage = 10f;
    public float spearSpeed = 20f;

    [Header("Spawn")]
    public Vector3 localSpawnOffset = new Vector3(0f, 1.2f, 0.8f); // biraz yukarı ve ileri

    private float timer;

    void Update()
    {
        if (!skillActive) return;

        timer += Time.deltaTime;
        if (timer >= throwInterval)
        {
            timer = 0f;
            ThrowSpearAtNearestEnemy();
        }
    }

    void ThrowSpearAtNearestEnemy()
    {
        GameObject target = FindNearestEnemy();
        if (target == null) return;

        Vector3 start = transform.TransformPoint(localSpawnOffset);
        Vector3 targetPos = target.transform.position + Vector3.up * 0.7f;

        Vector3 dir = (targetPos - start).normalized;
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

        GameObject spear = Instantiate(spearPrefab, start, rot);

        var proj = spear.GetComponent<SpearProjectile>();
        if (proj != null)
        {
            proj.Init(owner: gameObject, damage: spearDamage, speed: spearSpeed);
        }
    }

    GameObject FindNearestEnemy()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return null;

        GameObject nearest = null;
        float minSqr = float.MaxValue;
        Vector3 p = transform.position;

        foreach (var e in enemies)
        {
            float d = (e.transform.position - p).sqrMagnitude;
            if (d < minSqr) { minSqr = d; nearest = e; }
        }
        return nearest;
    }
}
