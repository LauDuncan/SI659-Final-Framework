using UnityEngine;

public class avatarMoveTrigger : MonoBehaviour
{
    public Animator avatarAnimator; // Reference to the avatar's animator
    public bool avatarMove = false; // Boolean to check if the avatar is moving

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "player" && !avatarMove) // Ensure only the player triggers it
        {
            avatarAnimator.SetBool("move", true); // Set the move trigger
            avatarMove = true; // Set the boolean to true
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "player" && avatarMove) // Ensure only the player triggers it
        {
            avatarAnimator.SetBool("move", false); // Reset to idle when leaving the area
            avatarMove = false; // Set the boolean to false
        }

    }
}