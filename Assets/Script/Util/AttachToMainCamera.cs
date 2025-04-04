using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToMainCamera : MonoBehaviour
{
    void Awake()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            transform.SetParent(mainCamera.transform, worldPositionStays: false);
        }
        else
        {
            Debug.LogError($"No main camera found, {name} did not attach to the main camera");
        }
    }
}
