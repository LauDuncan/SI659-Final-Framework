using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public static SceneLoad Instance;
    public Scene sceneToLoad;

    private void Awake()
    {
        Instance = this;
    }

    public enum Scene
    {
        Scene1,
        Scene2,
        Scene3
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad.ToString());
    }
}
