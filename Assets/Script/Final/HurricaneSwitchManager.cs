using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurricaneSwitchManager : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private GameObject rainAudio;
    [SerializeField] private GameObject earthquakeCity;
    [SerializeField] private GameObject hurricaneCity;
    [SerializeField] private GameObject drawingTip;
    // Start is called before the first frame update
    void Start()
    {
        rainAudio.SetActive(false);
        drawingTip.SetActive(true);
    }

    public void OnHoverEnter()
    {
        Animator.SetBool("isRaining", true);
        rainAudio.SetActive(true);
    }

    public void OnHoverExit()
    {
        Animator.SetBool("isRaining", false);
        rainAudio.SetActive(false);
    }

    public void OnSelect()
    {
        earthquakeCity.SetActive(false);
        hurricaneCity.SetActive(true);
        drawingTip.SetActive(false);
    }
}
