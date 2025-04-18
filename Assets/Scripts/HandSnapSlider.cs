using UnityEngine;
using UnityEngine.SceneManagement;  // Required for scene management

public class HandSnapSlider : MonoBehaviour
{
    // Define three snap positions
    public Vector3 snapPoint1 = new Vector3(1.0f, 0.0f, 0.0f); // Example position
    public Vector3 snapPoint2 = new Vector3(2.0f, 0.0f, 0.0f); // Example position
    public Vector3 snapPoint3 = new Vector3(3.0f, 0.0f, 0.0f); // Example position

    // Define the snap threshold (how close the slider needs to be to a snap point)
    public float snapThreshold = 0.1f;

    private void Update()
    {
        // Get the current position of the slider
        Vector3 currentPosition = transform.position;

        // Check if the slider is close to any of the snap points
        if (Vector3.Distance(currentPosition, snapPoint1) < snapThreshold)
        {
            ChangeScene("9am");
        }
        else if (Vector3.Distance(currentPosition, snapPoint2) < snapThreshold)
        {
            ChangeScene("12noon");
        }
        else if (Vector3.Distance(currentPosition, snapPoint3) < snapThreshold)
        {
            ChangeScene("3pm");
        }
    }

    // Method to change the scene
    private void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);  // Load the scene based on the name
    }
}
