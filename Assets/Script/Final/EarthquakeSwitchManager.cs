using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeSwitchManager : MonoBehaviour
{
    // public GameObject[] buildings; // Assign in the inspector
    // public PowerLineDamage[] powerLines; // Assign in the inspector
    // public AudioSource damageSound; // Assign in the inspector
    [SerializeField] private GameObject earthquakeModel; // Assign in the inspector
    [SerializeField] private GameObject hurricaneModel; // Assign in the inspector
    [SerializeField] private GameObject smoke0; // Assign in the inspector
    [SerializeField] private GameObject smoke1; // Assign in the inspector
    [SerializeField] private GameObject smoke2; // Assign in the inspector
    [SerializeField] private GameObject earthquakeAudio; // Assign in the inspector

    // public void ToggleDamage() {
    //     // Check if any building is damaged
    //     bool anyBuildingDamaged = false;
    //     foreach (var building in buildings)
    //     {
    //         var buildingDamage = building.GetComponent<BuildingDamage>();
    //         if (buildingDamage != null && buildingDamage.isDamaged)
    //         {
    //             anyBuildingDamaged = true;
    //             break;
    //         }
    //     }

    // If any building is damaged, reset damage; otherwise, simulate damage
    //     if (anyBuildingDamaged)
    //     {
    //         ResetDamage();
    //     }
    //     else
    //     {
    //         SimulateDamage();
    //     }
    // }
    // public void SimulateDamageForSwitch()
    // {
    //     // Loop through all buildings and simulate damage
    //     foreach (var building in buildings)
    //     {
    //         var buildingDamage = building.GetComponent<BuildingDamage>();
    //         if (buildingDamage != null)
    //         {
    //             buildingDamage.SimulateDamage();
    //         }
    //         else
    //         {
    //             Debug.LogWarning($"GameObject {building.name} does not have a BuildingDamage component.");
    //         }
    //     }

    //     // Loop through all power lines and simulate electricity
    //     foreach (var powerLine in powerLines)
    //     {
    //         powerLine.SimulateElectricity();
    //     }

    //     // Play damage sound
    //     damageSound.Play();
    // }

    // public void ResetDamageForSwitch()
    // {
    //     // Loop through all buildings and reset damage
    //     foreach (var building in buildings)
    //     {
    //         var buildingDamage = building.GetComponent<BuildingDamage>();
    //         if (buildingDamage != null)
    //         {
    //             buildingDamage.ResetDamage();
    //         }
    //         else
    //         {
    //             Debug.LogWarning($"GameObject {building.name} does not have a BuildingDamage component.");
    //         }
    //     }

    //     // Loop through all power lines and reset electricity
    //     // foreach (var powerLine in powerLines)
    //     // {
    //     //     powerLine.ResetElectricity();
    //     // }
    // }

    public void Start()
    {
        smoke0.SetActive(false);
        smoke1.SetActive(false);
        smoke2.SetActive(false);
        earthquakeAudio.SetActive(false);
    }
    public void hoverEarthquake()
    {
        smoke0.SetActive(true);
        smoke1.SetActive(true);
        smoke2.SetActive(true);
        earthquakeAudio.SetActive(true);
    }

    public void unhoverEarthquake()
    {
        smoke0.SetActive(false);
        smoke1.SetActive(false);
        smoke2.SetActive(false);
        earthquakeAudio.SetActive(false);
    }

    public void switchToEarthquake()
    {
        earthquakeModel.SetActive(true);
        hurricaneModel.SetActive(false);
    }
}