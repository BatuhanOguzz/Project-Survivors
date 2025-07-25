using UnityEngine;

public class SpearThrowSkill : MonoBehaviour
{
    public bool skillActive = false;
    public float throwInterval = 2f;
    public GameObject spearPrefab;
    public float spearDamage = 10f;
    public float spearSpeed = 20f;

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

        Vector3 start = transform.position + Vector3.up * 1.2f;
        Vector3 targetPos = target.transform.position + Vector3.up * 0.7f;

        Vector3 dir = (targetPos - start).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);

        GameObject spear = Instantiate(spearPrefab, start, rot);

        var spearScript = spear.GetComponent<SpearProjectile>();
        if (spearScript != null)
        {
            spearScript.damage = spearDamage;
            spearScript.speed = spearSpeed;
        }
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
