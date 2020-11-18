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
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;
using Microsoft.MixedReality.Toolkit.Experimental.Dialog;
using Microsoft.MixedReality.Toolkit.UI;

public class ManualSelection : MonoBehaviour
{

    #region Parameters
    [SerializeField]
    [Tooltip("Assign DialogSmall_192x96.prefab")]
    private GameObject dialogPrefabSmall;
    [SerializeField] private GameObject PointingChevron;

    public GameObject RunButton;
    public GameObject ManualSelectionListPanel;
    //public GameObject UIMenuManager;
    public GameObject ManualSelectionMenuSelf;
    public GameObject ActivityManagerScript;
    public GameObject TagButton;
    public GameObject ResetButton;

    public CustomDropdown Dropdown1;
    public CustomDropdown Dropdown2;

    List<string> ActivityList = new List<string> { "Activities" };
    List<string> SensorsList;

    private int SelectedActivityIndex;
    private int SelectedSensorIndex;
    private int ActualActivityNumber;

    private bool onSensorChanged;
    public bool ResourceTaggedBool = false;

    public GameObject A1Dozer;
    public GameObject A1Stockpile;
    public GameObject A2Crane;
    public GameObject A2SteelBeam;
    public GameObject A3Truck;
    public GameObject A3Rebar;
    public GameObject A4Worker1;
    public GameObject A5Loader;
    public GameObject A5DumpTruck;
    public GameObject A5Stockpile;
    public GameObject A6wood;
    public GameObject A6log;
    public GameObject A6rebar;
    public GameObject A7worker1;
    public GameObject A7worker2;
    public GameObject A7worker3;
    public GameObject A13Painter;
    public GameObject A13Laborer;
    public GameObject A13Carpenter;
    public GameObject A13Carpenter2;

    public bool RFIDReportEnable;
    public bool GPSReportEnable;
    public bool IMUReportEnable;
    private string GPSReportString;
    private string RFIDReportString;
    private string IMUReportString;
    public TextMeshProUGUI GPSReportText;
    public TextMeshProUGUI RFIDReportText;
    public TextMeshProUGUI IMUReportText;
    [SerializeField] private GameObject IMUReportCanvas;
    [SerializeField] private GameObject GPSReportCanvas;
    [SerializeField] private GameObject RFIDReportCanvas;
    #endregion

    #region Start Update
    // Start is called before the first frame update
    public void Start()
    {
        //create sensors list
        CreateActivityInitialDropdown();
        CreateSensorsDropdown();

        SetInteractablesFalse();
        SetCubeFalse();

        TagButton.SetActive(false);
        ManualSelectionListPanel.SetActive(false);
        RunButton.SetActive(false);
        ResetButton.SetActive(false);

    }

    public void SetInteractablesFalse()
    {
        //set interactable to false
        A1Dozer.GetComponent<Interactable>().IsEnabled = false;
        A1Stockpile.GetComponent<Interactable>().IsEnabled = false;
        A2Crane.GetComponent<Interactable>().IsEnabled = false;
        A2SteelBeam.GetComponent<Interactable>().IsEnabled = false;
        A3Truck.GetComponent<Interactable>().IsEnabled = false;
        A3Rebar.GetComponent<Interactable>().IsEnabled = false;
        A4Worker1.GetComponent<Interactable>().IsEnabled = false;
        A5Stockpile.GetComponent<Interactable>().IsEnabled = false;
        A5Loader.GetComponent<Interactable>().IsEnabled = false;
        A5DumpTruck.GetComponent<Interactable>().IsEnabled = false;
        A6log.GetComponent<Interactable>().IsEnabled = false;
        A6rebar.GetComponent<Interactable>().IsEnabled = false;
        A6wood.GetComponent<Interactable>().IsEnabled = false;
        A7worker1.GetComponent<Interactable>().IsEnabled = false;
        A7worker2.GetComponent<Interactable>().IsEnabled = false;
        A7worker3.GetComponent<Interactable>().IsEnabled = false;
        A13Painter.GetComponent<Interactable>().IsEnabled = false;
        A13Laborer.GetComponent<Interactable>().IsEnabled = false;
        A13Carpenter.GetComponent<Interactable>().IsEnabled = false;
        A13Carpenter2.GetComponent<Interactable>().IsEnabled = false;
    }

    public void SetCubeFalse()
    {
        //set cube to inactive.
        A1Dozer.transform.Find("Cube").gameObject.SetActive(false);
        A1Stockpile.transform.Find("Cube").gameObject.SetActive(false);
        A2SteelBeam.transform.Find("Cube").gameObject.SetActive(false);
        A2Crane.transform.Find("Cube").gameObject.SetActive(false);
        A3Rebar.transform.Find("Cube").gameObject.SetActive(false);
        A3Truck.transform.Find("Cube").gameObject.SetActive(false);
        A4Worker1.transform.Find("Cube").gameObject.SetActive(false);
        A5DumpTruck.transform.Find("Cube").gameObject.SetActive(false);
        A5Loader.transform.Find("Cube").gameObject.SetActive(false);
        A5Stockpile.transform.Find("Cube").gameObject.SetActive(false);
        A6wood.transform.Find("Cube").gameObject.SetActive(false);
        A6rebar.transform.Find("Cube").gameObject.SetActive(false);
        A6log.transform.Find("Cube").gameObject.SetActive(false);
        A7worker1.transform.Find("Cube").gameObject.SetActive(false);
        A7worker2.transform.Find("Cube").gameObject.SetActive(false);
        A7worker3.transform.Find("Cube").gameObject.SetActive(false);
        A13Painter.transform.Find("Cube").gameObject.SetActive(false);
        A13Laborer.transform.Find("Cube").gameObject.SetActive(false);
        A13Carpenter.transform.Find("Cube").gameObject.SetActive(false);
        A13Carpenter2.transform.Find("Cube").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (onSensorChanged)
        {
            UpdateActivityList();
            ClearActivitiesDropdown();
            CreateActivitiesDropdown();
            onSensorChanged = false;
        }

        //after selection is made, activate run button.
        if (ResourceTaggedBool == true)
        { RunButton.SetActive(true); ResourceTaggedBool = false; }
        //if (ResourceTaggedBool == false)
        //{ RunButton.SetActive(false);}

        //display results
        //Display Report Panels
        if (GPSReportEnable)
        {
            GPSReportCanvas.SetActive(true);
            //GPS reporting function
            PrepareGPSString();
            GPSReportText.GetComponent<TextMeshProUGUI>().text = GPSReportString;
        }
        else
            GPSReportCanvas.SetActive(false);

        if (RFIDReportEnable)
        {
            RFIDReportCanvas.SetActive(true);
            //RFID reporting function
            PrepareRFIDString();
            RFIDReportText.GetComponent<TextMeshProUGUI>().text = RFIDReportString;
        }
        else
            RFIDReportCanvas.SetActive(false);


        if (IMUReportEnable)
        {
            //Debug.Log("IMU report inside Update().");
            //IMU reporting function
            IMUReportCanvas.SetActive(true);
            PrepareIMUString();
            IMUReportText.GetComponent<TextMeshProUGUI>().text = IMUReportString;
        }
        else
            IMUReportCanvas.SetActive(false);
    }
    #endregion


    #region supporting functions
    private void PrepareGPSString()
    {
        // Debug.Log("TEST PASS PARAM"+ ActivityManager.GetComponent<ActivityManagerScript>().A1_Dozer_GPS);
        GPSReportString = "";
        GPSReportString = ActivityManagerScript.GetComponent<ActivityManagerScript>().A1_Dozer_GPS
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A2_Load_GPS
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A3_Truck_GPS
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A5_Loader_GPS
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A5_Dumptruck_GPS
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A4_worker_GPS
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A7_w1_GPS
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A7_w2_GPS
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A7_w3_GPS;
    }

    //this is mainly for A6, original RFID
    private void PrepareRFIDString()
    {
        RFIDReportString = "";
        RFIDReportString = ActivityManagerScript.GetComponent<ActivityManagerScript>().A6_Wood_RFID
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A6_Log_RFID
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A6_Rebar_RFID
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A3RFID;
    }

    //A13 IMU
    private void PrepareIMUString()
    {
        IMUReportString = "";
        IMUReportString = ActivityManagerScript.GetComponent<ActivityManagerScript>().A14_c1_report
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A14_c2_report
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A14_l_report
            + ActivityManagerScript.GetComponent<ActivityManagerScript>().A14_p_report;
    }
    private void CreateSensorsDropdown()//Dropdown 1
    {
        SensorsList = new List<string> { "Sensors","GPS", "RFID", "Laser Scanner", "Drone", "IMU" };
        foreach (string option in SensorsList)
        {
            Dropdown1.CreateNewItem(option, null);
        }
        Dropdown1.dropdownEvent.AddListener(delegate { DropdownValueChanged(Dropdown1); });
        Dropdown1.SetupDropdown();
    }

    private void CreateActivityInitialDropdown()
    {
        foreach (string option in ActivityList)
        {
            Dropdown2.CreateNewItem(option, null);
        }
        //Dropdown2.dropdownEvent.AddListener(delegate { DropdownValueChanged(Dropdown2); });
        Dropdown2.SetupDropdown();
    }

    void DropdownValueChanged(CustomDropdown dropdown)
    {
        //Debug.Log("Dropdown Value Changed : " + dropdown.selectedItemIndex);
        //for (int i = 0; i < 5; ++i){ SelectedSensors[i] = false; }
        //SelectedSensors[dropdown.selectedItemIndex] = true;
        SelectedSensorIndex = dropdown.selectedItemIndex - 1;
        onSensorChanged = true;
    }

    void DropdownValueChangedActivity(CustomDropdown dropdown)
    {
        SelectedActivityIndex = dropdown.selectedItemIndex - 1;
        //Debug.Log("Dropdown 2 value change:"+dropdown.selectedItemIndex);
        TagButton.SetActive(true);
    }

    private void LUT()
    {
        if (SelectedSensorIndex == 0)//GPS
        {
            if (SelectedActivityIndex == 0) ActualActivityNumber = 1;
            if (SelectedActivityIndex == 1) ActualActivityNumber = 2;
            if (SelectedActivityIndex == 2) ActualActivityNumber = 3;
            if (SelectedActivityIndex == 3) ActualActivityNumber = 4;
            if (SelectedActivityIndex == 4) ActualActivityNumber = 5;
            if (SelectedActivityIndex == 5) ActualActivityNumber = 7;
        }
        if (SelectedSensorIndex == 1)//RFID
        {
            if (SelectedActivityIndex == 0) ActualActivityNumber = 3;
            if (SelectedActivityIndex == 1) ActualActivityNumber = 4;
            if (SelectedActivityIndex == 2) ActualActivityNumber = 6;
            if (SelectedActivityIndex == 3) ActualActivityNumber = 7;
        }
        if (SelectedSensorIndex == 2)//LS
        {
            if (SelectedActivityIndex == 0) ActualActivityNumber = 8;
            if (SelectedActivityIndex == 1) ActualActivityNumber = 9;
            if (SelectedActivityIndex == 2) ActualActivityNumber = 10;
            if (SelectedActivityIndex == 3) ActualActivityNumber = 11;
        }
        if (SelectedSensorIndex == 3)//Drone
        {
            if (SelectedActivityIndex == 0) ActualActivityNumber = 11;
            if (SelectedActivityIndex == 1) ActualActivityNumber = 12;
        }
        if (SelectedSensorIndex == 4)//IMU
        {
            if (SelectedActivityIndex == 0) ActualActivityNumber = 13;
        }
    }


    private void ClearActivitiesDropdown()
    {
        //To clear existing items.
        Dropdown2.dropdownItems.Clear(); 
        Dropdown2.SetupDropdown();
    }

    private void CreateActivitiesDropdown()
    {
        //Create using updated ResourcesList
        foreach (string option in ActivityList)
        {
            Dropdown2.CreateNewItem(option,null);
        }
        Dropdown2.dropdownEvent.AddListener(delegate { DropdownValueChangedActivity(Dropdown2); });
        Dropdown2.SetupDropdown();   
    }

    public void TagButtonAction()
    {
        //SelectedActivityIndex to ActualActivityNumber.
        LUT();

        //pointing arrow
        ActivityIndicator();

        //activate canvas that shows a list of resources.
        UpdateResourceText();
        ManualSelectionListPanel.SetActive(true);

        Debug.Log("Actual Activity Number: "+ActualActivityNumber);

        //Turn on interactable and box according to activity.
        if (ActualActivityNumber == 1)
        {
            A1Dozer.GetComponent<Interactable>().IsEnabled = true;
            A1Stockpile.GetComponent<Interactable>().IsEnabled = true;
            A1Dozer.transform.Find("Cube").gameObject.SetActive(true);
            A1Stockpile.transform.Find("Cube").gameObject.SetActive(true);
        }
        if (ActualActivityNumber == 2)
        {
            A2Crane.GetComponent<Interactable>().IsEnabled = true;
            A2SteelBeam.GetComponent<Interactable>().IsEnabled = true;
            A2Crane.transform.Find("Cube").gameObject.SetActive(true);
            A2SteelBeam.transform.Find("Cube").gameObject.SetActive(true);
        }
        if (ActualActivityNumber == 3)
        { 
            A3Rebar.transform.Find("Cube").gameObject.SetActive(true);
            A3Rebar.GetComponent<Interactable>().IsEnabled = true;
            A3Truck.transform.Find("Cube").gameObject.SetActive(true);
            A3Truck.GetComponent<Interactable>().IsEnabled = true;
        }
        if (ActualActivityNumber == 4)
        {
            A4Worker1.transform.Find("Cube").gameObject.SetActive(true);
            A4Worker1.GetComponent<Interactable>().IsEnabled = true;
        }
        if (ActualActivityNumber == 5)
        { 
            A5DumpTruck.GetComponent<Interactable>().IsEnabled = true;
            A5DumpTruck.transform.Find("Cube").gameObject.SetActive(true);
            A5Loader.GetComponent<Interactable>().IsEnabled = true;
            A5Loader.transform.Find("Cube").gameObject.SetActive(true);
            A5Stockpile.GetComponent<Interactable>().IsEnabled = true;
            A5Stockpile.transform.Find("Cube").gameObject.SetActive(true);
        }
        if (ActualActivityNumber == 6)
        {
            A6log.GetComponent<Interactable>().IsEnabled = true;
            A6log.transform.Find("Cube").gameObject.SetActive(true);
            A6rebar.GetComponent<Interactable>().IsEnabled = true;
            A6rebar.transform.Find("Cube").gameObject.SetActive(true);
            A6wood.GetComponent<Interactable>().IsEnabled = true;
            A6wood.transform.Find("Cube").gameObject.SetActive(true);
        }
        if (ActualActivityNumber == 7)
        { 
            A7worker1.GetComponent<Interactable>().IsEnabled = true;
            A7worker1.transform.Find("Cube").gameObject.SetActive(true);
            A7worker2.GetComponent<Interactable>().IsEnabled = true;
            A7worker2.transform.Find("Cube").gameObject.SetActive(true);
            A7worker3.GetComponent<Interactable>().IsEnabled = true;
            A7worker3.transform.Find("Cube").gameObject.SetActive(true);
        }

        //Skip for LS and Drone
        if (ActualActivityNumber == 8) RunButton.SetActive(true);
        if (ActualActivityNumber == 9) RunButton.SetActive(true);
        if (ActualActivityNumber == 10) RunButton.SetActive(true);
        if (ActualActivityNumber == 11) RunButton.SetActive(true);
        if (ActualActivityNumber == 12) RunButton.SetActive(true);

        //Worker IMU
        if (ActualActivityNumber == 13)
        {
            A13Painter.GetComponent<Interactable>().IsEnabled = true;
            A13Painter.transform.Find("Cube").gameObject.SetActive(true);
            A13Laborer.GetComponent<Interactable>().IsEnabled = true;
            A13Laborer.transform.Find("Cube").gameObject.SetActive(true);
            A13Carpenter.GetComponent<Interactable>().IsEnabled = true;
            A13Carpenter.transform.Find("Cube").gameObject.SetActive(true);
            A13Carpenter2.GetComponent<Interactable>().IsEnabled = true;
            A13Carpenter2.transform.Find("Cube").gameObject.SetActive(true);
        }
    }

    //List resources correspond to selected activity.
    private void UpdateResourceText()
    {
        string Text = "Dummy test resource for A1. \n A1R2. \n A1R3.";

        if (ActualActivityNumber == 1) Text = "Dozer \n Stockpile \n";
        if (ActualActivityNumber == 2) Text = "Crane \n Steel Beam \n";
        if (ActualActivityNumber == 3) Text = "Truck \n Rebar \n";
        if (ActualActivityNumber == 4) Text = "Worker 1 \n";
        if (ActualActivityNumber == 5) Text = "Loaser \n Dumptruck \n Stockpile \n";
        if (ActualActivityNumber == 6) Text = "Wood \n Log \n Rebar \n";
        if (ActualActivityNumber == 7) Text = "Worker 1 \n Worker 2 \n Worker 3 \n";
        if (ActualActivityNumber == 8) Text = "Scan Building. \n";
        if (ActualActivityNumber == 9) Text = "Scan Floor. \n";
        if (ActualActivityNumber == 10) Text = "Stockpile 1 \n Stockpile 2 \n";
        if (ActualActivityNumber == 11) Text = "Old Building \n";
        if (ActualActivityNumber == 12) Text = "Entire Jobsite. \n";
        if (ActualActivityNumber == 13) Text = " Painter \n Laborer \n Carpenter 1 \n Carpenter 2 \n";

        ManualSelectionListPanel.transform.Find("ResourceList").GetComponent<TextMeshProUGUI>().text = Text;

    }

    public void RunButtonFunction()
    {
        Debug.Log("Confirm selection and execute.");

        //disable canvas
        //gameObject.SetActive(false);
        ManualSelectionListPanel.SetActive(false);
        //ManualSelectionMenuSelf.SetActive(false);
        //enable reset button
        ResetButton.SetActive(true);

        //Turn off all interactable
        SetInteractablesFalse();

        //Turn off all box
        SetCubeFalse();

        //execute command
        ExecuteActivity();
        
    }

    private void ExecuteActivity()
    {
        
        //A1 + GPS
        if (SelectedSensorIndex == 0 && ActualActivityNumber == 1)
        {
            ActivityManagerScript.GetComponent<ActivityManagerScript>().select_1();
            GPSReportEnable = true; 
            ActivityManagerScript.GetComponent<ActivityManagerScript>().select_1_Dozer_GPS(); 
        }

        //A2 + GPS
        if (ActualActivityNumber == 2 && SelectedSensorIndex == 0)
        {
            ActivityManagerScript.GetComponent<ActivityManagerScript>().select_2();
            GPSReportEnable = true;
            ActivityManagerScript.GetComponent<ActivityManagerScript>().select_2_CraneLoad_GPS();
        }

        //A3 + GPS/RFID
        if (ActualActivityNumber == 3)
        {
            ActivityManagerScript.GetComponent<ActivityManagerScript>().select_3();
            if(A3Truck.GetComponent<ManualClickSelect>().TagStatus == true && SelectedSensorIndex == 0)
            { GPSReportEnable = true; ActivityManagerScript.GetComponent<ActivityManagerScript>().select_3_truck_GPS(); }
            if (A3Truck.GetComponent<ManualClickSelect>().TagStatus == true && SelectedSensorIndex == 1)
            { RFIDReportEnable = true; }// 3.Rebar RFID
        } 

        //A4
        if (ActualActivityNumber == 4)
        {
            ActivityManagerScript.GetComponent<ActivityManagerScript>().select_4(); 
            ActivityManagerScript.GetComponent<ActivityManagerScript>().select_2(); //Crane move
            //GPS
            if(A4Worker1.GetComponent<ManualClickSelect>().TagStatus == true && SelectedSensorIndex == 0)
            { GPSReportEnable = true; ActivityManagerScript.GetComponent<ActivityManagerScript>().select_4worker_gps(); }
            //RFID
            if (A4Worker1.GetComponent<ManualClickSelect>().TagStatus == true && SelectedSensorIndex == 1)
            { ActivityManagerScript.GetComponent<ActivityManagerScript>().select_4worker_RFID(); }
        } 

        //A5 Load and haul
        if (ActualActivityNumber == 5)
        {
            ActivityManagerScript.GetComponent<ActivityManagerScript>().select_5();
            if (A5DumpTruck.GetComponent<ManualClickSelect>().TagStatus == true) 
            { GPSReportEnable = true; ActivityManagerScript.GetComponent<ActivityManagerScript>().select_5_dumptruck_GPS(); }
            if(A5Loader.GetComponent<ManualClickSelect>().TagStatus == true) 
            { GPSReportEnable = true; ActivityManagerScript.GetComponent<ActivityManagerScript>().select_5_Loader_GPS(); }
        }
        
        //A6. RFID
        if (ActualActivityNumber == 6)
        {
            if (A6wood.GetComponent<ManualClickSelect>().TagStatus == true) ActivityManagerScript.GetComponent<ActivityManagerScript>().A6_wood_flag = true;
            if (A6log.GetComponent<ManualClickSelect>().TagStatus == true) ActivityManagerScript.GetComponent<ActivityManagerScript>().A6_log_flag = true;
            if (A6rebar.GetComponent<ManualClickSelect>().TagStatus == true) ActivityManagerScript.GetComponent<ActivityManagerScript>().A6_rebar_flag = true;
            ActivityManagerScript.GetComponent<ActivityManagerScript>().A6_RFID();
            RFIDReportEnable = true;
        }

            //A7.Workers on top floor + RFID / GPS
            if (ActualActivityNumber == 7)
        {
            if (A7worker1.GetComponent<ManualClickSelect>().TagStatus == true) ActivityManagerScript.GetComponent<ActivityManagerScript>().A7_w1_flag = true;
            if (A7worker2.GetComponent<ManualClickSelect>().TagStatus == true) ActivityManagerScript.GetComponent<ActivityManagerScript>().A7_w2_flag = true;
            if (A7worker3.GetComponent<ManualClickSelect>().TagStatus == true) ActivityManagerScript.GetComponent<ActivityManagerScript>().A7_w3_flag = true;
            //Run basic activity: danger zone red box. Based on worker selection bool.
            ActivityManagerScript.GetComponent<ActivityManagerScript>().select_7_new();
            //GPS only
            if (SelectedSensorIndex == 0) { GPSReportEnable = true; ActivityManagerScript.GetComponent<ActivityManagerScript>().select_7_GPS(); }
            //RFID only
            if (SelectedSensorIndex == 1) { ActivityManagerScript.GetComponent<ActivityManagerScript>().select_7_RFID(); }
        }

        //A8 scan part of building
        if (ActualActivityNumber == 8) ActivityManagerScript.GetComponent<ActivityManagerScript>().select_8();

        //A9 scan concrete slab
        if (ActualActivityNumber == 9) ActivityManagerScript.GetComponent<ActivityManagerScript>().select_9();

        //A10 scan stockpile, random pick between 10A 10B.
        if (ActualActivityNumber == 10) ActivityManagerScript.GetComponent<ActivityManagerScript>().select_10A();

        //A11 old bldg LS
        if (ActualActivityNumber == 11 && SelectedSensorIndex == 2) ActivityManagerScript.GetComponent<ActivityManagerScript>().select_11Laser();

        //A11 old bldg drone
        if (ActualActivityNumber == 11 && SelectedSensorIndex == 3) ActivityManagerScript.GetComponent<ActivityManagerScript>().select_11Drone();

        //A12 drone
        if (ActualActivityNumber == 12 && SelectedSensorIndex == 3) ActivityManagerScript.GetComponent<ActivityManagerScript>().select_12(); 

        //A13 IMU, dont need to specify sensor, because A13 is only reachable by selecting IMU in the first place.
        if (ActualActivityNumber == 13 && SelectedSensorIndex == 4)
        {
            if (A13Painter.GetComponent<ManualClickSelect>().TagStatus == true)
            { ActivityManagerScript.GetComponent<ActivityManagerScript>().A14_painter = true; }
            if (A13Laborer.GetComponent<ManualClickSelect>().TagStatus == true)
            { ActivityManagerScript.GetComponent<ActivityManagerScript>().A14_laborer = true; }
            if (A13Carpenter.GetComponent<ManualClickSelect>().TagStatus == true)
            { ActivityManagerScript.GetComponent<ActivityManagerScript>().A14_c1 = true; }
            if (A13Carpenter2.GetComponent<ManualClickSelect>().TagStatus == true)
            { ActivityManagerScript.GetComponent<ActivityManagerScript>().A14_c2 = true; }
            //With given worker bool, get IMU string, handle finish file write use backselected()
            ActivityManagerScript.GetComponent<ActivityManagerScript>().select_13_new();
            //display IMU
            IMUReportEnable = true;
            Debug.Log("IMU report enabled");
        }
    }


    private void ActivityIndicator()
    {
        string name = "A" + ActualActivityNumber + "POS";
        //Activate Chevron and live for 10 seconds.
        StartCoroutine(ShowAndHide(PointingChevron, name, 10.0f));
    }

    // Activate chevron, give location, and keep it active for 5 seconds.
    public IEnumerator ShowAndHide(GameObject go, string name, float delay)
    {
        go.GetComponent<DirectionalIndicator>().DirectionalTarget = GameObject.Find(name).transform;
        go.SetActive(true);
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }

    /// <summary>
    /// Update activity list based on selected sensors.
    /// </summary>
    private void UpdateActivityList()
    {
        ActivityList.Clear();
        //Activity correspond to GPS
        if (SelectedSensorIndex == 0) { ActivityList.AddRange(new string[] { "Activities","Dozer backfilling", "Crane Loading", "Material Delivery", "Worker's Close Call", "Load & Haul",
             "Detecting Fall" }); }
        if (SelectedSensorIndex == 1) { ActivityList.AddRange(new string[] {  "Activities","Material Delivery", "Worker's Close Call", 
            "Material Inventory", "Detecting Fall" }); }
        if (SelectedSensorIndex == 2) { ActivityList.AddRange(new string[] { "Activities", "Scan Building", "Scan Floor", "Scan Stockpile", "Scan Old Building" }); }
        if (SelectedSensorIndex == 3) { ActivityList.AddRange(new string[] { "Activities", "Scan Old Building", "Jobsite Inspection" }); }
        if (SelectedSensorIndex == 4) { ActivityList.AddRange(new string[] { "Activities", "Worker Ergonomics" }); }
    }

    public void ReloadSceneButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    
    #endregion
}
