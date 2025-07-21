using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [HideInInspector] public int xpAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerXP playerXP = other.GetComponent<PlayerXP>();
            if (playerXP != null)
            {
                playerXP.AddXP(xpAmount);
            }
            Destroy(gameObject);
        }
    }
}
