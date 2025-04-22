using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OVR; // Make sure you have the Oculus Integration imported

public class ToggleTipWithMenuButton : MonoBehaviour
{
    [Tooltip("The Controller Tip GameObject to toggle.")]
    public GameObject controllerTipObject;

    [Tooltip("Which controller's menu button to listen to.")]
    public OVRInput.Controller controller = OVRInput.Controller.LTouch; // LTouch for Left Menu/Start button

    void Start()
    {
        if (controllerTipObject == null)
        {
            Debug.LogError("ToggleTipWithMenuButton: Controller Tip Object not assigned!", this);
            this.enabled = false; // Disable script if reference is missing
            return;
        }

        // Optional: Try to sync initial state with the menu ONCE at the start
        // This requires a reference to the menu, uncomment and assign if needed.
        // GameObject menuPanel = GameObject.Find("Menu Panel"); // Or assign via Inspector
        // if (menuPanel != null)
        // {
        //     controllerTipObject.SetActive(menuPanel.activeSelf);
        // }
        // else
        // {
        //     // Default to hidden if menu isn't found or not assigned
        //     controllerTipObject.SetActive(false);
        // }
    }

    void Update()
    {
        if (controllerTipObject == null) return; // Safety check

        // Check if the Menu button (often called Start on Oculus controllers) was just pressed down THIS FRAME
        if (OVRInput.GetDown(OVRInput.Button.Start, controller))
        {
            // Toggle the active state of the controller tip
            controllerTipObject.SetActive(!controllerTipObject.activeSelf);
        }

         // --- Optional but Recommended: Add a safety check ---
         // If you have a reference to the menu panel, you could periodically
         // force the tip to match the menu's state in case they get out of sync.
         // GameObject menuPanel = ... // Get reference
         // if (menuPanel != null && controllerTipObject.activeSelf != menuPanel.activeSelf)
         // {
         //     controllerTipObject.SetActive(menuPanel.activeSelf);
         // }
         // --- End Optional Check ---
    }
}