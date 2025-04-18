using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject detectiveVisionPrefab;  // Assign your prefab in the Inspector
    public OVRHand leftHand;
    public OVRHand rightHand;

    public float clapThreshold = 0.1f; // Adjust based on testing

    private bool isClapping = false;

    void Update()
    {
        if (DetectClap())
        {
            if (!isClapping)
            {
                isClapping = true;
                EnableDetectiveVision();
                StartCoroutine(DisableAfterSeconds(3)); // Disable after 3 seconds
            }
        }
        else
        {
            isClapping = false;
        }
    }

    bool DetectClap()
    {
        // Get the positions of the palms
        Vector3 leftPalmPos = leftHand.transform.position;
        Vector3 rightPalmPos = rightHand.transform.position;

        // Check if the hands are close enough
        float distance = Vector3.Distance(leftPalmPos, rightPalmPos);
        return distance < clapThreshold;
    }

    void EnableDetectiveVision()
    {
        if (detectiveVisionPrefab != null)
        {
            detectiveVisionPrefab.SetActive(true);
        }
    }

    IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (detectiveVisionPrefab != null)
        {
            detectiveVisionPrefab.SetActive(false);
        }
    }
}
