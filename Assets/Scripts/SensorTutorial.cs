using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SensorTutorial : MonoBehaviour
{

    public GameObject GPSPanel;
    public GameObject RFIDpanel;
    public GameObject LSPanel;
    public GameObject DronePanel;
    public GameObject IMUPanel;
    public GameObject SelectionPanel;
    public GameObject BackButton;
    public GameObject ActivityManager;
    public GameObject drone;
    public GameObject LScanner;
    public TextMeshProUGUI DroneTutorialText;
    public TextMeshProUGUI IMUReportText;
    public GameObject GPSParent;
    public GameObject RFIDParent;
    public GameObject IMUParent;
    private string IMUReportString;
    public GameObject IMUReportCanvas;
    public GameObject GPSReportCanvas;
    private string GPSReportString;
    public TextMeshProUGUI GPSReportText;


    // Start is called before the first frame update
    void Start()
    {
        SelectionPanel.SetActive(true);
        BackButton.SetActive(false);
        GPSPanel.SetActive(false);
        RFIDpanel.SetActive(false);
        LSPanel.SetActive(false);
        DronePanel.SetActive(false);
        IMUPanel.SetActive(false);
        RFIDParent.SetActive(false);
        IMUParent.SetActive(false);
        GPSParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PrepareIMUString();
        IMUReportText.GetComponent<TextMeshProUGUI>().text = IMUReportString;
        PrepareGPSString();
        GPSReportText.GetComponent<TextMeshProUGUI>().text = GPSReportString;
    }

    private void PrepareIMUString()
    {
        IMUReportString = "";
        IMUReportString = ActivityManager.GetComponent<ActivityManagerScript>().A14_c1_report
            + ActivityManager.GetComponent<ActivityManagerScript>().A14_c2_report
            + ActivityManager.GetComponent<ActivityManagerScript>().A14_l_report
            + ActivityManager.GetComponent<ActivityManagerScript>().A14_p_report;
    }

    private void PrepareGPSString()
    {
        // Debug.Log("TEST PASS PARAM"+ ActivityManager.GetComponent<ActivityManagerScript>().A1_Dozer_GPS);
        GPSReportString = "";
        GPSReportString = ActivityManager.GetComponent<ActivityManagerScript>().A1_Dozer_GPS;
    }
    public void exitbutton()
    {
        SceneManager.LoadScene(0);
    }

    public void GPS()
    {
        GPSPanel.SetActive(true);
        SelectionPanel.SetActive(false);
    }

    public void GPSexe()
    {
        GPSParent.SetActive(true);
        GPSPanel.SetActive(false);
        GPSReportCanvas.SetActive(true);
        ActivityManager.GetComponent<ActivityManagerScript>().TutorialGPS();
    }

    public void RFID()
    {
        RFIDpanel.SetActive(true);
        SelectionPanel.SetActive(false);
    }

    public void RFIDexe()
    {
        RFIDParent.SetActive(true);
    }

    public void LS()
    {
        LSPanel.SetActive(true);
        SelectionPanel.SetActive(false);
    }

    public void LSexe()
    {
        LScanner.SetActive(true);
        LSPanel.SetActive(false);
        ActivityManager.GetComponent<ActivityManagerScript>().TutorialLS();
    }

    public void Drone()
    {
        DronePanel.SetActive(true);
        SelectionPanel.SetActive(false);
    }

    public void Droneexe()
    {
        DronePanel.SetActive(false);
        drone.SetActive(true);
        ActivityManager.GetComponent<ActivityManagerScript>().TutorialDrone();
    }

    public void IMU()
    {
        IMUPanel.SetActive(true);
        SelectionPanel.SetActive(false);
    }

    public void IMUexe()
    {
        IMUPanel.SetActive(false);
        IMUParent.SetActive(true);
        ActivityManager.GetComponent<ActivityManagerScript>().TutorialIMU();
        IMUReportCanvas.SetActive(true);
    }

    public void BackToSelection()
    {
        SceneManager.LoadScene(2);
    }

}
