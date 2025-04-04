using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RoomToSceneTransition : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [Tooltip("Name of the scene to load after countdown")]
    [SerializeField] private string targetSceneName;
    
    [Tooltip("Duration of the countdown in seconds")]
    [SerializeField] private float countdownDuration = 5f;
    
    [Header("UI References")]
    [Tooltip("Text component to display the countdown")]
    [SerializeField] private TMP_Text countdownText;
    
    [Tooltip("Canvas group to fade in/out the countdown UI")]
    [SerializeField] private GameObject countdownCanvas;

    [Header("Transition Effects")]
    [Tooltip("Optional fade screen controller for transition effect")]
    [SerializeField] private FadeScreenController fadeScreenController;

    private bool playerInside = false;
    private bool countdownStarted = false;
    private bool isLoadingScene = false;
    private float currentCountdown;
    private Coroutine loadSceneCoroutine;

    private void Start()
    {
        // Hide the countdown text at start
        if (countdownCanvas != null)
        {
            countdownCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInside && !countdownStarted && !isLoadingScene)
        {
            StartCountdown();
        }

        if (countdownStarted && !isLoadingScene)
        {
            UpdateCountdown();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is the player
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            
            // Cancel the countdown if player exits and we're not already loading a scene
            if (countdownStarted && !isLoadingScene)
            {
                ResetCountdown();
            }
        }
    }

    private void StartCountdown()
    {
        countdownStarted = true;
        isLoadingScene = false;
        currentCountdown = countdownDuration;
        
        // Show countdown UI
        if (countdownCanvas != null)
        {
            countdownCanvas.SetActive(true);
        }
        
        UpdateCountdownText();
    }

    private void StopCountdown()
    {
        countdownStarted = false;
        
        // Hide countdown UI
        if (countdownCanvas != null)
        {
            countdownCanvas.SetActive(false);
        }
    }

    private void ResetCountdown()
    {
        // Stop any current scene loading in progress
        if (loadSceneCoroutine != null)
        {
            StopCoroutine(loadSceneCoroutine);
            loadSceneCoroutine = null;
        }
        
        // Reset countdown state
        countdownStarted = false;
        isLoadingScene = false;
        
        // Reset fade controller if it was in the middle of fading out
        if (fadeScreenController != null)
        {
            fadeScreenController.FadeIn();
        }
        
        if (countdownCanvas != null)
        {
            countdownCanvas.SetActive(false);
        }
        
        Debug.Log("Countdown reset - player left trigger area");
    }

    private void UpdateCountdown()
    {
        currentCountdown -= Time.deltaTime;
        UpdateCountdownText();
        
        if (currentCountdown <= 0f && !isLoadingScene)
        {
            // Set flag to prevent multiple scene loads
            isLoadingScene = true;
            LoadTargetScene();
        }
    }

    private void UpdateCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.text = Mathf.CeilToInt(currentCountdown).ToString();
        }
    }

    private void LoadTargetScene()
    {
        Debug.Log("Starting scene load for: " + targetSceneName);
        loadSceneCoroutine = StartCoroutine(LoadSceneAsync(targetSceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Fade out if we have a fade screen controller
        if (fadeScreenController != null)
        {
            fadeScreenController.FadeOut();
        }

        // Start loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Wait for the fade out to complete if we have a fade controller
        if (fadeScreenController != null)
        {
            float timer = 0f;
            while (timer <= fadeScreenController.fadeDuration && !asyncLoad.isDone)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            // If no fade controller, wait a short time before activating the scene
            yield return new WaitForSeconds(0.5f);
        }

        // Allow the scene to activate
        asyncLoad.allowSceneActivation = true;
    }
}
