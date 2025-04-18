using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChange : MonoBehaviour
{
    // Name of the scene to load when collision occurs
    public string sceneName = "YourSceneName";

    // This function will be called when the collider enters the trigger
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object that collided has the desired tag or condition
        if (collision.gameObject.CompareTag("Player"))
        {
            // Load the new scene
            SceneManager.LoadScene(sceneName);
        }
    }
}
