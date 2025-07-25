using UnityEngine;

public class CrowOrbitSkill : MonoBehaviour
{
    public bool skillActive = false;
    public GameObject axePrefab;
    public float orbitRadius = 2f;
    public float orbitSpeed = 120f;      // Orbitte dönme hızı (derece/sn)
    public float spinSpeed = 250f;       // Balta kendi ekseninde dönme hızı (Y)
    public float damage = 10f;
    public int axeCount = 2;
    public float orbitHeight = 1.8f;     // Baltalar yukarıda dönecek

    private GameObject[] axes;
    private float[] axeAngles;

    void Update()
    {
        if (!skillActive) return;

        // Eksik prefab kontrolü
        if (axePrefab == null)
        {
            Debug.LogWarning("Axe prefabı atanmadı!");
            return;
        }

        // Baltalar yoksa oluştur
        if (axes == null || axes.Length == 0)
        {
            axes = new GameObject[axeCount];
            axeAngles = new float[axeCount];
            for (int i = 0; i < axeCount; i++)
            {
                axeAngles[i] = (360f / axeCount) * i;
                axes[i] = Instantiate(axePrefab, transform.position, Quaternion.Euler(-90, -90, 0));
                var axeProj = axes[i].GetComponent<CrowProjectile>();
                if (axeProj != null) axeProj.damage = damage;
            }
        }

        // Orbit hareketi ve Y ekseninde spin
        for (int i = 0; i < axes.Length; i++)
        {
            if (axes[i] == null) continue;

            axeAngles[i] += orbitSpeed * Time.deltaTime;
            if (axeAngles[i] > 360f) axeAngles[i] -= 360f;
            float rad = axeAngles[i] * Mathf.Deg2Rad;

            Vector3 orbitPos = transform.position
                            + new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * orbitRadius
                            + Vector3.up * orbitHeight;

            axes[i].transform.position = orbitPos;

            // İstediğin gibi: X=0, Y=spin, Z=90
            axes[i].transform.rotation = Quaternion.Euler(0, spinSpeed * Time.time, 90);

            var axeProj = axes[i].GetComponent<CrowProjectile>();
            if (axeProj != null) axeProj.damage = damage;
        }
    }
}
