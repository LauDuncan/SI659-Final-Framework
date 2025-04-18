using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLineDamage : MonoBehaviour
{
    public GameObject sparksPrefab; // Reference to spark particles or prefab

    private bool isDamaged = false;

    public void SimulateElectricity()
    {
        if (!isDamaged)
        {
            if (sparksPrefab != null)
            {
                // Simulate sparks from power lines
                Instantiate(sparksPrefab, transform.position, Quaternion.identity);
                isDamaged = true;
            }
            else
            {
                Debug.LogWarning("Sparks prefab is not assigned to PowerLineDamage.");
            }
        }
    }

    // Call this method to reset the power line damage
    public void ResetDamage()
    {
        isDamaged = false;
    }
}