using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutePlanning : MonoBehaviour
{
    public GameObject primaryHand;
    public GameObject linePrefab;

    // Add a parent transform in the inspector
    // (the object you want the line to move with)
    public Transform lineParent;

    public List<GameObject> routeLines = new List<GameObject>();

    private GameObject routeLineHelper;
    private Transform trackingReference;
    private LineRenderer currentLineRenderer;
    

    private float lineWidth = 0.01f;
    private bool isDrawing = false;
    private bool allowDrawing = false;
    private Vector3 startPosition;

    void Start()
    {
        // For Oculus, you might be using the right hand or left hand
        // Or any transform that indicates the controller position
        trackingReference = transform.Find("RouteLineHelper");
        
        routeLineHelper = trackingReference.gameObject;
        print(routeLineHelper.name);
    }

    void Update()
    {
        if (allowDrawing)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                StartDrawing();
            }
            else if (isDrawing && OVRInput.Get(OVRInput.RawButton.A))
            {
                UpdateDrawing();
            }
            else if (isDrawing && OVRInput.GetUp(OVRInput.RawButton.A))
            {
                StopDrawing();
            }
        }
        

        if (OVRInput.GetDown(OVRInput.RawButton.B)) 
        {
            // Delete the last line
            if (routeLines.Count > 0)
            {
                int previousLineIndex = routeLines.Count - 1;
                Destroy(routeLines[previousLineIndex]);
                routeLines.RemoveAt(previousLineIndex);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Controller"))
        {
            routeLineHelper.SetActive(true);
            allowDrawing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Controller"))
        {
            routeLineHelper.SetActive(false);
            allowDrawing = false;
        }
    }

    void StartDrawing()
    {
        if (trackingReference == null || lineParent == null)
            return;

        isDrawing = true;

        // We'll store the *world* position first
        startPosition = trackingReference.position;

        // Instantiate the line as a child of 'lineParent' 
        // so the entire line object moves with that parent
        GameObject newRouteLine = Instantiate(linePrefab, lineParent);
        routeLines.Add(newRouteLine);
        

        currentLineRenderer = newRouteLine.GetComponent<LineRenderer>();
        SetupLineRenderer(currentLineRenderer);

        // IMPORTANT: Because we want the line to follow the parent,
        // we set useWorldSpace = false, meaning "positions are local."
        currentLineRenderer.useWorldSpace = false;

        // Convert the controller’s *world* startPosition to the lineParent’s local coordinates
        Vector3 localStart = lineParent.InverseTransformPoint(startPosition);

        // Initialize positions in local space
        currentLineRenderer.SetPosition(0, localStart);
        currentLineRenderer.SetPosition(1, localStart);
    }

    void UpdateDrawing()
    {
        if (!isDrawing || currentLineRenderer == null || lineParent == null)
            return;

        // Convert current controller world-position to parent's local space
        Vector3 currentWorldPos = trackingReference.position;
        Vector3 localPos = lineParent.InverseTransformPoint(currentWorldPos);

        currentLineRenderer.SetPosition(1, localPos);
    }

    void StopDrawing()
    {
        if (trackingReference == null || lineParent == null)
            return;

        isDrawing = false;

        // Final local end position
        Vector3 endWorldPos = trackingReference.position;
        Vector3 localEnd = lineParent.InverseTransformPoint(endWorldPos);

        currentLineRenderer.SetPosition(1, localEnd);
        currentLineRenderer = null;
    }

    void SetupLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 2; 
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // This is the key for letting the parent's transform control the line:
        // positions passed to SetPosition are interpreted as local coords.
        lineRenderer.useWorldSpace = false;  

        lineRenderer.numCapVertices = 5; // Smoother caps
    }
}
