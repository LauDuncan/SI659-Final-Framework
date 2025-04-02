 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.SceneManagement;

 public class SceneTansitionScript : MonoBehaviour
 {
  public string sceneToLoad; // The name of the scene to load when this door is touched.
  public string startingSceneName; // Core scene name to unload after entering the portal.
  public Vector3 playerStartPosition = Vector3.zero; // The location to start in the Portal scene if desired.
  private bool canTeleport = false;

  private void OnTriggerEnter(Collider other)
  {
   Debug.Log("OnTriggerEnter called: Collision with " + other.gameObject.name); // ADDED - Check if trigger is called at all
   // Check if the colliding object is the player (you might need to adjust the tag check).
   Debug.Log("Colliding object tag is: " + other.gameObject.tag); // ADDED - Check the tag
   if (other.gameObject.CompareTag("Player") && canTeleport)
   {
    Debug.Log("Player entered door to " + sceneToLoad);
    LoadPortalScene();
   }
   else
   {
    Debug.Log("Collision was NOT with the player."); // ADDED - If the tag is wrong
   }
  }

  public void LoadPortalScene()
  {
   Debug.Log("LoadPortalScene function called. Loading: " + sceneToLoad + " and Unloading: " + startingSceneName); // ADDED - Make sure the function is called
   // Optionally Save Current Scene Position here if you will return.
   // PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

   // Saves the player's current coordinates
   // lastCoordinates = new Vector3(controller.transform.position.x, controller.transform.position.y, controller.transform.position.z);

   // Loads the new scene to the portal
   SceneManager.LoadScene(sceneToLoad);
   SceneManager.UnloadSceneAsync(startingSceneName); //Unload the core scene

   // Optionally Set the new portal position from the saved coordinates.
   // GameObject.FindGameObjectWithTag("Player").transform.position = playerStartPosition;

   // After the scenes are loaded
   // StartCoroutine(AfterSceneLoaded());

  }
  IEnumerator Start()
  {
   // Wait for 3 seconds
   yield return new WaitForSeconds(3);

   canTeleport = true;
  }

  /*
  IEnumerator AfterSceneLoaded()
  {
   yield return new WaitForEndOfFrame();

   //GameObject.FindGameObjectWithTag("Player").transform.position = playerStartPosition;
  }*/
 }