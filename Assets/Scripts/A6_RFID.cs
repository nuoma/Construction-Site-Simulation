using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A6_RFID : MonoBehaviour
{
    public string wood;
    public string log;
    public string rebar;
    public bool WoodFlag = false;
    public bool LogFlag = false;
    public bool RebarFlag = false;
    private bool RFIDToggle;
    private int WoodNumber;
    private int LogNumber;
    private int RebarNumber;


    // Start is called before the first frame update
    public void start()
    {
        RFIDToggle = true;

        if (LogFlag)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Log") as GameObject[];
            LogNumber = objectsWithTag.Length;
            log = "Log Number:" + LogNumber + "\n";
        }
        if (WoodFlag)
        {
            // All wood object should have a tag with "wood"
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("wood") as GameObject[];
            WoodNumber = objectsWithTag.Length;
            wood = "Wood Number:" + WoodNumber + "\n";
        }
        if (RebarFlag)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("rebar") as GameObject[];
            RebarNumber = objectsWithTag.Length;
            rebar = "Rebar Number:" + RebarNumber + "\n";
        }
    }
    
    /*
    // Update is called once per frame
    void Update()
    {
        if (RFIDToggle)
        {
           
            
            
        }

    }

    // selected button
    public void WoodSelect()
    {
        WoodFlag = true;
    }
    public void LogSelect()
    {
        LogFlag = true;
    }
    public void RebarSelect()
    {
        RebarFlag = true;
    }

   
    public void back()
    {
        Activity6_ResourceCanvas.SetActive(false);
        Activity6_SensorCanvas.SetActive(false);
        Activity6_ReportCanvas.SetActive(false);
        Activity6_WrongCanvas.SetActive(false);
        WoodFlag = false;
        LogFlag = false;
        RebarFlag = false;
    }
    */


}
