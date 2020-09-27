using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateControl : MonoBehaviour
{
    public GameObject GateArea; //manually fill in gate trigger area
    public GameObject GateBar; //fill in moving trigger bar
    public float turnSpeed = 50;
    private bool closeCommand = false;
    private bool openCommand = false;
    public float gatemin = 0;
    public float gatemax = 270;


    private void Update()
    {
        
        if (openCommand && !closeCommand)
        {
            //do open door
            BarUp();
            //Debug.Log("open");
        }
        else if (closeCommand && !openCommand)
        {
            //do close door
            BarDown();
            //Debug.Log("close");
        }
        else
        {
            GateBar.transform.Rotate(0.0f, 0.0f, 0.0f, Space.World);
        }
        
    }

    private void OnTriggerEnter(Collider truck)
    {
        //if (truck.gameObject.tag == "TruckActivity3")
        //{
            openCommand = true;
            closeCommand = false;
            //Debug.Log("trigger open");
        //}
    }

    private void OnTriggerExit(Collider truck)
    {
        //if (truck.gameObject.tag == "TruckActivity3")
        //{
            openCommand = false;
            closeCommand = true;
        //}
    }

    // rotates the gate bar, if reach threshold, then stop rotation.
    private void BarUp()
    {
        //Debug.Log(GateBar.transform.localEulerAngles.z);
        if (GateBar.transform.localEulerAngles.z > gatemax)
        {
            GateBar.transform.Rotate(Vector3.forward, -turnSpeed * Time.deltaTime);
        }
        else
        {
            GateBar.transform.Rotate(0.0f, 0.0f, 0.0f, Space.World);

        }
    }

    // rotates the gate bar, if reach threshold, then stop rotation. 
    private void BarDown()
    {
        if (GateBar.transform.localEulerAngles.z < 359f)
        {
            GateBar.transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime);
        }
        else
        {
            GateBar.transform.Rotate(0.0f, 0.0f, 0.0f, Space.World);
        }
    }

}
