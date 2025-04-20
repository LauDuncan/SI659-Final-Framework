using UnityEngine;
using Meta.WitAi.TTS.Utilities;  // Import Meta Voice SDK

public class VoiceTrigger : MonoBehaviour
{
    public TTSSpeaker ttsSpeaker;  // Expose this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && ttsSpeaker != null)
        {
            ttsSpeaker.Speak("Welcome to your training!"); // Replace with your script input
        }
    }
}