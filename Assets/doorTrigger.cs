using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour
{
    public Animator doorAnimator;
    public bool doorOpen = false;

    void OnTriggerEnter(Collider other)
    {
        //Check if PlayerCollider entered trigger
        if (other.gameObject.name == "player" && !doorOpen)
        {
            Debug.Log("Player entered door zone");
            doorAnimator.SetBool("doorOpen", true);
            doorOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
         //Check if PlayerCollider entered trigger
        if (other.gameObject.name == "player" && doorOpen)
        {
            Debug.Log("Player Exited door zone");
            doorAnimator.SetBool("doorOpen", false);
            doorOpen = false;
        }
    }
}
