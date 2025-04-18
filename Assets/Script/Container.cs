using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Container : MonoBehaviour
{
    public enum Size { Small, Medium, Large }
    public Size size;

    private string sizeStr;
    private string currentObjectName = "";
    private Renderer objectRenderer;
    private Color errorColor = new Color(1, 0, 0, 0.5f);
    private Color successColor = new Color(0, 1, 0, 0.5f);
    private Color normalColor;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        normalColor = objectRenderer.material.color;
        sizeStr = size.ToString();
    }

    void Update()
    {
    }

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (currentObjectName == "")
        {
            string enterObjectName = other.gameObject.name;
            if (enterObjectName.Contains("Gear"))
            {
                if (enterObjectName.Contains(sizeStr))
                {
                    ChangeColor(successColor);
                } else
                {
                    ChangeColor(errorColor);
                }
                currentObjectName = enterObjectName;
            }
        }
    }

    public void OnTriggerExit(UnityEngine.Collider other)
    {
        string exitObjectName = other.gameObject.name;
        if (currentObjectName == exitObjectName)
        {
            ChangeColor(normalColor);
            currentObjectName = "";
        }
    }

    public void ChangeColor(Color color)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = color;
        }
    }
}
