using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SensorTutorial : MonoBehaviour
{

    public GameObject GPSPanel1;
    public GameObject GPSPanel2;
    public GameObject GPSPanel3;
    public GameObject GPShand1;
    public GameObject GPShand2;
    public GameObject RFIDhand1;
    public GameObject RFIDhand2;
    public GameObject RFIDpanel;
    
    public GameObject LSPanel;
    public GameObject DronePanel;
    public GameObject DroneInspectPanel;
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

    public GameObject DroneParent;
    public GameObject LSParent;
    public GameObject LSBackButton;

    bool gpsB;
    bool rfidB;
    bool lsB;
    bool droneB;
    bool imuB;

    public GameObject RFIDWood;
    public GameObject RFIDVehicle;
    public GameObject RFIDButton;

    public GameObject dronett1;
    public GameObject dronett2;
    public GameObject dronett3;

    public GameObject IMUtagpanel;
    public GameObject IMUstt;
    public GameObject IMUbtt;
    public GameObject IMUttt;
    public GameObject IMUntt;
    public GameObject imucarpenter;

    public GameObject RFIDwoodtooltip;
    public GameObject RFIDvehicletooltip;

    public GameObject GPSpanelbutton;
    public GameObject GpsDozer;

    public GameObject RFIDTagCanvas;

    


    // Start is called before the first frame update
    void Start()
    {
        SelectionPanel.SetActive(true);
        BackButton.SetActive(false);
        GPSPanel1.SetActive(false);
        RFIDpanel.SetActive(false);
        LSPanel.SetActive(false);
        DronePanel.SetActive(false);
        IMUPanel.SetActive(false);
        RFIDParent.SetActive(false);
        IMUParent.SetActive(false);
        GPSParent.SetActive(false);
        DroneParent.SetActive(false);
        LSParent.SetActive(false);
        RFIDwoodtooltip.SetActive(false);
        RFIDvehicletooltip.SetActive(false);

        RFIDButton.SetActive(false);
        GPSpanelbutton.SetActive(false);
        dronett1.SetActive(false);
        dronett2.SetActive(false);
        dronett3.SetActive(false);

        RFIDTagCanvas.SetActive(false);
        GPShand1.SetActive(false);
        GPShand2.SetActive(false);
        RFIDhand1.SetActive(false);
        RFIDhand2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PrepareIMUString();
        IMUReportText.GetComponent<TextMeshProUGUI>().text = IMUReportString;
        PrepareGPSString();
        GPSReportText.GetComponent<TextMeshProUGUI>().text = GPSReportString;

        //if (gpsB == true) { if()}
        bool RFIDTagged = false;
        if (RFIDWood.GetComponent<TutorialManualClick>().TagStatus == true || RFIDVehicle.GetComponent<TutorialManualClick>().TagStatus == true) RFIDTagged = true;
        if (rfidB == true && RFIDTagged == true) RFIDButton.SetActive(true);

        if(gpsB == true && GpsDozer.GetComponent<TutorialManualClick>().TagStatus == true) GPSpanelbutton.SetActive(true);
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
        gpsB = true;
        GPSPanel1.SetActive(true);
        SelectionPanel.SetActive(false);
        GPSParent.SetActive(true);
    }

    public void GPSButton1()
    {
        GPSPanel2.SetActive(true);
        GPSPanel1.SetActive(false);
    }

    public void GPSButton2()
    {
        GPSPanel3.SetActive(true);
        GPSPanel2.SetActive(false);

        //enable hand coach
        GPShand1.SetActive(true);
        GPShand2.SetActive(true);
    }
    public void GPSButtonFinal()
    {
        GPSPanel1.SetActive(false);
        GPSPanel2.SetActive(false);
        GPSPanel3.SetActive(false);
        GPSReportCanvas.SetActive(true);
        ActivityManager.GetComponent<ActivityManagerScript>().TutorialGPS();
        GPShand1.SetActive(false);
        GPShand2.SetActive(false);
    }



    public void RFID()
    {
        rfidB = true;
        RFIDpanel.SetActive(true);
        SelectionPanel.SetActive(false);
        RFIDParent.SetActive(true);
        RFIDhand1.SetActive(true);
        RFIDhand2.SetActive(true);
    }

    public void RFIDexe()
    {
        //RFIDTagCanvas.SetActive(false);
        if (RFIDWood.GetComponent<TutorialManualClick>().TagStatus == true)
        { RFIDwoodtooltip.SetActive(true); }

        if (RFIDVehicle.GetComponent<TutorialManualClick>().TagStatus == true)
        { RFIDvehicletooltip.SetActive(true); }

        RFIDhand1.SetActive(false);
        RFIDhand2.SetActive(false);
    }

    public void RFIDTag()
    {
        RFIDTagCanvas.SetActive(true);
        RFIDpanel.SetActive(false);
    }

    public void LS()
    {
        lsB = true;
        LSPanel.SetActive(true);
        SelectionPanel.SetActive(false);
        LSParent.SetActive(true);
    }

    public void LSexe()
    {
        LScanner.SetActive(true);
        LSPanel.SetActive(false);
        ActivityManager.GetComponent<ActivityManagerScript>().TutorialLS();
        //LSBackButton.SetActive(true);
    }

    public void Drone()
    {
        droneB = true;
        DroneInspectPanel.SetActive(true);
        SelectionPanel.SetActive(false);
        DroneParent.SetActive(true);
        drone.SetActive(true);

        //show tooltips
        dronett1.SetActive(true);
        dronett2.SetActive(true);
        dronett3.SetActive(true);
    }

    public void Droneexe()
    {
        DronePanel.SetActive(false);
        ActivityManager.GetComponent<ActivityManagerScript>().TutorialDrone();
    }

    public void DroneInspect()
    {
        DronePanel.SetActive(true);
        DroneInspectPanel.SetActive(false);
    }

    public void IMU()
    {
        imuB = true;
        IMUPanel.SetActive(true);
        SelectionPanel.SetActive(false);
        IMUParent.SetActive(true);
        imucarpenter.GetComponent<workerScript>().TutorialInitial();
    }

    public void IMUexe()
    {
        IMUPanel.SetActive(false);
        IMUtagpanel.SetActive(false);
        ActivityManager.GetComponent<ActivityManagerScript>().TutorialIMU();
        IMUReportCanvas.SetActive(true);
    }

    public void imutag()
    {
        IMUPanel.SetActive(false);
        IMUtagpanel.SetActive(true);
    }

    public void imuN()
    {
        IMUntt.SetActive(true);
        imucarpenter.GetComponent<workerScript>().neckSelect();
    }
    public void imuS()
    {
        IMUstt.SetActive(true);
        imucarpenter.GetComponent<workerScript>().shoulderSelect();
    }

    public void imuT()
    {
        IMUttt.SetActive(true);
        imucarpenter.GetComponent<workerScript>().thighSelect();
    }

    public void imuBack()
    {
        IMUbtt.SetActive(true);
        imucarpenter.GetComponent<workerScript>().backSelect();
    }
    public void BackToSelection()
    {
        SceneManager.LoadScene(2);
    }

}
