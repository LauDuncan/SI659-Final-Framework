using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUIPanel : MonoBehaviour
{
    [System.Serializable]
    public class CuboidPanelPair
    {
        public GameObject cuboid;  // Specific cuboid
        public GameObject uiPanel; // Its specific UI panel
    }

    public List<CuboidPanelPair> cuboidPanelPairs = new List<CuboidPanelPair>();
    private Dictionary<GameObject, GameObject> panelDictionary = new Dictionary<GameObject, GameObject>();

    private void Start()
    {
        foreach (var pair in cuboidPanelPairs)
        {
            panelDictionary[pair.cuboid] = pair.uiPanel;
            pair.uiPanel.SetActive(false); // Hide all UI panels initially
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (panelDictionary.ContainsKey(other.gameObject)) // If the cuboid is in the dictionary
        {
            panelDictionary[other.gameObject].SetActive(true); // Show its specific UI panel
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (panelDictionary.ContainsKey(other.gameObject)) // If it's in the dictionary
        {
            panelDictionary[other.gameObject].SetActive(false); // Hide its specific UI panel
        }
    }
}
