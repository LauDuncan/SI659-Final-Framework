using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulse : MonoBehaviour
{
    private Coroutine pulseCoroutine;
    private Vector3 originalScale; // Store the original scale of the object

    private void Awake()
    {
        // Save the original scale of the object when the script is initialized
        originalScale = transform.localScale;
    }

    // Call this method to start the pulsing effect
    public void StartPulsing(GameObject target, float pulseSpeed, float pulseScale)
    {
        // Reset the object's scale to its original size
        target.transform.localScale = originalScale;

        // Stop any existing pulsing coroutine
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
        }

        // Start a new pulsing coroutine
        pulseCoroutine = StartCoroutine(PulseEffect(target, pulseSpeed, pulseScale));
    }

    // Call this method to stop the pulsing effect
    public void StopPulsing(GameObject target)
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
        }

        // Reset the object's scale to its original size
        target.transform.localScale = originalScale;
    }

    // Coroutine to handle the pulsing effect
    private IEnumerator PulseEffect(GameObject target, float pulseSpeed, float pulseScale)
    {
        Vector3 originalScale = target.transform.localScale;
        float timer = 0f;

        while (true)
        {
            // Calculate the scale factor using a sine wave
            float scaleFactor = 1 + Mathf.Sin(timer * pulseSpeed) * pulseScale;

            // Apply the scale factor
            target.transform.localScale = originalScale * scaleFactor;

            // Increment the timer
            timer += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }
    }

    private void Start()
    {
        // Example usage: Start pulsing the GameObject this script is attached to
        StartPulsing(gameObject, 2f, 0.05f); // Adjust pulseSpeed and pulseScale as needed
    }
}