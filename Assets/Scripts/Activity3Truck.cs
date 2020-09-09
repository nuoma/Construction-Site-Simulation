using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Activity3Truck : MonoBehaviour
{
    #region param
    // code base inherit from BulldozerActivity1.cs
    [SerializeField] private float speed; //default 0.1
    [SerializeField] private float precision; //default 0.1
    [SerializeField] private Transform[] moveSpots;
    [SerializeField] private GameObject Activity3Report;

    private bool enable = false;
    private int arrayPosition = 0;
    public GameObject Truck; //manually fill in truck asset
    private bool DisplayResults = false;

    [HideInInspector] public bool tagged = true;
    [HideInInspector] public int lapCount;
    #endregion


    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveSpots[arrayPosition].position, speed * Time.deltaTime);
            //transform.position += transform.forward * Time.deltaTime * speed;

            Quaternion lookDirection = Quaternion.LookRotation(transform.position - moveSpots[arrayPosition].position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, 30* Time.deltaTime);

            if (Vector3.Distance(transform.position, moveSpots[arrayPosition].position) < precision)
            {
                if (arrayPosition < moveSpots.Length - 1)
                {
                    arrayPosition++;
                }
                else
                {
                    enable = false; // Stop vehicle when reached to final point, and exit out of this movement loop
                    DisplayResults = true;
                }
            }
            
        }

        // After truck movement finish, pull out RFID counting menu
        if (DisplayResults)
        {
            // 5 Log
            // objects in truck should have tagged TruckActivity3
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("TruckActivity3") as GameObject[];
            int LogNumber = objectsWithTag.Length;
            Activity3Report.SetActive(true);
            //Debug.Log("did it output?" + LogNumber);
            Activity3Report.GetComponent<TextMeshPro>().text = "Truck Carry Log:" + LogNumber;
        }

    }

    public void start()
    {
        //this section is used to activate from activity menu
        if (enable)
        {
            enable = false;
        }
        else
        {
            enable = true;
        }

        Truck.transform.Find("Arrow").gameObject.SetActive(true);

    }



    public void stop_3()
    {
        Truck.transform.Find("Arrow").gameObject.SetActive(false);
    }

}
