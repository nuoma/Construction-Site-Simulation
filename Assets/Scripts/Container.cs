using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private bool inTruck = false;   // is the container currently in the truck?

    // called when we enter the collider of another object
    void OnTriggerEnter (Collider other)
    {
        // if we're in the truck, then don't worry about triggering anything
        if (inTruck)
            return;

        // was it the hook that hit us?
        if(other.CompareTag("Hook"))
        {
            // attach to the hook
            transform.position = other.transform.Find("ContainerHoldPosition").position;
            transform.parent = other.transform;
        }
        // was it the truck?
        else if(other.CompareTag("Truck"))
        {
            // attach to the truck
            transform.position = other.transform.Find("ContainerHoldPosition").position;
            transform.rotation = other.transform.rotation;
            transform.parent = other.transform;
            inTruck = true;
        }
    }
}