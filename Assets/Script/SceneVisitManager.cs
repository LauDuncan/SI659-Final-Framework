using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneVisitManager : MonoBehaviour
{
    // Singleton instance
    public static SceneVisitManager Instance { get; private set; }
    
    // Dictionary to track visited scenes
    private Dictionary<string, bool> visitedScenes = new Dictionary<string, bool>();
    
    // Reference to the guide voice in the office scene
    private guideVoice officeGuide;
    
    // Track current message sequence
    private int currentMessageIndex = 0;
    private List<int> messageSequence = new List<int>();
    
    // Configuration for each scene's effect when returning to office
    [System.Serializable]
    public class SceneEffect
    {
        public string sceneName;
        public int ttsLineIndex;
        public List<GameObject> objectsToActivate;
        public List<GameObject> objectsToDeactivate;
    }
    
    [SerializeField] private List<SceneEffect> sceneEffects = new List<SceneEffect>();
    
    // Name of the office scene
    [SerializeField] private string officeSceneName;
    
    // First time office welcome configuration
    [Header("First Time Visit Configuration")]
    [SerializeField] private bool playMessagesOnFirstVisit = true;
    [SerializeField] private float initialDelay = 2.0f;
    
    [System.Serializable]
    public class TTSMessage
    {
        public int scriptIndex;
        [Tooltip("Optional description for editor reference")]
        public string description;
    }
    
    [Header("Sequential Messages")]
    [Tooltip("List of TTS messages to play in sequence on first visit")]
    [SerializeField] private List<TTSMessage> firstVisitMessages = new List<TTSMessage>();
    
    [Header("Scene Objects")]
    [SerializeField] private List<GameObject> firstVisitObjectsToActivate;
    [SerializeField] private List<GameObject> firstVisitObjectsToDeactivate;
    
    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Subscribe to scene loading events
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        // Clean up event subscription
        SceneManager.sceneLoaded -= OnSceneLoaded;
        UnsubscribeFromTTSEvents();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        
        // Check if this is the office scene and the first time loading it
        bool isFirstTimeOffice = !visitedScenes.ContainsKey(sceneName) && sceneName == officeSceneName;
        
        // Mark the scene as visited
        if (!visitedScenes.ContainsKey(sceneName))
        {
            visitedScenes[sceneName] = true;
            Debug.Log($"Marked scene '{sceneName}' as visited");
        }
        
        // If returning to office scene, apply effects based on visited scenes
        if (sceneName == officeSceneName)
        {
            // Find the guide voice component in the office scene
            officeGuide = FindObjectOfType<guideVoice>();
            
            if (officeGuide == null)
            {
                Debug.LogWarning("guideVoice component not found in office scene");
                return;
            }
            
            // Subscribe to the TTS completion event
            SubscribeToTTSEvents();
            
            if (isFirstTimeOffice && playMessagesOnFirstVisit)
            {
                HandleFirstVisit();
            }
            else
            {
                ApplyOfficeSceneEffects();
            }
        }
        else
        {
            // If leaving the office scene, unsubscribe from TTS events
            UnsubscribeFromTTSEvents();
        }
    }
    
    private void SubscribeToTTSEvents()
    {
        if (officeGuide != null)
        {
            UnsubscribeFromTTSEvents(); // Ensure we don't double-subscribe
            officeGuide.OnSpeakingFinished += OnTTSSpeakingFinished;
            Debug.Log("Subscribed to guideVoice OnSpeakingFinished event");
        }
    }
    
    private void UnsubscribeFromTTSEvents()
    {
        if (officeGuide != null)
        {
            officeGuide.OnSpeakingFinished -= OnTTSSpeakingFinished;
            Debug.Log("Unsubscribed from guideVoice OnSpeakingFinished event");
        }
    }
    
    private void OnTTSSpeakingFinished()
    {
        Debug.Log("SceneVisitManager received TTS completion event");
        
        // If we're in the middle of a message sequence, continue to the next message
        if (messageSequence.Count > 0 && currentMessageIndex < messageSequence.Count - 1)
        {
            currentMessageIndex++;
            PlayCurrentSequenceMessage();
        }
        else
        {
            // End of sequence
            messageSequence.Clear();
            currentMessageIndex = 0;
            Debug.Log("Completed TTS message sequence");
        }
    }
    
    private void HandleFirstVisit()
    {
        if (officeGuide == null)
        {
            Debug.LogWarning("guideVoice component not found in office scene for welcome message");
            return;
        }
        
        // Apply first visit scene effects immediately
        ApplyFirstVisitSceneEffects();
        
        // Set up the message sequence from firstVisitMessages
        PrepareMessageSequence(firstVisitMessages);
        
        // Start the sequence with a delay
        StartCoroutine(StartSequenceWithDelay(initialDelay));
    }
    
    private void PrepareMessageSequence(List<TTSMessage> messages)
    {
        messageSequence.Clear();
        currentMessageIndex = 0;
        
        foreach (var message in messages)
        {
            messageSequence.Add(message.scriptIndex);
        }
        
        Debug.Log($"Prepared message sequence with {messageSequence.Count} messages");
    }
    
    private IEnumerator StartSequenceWithDelay(float delay)
    {
        // Wait for the initial delay before starting the sequence
        yield return new WaitForSeconds(delay);
        
        if (messageSequence.Count > 0)
        {
            PlayCurrentSequenceMessage();
        }
    }
    
    private void PlayCurrentSequenceMessage()
    {
        if (officeGuide != null && messageSequence.Count > 0 && currentMessageIndex < messageSequence.Count)
        {
            int messageIndex = messageSequence[currentMessageIndex];
            Debug.Log($"Playing message {currentMessageIndex + 1}/{messageSequence.Count} (script index: {messageIndex})");
            officeGuide.readScriptAtIndex(messageIndex);
        }
    }
    
    private void ApplyFirstVisitSceneEffects()
    {
        // Activate objects for first visit
        foreach (var obj in firstVisitObjectsToActivate)
        {
            if (obj != null) obj.SetActive(true);
        }
        
        // Deactivate objects for first visit
        foreach (var obj in firstVisitObjectsToDeactivate)
        {
            if (obj != null) obj.SetActive(false);
        }
        
        Debug.Log("Applied scene effects for first office visit");
    }
    
    private void ApplyOfficeSceneEffects()
    {
        if (officeGuide == null)
        {
            Debug.LogWarning("guideVoice component not found in office scene");
            return;
        }
        
        // Find the most recently visited scene that has a defined effect
        foreach (var effect in sceneEffects)
        {
            if (visitedScenes.ContainsKey(effect.sceneName) && visitedScenes[effect.sceneName])
            {
                // For return visits, set up a single-message sequence
                messageSequence.Clear();
                currentMessageIndex = 0;
                messageSequence.Add(effect.ttsLineIndex);
                
                // Play the corresponding TTS line
                PlayCurrentSequenceMessage();
                
                // Activate and deactivate objects as defined
                foreach (var obj in effect.objectsToActivate)
                {
                    if (obj != null) obj.SetActive(true);
                }
                
                foreach (var obj in effect.objectsToDeactivate)
                {
                    if (obj != null) obj.SetActive(false);
                }
                
                Debug.Log($"Applied office effects for visited scene '{effect.sceneName}'");
                
                break; // Only apply the first matching effect
            }
        }
    }
    
    // Public method to manually check if a scene has been visited
    public bool HasVisitedScene(string sceneName)
    {
        return visitedScenes.ContainsKey(sceneName) && visitedScenes[sceneName];
    }
    
    // Public method to manually trigger effects
    public void TriggerEffectForScene(string sceneName)
    {
        foreach (var effect in sceneEffects)
        {
            if (effect.sceneName == sceneName)
            {
                if (officeGuide == null)
                {
                    officeGuide = FindObjectOfType<guideVoice>();
                    // Make sure we subscribe to events
                    SubscribeToTTSEvents();
                }
                
                if (officeGuide != null)
                {
                    // Set up a single-message sequence
                    messageSequence.Clear();
                    currentMessageIndex = 0;
                    messageSequence.Add(effect.ttsLineIndex);
                    
                    // Play the message
                    PlayCurrentSequenceMessage();
                    
                    foreach (var obj in effect.objectsToActivate)
                    {
                        if (obj != null) obj.SetActive(true);
                    }
                    
                    foreach (var obj in effect.objectsToDeactivate)
                    {
                        if (obj != null) obj.SetActive(false);
                    }
                }
                break;
            }
        }
    }
    
    // Public method to play a custom sequence of messages
    public void PlayMessageSequence(List<int> messageIndices, float initialDelaySeconds = 0f)
    {
        if (officeGuide == null)
        {
            officeGuide = FindObjectOfType<guideVoice>();
            if (officeGuide == null)
            {
                Debug.LogWarning("Cannot play message sequence: guideVoice not found");
                return;
            }
            SubscribeToTTSEvents();
        }
        
        // Set up the sequence
        messageSequence = new List<int>(messageIndices);
        currentMessageIndex = 0;
        
        if (initialDelaySeconds > 0)
        {
            StartCoroutine(StartSequenceWithDelay(initialDelaySeconds));
        }
        else
        {
            PlayCurrentSequenceMessage();
        }
    }
}
