using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Activity6_Menu : MonoBehaviour
{
    private bool WoodFlag = false;
    private bool LogFlag = false;
    private bool RebarFlag = false;
    [SerializeField] private GameObject Activity6_ResourceCanvas;
    [SerializeField] private GameObject Activity6_SensorCanvas;
    [SerializeField] private GameObject Activity6_ReportCanvas;
    [SerializeField] private GameObject Activity6_WrongCanvas;
    private string reportText;


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

    public void Done()
    {
        Activity6_SensorCanvas.SetActive(true);
    }

    public void Activity6_RFID()
    {
        //activate report canvas and report actual number
        Activity6_ReportCanvas.SetActive(true);
 

        if (LogFlag)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Log") as GameObject[];
            int LogNumber = objectsWithTag.Length;
            reportText = string.Concat("Log Number:" , LogNumber , System.Environment.NewLine);
        }
        if (WoodFlag)
        {
            // All wood object should have a tag with "wood"
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("wood") as GameObject[];
            int WoodNumber = objectsWithTag.Length;
            reportText = string.Concat(reportText, "Wood Number:" , WoodNumber , System.Environment.NewLine);
        }
        if (RebarFlag)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("rebar") as GameObject[];
            int RebarNumber = objectsWithTag.Length;
            reportText = string.Concat(reportText, "Rebar Number:", RebarNumber, System.Environment.NewLine);
        }

        //Finally output all flagged reports
        Activity6_ReportCanvas.transform.Find("Report").GetComponent<TextMeshProUGUI>().text = reportText;


    }

     


    public void OtherSensor()
    {
        //prompt wrong
        Activity6_WrongCanvas.SetActive(true);
        Activity6_WrongCanvas.transform.Find("Report").GetComponent<TextMeshProUGUI>().text = "Wrong sensor selected because of reason 1.";
    }

    public void resetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
    
    
   
}
