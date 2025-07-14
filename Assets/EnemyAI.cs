using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    private NavMeshAgent agent;

    void Start()
    {
        EnemyManager.Instance.RegisterEnemy(this);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    public void ManualUpdate(Vector3 targetPosition)
    {
        if (agent != null)
            agent.SetDestination(targetPosition);
    }

    void OnDestroy()
    {
        if (EnemyManager.Instance != null)
            EnemyManager.Instance.UnregisterEnemy(this);
    }
}
