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
    public GameObject ActivityManager;

    public DropdownMultiSelect Mdropdown1;// single selection activity
    public DropdownMultiSelect Mdropdown2;// sensors
    public DropdownMultiSelect Mdropdown3;// resources

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
    List<string> ResourcesList = new List<string> {};
    Dictionary<int, List<string>> ComboList = new Dictionary<int, List<string>>();
    

    private bool onActivityChanged;
    private bool MultiLaserScan;
    private bool RFIDReportEnable;
    private bool GPSReportEnable;
    private bool IMUReportEnable;
    private bool ChangeResourcesBool;
    private bool ShowComboBool;
    private bool CurrentConfigurationBool = true;

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
        ActivityList = new List<string> { "1. Dozer backfilling.", "2. Crane Loading.", "3. Material Delivery.", "4. Worker's Close Call.", "5. Load & Haul.",
            "6. Material Inventory.", "7. Detecting Fall.", "8. Scan Building.", "9. Scan Floor.", "10. Scan Stockpile.", "11. Scan Old Building.", "12. Jobsite Inspection.", "13. Worker Ergonomics."};
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
        }
    }

    private void ResetEverything()
    {
        //how to reset dropdowns
    }



    //update resources list accroding to dropdown 1's selection
    public void UpdateResourcesList()
    {
        //Initialize as empty list
        ResourcesList.Clear();

        //Add items according to different activities
        if (SelectedActivities[0] == true) ResourcesList.AddRange(new string[] { "1.Dozer", "1.Stockpile" });//1
        if (SelectedActivities[1] == true) ResourcesList.AddRange(new string[] { "2.Crane", "2.Load" });//2
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
        if (SelectedActivities[12] == true) ResourcesList.AddRange(new string[] { "13.Painter", "13.Laborer", "13.Carpenter 1", "13.Carpenter 2" });//14

    }

    //update resources list accroding to new ui button
    public void UpdateResourcesListSolo()
    {
        //Initialize as empty list
        ResourcesList.Clear();

        //Add items according to different activities
        if (CurrentActivitySelection == 0) ResourcesList.AddRange(new string[] { "1.Dozer", "1.Stockpile" });//1
        if (CurrentActivitySelection == 1) ResourcesList.AddRange(new string[] { "2.Crane", "2.Load" });//2
        if (CurrentActivitySelection == 2) ResourcesList.AddRange(new string[] { "3.Truck", "3.Rebar" });
        if (CurrentActivitySelection == 3) ResourcesList.AddRange(new string[] { "4.Worker 1" });
        if (CurrentActivitySelection == 4) ResourcesList.AddRange(new string[] { "5.Loader", "5.dumptruck", "5.stockpile" });
        if (CurrentActivitySelection == 5) ResourcesList.AddRange(new string[] { "6.wood", "6.Log", "6.Rebar" });
        if (CurrentActivitySelection == 6) ResourcesList.AddRange(new string[] { "7.Worker 1", "7.Worker 2", "7.Worker 3" });
        if (CurrentActivitySelection == 7) ResourcesList.AddRange(new string[] { "8.Building 1" });
        if (CurrentActivitySelection == 8) ResourcesList.AddRange(new string[] { "9.Top Floor" });
        if (CurrentActivitySelection == 9) ResourcesList.AddRange(new string[] { "10.Stockpile 1", "10.Stockpile 2" });//11
        if (CurrentActivitySelection == 10) ResourcesList.AddRange(new string[] { "11.Old Building" });//12
        if (CurrentActivitySelection == 11) ResourcesList.AddRange(new string[] { "12.Jobsite" });//13
        if (CurrentActivitySelection == 12) ResourcesList.AddRange(new string[] { "13.Painter", "13.Laborer", "13.Carpenter 1", "13.Carpenter 2" });//14

    }


    //10/4/2020 New implementation based on previous select function
    public void Select()
    {
        //disable canvas that show combo selections
        ComboSelectionCanvas.SetActive(false);

        //Initialize combos
        string[] ExeList = new string[14];
        for (int j = 0; j < ComboList.Count; j++)
        {
            ExeList[ComboList.ElementAt(j).Key] = string.Join(", ", ComboList.ElementAt(j).Value);
        }

        

        if (SelectedActivities[0] == true)
            ActivityManager.GetComponent<ActivityManagerScript>().select_1();
        if (SelectedActivities[0] == true && ExeList[0].Contains("GPS") == true && ExeList[0].Contains("1.Dozer") == true)
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_1_Dozer_GPS(); } //1.Dozer GPS

        if (SelectedActivities[1] == true)
            ActivityManager.GetComponent<ActivityManagerScript>().select_2();
        if (SelectedActivities[1] == true && ExeList[1].Contains("GPS") == true && ExeList[1].Contains("2.Load") == true)
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_2_CraneLoad_GPS(); }//2.Load GPS
        
        if (SelectedActivities[2] == true)
            ActivityManager.GetComponent<ActivityManagerScript>().select_3();
        if (SelectedActivities[2] == true && ExeList[2].Contains("GPS") == true && ExeList[2].Contains( "3.Truck") == true)
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_3_truck_GPS(); }//3.truck GPS
        if (SelectedActivities[2] == true && ExeList[2].Contains("RFID") == true && ExeList[2].Contains( "3.Rebar") == true)
        { RFIDReportEnable = true; } //3.Rebar RFID


        if (SelectedActivities[3] == true) ActivityManager.GetComponent<ActivityManagerScript>().select_4();
        if (SelectedActivities[3] == true && ExeList[3].Contains("GPS") == true && ExeList[3].Contains( "4.Worker 1"))
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_4worker_gps(); }
        if (SelectedActivities[3] == true && ExeList[3].Contains("RFID") == true && ExeList[3].Contains( "4.Worker 1"))
        { ActivityManager.GetComponent<ActivityManagerScript>().select_4worker_RFID(); }//Since we are using panel above worker, so no RFID panel needed.
        if (SelectedActivities[3] == true && ExeList[3].Contains("GPS") == true && ExeList[3].Contains("RFID") == true && ExeList[3].Contains( "4.Worker 1"))
        { ActivityManager.GetComponent<ActivityManagerScript>().A4_worker_GPSRFID = true; ActivityManager.GetComponent<ActivityManagerScript>().select_4worker_RFID(); }

        if (SelectedActivities[4] == true)
            ActivityManager.GetComponent<ActivityManagerScript>().select_5();
        if (SelectedActivities[4] == true && ExeList[4].Contains("GPS") == true && ExeList[4].Contains( "5.Loader"))
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_5_Loader_GPS(); }
        if (SelectedActivities[4] == true && ExeList[4].Contains("GPS") == true && ExeList[4].Contains( "5.dumptruck"))
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_5_dumptruck_GPS(); }

        //RFID & rebar, wood and log.
        if (SelectedActivities[5] == true && ExeList[5].Contains("RFID") == true)
        {
            if (ExeList[5].Contains( "6.wood")) ActivityManager.GetComponent<ActivityManagerScript>().A6_wood_flag = true;
            if (ExeList[5].Contains( "6.Log")) ActivityManager.GetComponent<ActivityManagerScript>().A6_log_flag = true;
            if (ExeList[5].Contains( "6.Rebar")) ActivityManager.GetComponent<ActivityManagerScript>().A6_rebar_flag = true;
            ActivityManager.GetComponent<ActivityManagerScript>().A6_RFID();
            RFIDReportEnable = true;
        }

        //7. Workers on top floor
        if (SelectedActivities[6] == true)
        {
            if (ExeList[6].Contains( "7.Worker 1")) ActivityManager.GetComponent<ActivityManagerScript>().A7_w1_flag = true;
            if (ExeList[6].Contains( "7.Worker 2")) ActivityManager.GetComponent<ActivityManagerScript>().A7_w2_flag = true;
            if (ExeList[6].Contains( "7.Worker 3")) ActivityManager.GetComponent<ActivityManagerScript>().A7_w3_flag = true;

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
        if (MultiLaserScan == false) { if (SelectedActivities[9] == true && ExeList[9].Contains("LaserScanner") == true && ExeList[9].Contains( "10.Stockpile 1")) ActivityManager.GetComponent<ActivityManagerScript>().select_10A(); }
        if (MultiLaserScan == false) { if (SelectedActivities[9] == true && ExeList[9].Contains("LaserScanner") == true && ExeList[9].Contains( "10.Stockpile 2")) ActivityManager.GetComponent<ActivityManagerScript>().select_10B(); }
        if (MultiLaserScan == false) { if (SelectedActivities[10] == true && ExeList[10].Contains("LaserScanner") == true && ExeList[10].Contains( "11.Old Building")) ActivityManager.GetComponent<ActivityManagerScript>().select_11Laser(); }

        //11. Old house drone
        if (SelectedActivities[10] == true && ExeList[10].Contains("Drone") == true && ExeList[10].Contains( "11.Old Building")) ActivityManager.GetComponent<ActivityManagerScript>().select_11Drone();
        //12. Jobsite drone
        if (SelectedActivities[11] == true && ExeList[11].Contains("Drone") == true && ExeList[11].Contains("12.Jobsite")) ActivityManager.GetComponent<ActivityManagerScript>().select_12();

        //13.IMU workers
        if (SelectedActivities[12] == true && ExeList[12].Contains("IMU") == true)
        {
            if (ExeList[12].Contains( "13.Painter"))
            { ActivityManager.GetComponent<ActivityManagerScript>().A14_painter = true; }
            if (ExeList[12].Contains( "13.Laborer"))
            { ActivityManager.GetComponent<ActivityManagerScript>().A14_laborer = true; }
            if (ExeList[12].Contains( "13.Carpenter 1"))
            { ActivityManager.GetComponent<ActivityManagerScript>().A14_c1 = true; }
            if (ExeList[12].Contains( "13.Carpenter 2"))
            { ActivityManager.GetComponent<ActivityManagerScript>().A14_c2 = true; }
            //With given worker bool, get IMU string, handle finish file write use backselected()
            ActivityManager.GetComponent<ActivityManagerScript>().select_13_new();
            //display IMU
            IMUReportEnable = true;
        }

        //Acitivity Idrection Indicator
        ActivityIndicator();
        
    }


    /* old implementation, commented 10/4/2020
    public void Select()
    {
        ComboSelectionCanvas.SetActive(false);

        if (SelectedActivities[0] == true)
            ActivityManager.GetComponent<ActivityManagerScript>().select_1();
        if (SelectedActivities[0] == true && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "1.Dozer"))
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_1_Dozer_GPS(); } //1.Dozer GPS

        if (SelectedActivities[1] == true)
            ActivityManager.GetComponent<ActivityManagerScript>().select_2();
        if (SelectedActivities[1] == true && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "2.Load"))
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_2_CraneLoad_GPS(); }//2.Load GPS

        if (SelectedActivities[2] == true)
            ActivityManager.GetComponent<ActivityManagerScript>().select_3();
        if (SelectedActivities[2] == true && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "3.Truck"))
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_3_truck_GPS(); }//3.truck GPS
        if (SelectedActivities[2] == true && SelectedSensors[1] == true && Array.Exists(SelectedResources, element => element == "3.Rebar"))
        { RFIDReportEnable = true; } //3.Rebar RFID


        if (SelectedActivities[3] == true) ActivityManager.GetComponent<ActivityManagerScript>().select_4();
        if (SelectedActivities[3] == true && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "4.Worker 1"))
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_4worker_gps(); }
        if (SelectedActivities[3] == true && SelectedSensors[1] == true && Array.Exists(SelectedResources, element => element == "4.Worker 1"))
        { ActivityManager.GetComponent<ActivityManagerScript>().select_4worker_RFID(); }//Since we are using panel above worker, so no RFID panel needed.
        if (SelectedActivities[3] == true && SelectedSensors[0] == true && SelectedSensors[1] == true && Array.Exists(SelectedResources, element => element == "4.Worker 1"))
        { ActivityManager.GetComponent<ActivityManagerScript>().A4_worker_GPSRFID = true; ActivityManager.GetComponent<ActivityManagerScript>().select_4worker_RFID(); }

        if (SelectedActivities[4] == true)
            ActivityManager.GetComponent<ActivityManagerScript>().select_5();
        if (SelectedActivities[4] == true && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "5.Loader"))
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_5_Loader_GPS(); }
        if (SelectedActivities[4] == true && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "5.dumptruck"))
        { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_5_dumptruck_GPS(); }

        //RFID & rebar, wood and log.
        if (SelectedActivities[5] == true && SelectedSensors[1] == true)
        {
            if (Array.Exists(SelectedResources, element => element == "6.wood")) ActivityManager.GetComponent<ActivityManagerScript>().A6_wood_flag = true;
            if (Array.Exists(SelectedResources, element => element == "6.Log")) ActivityManager.GetComponent<ActivityManagerScript>().A6_log_flag = true;
            if (Array.Exists(SelectedResources, element => element == "6.Rebar")) ActivityManager.GetComponent<ActivityManagerScript>().A6_rebar_flag = true;
            ActivityManager.GetComponent<ActivityManagerScript>().A6_RFID();
            RFIDReportEnable = true;
        }

        //7. Workers on top floor
        if (SelectedActivities[6] == true)
        {
            if (Array.Exists(SelectedResources, element => element == "7.Worker 1")) ActivityManager.GetComponent<ActivityManagerScript>().A7_w1_flag = true;
            if (Array.Exists(SelectedResources, element => element == "7.Worker 2")) ActivityManager.GetComponent<ActivityManagerScript>().A7_w2_flag = true;
            if (Array.Exists(SelectedResources, element => element == "7.Worker 3")) ActivityManager.GetComponent<ActivityManagerScript>().A7_w3_flag = true;

            //Run basic activity: danger zone red box. Based on worker selection bool.
            ActivityManager.GetComponent<ActivityManagerScript>().select_7_new();

            //GPS only
            if (SelectedSensors[0] == true) { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_7_GPS(); }

            //RFID only, GPS hidden.
            if (SelectedSensors[1] == true) { ActivityManager.GetComponent<ActivityManagerScript>().select_7_RFID(); }

            //RFID and GPS
            if (SelectedSensors[0] == true && SelectedSensors[1] == true)
            { ActivityManager.GetComponent<ActivityManagerScript>().A7_worker_GPSRFID = true; ActivityManager.GetComponent<ActivityManagerScript>().select_7_RFID(); }

        }


        //Laser Scan related activities [7,8,9,10]
        //if MultiLaserScan, then cannot proceed
        if (MultiLaserScan == false) { if (SelectedActivities[7] == true) ActivityManager.GetComponent<ActivityManagerScript>().select_8(); }
        if (MultiLaserScan == false) { if (SelectedActivities[8] == true) ActivityManager.GetComponent<ActivityManagerScript>().select_9(); }
        if (MultiLaserScan == false) { if (SelectedActivities[9] == true && SelectedSensors[2] == true && Array.Exists(SelectedResources, element => element == "10.Stockpile 1")) ActivityManager.GetComponent<ActivityManagerScript>().select_10A(); }
        if (MultiLaserScan == false) { if (SelectedActivities[9] == true && SelectedSensors[2] == true && Array.Exists(SelectedResources, element => element == "10.Stockpile 2")) ActivityManager.GetComponent<ActivityManagerScript>().select_10B(); }
        if (MultiLaserScan == false) { if (SelectedActivities[10] == true && SelectedSensors[2] == true && Array.Exists(SelectedResources, element => element == "11.Old Building")) ActivityManager.GetComponent<ActivityManagerScript>().select_11Laser(); }

        //11. Old house drone
        if (SelectedActivities[10] == true && SelectedSensors[3] == true && Array.Exists(SelectedResources, element => element == "11.Old Building")) ActivityManager.GetComponent<ActivityManagerScript>().select_11Drone();
        //12. Jobsite drone
        if (SelectedActivities[11] == true && SelectedSensors[3] == true) ActivityManager.GetComponent<ActivityManagerScript>().select_12();

        //13.IMU workers
        if (SelectedActivities[12] == true && SelectedSensors[4] == true)
        {
            if (Array.Exists(SelectedResources, element => element == "13.Painter"))
            { ActivityManager.GetComponent<ActivityManagerScript>().A14_painter = true; }
            if (Array.Exists(SelectedResources, element => element == "13.Laborer"))
            { ActivityManager.GetComponent<ActivityManagerScript>().A14_laborer = true; }
            if (Array.Exists(SelectedResources, element => element == "13.Carpenter 1"))
            { ActivityManager.GetComponent<ActivityManagerScript>().A14_c1 = true; }
            if (Array.Exists(SelectedResources, element => element == "13.Carpenter 2"))
            { ActivityManager.GetComponent<ActivityManagerScript>().A14_c2 = true; }
            //With given worker bool, get IMU string, handle finish file write use backselected()
            ActivityManager.GetComponent<ActivityManagerScript>().select_13_new();
            //display IMU
            IMUReportEnable = true;
        }

        //Acitivity Idrection Indicator
        ActivityIndicator();

    }
    */

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
            ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[ComboList.ElementAt(j).Key].itemName + string.Join(", ", ComboList.ElementAt(j).Value) + ".\n";
        }
    }

    private void PrepareCurrentConfigDisplay()
    {
        //Current Activity Selection by default is 0, but it may not be active.
        if (CurrentActivitySelection == 0 && SelectedActivities[0] == false) DisplayString = "Waiting to select activity.";
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
        for (int i = 0; i < 12; ++i)
        {
            if(SelectedActivities[i] == true)
            { CurrentActivitySelection = i; break; }
                
        }
            
    }

    //Sensor Confirm.
    public void S_Confirm()
    {
        CheckActivitySensors();

        //update resources list for CurrentActivitySelection
        ChangeResourcesBool = true;
    }

    public void CheckActivitySensors()
    {
        bool pass = false;
        //Debug.Log("CAS"+CurrentActivitySelection+ SelectedSensors[0]+ SelectedSensors[1]);
        //Check if selected sensors fit selected activity? If fit, give pass.
        //"GPS", "RFID", "LaserScanner", "Drone", "IMU"
        //A1+GPS[0]
        if (CurrentActivitySelection == 0 && SelectedSensors[0] == true && SelectedSensors[1] == false && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) pass = true;
        //A2+GPS
        if (CurrentActivitySelection == 1 && SelectedSensors[0] == true && SelectedSensors[1] == false && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) pass = true;
        //A3+GPS/RFID
        if (CurrentActivitySelection == 2 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) pass = true;
        //A4+GPS/RFID
        if (CurrentActivitySelection == 3 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) pass = true;
        //A5+GPS
        if (CurrentActivitySelection == 4 && SelectedSensors[0] == true && SelectedSensors[1] == false && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) pass = true;
        //A6+RFID
        if (CurrentActivitySelection == 5 && SelectedSensors[0] == false && SelectedSensors[1] == true && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) pass = true;
        //A7+GPSRFID
        if (CurrentActivitySelection == 6 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) pass = true;
        //A8+LS
        if (CurrentActivitySelection == 7 && SelectedSensors[0] == false && SelectedSensors[1] == false && SelectedSensors[2] == true && SelectedSensors[3] == false && SelectedSensors[4] == false) pass = true;
        //A9+LS
        if (CurrentActivitySelection == 8 && SelectedSensors[0] == false && SelectedSensors[1] == false && SelectedSensors[2] == true && SelectedSensors[3] == false && SelectedSensors[4] == false) pass = true;
        //A10+LS
        if (CurrentActivitySelection == 9 && SelectedSensors[0] == false && SelectedSensors[1] == false && SelectedSensors[2] == true && SelectedSensors[3] == false && SelectedSensors[4] == false) pass = true;
        //A11+LS/Drone
        if (CurrentActivitySelection == 10 && SelectedSensors[0] == false && SelectedSensors[1] == false && (SelectedSensors[2] == true || SelectedSensors[3] == true) && SelectedSensors[4] == false) pass = true;
        //A12+drone
        if (CurrentActivitySelection == 11 && SelectedSensors[0] == false && SelectedSensors[1] == false && SelectedSensors[2] == false && SelectedSensors[3] == true && SelectedSensors[4] == false) pass = true;
        //A13+IMU
        if (CurrentActivitySelection == 12 && SelectedSensors[0] == false && SelectedSensors[1] == false && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == true) pass = true;

        if (pass)
        {
            //Dropdown 2 inactive.
            Mdropdown2.GetComponent<Button>().interactable = false;
            //Dropdown 3 active.
            Mdropdown3.GetComponent<Button>().interactable = true;
        }
        else
        {
            //dialogue warning
            Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Warning",
                "Wrong sensor selected for this activity, Please correct your selection!", false);
            //reset selection
        }
    }

    //Resources Confirm
    public void R_Confirm()
    {
        //Dropdown 3 active.
        Mdropdown3.GetComponent<Button>().interactable = false;
        //TODO: Record current Combo
        UpdateComboList();
    }

    //Find next active activity, or out of bound.
    public void NextActivityButton()
    {
        //show combos
        ShowComboBool = true;

        //Find next active activity
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
    }

    public void UpdateComboList()
    {
        //currently focusing on CurrentActivitySelection
        //Initialize as empty list
        //ComboEntry.Clear();
        List<string> ComboEntry = new List<string>();

        //refresh sensor selection status
        UpdateSensorSelected();

        //add sensor entry
        if (SelectedSensors[0]) ComboEntry.AddRange(new string[] { "GPS"});
        if (SelectedSensors[1]) ComboEntry.AddRange(new string[] { "RFID" });
        if (SelectedSensors[2]) ComboEntry.AddRange(new string[] { "LaserScanner" });
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

        //complete current combo list
        ComboList.Add(CurrentActivitySelection, ComboEntry);

        /*
        if (CurrentActivitySelection == 0) ComboList.Add(CurrentActivitySelection, ComboEntry);
        if (CurrentActivitySelection == 1) ComboList.Add(CurrentActivitySelection, new List<string>(new string[] { "for 1" }));
        if (CurrentActivitySelection == 2) ComboList.Add(CurrentActivitySelection, new List<string>(new string[] { "for 2" }));
        if (CurrentActivitySelection == 3) ComboList.Add(CurrentActivitySelection, new List<string>(new string[] { "for 3" }));
        */
        //ComboList.Add(100, new List<string>(new string[] { "element1", "element2", "element3" }));
       
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

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuHub",LoadSceneMode.Single);
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

    #endregion
}
