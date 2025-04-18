using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public GameObject[] buildings; // Assign in the inspector

    public AudioSource damageSound; // Assign in the inspector

    public GameObject arrow;

    public GameObject avatar;
    private GameObject avatarInstance; // Store the instance of the avatar

    public Vector3[] spawnLocations;

    public ButtonState buttonState; // Reference to the ButtonState script

    public pulse pulseState;

    public GameObject button;

    public void ToggleDamage() {
        // Check if any building is damaged
        bool anyBuildingDamaged = false;
        foreach (var building in buildings)
        {
            var buildingDamage = building.GetComponent<BuildingDamage>();
            if (buildingDamage != null && buildingDamage.isDamaged)
            {
                anyBuildingDamaged = true;
                break;
            }
        }

        // If any building is damaged, reset damage; otherwise, simulate damage
        if (anyBuildingDamaged)
        {
            ResetDamage();
            arrow.SetActive(false); // Hide the arrow when resetting damage
            buttonState.SetState(ButtonState.State.Undamaged); // Reset button state to undamaged
        }
        else
        {
            SimulateDamage();
            arrow.SetActive(true); // Show the arrow when simulating damage
            buttonState.SetState(ButtonState.State.Damaging); // Set button state to damaging
            StartCoroutine(WaitForAudioToEnd()); // Start the coroutine to wait for audio to finish
        }
    }
    private void SimulateDamage()
    {
        // Loop through all buildings and simulate damage
        foreach (var building in buildings)
        {
            var buildingDamage = building.GetComponent<BuildingDamage>();
            if (buildingDamage != null)
            {
                buildingDamage.SimulateDamage();
            }
            else
            {
                Debug.LogWarning($"GameObject {building.name} does not have a BuildingDamage component.");
            }
        }


        // Play damage sound
        damageSound.Play();

        pulseState.StopPulsing(button); // Stop any existing pulsing effect

        if (avatar != null && spawnLocations.Length > 0)
        {
            // Spawn the avatar at a random location
            int randomIndex = Random.Range(0, spawnLocations.Length);
            Vector3 spawnPosition = spawnLocations[randomIndex];
            avatarInstance = Instantiate(avatar, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Avatar or spawn locations are not set correctly.");
        }
    }

    private void ResetDamage()
    {
        // Loop through all buildings and reset damage
        foreach (var building in buildings)
        {
            var buildingDamage = building.GetComponent<BuildingDamage>();
            if (buildingDamage != null)
            {
                buildingDamage.ResetDamage();
            }
            else
            {
                Debug.LogWarning($"GameObject {building.name} does not have a BuildingDamage component.");
            }
        }

        if (avatarInstance != null)
        {
            Destroy(avatarInstance); // Destroy the avatar instance
            avatarInstance = null; // Reset the instance reference
        }

    }

    private IEnumerator WaitForAudioToEnd()
    {
        // Wait until the audio finishes playing
        while (damageSound.isPlaying)
        {
            yield return null; // Wait for the next frame
        }

        // Change the button state to "Damaged" after the audio ends
        buttonState.SetState(ButtonState.State.Damaged);
        pulseState.StartPulsing(button, 2f, 0.05f); // Start pulsing effect
    }
}