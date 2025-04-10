using UnityEngine;
using System.Collections;
using Meta.WitAi.TTS.Utilities;

public class guideVoice : MonoBehaviour
{
    public TTSSpeaker ttsSpeaker;  // Assign this in the Inspector
    [TextArea]
    public string[] scriptLines;  // Array of strings for the script
    public float delayInSeconds = 1.5f;  // You can tweak this too

    void Start()
    {
        StartCoroutine(PlayVoiceAfterDelay());
    }

    public void readScriptAtIndex(int index) {
        // create an array of string
        if (index < scriptLines.Length) {
            string line = scriptLines[index];
            if (ttsSpeaker != null)
            {
                // add delay if you want
                ttsSpeaker.Speak(line);
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
