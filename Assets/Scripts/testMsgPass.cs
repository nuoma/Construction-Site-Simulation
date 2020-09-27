using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class testMsgPass : MonoBehaviour
{
    private bool GGPSToggle;
    public string GGPSConent;

    // Start is called before the first frame update
    public string Start()
    {
        GGPSToggle = true;
        return GGPSConent;
    }

    // Update is called once per frame
    void Update()
    {
        if (GGPSToggle)
        {
            GGPSConent = "Test pass msg signal: " + System.DateTime.UtcNow.ToString()+ "\n";
            Debug.Log(GGPSConent);
        }

    }
}
