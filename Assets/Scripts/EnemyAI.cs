using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    public float attackDistance = 1f;

    [Header("NavMesh Auto-Fix")]
    public bool autoFixOffMesh = true;
    public float reanchorRadius = 3f; // Yakındaki NavMesh'i ara
    public int navMeshAreaMask = NavMesh.AllAreas;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = speed;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) target = playerObj.transform;

        TryReanchorToNavMesh();
    }

    void OnEnable()
    {
        // Pool'dan dönünce ilk framede NavMesh'e oturt
        TryReanchorToNavMesh();
    }

    void Update()
    {
        if (agent == null || animator == null || target == null) return;

        // Agent kapalıysa veya NavMesh’te değilse SetDestination çağırma
        if (!agent.enabled) return;

        if (!agent.isOnNavMesh)
        {
            if (autoFixOffMesh) TryReanchorToNavMesh();
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        // Takip
        agent.SetDestination(target.position);

        // Animasyon
        bool isAttacking = distance <= attackDistance;
        bool isMoving = agent.velocity.magnitude > 0.1f && !isAttacking;

        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isAttacking", isAttacking);

        // Saldırırken dur
        agent.isStopped = isAttacking;
    }

    bool TryReanchorToNavMesh()
    {
        if (agent == null || !agent.enabled) return false;
        if (agent.isOnNavMesh) return true;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, reanchorRadius, navMeshAreaMask))
        {
            // Agent'ı güvenli şekilde en yakın NavMesh noktasına ışınla
            agent.Warp(hit.position);
            return true;
        }
        return false; // Yakında NavMesh yok → spawn noktasını düzeltmelisin
    }
}
