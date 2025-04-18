using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteLineHelper : MonoBehaviour
{
    public Transform primaryHand;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = new Vector3(primaryHand.position.x, transform.position.y, primaryHand.position.z);
    }
}
