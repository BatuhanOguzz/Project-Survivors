using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    public float attackDistance = 1f; // Sald�r� menzili

    private NavMeshAgent agent;
    private Animator animator;
    private Transform target;

    void Start()
    {
        // NavMesh ve Animator referanslar�n� al
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = speed;

        // Oyuncuyu bul
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
    }

    void Update()
    {
        if (agent == null || animator == null || target == null)
            return;

        float distance = Vector3.Distance(transform.position, target.position);

        // Oyuncuyu takip et
        agent.SetDestination(target.position);

        // Y�r�me kontrol�
        bool isMoving = agent.velocity.magnitude > 0.1f && distance > attackDistance;
        animator.SetBool("isWalking", isMoving);

        // Sald�r� kontrol�
        bool isAttacking = distance <= attackDistance;
        animator.SetBool("isAttacking", isAttacking);

        // Sald�r� s�ras�nda durmas�n� istiyorsan bunu a�:
         agent.isStopped = isAttacking;
    }
}
