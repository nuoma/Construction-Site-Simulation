using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericGPS : MonoBehaviour
{
    private bool GGPSToggle;
    public string GGPSConent;

    // Start is called before the first frame update
    public void start()
    {
        GGPSToggle = true;
        /*
        if (GGPSToggle)
        {
            GGPSToggle = false;
        }
        else
        {
            GGPSToggle = true;
            GGPSConent = "";
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (GGPSToggle)
        {
            GGPSConent = gameObject.name+ " coordinates: " + " x: " + transform.position.x + ". y: " + transform.position.y + ". z: " + transform.position.z + "\n";
            //Debug.Log(GGPSConent);
        }

    }
}
