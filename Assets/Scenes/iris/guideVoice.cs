using UnityEngine;
using System.Collections;
using Meta.WitAi.TTS.Utilities;
using System;

public class guideVoice : MonoBehaviour
{
    public TTSSpeaker ttsSpeaker;  // Assign this in the Inspector
    
    [TextArea]
    public string[] scriptLines;  // Array of strings for the script
    public float delayInSeconds = 1.5f;  // You can tweak this too

    // Event that will be triggered when speaking finishes
    public event Action OnSpeakingFinished;

    private void OnEnable()
    {
        // Subscribe to the TTSSpeaker's completion event
        if (ttsSpeaker != null)
        {
            // Use the generic event handler approach
            ttsSpeaker.Events.OnTextPlaybackFinished.AddListener(OnTextPlaybackFinished);
        }
    }

    // Event handler that works with various signatures
    private void OnTextPlaybackFinished(string text)
    {
        Debug.Log("TTS finished speaking: " + text);
        // Invoke our custom event
        OnSpeakingFinished?.Invoke();
    }

    private void OnDisable()
    {
        // Unsubscribe when disabled
        if (ttsSpeaker != null)
        {
            ttsSpeaker.Events.OnTextPlaybackFinished.RemoveListener(OnTextPlaybackFinished);
        }
    }

    public void readScriptAtIndex(int index) {
        // create an array of string
        Debug.Log("Reading script at index: " + index);
        if (index < scriptLines.Length) {
            string line = scriptLines[index];
            Debug.Log("Line: " + line);
            if (ttsSpeaker != null)
            {
                Debug.Log("Speaking line: " + line);
                // add delay if you want
                ttsSpeaker.Speak(line);
                Debug.Log("Finished speaking line: " + line);
            }
        } else {
            Debug.Log("Index out of range");
        }
    }

    IEnumerator PlayVoiceAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);

        if (ttsSpeaker != null)
        {
            ttsSpeaker.Speak(scriptLines[0]);
        }
    }
}
