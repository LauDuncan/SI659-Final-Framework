using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // Find the main camera
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Make the panel face the camera
        transform.LookAt(cameraTransform);

        // Reverse the panel so it doesn’t appear flipped
        transform.Rotate(0, 180, 0);
    }
}
