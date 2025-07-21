using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    public int currentXP = 0;

    public void AddXP(int amount)
    {
        currentXP += amount;
        // Seviye atlama, efekt, vb. ekleyebilirsin.
    }
}
