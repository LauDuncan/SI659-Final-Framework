using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDamage : MonoBehaviour
{
    public bool isDamaged = false; // Track if the building is damaged
    public float fallSpeed = 1f; // How fast the building falls
    public GameObject damageEffect; // Reference to the object to show (e.g., a particle effect or visual indicator)

    private Vector3 originalPosition;
    private Coroutine damageCoroutine;

    public AudioSource damageSound; // Reference to the audio source for sound effects

    

    private void Start()
    {
        originalPosition = transform.position;

        // Ensure the damage effect is hidden at the start
        if (damageEffect != null)
        {
            damageEffect.SetActive(false);
        }
    }

    // Call this method to simulate the damage
    public void SimulateDamage()
    {
        if (isDamaged) return; // Prevent multiple calls
        isDamaged = true; // Mark the building as damaged

        // Show the damage effect
        if (damageEffect != null)
        {
            damageEffect.SetActive(true);
            
        }

        // Start the coroutine to simulate the building falling
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        damageCoroutine = StartCoroutine(FallCoroutine());
    }

    private IEnumerator FallCoroutine()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, -10, transform.position.z);

        // Ensure the building falls gradually
        while (transform.position.y > targetPosition.y)
        {
            // Move the building downward gradually
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                fallSpeed * Time.deltaTime // Scale movement by Time.deltaTime
            );

            // Wait for the next frame
            yield return null;
        }

        // Ensure the building snaps to the target position at the end
        transform.position = targetPosition;
    }

    // Call this method to reset the building to its original position
    public void ResetDamage()
    {
        isDamaged = false; // Mark the building as not damaged

        // Stop any ongoing fall coroutine
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }

        transform.position = originalPosition;

        // Hide the damage effect
        if (damageEffect != null)
        {
            damageEffect.SetActive(false);
            
        }

        // Stop any audio if an AudioSource is attached
        
        if (damageSound != null && damageSound.isPlaying)
        {
            damageSound.Stop();
        }
    }
}