using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region Parameters

    [SerializeField] private GameObject LegacyMainMenu; //LegacyMainMenu
    [SerializeField] private GameObject IMUReportCanvas;
    [SerializeField] private GameObject GPSReportCanvas;
    [SerializeField] private GameObject RFIDReportCanvas;
    public GameObject MainMenuScript;

    public DropdownMultiSelect Mdropdown1;// single selection activity
    public DropdownMultiSelect Mdropdown2;// sensors
    public DropdownMultiSelect Mdropdown3;// resources

    public GameObject Dropdown1_ListParentNode;
    public GameObject Dropdown2_ListParentNode;
    public GameObject Dropdown3_ListParentNode;

    private string SelectedString;
    public TextMeshProUGUI DisplayResults;
    public TextMeshProUGUI DisplayWarning;
    public TextMeshProUGUI GPSReportText;
    public TextMeshProUGUI RFIDReportText;
    public TextMeshProUGUI IMUReportText;

    private bool[] SelectedActivities = new bool[16];
    private bool[] SelectedSensors = new bool[5];
    private string[] SelectedResources = new string[16];
    private bool[] LastIterationSelectedActivities = new bool[16];

    List<string> ActivityList;
    List<string> SensorsList;
    List<string> ResourcesList = new List<string> { "Resources" };

    private bool onActivityChanged;
    private bool MultiLaserScan;
    private bool RFIDReportEnable;
    private bool GPSReportEnable;
    private bool IMUReportEnable;

    private string GPSReportString;
    private string RFIDReportString;
    private string IMUReportString;

    #endregion

    //----------------------------------------------------------

    #region Start Update

    void Start()
    {

        //create activity list
        CreateActivityDropdown();

        //create sensors list
        CreateSensorsDropdown();

        //resources list should be selected accordingly
        CreateResourcesDropdown();

        IMUReportCanvas.SetActive(false);
        GPSReportCanvas.SetActive(false);
        RFIDReportCanvas.SetActive(false);
    }


    void Update()
    {
        //update selected activity for each frame
        UpdateActivitySelected();

        UpdateSensorSelected();

        //Check if multiple laser scan activity selected
        MultiLaserScanCheck();

        //if Dropdown 1 changed, then update resources dropdown.
        if (onActivityChanged)//transform.hasChanged
        {
            Debug.Log("DetectedChange");
            UpdateResourcesList();
            ClearResourcesDropdown();
            CreateResourcesDropdown();
        }

        UpdateResourcesSelected();
        //if multiple selected, then prompt.
        if (MultiLaserScan)
        { DisplayWarning.GetComponent<TextMeshProUGUI>().text = "We only have 1 Laser Scanner, Please only select 1 Laser Scanner Activity."; }
        else DisplayWarning.GetComponent<TextMeshProUGUI>().text = "";

        //Display results that fit filter condition
        DisplayResults.GetComponent<TextMeshProUGUI>().text = "Selected" + SelectedString;
 
        //reset after each frame
        SelectedString = "";

        //follow test message passing using return and update
        //string TestPassMsg = TestCube.GetComponent<testMsgPass>().GGPSConent;
        //Debug.Log("Test passing return from another function:" + TestPassMsg);

        if (GPSReportEnable)
        {
            GPSReportCanvas.SetActive(true);
            //GPS reporting function
            PrepareGPSString();
            GPSReportText.GetComponent<TextMeshProUGUI>().text = GPSReportString;
        }
        

        if (RFIDReportEnable)
        {
            RFIDReportCanvas.SetActive(true);
            //RFID reporting function
            PrepareRFIDString();
            RFIDReportText.GetComponent<TextMeshProUGUI>().text = RFIDReportString;
        }

        if (IMUReportEnable)
        {
            //IMU reporting function
            IMUReportCanvas.SetActive(true);
            PrepareIMUString();
            IMUReportText.GetComponent<TextMeshProUGUI>().text = IMUReportString;
        }
    }

    #endregion

    //----------------------------------------------------------

    #region supporting functions

    private void CreateActivityDropdown()
    {
        ActivityList = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "StopAll", "Reset" };
        foreach (string option in ActivityList)
        {
            Mdropdown1.SetItemTitle(option);
            Mdropdown1.CreateNewItem();
        }
        Mdropdown1.SetupDropdown();
    }

    private void CreateSensorsDropdown()
    {
        SensorsList = new List<string> { "GPS", "RFID", "LaserScanner", "Drone", "IMU" };
        foreach (string option in SensorsList)
        {
            Mdropdown2.SetItemTitle(option);
            Mdropdown2.CreateNewItem();
        }
        Mdropdown2.SetupDropdown();
    }

    private void ClearResourcesDropdown()
    {
        //To clear existing items.
        Mdropdown3.dropdownItems.Clear();
        Mdropdown3.SetupDropdown();
    }

    private void CreateResourcesDropdown()
    {
        //Create using updated ResourcesList
        foreach (string option in ResourcesList)
        {
            Mdropdown3.SetItemTitle(option);
            Mdropdown3.CreateNewItem();
        }
        Mdropdown3.SetupDropdown();
    }


    private void UpdateActivitySelected()
    {
        //Verify if value changed since last iteration

        onActivityChanged = false;
        int children = Dropdown1_ListParentNode.transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            GameObject childnode = Dropdown1_ListParentNode.transform.GetChild(i).gameObject;
            Toggle go = childnode.GetComponent<Toggle>();

            if (go.isOn == true)
            {
                //append corresponding name
                SelectedString = SelectedString + " , " + Mdropdown1.dropdownItems[i].itemName;
                if (SelectedActivities[i] != true)
                {
                    SelectedActivities[i] = true;
                    onActivityChanged = true;
                }
            }
            else
            {
                //isOn is false
                if (SelectedActivities[i] != false)
                {
                    SelectedActivities[i] = false;
                    onActivityChanged = true;
                }
            }


        }
    }

    private void UpdateSensorSelected()
    {
        int children = Dropdown2_ListParentNode.transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            GameObject childnode = Dropdown2_ListParentNode.transform.GetChild(i).gameObject;
            Toggle go = childnode.GetComponent<Toggle>();

            if (go.isOn == true)
            {
                //append corresponding name
                SelectedString = SelectedString + " , " + Mdropdown2.dropdownItems[i].itemName;
                SelectedSensors[i] = true;
            }
            else
                SelectedSensors[i] = false;
        }
    }

    private void UpdateResourcesSelected()
    {
        int children = Dropdown3_ListParentNode.transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            GameObject childnode = Dropdown3_ListParentNode.transform.GetChild(i).gameObject;
            Toggle go = childnode.GetComponent<Toggle>();

            if (go.isOn == true)
            {
                //append corresponding name
                SelectedString = SelectedString + " , " + Mdropdown3.dropdownItems[i].itemName;
                SelectedResources[i] = Mdropdown3.dropdownItems[i].itemName;
            }
        }
    }

    private void ResetEverything()
    {
        //how to reset dropdowns
    }

    private void CameraMovement()
    {
        //More than 1 activity selected?
        //What is current selected activity? what is its movement parameter?
    }

    //update resources list accroding to dropdown 1's selection
    public void UpdateResourcesList()
    {
        //Initialize as empty list
        ResourcesList.Clear();

        //Add items according to different activities
        if (SelectedActivities[0] == true) ResourcesList.AddRange(new string[] { "1.Dozer", "1.Stockpile"});//1
        if (SelectedActivities[1] == true) ResourcesList.AddRange(new string[] { "2.Crane","2.Load"});//2
        if (SelectedActivities[2] == true) ResourcesList.AddRange(new string[] { "3.Truck", "3.Rebar" });
        if (SelectedActivities[3] == true) ResourcesList.AddRange(new string[] { "4.Worker 1" });
        if (SelectedActivities[4] == true) ResourcesList.AddRange(new string[] { "5.Loader", "5.dumptruck", "5.stockpile" });
        if (SelectedActivities[5] == true) ResourcesList.AddRange(new string[] { "6.wood", "6.Log", "6.Rebar" });
        if (SelectedActivities[6] == true) ResourcesList.AddRange(new string[] { "7.Worker 1", "7.Worker 2", "7.Worker 3" });
        if (SelectedActivities[7] == true) ResourcesList.AddRange(new string[] { "8.Building 1" });
        if (SelectedActivities[8] == true) ResourcesList.AddRange(new string[] { "9.Top Floor" });
        if (SelectedActivities[9] == true) ResourcesList.AddRange(new string[] { "10.Stockpile 1", "10.Stockpile 2" });//11
        if (SelectedActivities[10] == true) ResourcesList.AddRange(new string[] { "11.Old Building" });//12
        if (SelectedActivities[11] == true) ResourcesList.AddRange(new string[] { "12.Jobsite" });//13
        if (SelectedActivities[12] == true) ResourcesList.AddRange(new string[] { "13.Painter","13.Laborer","13.Carpenter 1","13.Carpenter 2"});//14

    }

    public void Select()
    {
        if (SelectedActivities[0] == true)
                MainMenuScript.GetComponent<menuScript>().select_1();
        if (SelectedActivities[0] == true && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "1.Dozer"))
                { GPSReportEnable = true; MainMenuScript.GetComponent<menuScript>().select_1_Dozer_GPS(); } //1.Dozer GPS

        if (SelectedActivities[1] == true)
                MainMenuScript.GetComponent<menuScript>().select_2();
        if (SelectedActivities[1] == true && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "2.Load"))
                { GPSReportEnable = true; MainMenuScript.GetComponent<menuScript>().select_2_CraneLoad_GPS(); }//2.Load GPS

        if (SelectedActivities[2] == true)
                MainMenuScript.GetComponent<menuScript>().select_3();
        if (SelectedActivities[2] == true && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "3.Truck"))
                { GPSReportEnable = true; MainMenuScript.GetComponent<menuScript>().select_3_truck_GPS(); }//3.truck GPS
        if (SelectedActivities[2] == true && SelectedSensors[1] == true && Array.Exists(SelectedResources, element => element == "3.Rebar"))
                { RFIDReportEnable = true; } //3.Rebar RFID
    

        if (SelectedActivities[3] == true) MainMenuScript.GetComponent<menuScript>().select_4();
        if (SelectedActivities[3] == true && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "4.Worker 1"))
                { GPSReportEnable = true; MainMenuScript.GetComponent<menuScript>().select_4worker_gps(); }
        if (SelectedActivities[3] == true && SelectedSensors[1] == true && Array.Exists(SelectedResources, element => element == "4.Worker 1"))
                { MainMenuScript.GetComponent<menuScript>().select_4worker_RFID(); }//Since we are using panel above worker, so no RFID panel needed.
        if (SelectedActivities[3] == true && SelectedSensors[0] == true && SelectedSensors[1] == true && Array.Exists(SelectedResources, element => element == "4.Worker 1"))
                { MainMenuScript.GetComponent<menuScript>().A4_worker_GPSRFID = true; MainMenuScript.GetComponent<menuScript>().select_4worker_RFID(); } 

        if (SelectedActivities[4] == true)
                MainMenuScript.GetComponent<menuScript>().select_5();
        if (SelectedActivities[4] == true && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "5.Loader"))
                { GPSReportEnable = true; MainMenuScript.GetComponent<menuScript>().select_5_Loader_GPS(); }
        if (SelectedActivities[4] == true && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "5.dumptruck"))
                { GPSReportEnable = true; MainMenuScript.GetComponent<menuScript>().select_5_dumptruck_GPS(); }

        //RFID & rebar, wood and log.
        if (SelectedActivities[5] == true && SelectedSensors[1] == true)
        {
            if (Array.Exists(SelectedResources, element => element == "6.wood")) MainMenuScript.GetComponent<menuScript>().A6_wood_flag = true;
            if (Array.Exists(SelectedResources, element => element == "6.Log")) MainMenuScript.GetComponent<menuScript>().A6_log_flag = true;
            if (Array.Exists(SelectedResources, element => element == "6.Rebar")) MainMenuScript.GetComponent<menuScript>().A6_rebar_flag = true;
            MainMenuScript.GetComponent<menuScript>().A6_RFID();
            RFIDReportEnable = true;
        }

        //7. Workers on top floor
        if (SelectedActivities[6] == true)
        {
            if (Array.Exists(SelectedResources, element => element == "7.Worker 1")) MainMenuScript.GetComponent<menuScript>().A7_w1_flag = true;
            if (Array.Exists(SelectedResources, element => element == "7.Worker 2")) MainMenuScript.GetComponent<menuScript>().A7_w2_flag = true;
            if (Array.Exists(SelectedResources, element => element == "7.Worker 3")) MainMenuScript.GetComponent<menuScript>().A7_w3_flag = true;

            //Run basic activity: danger zone red box. Based on worker selection bool.
            MainMenuScript.GetComponent<menuScript>().select_7_new();

            //GPS only
            if (SelectedSensors[0] == true) { GPSReportEnable = true; MainMenuScript.GetComponent<menuScript>().select_7_GPS(); }

            //RFID only, GPS hidden.
            if (SelectedSensors[1] == true) { MainMenuScript.GetComponent<menuScript>().select_7_RFID(); }

            //RFID and GPS
            if (SelectedSensors[0] == true && SelectedSensors[1] == true)
                { MainMenuScript.GetComponent<menuScript>().A7_worker_GPSRFID = true; MainMenuScript.GetComponent<menuScript>().select_7_RFID(); }

        }
        

        //Laser Scan related activities [7,8,9,10]
        //if MultiLaserScan, then cannot proceed
        if (MultiLaserScan == false) { if (SelectedActivities[7] == true) MainMenuScript.GetComponent<menuScript>().select_8(); }
        if (MultiLaserScan == false) { if (SelectedActivities[8] == true) MainMenuScript.GetComponent<menuScript>().select_9(); }
        if (MultiLaserScan == false) { if (SelectedActivities[9] == true && SelectedSensors[2] == true && Array.Exists(SelectedResources, element => element == "10.Stockpile 1")) MainMenuScript.GetComponent<menuScript>().select_10A(); }
        if (MultiLaserScan == false) { if (SelectedActivities[9] == true && SelectedSensors[2] == true && Array.Exists(SelectedResources, element => element == "10.Stockpile 2")) MainMenuScript.GetComponent<menuScript>().select_10B(); }
        if (MultiLaserScan == false) { if (SelectedActivities[10] == true && SelectedSensors[2] == true && Array.Exists(SelectedResources, element => element == "11.Old Building")) MainMenuScript.GetComponent<menuScript>().select_11Laser(); }


        if (SelectedActivities[10] == true && SelectedSensors[3] == true && Array.Exists(SelectedResources, element => element == "11.Old Building")) MainMenuScript.GetComponent<menuScript>().select_11Drone();
        if (SelectedActivities[11] == true && SelectedSensors[3] == true) MainMenuScript.GetComponent<menuScript>().select_12();

        //IMU workers
        if (SelectedActivities[12] == true && SelectedSensors[4] == true)
        {
            if (Array.Exists(SelectedResources, element => element == "13.Painter"))
                { MainMenuScript.GetComponent<menuScript>().A14_painter = true; }
            if (Array.Exists(SelectedResources, element => element == "13.Laborer"))
                { MainMenuScript.GetComponent<menuScript>().A14_laborer = true; }
            if (Array.Exists(SelectedResources, element => element == "13.Carpenter 1"))
                { MainMenuScript.GetComponent<menuScript>().A14_c1 = true; }
            if (Array.Exists(SelectedResources, element => element == "13.Carpenter 2"))
                { MainMenuScript.GetComponent<menuScript>().A14_c2 = true; }
            //With given worker bool, get IMU string, handle finish file write use backselected()
            MainMenuScript.GetComponent<menuScript>().select_13_new();
            //display IMU
            IMUReportEnable = true;
        }
              

    }

    private void MultiLaserScanCheck()
    {
        MultiLaserScan = false;
        if ((SelectedActivities[7] ? 1 : 0) + (SelectedActivities[8] ? 1 : 0) + (SelectedActivities[9] ? 1 : 0) + (SelectedActivities[10] ? 1 : 0) > 1)
        { MultiLaserScan = true; }
    }

    private void PrepareGPSString()
    {
       // Debug.Log("TEST PASS PARAM"+ MainMenuScript.GetComponent<menuScript>().A1_Dozer_GPS);
        GPSReportString = "";
        GPSReportString = MainMenuScript.GetComponent<menuScript>().A1_Dozer_GPS
            + MainMenuScript.GetComponent<menuScript>().A2_Load_GPS
            + MainMenuScript.GetComponent<menuScript>().A3_Truck_GPS
            + MainMenuScript.GetComponent<menuScript>().A5_Loader_GPS
            + MainMenuScript.GetComponent<menuScript>().A5_Dumptruck_GPS
            + MainMenuScript.GetComponent<menuScript>().A4_worker_GPS
            + MainMenuScript.GetComponent<menuScript>().A7_w1_GPS
            + MainMenuScript.GetComponent<menuScript>().A7_w2_GPS
            + MainMenuScript.GetComponent<menuScript>().A7_w3_GPS;
    }

    //this is mainly for A6, original RFID
    private void PrepareRFIDString()
    {
        RFIDReportString = "";
        RFIDReportString = MainMenuScript.GetComponent<menuScript>().A6_Wood_RFID
            + MainMenuScript.GetComponent<menuScript>().A6_Log_RFID
            + MainMenuScript.GetComponent<menuScript>().A6_Rebar_RFID
            + MainMenuScript.GetComponent<menuScript>().A3RFID;
    }

    //A13 IMU
    private void PrepareIMUString()
    {
        IMUReportString = "";
        IMUReportString = MainMenuScript.GetComponent<menuScript>().A14_c1_report
            + MainMenuScript.GetComponent<menuScript>().A14_c2_report
            + MainMenuScript.GetComponent<menuScript>().A14_l_report
            + MainMenuScript.GetComponent<menuScript>().A14_p_report;
    }

    public void A13_stop()
    {
        IMUReportEnable = false;
        IMUReportCanvas.SetActive(false);
        MainMenuScript.GetComponent<menuScript>().A13_stop();
        
    }

    public void StopRFID()
    {
        RFIDReportEnable = false;
        RFIDReportCanvas.SetActive(false);
    }

    public void StopGPS()
    {
        GPSReportEnable = false;
        GPSReportCanvas.SetActive(false);
    }
    #endregion

    #region Other small functions
    //Activate legacy menu upon selection.
    public void SelectLegacyMenu()
    {
        LegacyMainMenu.SetActive(true);
    }

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
   
    #endregion


}
