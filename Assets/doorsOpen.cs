using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorsOpen : MonoBehaviour
{
    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;
    public bool doorOpen = false;

    void OnTriggerEnter(Collider other)
    {
        //Check if PlayerCollider entered trigger
        if (other.gameObject.name == "player" && !doorOpen)
        {
            Debug.Log("Player entered door zone");
            leftDoorAnimator.SetBool("leftDoorOpen", true);
            rightDoorAnimator.SetBool("rightDoorOpen", true);
            doorOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
         //Check if PlayerCollider entered trigger
        if (other.gameObject.name == "player" && doorOpen)
        {
            Debug.Log("Player Exited door zone");
            leftDoorAnimator.SetTrigger("close");
            rightDoorAnimator.SetTrigger("close");
            doorOpen = false;
        }
    }
}
