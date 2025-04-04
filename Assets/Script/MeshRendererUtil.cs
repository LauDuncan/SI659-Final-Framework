using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererUtil : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.LogError($"MeshRenderer is not attached to the GameObject {gameObject.name}");
        }
    }

    private void Update()
    {
        Debug.Log($"meshRendererUtil - enabled: {meshRenderer.enabled}");
    }

    public void enableMeshRenderer()
    {
        meshRenderer.enabled = true;
        Debug.Log($"meshRenderer enabled, actual: {meshRenderer.enabled}");
    }

    public void disableMeshRenderer()
    {
        meshRenderer.enabled = false;
        Debug.Log($"meshRenderer disabled, actual: {meshRenderer.enabled}");
    }

    public void toggleMeshRenderer()
    {
        meshRenderer.enabled = !meshRenderer.enabled;
        Debug.Log($"meshRenderer toggled, actual: {meshRenderer.enabled}");
    }
}
