using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xRayButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject xRay;
    private bool xRayIsActive = false;
    
    public void toggleXRay()
        { 
            xRayIsActive = !xRayIsActive;
            xRay.SetActive(xRayIsActive);
           }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
