using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreenController : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2f;
    public Color fadeColor = Color.black;
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        if (fadeOnStart) FadeIn();
    }

    public void toggleFade()
    {
        if (renderer.material.GetColor("_BaseColor").a == 0f) FadeIn();
        else FadeOut();
    }

    public void FadeIn()
    {
        Fade(1f, 0f);
    }

    public void FadeOut()
    {
        Fade(0f, 1f);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeCoroutine(alphaIn, alphaOut));
    }

    private IEnumerator FadeCoroutine(float alphaIn, float alphaOut) {
        float timer = 0f;
        while (timer <= fadeDuration) {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            renderer.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color finalColor = fadeColor;
        finalColor.a = alphaOut;
        renderer.material.SetColor("_BaseColor", finalColor);
    }
    
    
}
