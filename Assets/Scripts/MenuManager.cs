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

public class MenuManager : MonoBehaviour
{
    #region Parameters

    [SerializeField] private GameObject LegacyMainMenu; //LegacyMainMenu
    [SerializeField] private GameObject IMUReportCanvas;
    [SerializeField] private GameObject GPSReportCanvas;
    [SerializeField] private GameObject RFIDReportCanvas;
    [SerializeField] private GameObject ComboSelectionCanvas;
    [SerializeField] private GameObject PointingChevron;
    [SerializeField] private GameObject SelectButton;
    [SerializeField] private GameObject StopButtonObj;
    [SerializeField] private GameObject NAButton;
    public GameObject ActivityManager;

    public DropdownMultiSelect Mdropdown1;// single selection activity
    public DropdownMultiSelect Mdropdown2;// sensors
    public DropdownMultiSelect Mdropdown3;// resources
    public GameObject Dropdown1Title;
    public GameObject Dropdown2Title;
    public GameObject Dropdown1_ListParentNode;
    public GameObject Dropdown2_ListParentNode;
    public GameObject Dropdown3_ListParentNode;

    private string ComboDisplayString;
    private string SelectedString;
    private string DisplayString;
    public TextMeshProUGUI DisplayResults;
    public TextMeshProUGUI GPSReportText;
    public TextMeshProUGUI RFIDReportText;
    public TextMeshProUGUI IMUReportText;
    public TextMeshProUGUI ShowSelectionText;

    private bool[] SelectedActivities = new bool[16];
    private bool[] SelectedSensors = new bool[5];
    private string[] SelectedResources = new string[16];
    private bool[] LastIterationSelectedActivities = new bool[16];

    List<string> ActivityList;
    List<string> SensorsList;
    List<string> ResourcesList = new List<string> { };
    Dictionary<int, List<string>> ComboList = new Dictionary<int, List<string>>();


    private bool onActivityChanged;
    private bool MultiLaserScan;
    private bool RFIDReportEnable;
    private bool GPSReportEnable;
    private bool IMUReportEnable;
    private bool ChangeResourcesBool;
    private bool ShowComboBool;
    private bool CurrentConfigurationBool = true;
    private bool A11Drone;
    private bool A11LS;

    private string GPSReportString;
    private string RFIDReportString;
    private string IMUReportString;

    private bool ActivityDirectionalIndicatorBool;

    [SerializeField] private GameObject Everything;
    [SerializeField] private GameObject A1Position;

    private int CurrentActivitySelection;

    [SerializeField]
    [Tooltip("Assign DialogSmall_192x96.prefab")]
    private GameObject dialogPrefabSmall;

    private int CurrentSensorConfig;

    private bool Dropdown1ShowCurrentActivity;
    private bool Dropdown2ShowCurrentSensor;
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
        PointingChevron.SetActive(false);
        ComboSelectionCanvas.SetActive(false);
        StopButtonObj.SetActive(false);
        NAButton.SetActive(false);

        Mdropdown2.GetComponent<Button>().interactable = false;
        Mdropdown3.GetComponent<Button>().interactable = false;
        SelectButton.SetActive(false);

        CurrentActivitySelection = 0;
    }


    void Update()
    {
        //update selected activity for each frame
        UpdateActivitySelected();

        UpdateSensorSelected();

        //Check if multiple laser scan activity selected
        //MultiLaserScanCheck();

        //When press confirm sensors button, update resources dropdown.
        if (ChangeResourcesBool)//transform.hasChanged
        {
            //Debug.Log("Detect Activity Change");
            //UpdateResourcesList(); // old method
            UpdateResourcesListSolo();
            ClearResourcesDropdown();
            CreateResourcesDropdown();
            ChangeResourcesBool = false;
        }

        UpdateResourcesSelected();
        //if multiple selected, then prompt.
        //if (MultiLaserScan)
        //{
        //DisplayWarning.GetComponent<TextMeshProUGUI>().text = "We only have 1 Laser Scanner, Please only select 1 Laser Scanner Activity."; 
        //    Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Warning", "We only have 1 Laser Scanner, Please only select 1 Laser Scanner Activity.", false);
        //}


        //Display results that fit filter condition
        //DisplayResults.GetComponent<TextMeshProUGUI>().text = "Old String Selected" + SelectedString;
        //int ActivityDisplayNumber = CurrentActivitySelection + 1;

        //Show current configuration in dropdown 1, after press activity confirm button.
        if(Dropdown1ShowCurrentActivity) Dropdown1Title.GetComponent<TextMeshProUGUI>().text = Mdropdown1.dropdownItems[CurrentActivitySelection].itemName;

        //Show current sensor configuration in dropdown 1.
        if (Dropdown2ShowCurrentSensor) Dropdown2Title.GetComponent<TextMeshProUGUI>().text = Mdropdown2.dropdownItems[CurrentSensorConfig].itemName;


        //Show current configuration
        if (CurrentConfigurationBool)
        {
            PrepareCurrentConfigDisplay();
            DisplayResults.GetComponent<TextMeshProUGUI>().text = DisplayString;
        }
        else DisplayResults.GetComponent<TextMeshProUGUI>().text = "Finished Configuration, Press Select Button To Start.";

        //Show all configured combo in panel.
        if (ShowComboBool)
        {
            ComboSelectionCanvas.SetActive(true);
            PrepareComboDisplayString();
            ShowSelectionText.GetComponent<TextMeshProUGUI>().text = ComboDisplayString;
        }


        //Display Report Panels
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
        ActivityList = new List<string> { "Dozer backfilling", "Crane Loading", "Material Delivery", "Worker's Close Call", "Load & Haul",
            "Material Inventory", "Detecting Fall", "Scan Building", "Scan Floor", "Scan Stockpile", "Scan Old Building", "Jobsite Inspection", "Worker Ergonomics."};
        foreach (string option in ActivityList)
        {
            Mdropdown1.SetItemTitle(option);
            Mdropdown1.CreateNewItem();
        }
        Mdropdown1.SetupDropdown();
    }

    private void CreateSensorsDropdown()
    {
        SensorsList = new List<string> { "GPS", "RFID", "Laser Scanner", "Drone", "IMU" };
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
                //SelectedString = SelectedString + " , " + Mdropdown1.dropdownItems[i].itemName;
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
            else
            { SelectedResources[i] = "void"; }
        }
    }


    //update resources list accroding to dropdown 1's selection
    public void UpdateResourcesList()
    {
        //Initialize as empty list
        ResourcesList.Clear();

        //Add items according to different activities
        if (SelectedActivities[0] == true) ResourcesList.AddRange(new string[] { "Dozer", "Stockpile" });//1
        if (SelectedActivities[1] == true) ResourcesList.AddRange(new string[] { "Crane", "Load" });//2
        if (SelectedActivities[2] == true) ResourcesList.AddRange(new string[] { "Truck", "Rebar" });
        if (SelectedActivities[3] == true) ResourcesList.AddRange(new string[] { "Worker 1" });
        if (SelectedActivities[4] == true) ResourcesList.AddRange(new string[] { "Loader", "Dumptruck", "Stockpile" });
        if (SelectedActivities[5] == true) ResourcesList.AddRange(new string[] { "Wood", "Log", "Rebar" });
        if (SelectedActivities[6] == true) ResourcesList.AddRange(new string[] { "Worker 1", "Worker 2", "Worker 3" });
        if (SelectedActivities[7] == true) ResourcesList.AddRange(new string[] { "Building 1" });
        if (SelectedActivities[8] == true) ResourcesList.AddRange(new string[] { "Top Floor" });
        if (SelectedActivities[9] == true) ResourcesList.AddRange(new string[] { "Stockpile 1", "Stockpile 2" });//11
        if (SelectedActivities[10] == true) ResourcesList.AddRange(new string[] { "Old Building" });//12
        if (SelectedActivities[11] == true) ResourcesList.AddRange(new string[] { "Jobsite" });//13
        if (SelectedActivities[12] == true) ResourcesList.AddRange(new string[] { "Painter", "Laborer", "arpenter 1", "Carpenter 2" });//14

    }

    //update resources list accroding to new ui button
    public void UpdateResourcesListSolo()
    {
        //Initialize as empty list
        ResourcesList.Clear();

        //Add items according to different activities
        if (CurrentActivitySelection == 0) ResourcesList.AddRange(new string[] { "Dozer", "Stockpile" });//1
        if (CurrentActivitySelection == 1) ResourcesList.AddRange(new string[] { "Crane", "Load" });//2
        if (CurrentActivitySelection == 2) ResourcesList.AddRange(new string[] { "Truck", "Rebar" });
        if (CurrentActivitySelection == 3) ResourcesList.AddRange(new string[] { "Worker 1" });
        if (CurrentActivitySelection == 4) ResourcesList.AddRange(new string[] { "Loader", "Dumptruck", "Stockpile" });
        if (CurrentActivitySelection == 5) ResourcesList.AddRange(new string[] { "Wood", "Log", "Rebar" });
        if (CurrentActivitySelection == 6) ResourcesList.AddRange(new string[] { "Worker 1", "Worker 2", "Worker 3" });
        if (CurrentActivitySelection == 7) ResourcesList.AddRange(new string[] { "Building 1" });
        if (CurrentActivitySelection == 8) ResourcesList.AddRange(new string[] { "Top Floor" });
        if (CurrentActivitySelection == 9) ResourcesList.AddRange(new string[] { "Stockpile 1", "Stockpile 2" });//11
        if (CurrentActivitySelection == 10) ResourcesList.AddRange(new string[] { "Old Building" });//12
        if (CurrentActivitySelection == 11) ResourcesList.AddRange(new string[] { "Jobsite" });//13
        if (CurrentActivitySelection == 12) ResourcesList.AddRange(new string[] { "Painter", "Laborer", "Carpenter 1", "Carpenter 2" });//14

    }


    //10/4/2020 New implementation based on previous select function
    public void Select()
    {
        //disable canvas that show combo selections
        ComboSelectionCanvas.SetActive(false);
        //ActivityManager.GetComponent<ActivityManagerScript>().sensorSelected();

        //Initialize combos
        string[] ExeList = new string[14];
        for (int j = 0; j < ComboList.Count; j++)
        {
            ExeList[ComboList.ElementAt(j).Key] = string.Join(", ", ComboList.ElementAt(j).Value);
        }



        if (SelectedActivities[0] == true)
            ActivityManager.GetComponent<ActivityManagerScript>().select_1();
        if (SelectedActivities[0] == true && ExeList[0].Contains("GPS") == true && ExeList[0].Contains("Dozer") == true)
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_1_Dozer_GPS(); } //1.Dozer GPS

        if (SelectedActivities[1] == true)
            ActivityManager.GetComponent<ActivityManagerScript>().select_2();
        if (SelectedActivities[1] == true && ExeList[1].Contains("GPS") == true && ExeList[1].Contains("Load") == true)
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_2_CraneLoad_GPS(); }//2.Load GPS

        if (SelectedActivities[2] == true)
            ActivityManager.GetComponent<ActivityManagerScript>().select_3();
        if (SelectedActivities[2] == true && ExeList[2].Contains("GPS") == true && ExeList[2].Contains("Truck") == true)
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_3_truck_GPS(); }//3.truck GPS
        if (SelectedActivities[2] == true && ExeList[2].Contains("RFID") == true && ExeList[2].Contains("Rebar") == true)
        { RFIDReportEnable = true; } //3.Rebar RFID


        if (SelectedActivities[3] == true) ActivityManager.GetComponent<ActivityManagerScript>().select_4();
        if (SelectedActivities[3] == true && ExeList[3].Contains("GPS") == true && ExeList[3].Contains("Worker 1"))
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_4worker_gps(); }
        if (SelectedActivities[3] == true && ExeList[3].Contains("RFID") == true && ExeList[3].Contains("Worker 1"))
        { ActivityManager.GetComponent<ActivityManagerScript>().select_4worker_RFID(); }//Since we are using panel above worker, so no RFID panel needed.
        if (SelectedActivities[3] == true && ExeList[3].Contains("GPS") == true && ExeList[3].Contains("RFID") == true && ExeList[3].Contains("Worker 1"))
        { ActivityManager.GetComponent<ActivityManagerScript>().A4_worker_GPSRFID = true; ActivityManager.GetComponent<ActivityManagerScript>().select_4worker_RFID(); }

        if (SelectedActivities[4] == true)
            ActivityManager.GetComponent<ActivityManagerScript>().select_5();
        if (SelectedActivities[4] == true && ExeList[4].Contains("GPS") == true && ExeList[4].Contains("Loader"))
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_5_Loader_GPS(); }
        if (SelectedActivities[4] == true && ExeList[4].Contains("GPS") == true && ExeList[4].Contains("Dumptruck"))
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_5_dumptruck_GPS(); }

        //RFID & rebar, wood and log.
        if (SelectedActivities[5] == true && ExeList[5].Contains("RFID") == true)
        {
            if (ExeList[5].Contains("Wood")) ActivityManager.GetComponent<ActivityManagerScript>().A6_wood_flag = true;
            if (ExeList[5].Contains("Log")) ActivityManager.GetComponent<ActivityManagerScript>().A6_log_flag = true;
            if (ExeList[5].Contains("Rebar")) ActivityManager.GetComponent<ActivityManagerScript>().A6_rebar_flag = true;
            ActivityManager.GetComponent<ActivityManagerScript>().A6_RFID();
            RFIDReportEnable = true;
        }

        //7. Workers on top floor
        if (SelectedActivities[6] == true)
        {
            if (ExeList[6].Contains("Worker 1")) ActivityManager.GetComponent<ActivityManagerScript>().A7_w1_flag = true;
            if (ExeList[6].Contains("Worker 2")) ActivityManager.GetComponent<ActivityManagerScript>().A7_w2_flag = true;
            if (ExeList[6].Contains("Worker 3")) ActivityManager.GetComponent<ActivityManagerScript>().A7_w3_flag = true;

            //Run basic activity: danger zone red box. Based on worker selection bool.
            ActivityManager.GetComponent<ActivityManagerScript>().select_7_new();

            //GPS only
            if (ExeList[6].Contains("GPS") == true) { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_7_GPS(); }

            //RFID only, GPS hidden.
            if (ExeList[6].Contains("RFID") == true) { ActivityManager.GetComponent<ActivityManagerScript>().select_7_RFID(); }

            //RFID and GPS
            if (ExeList[6].Contains("GPS") == true && ExeList[6].Contains("RFID") == true)
            { ActivityManager.GetComponent<ActivityManagerScript>().A7_worker_GPSRFID = true; ActivityManager.GetComponent<ActivityManagerScript>().select_7_RFID(); }

        }


        //Laser Scan related activities [7,8,9,10]
        //if MultiLaserScan, then cannot proceed
        if (MultiLaserScan == false) { if (SelectedActivities[7] == true) ActivityManager.GetComponent<ActivityManagerScript>().select_8(); }//8.scan part of building
        if (MultiLaserScan == false) { if (SelectedActivities[8] == true) ActivityManager.GetComponent<ActivityManagerScript>().select_9(); }//9.scan concrete slab
        if (MultiLaserScan == false) { if (SelectedActivities[9] == true && ExeList[9].Contains("Laser Scanner") == true && ExeList[9].Contains("Stockpile 1")) ActivityManager.GetComponent<ActivityManagerScript>().select_10A(); }
        if (MultiLaserScan == false) { if (SelectedActivities[9] == true && ExeList[9].Contains("Laser Scanner") == true && ExeList[9].Contains("Stockpile 2")) ActivityManager.GetComponent<ActivityManagerScript>().select_10B(); }

        //A11 Old House LS 
        if (MultiLaserScan == false) { if (SelectedActivities[10] == true && ExeList[10].Contains("Laser Scanner") == true && ExeList[10].Contains("Drone") == false && ExeList[10].Contains("Old Building")) ActivityManager.GetComponent<ActivityManagerScript>().select_11Laser(); }
        //11. Old house drone
        if (SelectedActivities[10] == true && ExeList[10].Contains("Drone") == true && ExeList[10].Contains("Laser Scanner") == false && ExeList[10].Contains("Old Building")) ActivityManager.GetComponent<ActivityManagerScript>().select_11Drone();
        //11. Old House LS && Drone
        if (SelectedActivities[10] == true && ExeList[10].Contains("Drone") == true && ExeList[10].Contains("Laser Scanner") == true && ExeList[10].Contains("Old Building"))
        {
            OpenChoiceDialogSmall();
            if(A11Drone) ActivityManager.GetComponent<ActivityManagerScript>().select_11Drone();
            if(A11LS) ActivityManager.GetComponent<ActivityManagerScript>().select_11Laser();
        }

        //12. Jobsite drone
        if (SelectedActivities[11] == true && ExeList[11].Contains("Drone") == true && ExeList[11].Contains("Jobsite")) ActivityManager.GetComponent<ActivityManagerScript>().select_12();

        //13.IMU workers
        if (SelectedActivities[12] == true && ExeList[12].Contains("IMU") == true)
        {
            if (ExeList[12].Contains("Painter"))
            { ActivityManager.GetComponent<ActivityManagerScript>().A14_painter = true; }
            if (ExeList[12].Contains("Laborer"))
            { ActivityManager.GetComponent<ActivityManagerScript>().A14_laborer = true; }
            if (ExeList[12].Contains("Carpenter 1"))
            { ActivityManager.GetComponent<ActivityManagerScript>().A14_c1 = true; }
            if (ExeList[12].Contains("Carpenter 2"))
            { ActivityManager.GetComponent<ActivityManagerScript>().A14_c2 = true; }
            //With given worker bool, get IMU string, handle finish file write use backselected()
            ActivityManager.GetComponent<ActivityManagerScript>().select_13_new();
            //display IMU
            IMUReportEnable = true;
        }

        //Acitivity Idrection Indicator
        ActivityIndicator();
        //SelectButton.SetActive(false);
        StopButtonObj.SetActive(true);
    }


    

    private void MultiLaserScanCheck()
    {
        MultiLaserScan = false;
        if ((SelectedActivities[7] ? 1 : 0) + (SelectedActivities[8] ? 1 : 0) + (SelectedActivities[9] ? 1 : 0) + (SelectedActivities[10] ? 1 : 0) > 1)
        {
            MultiLaserScan = true;
            Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Warning", "We only have 1 Laser Scanner, Please only select 1 Laser Scanner Activity.", false);
        }
    }

    private void PrepareGPSString()
    {
        // Debug.Log("TEST PASS PARAM"+ ActivityManager.GetComponent<ActivityManagerScript>().A1_Dozer_GPS);
        GPSReportString = "";
        GPSReportString = ActivityManager.GetComponent<ActivityManagerScript>().A1_Dozer_GPS
            + ActivityManager.GetComponent<ActivityManagerScript>().A2_Load_GPS
            + ActivityManager.GetComponent<ActivityManagerScript>().A3_Truck_GPS
            + ActivityManager.GetComponent<ActivityManagerScript>().A5_Loader_GPS
            + ActivityManager.GetComponent<ActivityManagerScript>().A5_Dumptruck_GPS
            + ActivityManager.GetComponent<ActivityManagerScript>().A4_worker_GPS
            + ActivityManager.GetComponent<ActivityManagerScript>().A7_w1_GPS
            + ActivityManager.GetComponent<ActivityManagerScript>().A7_w2_GPS
            + ActivityManager.GetComponent<ActivityManagerScript>().A7_w3_GPS;
    }

    //this is mainly for A6, original RFID
    private void PrepareRFIDString()
    {
        RFIDReportString = "";
        RFIDReportString = ActivityManager.GetComponent<ActivityManagerScript>().A6_Wood_RFID
            + ActivityManager.GetComponent<ActivityManagerScript>().A6_Log_RFID
            + ActivityManager.GetComponent<ActivityManagerScript>().A6_Rebar_RFID
            + ActivityManager.GetComponent<ActivityManagerScript>().A3RFID;
    }

    //A13 IMU
    private void PrepareIMUString()
    {
        IMUReportString = "";
        IMUReportString = ActivityManager.GetComponent<ActivityManagerScript>().A14_c1_report
            + ActivityManager.GetComponent<ActivityManagerScript>().A14_c2_report
            + ActivityManager.GetComponent<ActivityManagerScript>().A14_l_report
            + ActivityManager.GetComponent<ActivityManagerScript>().A14_p_report;
    }

    private void PrepareComboDisplayString()
    {
        ComboDisplayString = "Current Configured Combos: \n";
        for (int j = 0; j < ComboList.Count; j++)
        {
            //Debug.Log(String.Format("Key: {0}, Value: {1}", ComboList.ElementAt(j).Key, string.Join(", ", ComboList.ElementAt(j).Value)));
            ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[ComboList.ElementAt(j).Key].itemName + string.Join(", ", ComboList.ElementAt(j).Value) + ". ";
        }
    }

    private void PrepareCurrentConfigDisplay()
    {
        //Current Activity Selection by default is 0, but it may not be active.
        if (CurrentActivitySelection == 0 && SelectedActivities[0] == false) DisplayString = "Select activity.";
        else
            DisplayString = "Currently Configuring Activity:" + Mdropdown1.dropdownItems[CurrentActivitySelection].itemName + ". \n" +
                "Selected:" + SelectedString + ".";

        SelectedString = "";
    }

    public void A13_stop()
    {
        IMUReportEnable = false;
        IMUReportCanvas.SetActive(false);
        ActivityManager.GetComponent<ActivityManagerScript>().A13_stop();

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

    private void ActivityIndicator()
    {
        int j = 0;
        //If more than 1 activity selected, or no activity selected, no indicator.
        if (((SelectedActivities[0] ? 1 : 0) + (SelectedActivities[1] ? 1 : 0)
            + (SelectedActivities[2] ? 1 : 0) + (SelectedActivities[3] ? 1 : 0)
            + (SelectedActivities[4] ? 1 : 0) + (SelectedActivities[5] ? 1 : 0)
            + (SelectedActivities[6] ? 1 : 0) + (SelectedActivities[7] ? 1 : 0)
            + (SelectedActivities[8] ? 1 : 0) + (SelectedActivities[9] ? 1 : 0)
            + (SelectedActivities[10] ? 1 : 0) + (SelectedActivities[11] ? 1 : 0)
            + (SelectedActivities[12] ? 1 : 0) > 1) || ((SelectedActivities[0] ? 1 : 0) + (SelectedActivities[1] ? 1 : 0)
            + (SelectedActivities[2] ? 1 : 0) + (SelectedActivities[3] ? 1 : 0)
            + (SelectedActivities[4] ? 1 : 0) + (SelectedActivities[5] ? 1 : 0)
            + (SelectedActivities[6] ? 1 : 0) + (SelectedActivities[7] ? 1 : 0)
            + (SelectedActivities[8] ? 1 : 0) + (SelectedActivities[9] ? 1 : 0)
            + (SelectedActivities[10] ? 1 : 0) + (SelectedActivities[11] ? 1 : 0)
            + (SelectedActivities[12] ? 1 : 0) == 0))
        { ActivityDirectionalIndicatorBool = false; }
        else { ActivityDirectionalIndicatorBool = true; }

        //Direction Indicator Execute
        if (ActivityDirectionalIndicatorBool)
        {
            //What is current selected activity?
            int children = Dropdown1_ListParentNode.transform.childCount;
            for (int i = 0; i < children; ++i)
            {
                if (SelectedActivities[i] == true)
                {
                    j = i + 1;
                    break;
                }
            }

            string name = "A" + j + "POS";
            //Activate Chevron and live for 5 seconds.
            StartCoroutine(ShowAndHide(PointingChevron, name, 5.0f));

        }

        //Vector3 Original = Everything.transform.position;
        //Vector3 ActivityPosition = A1Position.transform.position;
        //Vector3 TransformedCoordinates = Original - ActivityPosition;
        //Everything.transform.position = TransformedCoordinates;

    }

    // Activate chevron, give location, and keep it active for 5 seconds.
    IEnumerator ShowAndHide(GameObject go, string name, float delay)
    {
        go.GetComponent<DirectionalIndicator>().DirectionalTarget = GameObject.Find(name).transform;
        go.SetActive(true);
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }

    #endregion




    //----------------------------------------------------------


    #region UI related functions

    //Activities Confirm.
    public void A_Confirm()
    {

        //Check if multiple laser scan activity selected
        MultiLaserScanCheck();

        //cannot proceed if Multi Laser Scan Detected
        if (!MultiLaserScan)
        {
            //Dropdown 1 inactive.
            Mdropdown1.GetComponent<Button>().interactable = false;
            //Dropdown 2 active.
            Mdropdown2.GetComponent<Button>().interactable = true;
        }

        //Find first active activity number
        for (int i = 0; i < 13; ++i)
        {
            if (SelectedActivities[i] == true)
            { CurrentActivitySelection = i; break; }

        }

        Debug.Log("A-confirm activity:" + CurrentActivitySelection);
        Dropdown1ShowCurrentActivity = true;

    }

    //Sensor Confirm.
    public void S_Confirm()
    {
        CheckActivitySensors();

        //update resources list for CurrentActivitySelection
        ChangeResourcesBool = true;

        //Find first active sensor, and set as current configuring sensor.
        for (int i = 0; i < 4; ++i)
        {
            if (SelectedSensors[i] == true)
            { CurrentSensorConfig = i; break; }
        }

        Dropdown2ShowCurrentSensor = true;
}

    private void CheckActivitySensors()
    {
        bool pass = false;
        //Debug.Log("CAS"+CurrentActivitySelection+ SelectedSensors[0]+ SelectedSensors[1]);
        //Check if selected sensors fit selected activity? If fit, give pass.
        //"GPS", "RFID", "Laser Scanner", "Drone", "IMU"

        Debug.Log("CurrentActivitySelection: "+CurrentActivitySelection);
        for (int j = 0; j < SelectedSensors.Length;j++ )
        {
            Debug.Log("Selected sensor:" + SelectedSensors[j]);
        }


        //A1+GPS[0]
        if (CurrentActivitySelection == 0 && SelectedSensors[0] == true && SelectedSensors[1] == false && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else { if (CurrentActivitySelection == 0 && SelectedSensors[1] == true) RFIDWarning(); if (CurrentActivitySelection == 0 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 0 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 0 && SelectedSensors[4] == true) IMUWarning(); }
        //A2+GPS
        if (CurrentActivitySelection == 1 && SelectedSensors[0] == true && SelectedSensors[1] == false && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else { if (CurrentActivitySelection == 1 && SelectedSensors[1] == true) RFIDWarning(); if (CurrentActivitySelection == 1 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 1 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 1 && SelectedSensors[4] == true) IMUWarning(); }
        //A3+GPS/RFID
        if (CurrentActivitySelection == 2 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else {  if (CurrentActivitySelection == 2 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 2 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 2 && SelectedSensors[4] == true) IMUWarning(); }
        //A4+GPS/RFID
        if (CurrentActivitySelection == 3 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else {  if (CurrentActivitySelection == 3 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 3 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 3 && SelectedSensors[4] == true) IMUWarning(); }
        //A5+GPS
        if (CurrentActivitySelection == 4 && SelectedSensors[0] == true && SelectedSensors[1] == false && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else { if (CurrentActivitySelection == 4 && SelectedSensors[1] == true) RFIDWarning(); if (CurrentActivitySelection == 4 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 4 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 4 && SelectedSensors[4] == true) IMUWarning(); }
        //A6+RFID
        if (CurrentActivitySelection == 5 && SelectedSensors[0] == false && SelectedSensors[1] == true && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else { if (CurrentActivitySelection == 5 && SelectedSensors[0] == true) GPSWarning(); if (CurrentActivitySelection == 5 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 5 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 5 && SelectedSensors[4] == true) IMUWarning(); }
        //A7+GPSRFID
        if (CurrentActivitySelection == 6 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else {  if (CurrentActivitySelection == 6 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 6 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 6 && SelectedSensors[4] == true) IMUWarning(); }
        //A8+LS
        if (CurrentActivitySelection == 7 && SelectedSensors[0] == false && SelectedSensors[1] == false && SelectedSensors[2] == true && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else { if (CurrentActivitySelection == 7 && SelectedSensors[1] == true) RFIDWarning(); if (CurrentActivitySelection == 7 && SelectedSensors[0] == true) GPSWarning(); if (CurrentActivitySelection == 7 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 7 && SelectedSensors[4] == true) IMUWarning(); }
        //A9+LS
        if (CurrentActivitySelection == 8 && SelectedSensors[0] == false && SelectedSensors[1] == false && SelectedSensors[2] == true && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else { if (CurrentActivitySelection == 8 && SelectedSensors[1] == true) RFIDWarning(); if (CurrentActivitySelection == 8 && SelectedSensors[0] == true) GPSWarning(); if (CurrentActivitySelection == 8 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 8 && SelectedSensors[4] == true) IMUWarning(); }
        //A10+LS
        if (CurrentActivitySelection == 9 && SelectedSensors[0] == false && SelectedSensors[1] == false && SelectedSensors[2] == true && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else { if (CurrentActivitySelection == 9 && SelectedSensors[1] == true) RFIDWarning(); if (CurrentActivitySelection == 9 && SelectedSensors[0] == true) GPSWarning(); if (CurrentActivitySelection == 9 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 9 && SelectedSensors[4] == true) IMUWarning(); }
        //A11+LS/Drone
        if (CurrentActivitySelection == 10 && SelectedSensors[0] == false && SelectedSensors[1] == false && (SelectedSensors[2] == true || SelectedSensors[3] == true) && SelectedSensors[4] == false) { pass = true; }
        else { if (CurrentActivitySelection == 10 && SelectedSensors[1] == true) RFIDWarning(); if (CurrentActivitySelection == 10 && SelectedSensors[0] == true) GPSWarning();  if (CurrentActivitySelection == 10 && SelectedSensors[4] == true) IMUWarning(); }
        //A12+drone
        if (CurrentActivitySelection == 11 && SelectedSensors[0] == false && SelectedSensors[1] == false && SelectedSensors[2] == false && SelectedSensors[3] == true && SelectedSensors[4] == false) { pass = true; }
        else { if (CurrentActivitySelection == 11 && SelectedSensors[1] == true) RFIDWarning(); if (CurrentActivitySelection == 11 && SelectedSensors[0] == true) GPSWarning(); if (CurrentActivitySelection == 11 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 11 && SelectedSensors[4] == true) IMUWarning(); }
        //A13+IMU
        if (CurrentActivitySelection == 12 && SelectedSensors[0] == false && SelectedSensors[1] == false && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == true) { pass = true; }
        else { if (CurrentActivitySelection == 12 && SelectedSensors[1] == true) RFIDWarning(); if (CurrentActivitySelection == 12 && SelectedSensors[0] == true) GPSWarning(); if (CurrentActivitySelection == 12 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 12 && SelectedSensors[2] == true) LSWarning(); }

        if (pass)
        {
            //Dropdown 2 inactive.
            Mdropdown2.GetComponent<Button>().interactable = false;
            //Dropdown 3 active.
            Mdropdown3.GetComponent<Button>().interactable = true;
        }
        //else
        //{
            //dialogue warning
            //Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Warning",
             //   "Wrong sensor selected for this activity, Please correct your selection!", false);
            //reset selection
        //}
    }

            private void GPSWarning() {
                Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "GPS Warning",
        "Wrong sensor selected for this activity, Please correct your selection!", false);
            }

            private void RFIDWarning()
            {
                Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "RFID Warning",
        "Wrong sensor selected for this activity, Please correct your selection!", false);
            }

            private void LSWarning()
            {
                Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Laser Scanner Warning",
        "Wrong sensor selected for this activity, Please correct your selection!", false);
            }

            private void DroneWarning()
            {
                Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Drone Warning",
        "Wrong sensor selected for this activity, Please correct your selection!", false);
            }

            private void IMUWarning()
            {
                Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "IMU Warning",
        "Wrong sensor selected for this activity, Please correct your selection!", false);
            }

    //Resources Confirm
    public void R_Confirm()
    {
        //CheckSensorsResources();

        bool pass = false;

        //Check Sensors Resources.
        // Which activity, correct sensor (don't need all, already determined in previouys step), correct resources?
        //A1+GPS[0]+1.dozer
        if (CurrentActivitySelection == 0 && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "Dozer") && !Array.Exists(SelectedResources, element => element == "Stockpile")) { pass = true; }
        else { if(CurrentActivitySelection == 0 && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "Stockpile")) Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning",
        "Stockpile cannot be used with GPS", false);
        }
        //A2+GPS+2.load
        if (CurrentActivitySelection == 1 && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "Load")) pass = true;
        //A3+GPS+truck
        if (CurrentActivitySelection == 2 && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "Truck")) pass = true;
        //A3+RFID+Rebar
        if (CurrentActivitySelection == 2 && SelectedSensors[1] == true && Array.Exists(SelectedResources, element => element == "Rebar")) pass = true;
        //A3+(GPS&&RFID)+(truck&&rebar)
        if (CurrentActivitySelection == 2 && (SelectedSensors[0] == true && SelectedSensors[1] == true) && (Array.Exists(SelectedResources, element => element == "Truck") && Array.Exists(SelectedResources, element => element == "Rebar"))) pass = true;
        //A4+GPS/RFID+worker
        if (CurrentActivitySelection == 3 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && Array.Exists(SelectedResources, element => element == "Worker 1")) pass = true;
        //A5+GPS+loader/truck
        if (CurrentActivitySelection == 4 && SelectedSensors[0] == true && (Array.Exists(SelectedResources, element => element == "Loader")) || Array.Exists(SelectedResources, element => element == "Dumptruck")) pass = true;
        //A6+RFID+WLR
        if (CurrentActivitySelection == 5 && SelectedSensors[1] == true && (Array.Exists(SelectedResources, element => element == "Wood") || Array.Exists(SelectedResources, element => element == "Log") || Array.Exists(SelectedResources, element => element == "Rebar"))) pass = true;
        //A7+(GPS||RFID)&&(w1w2w3)
        if (CurrentActivitySelection == 6 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && (Array.Exists(SelectedResources, element => element == "Worker 1") || Array.Exists(SelectedResources, element => element == "Worker 2") || Array.Exists(SelectedResources, element => element == "Worker 3"))) pass = true;
        //A8+LS
        if (CurrentActivitySelection == 7 && SelectedSensors[2] == true) pass = true;
        //A9+LS
        if (CurrentActivitySelection == 8 && SelectedSensors[2] == true) pass = true;
        //A10+LS+(only 1 stockpile at a time)
        if (CurrentActivitySelection == 9 && SelectedSensors[2] == true && (Array.Exists(SelectedResources, element => element == "Stockpile 1") && !Array.Exists(SelectedResources, element => element == "Stockpile 2"))) pass = true;
        //A11+LS/Drone+old house
        if (CurrentActivitySelection == 10 && (SelectedSensors[2] == true || SelectedSensors[3] == true) && Array.Exists(SelectedResources, element => element == "Old Building")) pass = true;
        //A12+drone+jobsite
        if (CurrentActivitySelection == 11 && SelectedSensors[3] == true && Array.Exists(SelectedResources, element => element == "Jobsite")) pass = true;
        //A13+IMU+w1w2w3w4
        if (CurrentActivitySelection == 12 && SelectedSensors[4] == true && (Array.Exists(SelectedResources, element => element == "Painter") || Array.Exists(SelectedResources, element => element == "Laborer") || Array.Exists(SelectedResources, element => element == "Carpenter 1") || Array.Exists(SelectedResources, element => element == "Carpenter 2"))) pass = true;

        if (pass)
        {
            //find next active sensor. If not, show NA.
            for (int i = CurrentSensorConfig + 1; i < 6; ++i)
            {
                if (i == 5) // 4 is last sensor, 5 is boundary condition.
                {
                    ShowComboBool = true;//show combo selection panel
                    NAButton.SetActive(true); //boundary condition show next activity button.
                                              //Dropdown 3 active.
                    Mdropdown3.GetComponent<Button>().interactable = false;
                    break;
                }
                if (SelectedSensors[i] == true)
                {
                    CurrentSensorConfig = i; //currently configuring resource for sensor i
                    break;
                }

            }

            //Dropdown 3 active.
            //Mdropdown3.GetComponent<Button>().interactable = false;

            //Record current Combo
            //UpdateComboList();
        }
        

    }

    public void CheckSensorsResources()
    {
        bool pass = false;

        //Check Sensors Resources.
        // Which activity, correct sensor (don't need all, already determined in previouys step), correct resources?
        //A1+GPS[0]+1.dozer
        if (CurrentActivitySelection == 0 && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "Dozer")) pass = true;
        //A2+GPS+2.load
        if (CurrentActivitySelection == 1 && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "Load")) pass = true;
        //A3+GPS+truck
        if (CurrentActivitySelection == 2 && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "Truck")) pass = true;
        //A3+RFID+Rebar
        if (CurrentActivitySelection == 2 && SelectedSensors[1] == true && Array.Exists(SelectedResources, element => element == "Rebar")) pass = true;
        //A3+(GPS&&RFID)+(truck&&rebar)
        if (CurrentActivitySelection == 2 && (SelectedSensors[0] == true && SelectedSensors[1] == true) && (Array.Exists(SelectedResources, element => element == "Truck") && Array.Exists(SelectedResources, element => element == "Rebar"))) pass = true;
        //A4+GPS/RFID+worker
        if (CurrentActivitySelection == 3 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && Array.Exists(SelectedResources, element => element == "Worker 1")) pass = true;
        //A5+GPS+loader/truck
        if (CurrentActivitySelection == 4 && SelectedSensors[0] == true && (Array.Exists(SelectedResources, element => element == "Loader")) || Array.Exists(SelectedResources, element => element == "Dumptruck")) pass = true;
        //A6+RFID+WLR
        if (CurrentActivitySelection == 5 && SelectedSensors[1] == true && (Array.Exists(SelectedResources, element => element == "Wood") || Array.Exists(SelectedResources, element => element == "Log") || Array.Exists(SelectedResources, element => element == "Rebar"))) pass = true;
        //A7+(GPS||RFID)&&(w1w2w3)
        if (CurrentActivitySelection == 6 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && (Array.Exists(SelectedResources, element => element == "Worker 1") || Array.Exists(SelectedResources, element => element == "Worker 2") || Array.Exists(SelectedResources, element => element == "Worker 3"))) pass = true;
        //A8+LS
        if (CurrentActivitySelection == 7 && SelectedSensors[2] == true ) pass = true;
        //A9+LS
        if (CurrentActivitySelection == 8 && SelectedSensors[2] == true ) pass = true;
        //A10+LS+(only 1 stockpile at a time)
        if (CurrentActivitySelection == 9 && SelectedSensors[2] == true && (Array.Exists(SelectedResources, element => element == "Stockpile 1") && !Array.Exists(SelectedResources, element => element == "Stockpile 2"))) pass = true;
        //A11+LS/Drone+old house
        if (CurrentActivitySelection == 10 && (SelectedSensors[2] == true || SelectedSensors[3] == true) && Array.Exists(SelectedResources, element => element == "Old Building")) pass = true;
        //A12+drone+jobsite
        if (CurrentActivitySelection == 11 && SelectedSensors[3] == true && Array.Exists(SelectedResources, element => element == "Jobsite")) pass = true;
        //A13+IMU+w1w2w3w4
        if (CurrentActivitySelection == 12 && SelectedSensors[4] == true && (Array.Exists(SelectedResources, element => element == "Painter") || Array.Exists(SelectedResources, element => element == "Laborer") || Array.Exists(SelectedResources, element => element == "Carpenter 1") || Array.Exists(SelectedResources, element => element == "Carpenter 2"))) pass = true;

        if (pass)
        {
            //Dropdown 3 active.
            //Mdropdown3.GetComponent<Button>().interactable = false;
            //Record current Combo

            //UpdateComboList();
        }
        //else
        //{
            //dialogue warning
           // Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Warning",
           //     "Wrong resources selected for this sensor, Please correct your selection!", false);
            //reset selection
        //}
    }

    //Find next active activity, or out of bound.
    public void NextActivityButton()
    {
        

        //Find next active activity.
        //If out of bound, exit.
        for (int i = CurrentActivitySelection + 1 ; i < 14; ++i)
        {
            if (i == 13) // 12 is last Activity element. 13 is boundary condition.
            {
                SelectButton.SetActive(true);
                Debug.Log("Finished configuration. Can execute.");
                CurrentConfigurationBool = false;
                break;
            }
            if (SelectedActivities[i] == true)
            {
                CurrentActivitySelection = i;
                Mdropdown2.GetComponent<Button>().interactable = true;
                break;
            }

        }

        NAButton.SetActive(false);
    }

    public void UpdateComboList()
    {
        //currently focusing on CurrentActivitySelection
        //Initialize as empty list
        List<string> ComboEntry = new List<string>();

        //refresh sensor selection status
        UpdateSensorSelected();

        //add sensor entry
        if (SelectedSensors[0]) ComboEntry.AddRange(new string[] { "GPS"});
        if (SelectedSensors[1]) ComboEntry.AddRange(new string[] { "RFID" });
        if (SelectedSensors[2]) ComboEntry.AddRange(new string[] { "Laser Scanner" });
        if (SelectedSensors[3]) ComboEntry.AddRange(new string[] { "Drone" });
        if (SelectedSensors[4]) ComboEntry.AddRange(new string[] { "IMU" });

        //add resources entry
        int children = Dropdown3_ListParentNode.transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            GameObject childnode = Dropdown3_ListParentNode.transform.GetChild(i).gameObject;
            Toggle go = childnode.GetComponent<Toggle>();

            if (go.isOn == true)
            {
                ComboEntry.Add(Mdropdown3.dropdownItems[i].itemName);
            }
        }
        
        //test display current activity i
        //Debug.Log("Current Activity Selection: " + CurrentActivitySelection);
        
        //test display combo entry
        //foreach (var x in ComboEntry)
        //{
        //    Debug.Log("ComboEntry List: "+x.ToString());
        //}

        //Add current ComboEntry to completed ComboList
        ComboList.Add(CurrentActivitySelection, ComboEntry);

       
        //Display current result
        //for (int j = 0; j < ComboList.Count; j++)
        //{
         //   Debug.Log(String.Format("Key: {0}, Value: {1}", ComboList.ElementAt(j).Key, string.Join(", ", ComboList.ElementAt(j).Value)));
        //}

    }


    #endregion

    //----------------------------------------------------------

    #region Other small functions
    //Activate legacy menu upon selection.
    public void SelectLegacyMenu()
    {
        LegacyMainMenu.SetActive(true);
    }

    public void ReloadSceneButton()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MenuHub",LoadSceneMode.Single);
        ActivityManager.GetComponent<ActivityManagerScript>().resetScene();
    }

    public void StopButton()
    {
        //reset all sensors
        ActivityManager.GetComponent<ActivityManagerScript>().sensorSelected();
        //stop all activities.
        ActivityManager.GetComponent<ActivityManagerScript>().stopALL();
    }

    //For warning dialogue
    public GameObject DialogPrefabSmall
    {
        get => dialogPrefabSmall;
        set => dialogPrefabSmall = value;
    }

    public void OpenConfirmationDialogSmall()
    {
        //Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Confirmation Dialog, Small, Far", "This is an example of a small dialog with only one button, placed at near interaction range", false);
    }

    /// <summary>
    /// Opens choice dialog example
    /// </summary>
    public void OpenChoiceDialogSmall()
    {
        Dialog myDialog = Dialog.Open(DialogPrefabSmall, DialogButtonType.Yes | DialogButtonType.No, "Activity 11 Warning", "Both Laser Scanner and Drone selected. Do you want to use Drone first, and then Laser Scanner?", false);
        if (myDialog != null)
        {
            myDialog.OnClosed += OnClosedDialogEvent;
        }
    }

    private void OnClosedDialogEvent(DialogResult obj)
    {
        if (obj.Result == DialogButtonType.Yes)
        {
            Debug.Log(obj.Result);
            A11Drone = true;
        }
        if (obj.Result == DialogButtonType.No)
        {
            Debug.Log(obj.Result);
            A11LS = true;
        }

    }

    #endregion
}
