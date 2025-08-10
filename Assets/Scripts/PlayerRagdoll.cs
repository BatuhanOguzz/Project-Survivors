using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    public Animator animator;
    public Rigidbody[] ragdollBodies;
    public Collider[] ragdollColliders;

    void Awake()
    {
        EnableRagdoll(false);
    }

    public void EnableRagdoll(bool enabled)
    {
        foreach (var rb in ragdollBodies)
            rb.isKinematic = !enabled;
        foreach (var col in ragdollColliders)
            col.enabled = enabled;
        if (animator != null)
            animator.enabled = !enabled;
    }
}
