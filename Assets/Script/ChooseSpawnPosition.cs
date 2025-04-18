using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class ChooseSpawnPosition : MonoBehaviour
{
    public GameObject primaryHand;
    public MRUKAnchor.SceneLabels labels;
    
    public GameObject gears;

    public GameObject locationIndicator;
    

    private Transform rayStartPoint;
    private float rayLength = 5;

    private GameObject triggerTip;
    private GameObject gripTip;
    private GameObject checkIcon;
    private GameObject crossIcon;
    private TMPro.TextMeshPro locationIndicatorText;

    private Color successColor = new Color(0, 1, 0);
    private Color failureColor = new Color(1, 0, 0);

    private bool gearPlaced = false;

    void Start()
    {
        checkIcon = locationIndicator.transform.Find("CheckIcon").gameObject;
        crossIcon = locationIndicator.transform.Find("CrossIcon").gameObject;
        rayStartPoint = primaryHand.transform;
        triggerTip = primaryHand.transform.Find("TriggerTip").gameObject;
        gripTip = primaryHand.transform.Find("GripTip").gameObject;
        triggerTip.SetActive(true);
        gripTip.SetActive(false);
        locationIndicatorText = locationIndicator.transform.GetComponentInChildren<TMPro.TextMeshPro>();
    }

    void Update()
    {
        if (!gearPlaced) {
            Transform cameraTransform = primaryHand.transform;
            Ray ray = new Ray(rayStartPoint.position, rayStartPoint.forward);

            MRUKRoom room = MRUK.Instance.GetCurrentRoom();
            bool hasHit = room.Raycast(ray, rayLength, new LabelFilter(labels), out RaycastHit hit, out MRUKAnchor anchor);
            if (hasHit)
            {
                Vector3 hitPoint = hit.point;
                Vector3 hitNormal = hit.normal;

                MRUKAnchor.SceneLabels hitLabel = anchor.Label;
                float hitNormalDir = Vector3.Dot(hit.normal, Vector3.up);
                bool hitHorizontalTopSurface = hitNormalDir > 0.99f;
                bool hitHorizontalBottomSurface = hitNormalDir < -0.99f;
                bool hitTable = hitLabel == MRUKAnchor.SceneLabels.TABLE || hitLabel == MRUKAnchor.SceneLabels.FLOOR;
                bool hitTableTop = hitTable && hitHorizontalTopSurface;
                checkIcon.SetActive(hitTableTop);
                crossIcon.SetActive(!hitTableTop);

                locationIndicator.transform.position = hitPoint;
                
                if (hitHorizontalBottomSurface || hitHorizontalTopSurface)
                {
                    // Ensure locationIndicatior rotates with the camera
                    Vector3 toCamera = cameraTransform.position - hitPoint;
                    toCamera.y = 0;
                    toCamera = toCamera.normalized;

                    //Vector3 upVector = hitHorizontalSurfaceTop ? -toCamera : toCamera;
                    int topSurfaceFactor = hitHorizontalTopSurface ? -1 : 1;
                    locationIndicator.transform.rotation = Quaternion.LookRotation(-hitNormal, topSurfaceFactor * toCamera);
                } else
                {
                    locationIndicator.transform.rotation = Quaternion.LookRotation(-hitNormal);
                }

                locationIndicatorText.text = hitTableTop ? "Spawn Here" : "Can't Spawn";
                locationIndicatorText.color = hitTableTop ? successColor : failureColor;

                if (hitTableTop && OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    Debug.Log("Place Gears");
                    Vector3 locationIndicatorRot = locationIndicator.transform.eulerAngles;
                    Quaternion gearRotation = Quaternion.Euler(0, locationIndicatorRot.y, locationIndicatorRot.z);

                    Instantiate(gears, hitPoint, gearRotation);
                    locationIndicator.SetActive(false);
                    triggerTip.SetActive(false);
                    gripTip.SetActive(true);
                    gearPlaced = true;
                }
            }
        }
        
    }

    private void PlaceGear()
    {
        //TODO
    }
}
