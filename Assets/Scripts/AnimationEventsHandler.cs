using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
    public AxeHit rightAxe;
    public AxeHit leftAxe;

    public void EnableRightHitbox()
    {
        if (rightAxe != null) rightAxe.EnableHitbox();
    }

    public void DisableRightHitbox()
    {
        if (rightAxe != null) rightAxe.DisableHitbox();
    }

    public void EnableLeftHitbox()
    {
        if (leftAxe != null) leftAxe.EnableHitbox();
    }

    public void DisableLeftHitbox()
    {
        if (leftAxe != null) leftAxe.DisableHitbox();
    }
}
