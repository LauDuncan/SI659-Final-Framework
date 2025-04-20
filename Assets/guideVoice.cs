using UnityEngine;
using System.Collections;
using Meta.WitAi.TTS.Utilities;

public class guideVoice : MonoBehaviour
{
    public TTSSpeaker ttsSpeaker;
    public string welcomeMessage = "Welcome to your training! In front of you is a preparation table to help you get ready for your upcoming task.";
    public string topicsRoomsIntro = "Once you're done onboarding, you will see two task rooms on your left: Hurricane and Earthquake. These rooms offer immersive reporting experiences in extreme conditions.";
    public string decompressionsRoomsIntro = "To your right are decompression rooms, where you can stretch or take deep breaths to recover from high-stress situations.";
    public string endingMessage = "Take your time to prepare. When you're ready, feel free to begin.";
    public float delayInSeconds = 3f;

    void Start()
    {
        StartCoroutine(PlayVoiceAfterDelay());
    }

    IEnumerator PlayVoiceAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        if (ttsSpeaker != null)
        {
            ttsSpeaker.Speak(welcomeMessage);
            yield return new WaitForSeconds(9f); // Wait for the welcome message to finish
            ttsSpeaker.Speak(topicsRoomsIntro);
            yield return new WaitForSeconds(13f); // Wait for the topics rooms intro to finish
            ttsSpeaker.Speak(decompressionsRoomsIntro);
            yield return new WaitForSeconds(10f); // Wait for the decompressions rooms intro to finish
            ttsSpeaker.Speak(endingMessage);
        }
    }
}