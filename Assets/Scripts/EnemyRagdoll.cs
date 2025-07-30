using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    public Animator animator; // Enemy'nin Animator'u
    public Rigidbody[] ragdollBodies;
    public Collider[] ragdollColliders;

    void Awake()
    {
        SetRagdoll(false);
    }

    public void SetRagdoll(bool enabled)
    {
        foreach (var rb in ragdollBodies)
            rb.isKinematic = !enabled;

        foreach (var col in ragdollColliders)
            col.enabled = enabled;

        if (animator != null)
            animator.enabled = !enabled;

        // Ana Mesh Collider'ı kapat
        var mainCol = GetComponent<MeshCollider>();
        if (mainCol != null)
            mainCol.enabled = !enabled;
    }

    public void ActivateRagdoll()
    {
        SetRagdoll(true);
    }

    public void DeactivateRagdoll()
    {
        SetRagdoll(false);
    }

    // ✅ Ragdoll collider'larını player ile çarpışmadan çıkar
    public void IgnorePlayerCollision(Collider playerCollider)
    {
        foreach (var col in ragdollColliders)
        {
            if (col != null && playerCollider != null)
            {
                Physics.IgnoreCollision(col, playerCollider);
            }
        }
    }
}
