using UnityEngine;
using System.Collections;

public class SimpleCameraShaker : MonoBehaviour
{
    Vector3 originalLocalPos;
    Coroutine shakeCo;

    void Awake()
    {
        originalLocalPos = transform.localPosition;
    }

    public void Shake(float duration, float intensity)
    {
        if (shakeCo != null) StopCoroutine(shakeCo);
        shakeCo = StartCoroutine(DoShake(duration, intensity));
    }

    IEnumerator DoShake(float duration, float intensity)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localPosition = originalLocalPos + Random.insideUnitSphere * intensity;
            yield return null;
        }
        transform.localPosition = originalLocalPos;
        shakeCo = null;
    }
}
