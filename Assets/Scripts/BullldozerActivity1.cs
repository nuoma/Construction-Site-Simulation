using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullldozerActivity1 : MonoBehaviour
{
    //USED FOR BULLDOZER
    [SerializeField] private float speed;
    [SerializeField] private float precision;
    [SerializeField] private Transform[] moveSpots;
    [SerializeField] private GameObject gpsScript;

    private bool enable = false;
    [HideInInspector] public bool tagged = true;
    [HideInInspector] public int lapCount;
    private int arrayPosition = 0;
   // private string bulldozerContent = "\nBulldozer Data\n\n";

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveSpots[arrayPosition].position, speed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, moveSpots[arrayPosition].position, 0.0f);
            //transform.position += transform.forward * Time.deltaTime * speed;

            //Quaternion lookDirection = Quaternion.LookRotation(moveSpots[arrayPosition].position - transform.position);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, 30 * Time.deltaTime);

            if (Vector3.Distance(transform.position, moveSpots[arrayPosition].position) < precision)
            {
                if (arrayPosition < moveSpots.Length - 1)
                    arrayPosition++;
                else
                {
                    arrayPosition = 0;
                    lapCount++;
                }
                
                //original code for reporting function, disable.
                //bulldozerContent += "Position " + arrayPosition + ":  x:" + transform.position.x + "  y:" +
                //transform.position.y + "  z:" + transform.position.z + "\n";
                //}
            }
        }
    }

    public void start()
    {
        if (enable)
        {
            enable = false;
        }
        else
        {
            enable = true;
        }
    }

    public void stop()
    {
        enable = false;
    }
}
