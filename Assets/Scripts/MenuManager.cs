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

public class MenuManager : MonoBehaviour
{
    #region Parameters
    [SerializeField] private GameObject HideShowButton;
    [SerializeField] private GameObject ActivityResourcesNode;
    [SerializeField] private GameObject ConcurrentSelectionMenu; //LegacyMainMenu
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

    //public GameObject VisualBlock;
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
    private string CostBenefitString;


    public TextMeshProUGUI GPSReportText;
    public TextMeshProUGUI RFIDReportText;
    public TextMeshProUGUI IMUReportText;
    public TextMeshProUGUI ShowSelectionText;
    public TextMeshProUGUI CostBenefitText;
    public TextMeshProUGUI TitleText;

    private bool[] SelectedActivities = new bool[16];
    private bool[] SelectedSensors = new bool[5];
    private string[] SelectedResources = new string[16];
    private bool[] LastIterationSelectedActivities = new bool[16];
    Dictionary<int, int> LUT = new Dictionary<int, int>(); //Use hashtable for LUT, used to correspond activity selection panel buttons and actual activities.
    Dictionary<int, int> LUT2 = new Dictionary<int, int>(); // LUT2, used to convey execution command between R_confirm and select().

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

    private bool A11Drone;
    private bool A11LS;

    private string GPSReportString;
    private string RFIDReportString;
    private string IMUReportString;

    private bool ActivityDirectionalIndicatorBool;

    [SerializeField] private GameObject Everything;
    [SerializeField] private GameObject A1Position;

    private int CurrentActivitySelection;

    [SerializeField][Tooltip("Assign DialogSmall_192x96.prefab")] private GameObject dialogPrefabSmall;
    [SerializeField] [Tooltip("Assign DialogMedium_192x128.prefab")] private GameObject dialogPrefabMedium;


    private int CurrentSensorConfig;

    private bool Dropdown1ShowCurrentActivity;
    private bool Dropdown2ShowCurrentSensor;
    private int SelecedActivityNumber;
    private int RemainingActivityNumber;

    public GameObject ActivitySelectionPrefabButton;
    public Transform ActivitySelectionItemParent; //Parent that holds all
    public RectTransform ActivitySelectionParentPanel;

    public GameObject mainUICollection;
    public GameObject A_confirm_button;
    public GameObject R_confirm_button;
    public GameObject S_confirm_button;

    private int LUT2Index = 0;
    private int LSConcurrent;
    private int DConcurrent;
    private int ConcurrentSelection;
    public bool ConcurrencySignal;
    private bool ConcurrencyOccurred;

    private bool showhidetoggle = false; //true means hide

    public GameObject SensorParentNode;
    public GameObject MiscAssetNode;

    public string SensorWarningString;

    public GameObject ManualSelectionParent;
    public GameObject CostBenefitPanel;

    private bool AConfirmButtonSelected = false;

    public GameObject IMUPainter;
    public GameObject IMULabor;
    public GameObject IMUCarpenter;

    private bool initialFlag;
    #endregion

    //----------------------------------------------------------

    #region Start Update

    public void Start()
    {

        HideShowButton.SetActive(false);
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
        CostBenefitPanel.SetActive(false);
        StopButtonObj.SetActive(false);
        NAButton.SetActive(false);

        A_confirm_button.SetActive(false);
        S_confirm_button.SetActive(false);
        R_confirm_button.SetActive(false);

        //Mdropdown1.transform.Find("DisablePanel").gameObject.SetActive(false);
        Mdropdown2.GetComponent<Button>().interactable = false;
        //Mdropdown2.transform.Find("DisablePanel").gameObject.SetActive(true);
        Mdropdown3.GetComponent<Button>().interactable = false;
        //Mdropdown3.transform.Find("DisablePanel").gameObject.SetActive(true);
        SelectButton.SetActive(false);

        CurrentActivitySelection = 0;

        //activity selection panel
        ActivitySelectionParentPanel.gameObject.SetActive(false);

        ConcurrentSelectionMenu.SetActive(false);
       // VisualBlock.SetActive(false);
    }

    public void SceneInitialize()
    {
        HideShowButton.SetActive(false);
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
        CostBenefitPanel.SetActive(false);
        StopButtonObj.SetActive(false);
        NAButton.SetActive(false);

        A_confirm_button.SetActive(false);
        S_confirm_button.SetActive(false);
        R_confirm_button.SetActive(false);

        //Mdropdown1.transform.Find("DisablePanel").gameObject.SetActive(false);
        Mdropdown2.GetComponent<Button>().interactable = false;
        //Mdropdown2.transform.Find("DisablePanel").gameObject.SetActive(true);
        Mdropdown3.GetComponent<Button>().interactable = false;
        //Mdropdown3.transform.Find("DisablePanel").gameObject.SetActive(true);
        SelectButton.SetActive(false);

        CurrentActivitySelection = 0;

        //activity selection panel
        ActivitySelectionParentPanel.gameObject.SetActive(false);

        ConcurrentSelectionMenu.SetActive(false);
        // VisualBlock.SetActive(false);
    }


    void Update()
    {

        //update selected activity for each frame
        UpdateActivitySelected();
        UpdateSensorSelected();
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

        //Show current configuration in dropdown 1, after press activity confirm button.
        if (Dropdown1ShowCurrentActivity) Dropdown1Title.GetComponent<TextMeshProUGUI>().text = Mdropdown1.dropdownItems[CurrentActivitySelection].itemName;

        //Show current sensor configuration in dropdown 1.
        if (Dropdown2ShowCurrentSensor) Dropdown2Title.GetComponent<TextMeshProUGUI>().text = Mdropdown2.dropdownItems[CurrentSensorConfig].itemName;
        else Dropdown2Title.GetComponent<TextMeshProUGUI>().text = "Sensors";

        //Show all configured combo in panel.
        if (ShowComboBool)
        {
            ComboSelectionCanvas.SetActive(true);
            PrepareComboDisplayString();
            ShowSelectionText.GetComponent<TextMeshProUGUI>().text = ComboDisplayString;
            
            //20201130 redesign cost benefit in another scene.
            //CostBenefitPanel.SetActive(true);
            //PrepareCostBenefitString();
            //CostBenefitText.GetComponent<TextMeshProUGUI>().text = CostBenefitString;
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

        //look for concurrency congestion over signal
        //signal only need to happen once using concurrencysignal
        //execute residual function
        if (ConcurrencyOccurred && ActivityManager.GetComponent<ActivityManagerScript>().ConcurencySuspension && !ConcurrencySignal) { ConcurrencySignal = true; ConcurrencyResidualExe(); }
}

    #endregion

    //----------------------------------------------------------

    #region supporting functions

    private void CreateActivityDropdown()
    {
        //ActivityList = new List<string> { "Dozer backfilling", "Crane Loading", "Material Delivery", "Worker's Close Call", "Load & Haul",
        //    "Material Inventory", "Detecting Fall", "Work Progress Measurement – Building", "Scan Floor", "Scan Stockpile", "Scan Old Building", "Jobsite Inspection", "Worker Ergonomics"};
        ActivityList = new List<string> { "Backfilling", "Crane Loading", "Material Delivery", "Material handling (1)", "Truck Load/Haul",
            "Material Inventory", "Material Handling (2)", "Cladding", "Flooring", "Stockpile unloading", "Renovation", "Site Inspection", "Painting","Labor","Carpentry"};


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

    private void ResetSensorDropdown()
    {
        Mdropdown2.dropdownItems.Clear();
        Mdropdown2.SetupDropdown();
        CreateSensorsDropdown();
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
        SelecedActivityNumber = 0;
        onActivityChanged = false;
        int children = Dropdown1_ListParentNode.transform.childCount;

        for (int i = 0; i < children; ++i)
        {
            GameObject childnode = Dropdown1_ListParentNode.transform.GetChild(i).gameObject;
            Toggle go = childnode.GetComponent<Toggle>();

            if (go.isOn == true)
            {
                SelecedActivityNumber = SelecedActivityNumber + 1;
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

        if (SelecedActivityNumber != 0 && AConfirmButtonSelected == false)
            A_confirm_button.SetActive(true);
        else
            A_confirm_button.SetActive(false);
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
    //Old deprecated
    /*
    public void UpdateResourcesList()
    {
        //Initialize as empty list
        ResourcesList.Clear();
        //Add items according to different activities
        if (SelectedActivities[0] == true) ResourcesList.AddRange(new string[] { "Dozer", "Stockpile" });//1
        if (SelectedActivities[1] == true) ResourcesList.AddRange(new string[] { "Crane", "Steel Beam" });//2
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
        if (SelectedActivities[12] == true) ResourcesList.AddRange(new string[] { "Painter", "Laborer", "Carpenter 1", "Carpenter 2" });//14
    }
    */

    //update resources list accroding to new ui button
    public void UpdateResourcesListSolo()
    {
        //Initialize as empty list
        ResourcesList.Clear();

        //Add items according to different activities
        if (CurrentActivitySelection == 0) ResourcesList.AddRange(new string[] { "Dozer", "Stockpile" });//1
        if (CurrentActivitySelection == 1) ResourcesList.AddRange(new string[] { "Crane", "Steel Beam" });//2
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
        //Changed due to separate IMU into 3 activities and leave resources for joints
        //if (CurrentActivitySelection == 12) ResourcesList.AddRange(new string[] { "Painter", "Laborer", "Carpenter 1", "Carpenter 2" });//14
        if (CurrentActivitySelection == 12 && SelectedSensors[4] == true) ResourcesList.AddRange(new string[] { "Back","Neck","Shoulder","Thigh" });//14
        if (CurrentActivitySelection == 13 && SelectedSensors[4] == true) ResourcesList.AddRange(new string[] { "Back", "Neck", "Shoulder", "Thigh" });//14
        if (CurrentActivitySelection == 14 && SelectedSensors[4] == true) ResourcesList.AddRange(new string[] { "Back", "Neck", "Shoulder", "Thigh" });//14
        if (CurrentActivitySelection == 12 && SelectedSensors[4] == false) ResourcesList.AddRange(new string[] { "Painter" });//14
        if (CurrentActivitySelection == 13 && SelectedSensors[4] == false) ResourcesList.AddRange(new string[] { "Laborer" });//14
        if (CurrentActivitySelection == 14 && SelectedSensors[4] == false) ResourcesList.AddRange(new string[] { "Carpenter" });//14

    }

    public List<int> InterpretLUT2(int ActivityNumber)
    {
        List<int> ListOfKeys = new List<int>();
        //Interpret LUT2(i,A#)
        foreach (KeyValuePair<int, int> pair in LUT2)
        {
            if (pair.Value == ActivityNumber)
            {
                //Debug.Log("LUT2 A# = 3 corresponding keys:" + pair.Key);
                //Multiple keys, each key correspond to a ExeList[Key]
                ListOfKeys.Add(pair.Key);
            }
                
        }
        return ListOfKeys;
    }


    //10/4/2020 New implementation based on previous select function
    //10/20/2020 changing implementation due to per sensor resource selection will invalidate previous dictionary based implementation.
    //11/24/2020 Added RFID for vehicles A1,2,3,5
    public void Select()
    {
        HideShowButton.SetActive(true);

        bool Drone1 = false;
        bool Drone2 = false;
        bool MultiDroneBlock = false;

        ShowComboBool = false;
        //disable canvas that show combo selections
        ComboSelectionCanvas.SetActive(false);
        CostBenefitPanel.SetActive(false);
        //ActivityManager.GetComponent<ActivityManagerScript>().sensorSelected();

        //Interpret ComboList(i,selections)
        string[] ExeList = new string[ComboList.Count];//unknown size, because of selection limits, based on estimation (21?), size set to 50.
        Debug.Log("ComboList count:" + ComboList.Count);
        for (int j = 0; j < ComboList.Count; j++)
        {
            ExeList[ComboList.ElementAt(j).Key] = string.Join(", ", ComboList.ElementAt(j).Value);
        }

        /// <summary>
        /// This section check for multi drone selection
        /// </summary>
        //11. Old House Drone check
        if (SelectedActivities[10] == true) { foreach (int key in InterpretLUT2(10)) { if (ExeList[key].Contains("Drone") == true) { Drone1 = true; } } }
        //12. Jobsite drone check
        if (SelectedActivities[11] == true) { foreach (int key in InterpretLUT2(11)) { if (ExeList[key].Contains("Drone")) { Drone2 = true; } } }
        //MultiDroneBlock
        if (Drone1 && Drone2) { MultiDroneBlock = true; Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Warning", "We only have 1 Drone but multiple Drone activity selected.", false); }

        //Execute Activities
        //A1 Dozer Backfilling
        if (SelectedActivities[0] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_1();
            //Dozer GPS
            foreach (int key in InterpretLUT2(0))
            {
                if (ExeList[key].Contains("GPS") && ExeList[key].Contains("Dozer"))
                { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_1_Dozer_GPS(); }// 1.Dozer GPS
            }
            //Dozer RFID
            foreach (int key in InterpretLUT2(0))
            {
                if (ExeList[key].Contains("RFID") && ExeList[key].Contains("Dozer"))
                { ActivityManager.GetComponent<ActivityManagerScript>().select_1_Dozer_RFID(); }// 1.Dozer GPS
            }
        }

        //A2 Crane Loading
        if (SelectedActivities[1] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_2();
            //Steelbeam GPS
            foreach (int key in InterpretLUT2(1))
            {
                if (ExeList[key].Contains("GPS") && ExeList[key].Contains("Steel Beam"))
                { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_2_CraneLoad_GPS(); }// 2.Load GPS
            }
            //A2 Crane rfid
            foreach (int key in InterpretLUT2(1))
            {
                if (ExeList[key].Contains("RFID") && ExeList[key].Contains("Crane"))
                { ActivityManager.GetComponent<ActivityManagerScript>().select_2_crane_RFID(); }
            }
        } 

        //A3 Material delivery
        if (SelectedActivities[2] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_3();
            //3.Truck GPS
            foreach (int key in InterpretLUT2(2))
            {
                if (ExeList[key].Contains("GPS") && ExeList[key].Contains("Truck"))
                { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_3_truck_GPS(); }
            }
            //3.Rebar RFID
            foreach (int key in InterpretLUT2(2))
            {
                if (ExeList[key].Contains("RFID") && ExeList[key].Contains("Rebar"))
                { RFIDReportEnable = true; }
            }
            //A3 RFID truck
            foreach (int key in InterpretLUT2(2))
            {
                if (ExeList[key].Contains("RFID") && ExeList[key].Contains("Truck"))
                { ActivityManager.GetComponent<ActivityManagerScript>().select_3_truck_RFID(); }
            }
        }
           
        //A4 Worker Close Call
        if (SelectedActivities[3] == true) 
        { 
            ActivityManager.GetComponent<ActivityManagerScript>().select_4(); 
            ActivityManager.GetComponent<ActivityManagerScript>().select_2();
            //worker GPS
            foreach (int key in InterpretLUT2(3))
            {
                if (ExeList[key].Contains("GPS") && ExeList[key].Contains("Worker 1"))
                { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_4worker_gps(); }// 4.Worker GPS
            }
            //Worker RFID. Since we are using panel above worker, so no RFID panel needed.
            foreach (int key in InterpretLUT2(3))
            {
                if (ExeList[key].Contains("RFID") && ExeList[key].Contains("Worker 1"))
                { ActivityManager.GetComponent<ActivityManagerScript>().select_4worker_RFID(); }
            }
            //Worker GPS&RFID.
            foreach (int key in InterpretLUT2(3))
            {
                if (ExeList[key].Contains("GPS") && ExeList[key].Contains("RFID") && ExeList[key].Contains("Worker 1"))
                { ActivityManager.GetComponent<ActivityManagerScript>().A4_worker_GPSRFID = true; ActivityManager.GetComponent<ActivityManagerScript>().select_4worker_RFID(); }
            }
        }

        //A5 Load And Haul
        if (SelectedActivities[4] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_5();
            // 5.Loader GPS.
            foreach (int key in InterpretLUT2(4))
            {
                if (ExeList[key].Contains("GPS") && ExeList[key].Contains("Loader"))
                { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_5_Loader_GPS(); }
            }
            // 5.DumpTruck GPS.
            foreach (int key in InterpretLUT2(4))
            {
                if (ExeList[key].Contains("GPS") && ExeList[key].Contains("DumpTruck"))
                { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_5_dumptruck_GPS(); }
            }
            //5.Loader RFID
            foreach (int key in InterpretLUT2(4))
            {
                if (ExeList[key].Contains("RFID") && ExeList[key].Contains("Loader"))
                { ActivityManager.GetComponent<ActivityManagerScript>().select_5_backhoe_RFID(); }
            }
            //5.Dumptruck Rfid
            foreach (int key in InterpretLUT2(4))
            {
                if (ExeList[key].Contains("RFID") && ExeList[key].Contains("DumpTruck"))
                { ActivityManager.GetComponent<ActivityManagerScript>().select_5_dumptruck_RFID(); }
            }
        }


        //A6 RFID & rebar, wood and log. Because only 1 sensor can be selected
        if (SelectedActivities[5] == true)
        {
            foreach (int key in InterpretLUT2(5))
            {
                if (ExeList[key].Contains("RFID")) //RFID is selected
                {
                    if (ExeList[key].Contains("Wood")) ActivityManager.GetComponent<ActivityManagerScript>().A6_wood_flag = true;
                    if (ExeList[key].Contains("Log")) ActivityManager.GetComponent<ActivityManagerScript>().A6_log_flag = true;
                    if (ExeList[key].Contains("Rebar")) ActivityManager.GetComponent<ActivityManagerScript>().A6_rebar_flag = true;
                    ActivityManager.GetComponent<ActivityManagerScript>().A6_RFID();
                    RFIDReportEnable = true;
                }
            }
        }

        //A7. Workers on top floor
        if (SelectedActivities[6] == true)
        {
            foreach (int key in InterpretLUT2(6))
            {
                if (ExeList[key].Contains("Worker 1")) ActivityManager.GetComponent<ActivityManagerScript>().A7_w1_flag = true;
                if (ExeList[key].Contains("Worker 2")) ActivityManager.GetComponent<ActivityManagerScript>().A7_w2_flag = true;
                if (ExeList[key].Contains("Worker 3")) ActivityManager.GetComponent<ActivityManagerScript>().A7_w3_flag = true;

                //Run basic activity: danger zone red box. Based on worker selection bool.
                ActivityManager.GetComponent<ActivityManagerScript>().select_7_new();

                //GPS only
                if (ExeList[key].Contains("GPS") && !ExeList[key].Contains("RFID")) { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_7_GPS(); }

                //RFID only, GPS hidden.
                if (ExeList[key].Contains("RFID") && !ExeList[key].Contains("GPS")) { ActivityManager.GetComponent<ActivityManagerScript>().select_7_RFID(); }

                //RFID and GPS
                if (ExeList[key].Contains("GPS") && ExeList[key].Contains("RFID"))
                { ActivityManager.GetComponent<ActivityManagerScript>().A7_worker_GPSRFID = true; ActivityManager.GetComponent<ActivityManagerScript>().select_7_RFID(); }
            }
           

        }

        //Laser Scan related activities [7,8,9,10]
        //if MultiLaserScan, then cannot proceed
        if (MultiLaserScan == false) { if (SelectedActivities[7] == true)  LSConcurrent = 7; }//8.scan part of building, ActivityManager.GetComponent<ActivityManagerScript>().select_8();
        if (MultiLaserScan == false) { if (SelectedActivities[8] == true)  LSConcurrent = 8; }//9.scan concrete slab, ActivityManager.GetComponent<ActivityManagerScript>().select_9();
        if (MultiLaserScan == false)
        {
            if (SelectedActivities[9] == true)
            {
                foreach (int key in InterpretLUT2(9))
                {
                    if (ExeList[key].Contains("Laser Scanner") && ExeList[key].Contains("Stockpile 1"))
                    {  LSConcurrent = 9; } //ActivityManager.GetComponent<ActivityManagerScript>().select_10A();
                }
            }
        }
        if (MultiLaserScan == false)
        {
            if (SelectedActivities[9] == true)
            {
                foreach (int key in InterpretLUT2(9))
                {
                    if (ExeList[key].Contains("Laser Scanner") && ExeList[key].Contains("Stockpile 2"))
                    {  LSConcurrent = 10; }//ActivityManager.GetComponent<ActivityManagerScript>().select_10B();
                }
            }
        }

        //11. Old House LS only
        if (MultiLaserScan == false)
        {
            if (SelectedActivities[10] == true)
            {
                foreach (int key in InterpretLUT2(10))
                {
                    if (ExeList[key].Contains("Laser Scanner") && !ExeList[key].Contains("Drone") && ExeList[key].Contains("Old Building"))
                    {  LSConcurrent = 11; }//ActivityManager.GetComponent<ActivityManagerScript>().select_11Laser();
                }
            }
        }

        //11. Old house drone only
        if (SelectedActivities[10] == true)
        {
            foreach (int key in InterpretLUT2(10))
            {
                if (ExeList[key].Contains("Drone") == true && !ExeList[key].Contains("Laser Scanner") && ExeList[key].Contains("Old Building") && !MultiDroneBlock)
                //{ ActivityManager.GetComponent<ActivityManagerScript>().select_11Drone(); DConcurrent = 10; }
                {  DConcurrent = 10; }
            }
        }



        //11. Old House LS && Drone
        if (SelectedActivities[10] == true)
        {
            foreach (int key in InterpretLUT2(10))
            {
                if (ExeList[key].Contains("Drone") == true && ExeList[key].Contains("Laser Scanner") && ExeList[key].Contains("Old Building") && !MultiDroneBlock)
                {
                    //OpenChoiceDialogSmall();
                    //if (A11Drone) ActivityManager.GetComponent<ActivityManagerScript>().select_11Drone();
                    //if (A11LS) ActivityManager.GetComponent<ActivityManagerScript>().select_11Laser();
                    LSConcurrent = 11; DConcurrent = 10;
                }
            }
        }

        //12. Jobsite drone
        if (SelectedActivities[11] == true)
        {
            foreach (int key in InterpretLUT2(11))
            {
                if (ExeList[key].Contains("Drone") && ExeList[key].Contains("Jobsite") && !MultiDroneBlock)
                //{ ActivityManager.GetComponent<ActivityManagerScript>().select_12(); DConcurrent = 11; }
                {  DConcurrent = 11; }
            }
        }

        //Deprecated due to change from IMU to 3 separate activities
        /*
        //13.IMU workers
        if (SelectedActivities[12] == true)
        {
            foreach (int key in InterpretLUT2(12))
            {
                if (ExeList[key].Contains("IMU"))
                {
                    if (ExeList[key].Contains("Painter"))
                    { ActivityManager.GetComponent<ActivityManagerScript>().A14_painter = true; }
                    if (ExeList[key].Contains("Laborer"))
                    { ActivityManager.GetComponent<ActivityManagerScript>().A14_laborer = true; }
                    if (ExeList[key].Contains("Carpenter 1"))
                    { ActivityManager.GetComponent<ActivityManagerScript>().A14_c1 = true; }
                    if (ExeList[key].Contains("Carpenter 2"))
                    { ActivityManager.GetComponent<ActivityManagerScript>().A14_c2 = true; }
                    //With given worker bool, get IMU string, handle finish file write use backselected()
                    ActivityManager.GetComponent<ActivityManagerScript>().select_13_new();
                    //display IMU
                    IMUReportEnable = true;
                }      
            }
        }
        */

        //Reference
        /*
         foreach (int key in InterpretLUT2(4))
            {
                if (ExeList[key].Contains("GPS") && ExeList[key].Contains("Loader"))
                { GPSReportEnable = true; ActivityManager.GetComponent<ActivityManagerScript>().select_5_Loader_GPS(); }
            }
         */

        //20201209 New IMU implementation
        //P
        if (SelectedActivities[12] == true)
        {
            foreach (int key in InterpretLUT2(12))
            {
                //bigger condition is IMU, smaller condition is different joints
                if (ExeList[key].Contains("IMU"))
                {
                    if (ExeList[key].Contains("Back"))
                    {
                        IMUPainter.GetComponent<workerScript>().backSelect();
                    }
                    if (ExeList[key].Contains("Neck"))
                    {
                        IMUPainter.GetComponent<workerScript>().neckSelect();
                    }
                    if (ExeList[key].Contains("Shoulder"))
                    {
                        IMUPainter.GetComponent<workerScript>().shoulderSelect();
                    }
                    if (ExeList[key].Contains("Thigh"))
                    {
                        IMUPainter.GetComponent<workerScript>().thighSelect();
                    }

                    //Broad condition that IMU with a worker is selected
                    ActivityManager.GetComponent<ActivityManagerScript>().A14_painter = true;
                    //With given worker bool, get IMU string, handle finish file write use backselected()
                    ActivityManager.GetComponent<ActivityManagerScript>().select_13_new();
                    //display IMU
                    IMUReportEnable = true;
                }
               
                if (ExeList[key].Contains("GPS"))
                {
                    GPSReportEnable = true;
                    ActivityManager.GetComponent<ActivityManagerScript>().select_Painter_GPS();
                }

                if (ExeList[key].Contains("RFID"))
                {
                    ActivityManager.GetComponent<ActivityManagerScript>().select_Painter_RFID();
                }
            }
        }

        //L
        if (SelectedActivities[13] == true)
        {
            foreach (int key in InterpretLUT2(13))
            {
                //bigger condition is IMU, smaller condition is different joints
                if (ExeList[key].Contains("IMU"))
                {
                    if (ExeList[key].Contains("Back"))
                    {
                        IMULabor.GetComponent<workerScript>().backSelect();
                    }
                    if (ExeList[key].Contains("Neck"))
                    {
                        IMULabor.GetComponent<workerScript>().neckSelect();
                    }
                    if (ExeList[key].Contains("Shoulder"))
                    {
                        IMULabor.GetComponent<workerScript>().shoulderSelect();
                    }
                    if (ExeList[key].Contains("Thigh"))
                    {
                        IMULabor.GetComponent<workerScript>().thighSelect();
                    }

                    //Broad condition that IMU with a worker is selected
                    ActivityManager.GetComponent<ActivityManagerScript>().A14_laborer = true;
                    //With given worker bool, get IMU string, handle finish file write use backselected()
                    ActivityManager.GetComponent<ActivityManagerScript>().select_13_new();
                    //display IMU
                    IMUReportEnable = true;
                }

                if (ExeList[key].Contains("GPS"))
                {
                    GPSReportEnable = true;
                    ActivityManager.GetComponent<ActivityManagerScript>().select_Laborer_GPS();
                }

                if (ExeList[key].Contains("RFID"))
                {
                    ActivityManager.GetComponent<ActivityManagerScript>().select_Laborer_RFID();
                }
            }
        }

        //C
        if (SelectedActivities[14] == true)
        {
            foreach (int key in InterpretLUT2(14))
            {
                //bigger condition is IMU, smaller condition is different joints
                if (ExeList[key].Contains("IMU"))
                {
                    if (ExeList[key].Contains("Back"))
                    {
                        IMUCarpenter.GetComponent<workerScript>().backSelect();
                    }
                    if (ExeList[key].Contains("Neck"))
                    {
                        IMUCarpenter.GetComponent<workerScript>().neckSelect();
                    }
                    if (ExeList[key].Contains("Shoulder"))
                    {
                        IMUCarpenter.GetComponent<workerScript>().shoulderSelect();
                    }
                    if (ExeList[key].Contains("Thigh"))
                    {
                        IMUCarpenter.GetComponent<workerScript>().thighSelect();
                    }

                    //Broad condition that IMU with a worker is selected
                    ActivityManager.GetComponent<ActivityManagerScript>().A14_c1 = true;
                    //With given worker bool, get IMU string, handle finish file write use backselected()
                    ActivityManager.GetComponent<ActivityManagerScript>().select_13_new();
                    //display IMU
                    IMUReportEnable = true;
                }

                if (ExeList[key].Contains("GPS"))
                {
                    GPSReportEnable = true;
                    ActivityManager.GetComponent<ActivityManagerScript>().select_Carpenter_GPS();
                }

                if (ExeList[key].Contains("RFID"))
                {
                    ActivityManager.GetComponent<ActivityManagerScript>().select_Carpenter_RFID();
                }
            }
        }


        //check for LS and Drone concurrency
        if (LSConcurrent != 0 && DConcurrent != 0)
        {
            //has concurrent seleciton, activate canvas
            ConcurrentSelectionMenu.SetActive(true);
            ConcurrencyOccurred = true;
        }
        else
        { //no concurrent selection, execute individually, equal to previous implementation
            if (LSConcurrent == 7) ActivityManager.GetComponent<ActivityManagerScript>().select_8();
            if (LSConcurrent == 8) ActivityManager.GetComponent<ActivityManagerScript>().select_9();
            if (LSConcurrent == 9) ActivityManager.GetComponent<ActivityManagerScript>().select_10A();
            if (LSConcurrent == 10) ActivityManager.GetComponent<ActivityManagerScript>().select_10B();
            if (LSConcurrent == 11) ActivityManager.GetComponent<ActivityManagerScript>().select_11Laser();
            if (DConcurrent == 10) ActivityManager.GetComponent<ActivityManagerScript>().select_11Drone();
            if (DConcurrent == 11) ActivityManager.GetComponent<ActivityManagerScript>().select_12();
        }

        //Acitivity Idrection Indicator
        ActivityIndicator();
        SelectButton.SetActive(false);
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
            + ActivityManager.GetComponent<ActivityManagerScript>().A7_w3_GPS
            +ActivityManager.GetComponent<ActivityManagerScript>().A14_P_GPS
            + ActivityManager.GetComponent<ActivityManagerScript>().A14_L_GPS
            + ActivityManager.GetComponent<ActivityManagerScript>().A14_C_GPS;
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

    private void PrepareCostBenefitString()
    {
        CostBenefitString = "Cost Benefit Filler? \n line \t two with tab";

        //
    }

    private void PrepareComboDisplayString()
    {
        ComboDisplayString = "Already Configured Combinitions: \n";
        //for (int j = 0; j < ComboList.Count; j++)
        //{
        //    //Debug.Log(String.Format("Key: {0}, Value: {1}", ComboList.ElementAt(j).Key, string.Join(", ", ComboList.ElementAt(j).Value)));
        //    ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[ComboList.ElementAt(j).Key].itemName + string.Join(", ", ComboList.ElementAt(j).Value) + ". ";
        //}


        //Interpret ComboList(i,selections)
        string[] ExeList = new string[ComboList.Count];//unknown size, because of selection limits, based on estimation (21?), size set to 50.
        //Debug.Log("ComboList count:" + ComboList.Count);
        for (int j = 0; j < ComboList.Count; j++)
        {
            ExeList[ComboList.ElementAt(j).Key] = string.Join(", ", ComboList.ElementAt(j).Value);
        }


        if (SelectedActivities[0] == true)
            {
                foreach (int key in InterpretLUT2(0))
                {
                    ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[0].itemName + ", " + ExeList[key] + "\n";
                }
            }
        if (SelectedActivities[1] == true)
        {
            foreach (int key in InterpretLUT2(1))
            {
                ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[1].itemName + ", " + ExeList[key] + "\n";
            }
        }
        if (SelectedActivities[2] == true)
        {
            foreach (int key in InterpretLUT2(2))
            {
                ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[2].itemName + ", " + ExeList[key] + "\n";
            }
        }
        if (SelectedActivities[3] == true)
        {
            foreach (int key in InterpretLUT2(3))
            {
                ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[3].itemName + ", " + ExeList[key] + "\n";
            }
        }
        if (SelectedActivities[4] == true)
        {
            foreach (int key in InterpretLUT2(4))
            {
                ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[4].itemName + ", " + ExeList[key] + "\n";
            }
        }
        if (SelectedActivities[5] == true)
        {
            foreach (int key in InterpretLUT2(5))
            {
                ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[5].itemName + ", " + ExeList[key] + "\n";
            }
        }
        if (SelectedActivities[6] == true)
        {
            foreach (int key in InterpretLUT2(6))
            {
                ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[6].itemName + ", " + ExeList[key] + "\n";
            }
        }
        if (SelectedActivities[7] == true)
        {
            foreach (int key in InterpretLUT2(7))
            {
                ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[7].itemName + ", " + ExeList[key] + "\n";
            }
        }
        if (SelectedActivities[8] == true)
        {
            foreach (int key in InterpretLUT2(8))
            {
                ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[8].itemName + ", " + ExeList[key] + "\n";
            }
        }
        if (SelectedActivities[9] == true)
        {
            foreach (int key in InterpretLUT2(9))
            {
                ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[9].itemName + ", " + ExeList[key] + "\n";
            }
        }
        if (SelectedActivities[10] == true)
        {
            foreach (int key in InterpretLUT2(10))
            {
                ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[10].itemName + ", " + ExeList[key] + "\n";
            }
        }
        if (SelectedActivities[11] == true)
        {
            foreach (int key in InterpretLUT2(11))
            {
                ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[11].itemName + ", " + ExeList[key] + "\n";
            }
        }
        if (SelectedActivities[12] == true)
        {
            foreach (int key in InterpretLUT2(12))
            {
                ComboDisplayString = ComboDisplayString + "Activity: " + Mdropdown1.dropdownItems[12].itemName + ", " + ExeList[key] + "\n";
            }
        }


    }



    public void A13_stop() //AKA IMU stop button
    {
        IMUReportEnable = false;
        IMUReportCanvas.SetActive(false);
        ActivityManager.GetComponent<ActivityManagerScript>().A13_stop();
        //mainUICollection.SetActive(true);
        ManualSelectionParent.GetComponent<ManualSelection>().IMUReportEnable = false;
    }

    public void StopRFID()
    {
        RFIDReportEnable = false;
        RFIDReportCanvas.SetActive(false);
        //mainUICollection.SetActive(true);
        ManualSelectionParent.GetComponent<ManualSelection>().RFIDReportEnable = false;
    }

    public void StopGPS()
    {
        GPSReportEnable = false;
        GPSReportCanvas.SetActive(false);
        //mainUICollection.SetActive(true);
        ManualSelectionParent.GetComponent<ManualSelection>().GPSReportEnable = false;
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

            //string name = "A" + j + "POS";
            //Activate Chevron and live for 5 seconds.
            StartCoroutine(ActivityPointer(PointingChevron, j, 10.0f));

        }

        //Vector3 Original = Everything.transform.position;
        //Vector3 ActivityPosition = A1Position.transform.position;
        //Vector3 TransformedCoordinates = Original - ActivityPosition;
        //Everything.transform.position = TransformedCoordinates;

    }

    // Activate chevron, give location, and keep it active for 5 seconds.
    public IEnumerator ActivityPointer(GameObject go, int j, float delay)
    {
        if (j == 8 || j == 9 || j == 10 || j == 11)
        {//for LS is different 8,9,10,11,
            go.GetComponent<DirectionalIndicator>().DirectionalTarget = GameObject.Find("scanner").transform;
            go.SetActive(true);
            yield return new WaitForSeconds(60f);
            go.SetActive(false);
        }
        else
        {//normal activities
            string name = "A" + j + "POS";
            go.GetComponent<DirectionalIndicator>().DirectionalTarget = GameObject.Find(name).transform;
            go.SetActive(true);
            yield return new WaitForSeconds(delay);
            go.SetActive(false);
        }

    }

    #endregion




    //----------------------------------------------------------


    #region UI related functions

    //Activities Confirm.
    public void A_Confirm()
    {
        TitleText.GetComponent<TextMeshProUGUI>().text = "Automatic selection, please select sensors to be used with this activity.";
        Mdropdown1.ForceClose();

        int TempActivityNumber = 0;
        RemainingActivityNumber = SelecedActivityNumber;

        //Check if multiple laser scan activity selected
        MultiLaserScanCheck();

        //cannot proceed if Multi Laser Scan Detected
        if (!MultiLaserScan)
        {
            //Dropdown 1 inactive.
            //Mdropdown1.GetComponent<Button>().interactable = false;
            //Mdropdown1.transform.Find("DisablePanel").gameObject.SetActive(true);
            //Dropdown 2 active.
            S_confirm_button.SetActive(true);
            Mdropdown2.GetComponent<Button>().interactable = true;
            //Mdropdown2.transform.Find("DisablePanel").gameObject.SetActive(false);

            //Find first active activity number
            for (int j = 0; j < 15; ++j)
            {
                if (SelectedActivities[j] == true)
                { CurrentActivitySelection = j; TempActivityNumber = j; break; }
            }

            //Debug.Log("A-confirm activity:" + CurrentActivitySelection);
            Dropdown1ShowCurrentActivity = true;

            //hide entire big canvas
            //mainUICollection.SetActive(false);
            //activity selection panel
            ActivitySelectionParentPanel.gameObject.SetActive(true);

            //Build a array correspond i and activity number, since Activity selection is fixed, this only need to do once.
            //LUT(Alias, actual activity number); e.g. LUT.Add(1,3);

            //A_confirm button should not be used again.
            //A_confirm_button.SetActive(false);
            AConfirmButtonSelected = true;

            //instantiate activity selection panel
            for (int i = 0; i < SelecedActivityNumber; i++)
            {
                int RoundedNum = Mathf.RoundToInt(SelecedActivityNumber / 2);
                //instantiate row 1
                
                if (i < RoundedNum)
                {
                    GameObject goButton = Instantiate(ActivitySelectionPrefabButton, new Vector3((i + 1) * 500, 50, -50), Quaternion.identity) as GameObject;
                    string ButtonName = "ActivitySelection" + i;
                    goButton.name = ButtonName;
                    goButton.transform.SetParent(ActivitySelectionParentPanel, false);
                    //Button tempButton = goButton.GetComponent<Button>();
                    int tempInt = i;
                    //tempButton.onClick.AddListener(() => ButtonClicked(tempInt));
                    //correspond alias i with active activity number
                    LUT.Add(i, TempActivityNumber);
                    //tempButton.transform.Find("ButtonText").GetComponent<TextMeshProUGUI>().text = Mdropdown1.dropdownItems[TempActivityNumber].itemName;//given button names with activity names.

                    //test for hololens button
                    goButton.GetComponent<Interactable>().OnClick.AddListener(() => ButtonClicked(tempInt));
                    goButton.transform.Find("IconAndText").transform.Find("TextMeshPro").GetComponent<TextMeshPro>().text = Mdropdown1.dropdownItems[TempActivityNumber].itemName;
                }
                
                //instantiate row 2
                
                else
                {
                    GameObject goButton = Instantiate(ActivitySelectionPrefabButton, new Vector3((i - RoundedNum + 1) * 500, -100, -50), Quaternion.identity) as GameObject;
                    string ButtonName = "ActivitySelection" + i;
                    goButton.name = ButtonName;
                    goButton.transform.SetParent(ActivitySelectionParentPanel, false);
                    //Button tempButton = goButton.GetComponent<Button>();
                    int tempInt = i;
                    //tempButton.onClick.AddListener(() => ButtonClicked(tempInt));
                    //correspond alias i with active activity number
                    LUT.Add(i, TempActivityNumber);
                    //tempButton.transform.Find("ButtonText").GetComponent<TextMeshProUGUI>().text = Mdropdown1.dropdownItems[TempActivityNumber].itemName;//given button names with activity names.
                                
                    //test for hololens button
                    goButton.GetComponent<Interactable>().OnClick.AddListener(() => ButtonClicked(tempInt));
                    goButton.transform.Find("IconAndText").transform.Find("TextMeshPro").GetComponent<TextMeshPro>().text = Mdropdown1.dropdownItems[TempActivityNumber].itemName;
                }

                /* Original method
                GameObject goButton = Instantiate(ActivitySelectionPrefabButton, new Vector3(i * 500, 0, -10), Quaternion.identity) as GameObject;
                string ButtonName = "ActivitySelection" + i;
                goButton.name = ButtonName;
                goButton.transform.SetParent(ActivitySelectionParentPanel, false);
                Button tempButton = goButton.GetComponent<Button>();
                int tempInt = i;
                tempButton.onClick.AddListener(() => ButtonClicked(tempInt));
                //correspond alias i with active activity number
                LUT.Add(i, TempActivityNumber);
                tempButton.transform.Find("ButtonText").GetComponent<TextMeshProUGUI>().text = Mdropdown1.dropdownItems[TempActivityNumber].itemName;//given button names with activity names.
                */
                //First active activity is CurrentActivitySelection and TempActivityNumber.
                //It has only been assigned at beginning = 0, then find first active activity.
                //Because A_confirm button is designed to only press once, this execution order will be only executed once.
                //After this initial add, TempActivityNumber will be changed to switch to next active activity.

                //Find next active activity.
                //If out of bound, exit.
                for (int k = TempActivityNumber + 1; k < 16; ++k)
                {
                    if (k == 15) // 12 is last Activity element. 13 is boundary condition.
                    {
                        break;
                    }
                    if (SelectedActivities[k] == true)
                    {
                        TempActivityNumber = k;
                        break;
                    }
                }
            }
            GameObject.Destroy(ActivitySelectionPrefabButton);
           
            //close canvas and disble button
            //If added, then dropdown does not retract.
            //mainUICollection.SetActive(false);
            //Mdropdown1.GetComponent<Button>().interactable = false;

        }
    }



    //what is performed after click activity selection?
    private void ButtonClicked(int buttonNo)
    {
        //Debug.Log("Button clicked = " + buttonNo);
        //which activity is selected, and give value to CurrentActivitySelection.
        //VisualBlock.SetActive(false);
        Mdropdown1.GetComponent<Button>().interactable = false;
        //Activate main menu
        mainUICollection.SetActive(true);
        
        //temporary suspend for debugging
        //Deactivate activity selection panel
        ActivitySelectionParentPanel.gameObject.SetActive(false);

        //Find destroy button name
        string FindButton = "ActivitySelection" +buttonNo;
        //destroy selected button
        GameObject.Destroy(ActivitySelectionParentPanel.transform.Find(FindButton).gameObject);
        //record one less remaining activity.
        RemainingActivityNumber = RemainingActivityNumber - 1;

        //give value
        int value = 0;
        if (LUT.TryGetValue(buttonNo, out value))
        {
            CurrentActivitySelection = value;
            Debug.Log("Selected is:"+ Mdropdown1.dropdownItems[value].itemName);
        }
        else { Debug.Log("LUT didn't find matching value"); }
    }


    //Sensor Confirm.
    public void S_Confirm()
    {
        TitleText.GetComponent<TextMeshProUGUI>().text = "Automatic selection, please select resources to be used with selected sensor.";
        Mdropdown2.ForceClose();
        CheckActivitySensors();
        //S_confirm_button.SetActive(false);
        //R_confirm_button.SetActive(true);
        //Mdropdown2.GetComponent<Button>().interactable = false;
        //Mdropdown3.GetComponent<Button>().interactable = true;
        //update resources list for CurrentActivitySelection
        ChangeResourcesBool = true;

        //Find first active sensor, and set as current configuring sensor.
        for (int i = 0; i < 5; ++i)
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

        SensorWarningString = ""; //Empty
        //20201123 Add RFID for A1,2,3,5(0,1,2,4)

        //A1+GPS[0]
        if (CurrentActivitySelection == 0 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else { if (CurrentActivitySelection == 0 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 0 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 0 && SelectedSensors[4] == true) IMUWarning(); }
        //if (CurrentActivitySelection == 0 && SelectedSensors[1] == true) RFIDWarning();
        //A2+GPS
        if (CurrentActivitySelection == 1 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else {  if (CurrentActivitySelection == 1 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 1 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 1 && SelectedSensors[4] == true) IMUWarning(); }
        //if (CurrentActivitySelection == 1 && SelectedSensors[1] == true) RFIDWarning();
        //A3+GPS/RFID
        if (CurrentActivitySelection == 2 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else {  if (CurrentActivitySelection == 2 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 2 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 2 && SelectedSensors[4] == true) IMUWarning(); }
        //A4+GPS/RFID
        if (CurrentActivitySelection == 3 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else {  if (CurrentActivitySelection == 3 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 3 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 3 && SelectedSensors[4] == true) IMUWarning(); }
        //A5+GPS
        if (CurrentActivitySelection == 4 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == false) { pass = true; }
        else { if (CurrentActivitySelection == 4 && SelectedSensors[2] == true) LSWarning(); if (CurrentActivitySelection == 4 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 4 && SelectedSensors[4] == true) IMUWarning(); }
        //if (CurrentActivitySelection == 4 && SelectedSensors[1] == true) RFIDWarning();
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
        //if (CurrentActivitySelection == 12 && SelectedSensors[0] == false && SelectedSensors[1] == false && SelectedSensors[2] == false && SelectedSensors[3] == false && SelectedSensors[4] == true) { pass = true; }
        //else { if (CurrentActivitySelection == 12 && SelectedSensors[1] == true) RFIDWarning(); if (CurrentActivitySelection == 12 && SelectedSensors[0] == true) GPSWarning(); if (CurrentActivitySelection == 12 && SelectedSensors[3] == true) DroneWarning(); if (CurrentActivitySelection == 12 && SelectedSensors[2] == true) LSWarning(); }

        //20201209 IMU is dissected into 3 separate activities
        //Added possiblity that workers work with GPS and RFID
        //painter
        if (CurrentActivitySelection == 12 && (SelectedSensors[0] == true || SelectedSensors[1] == true || SelectedSensors[4] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false ) { pass = true; }
        //if (CurrentActivitySelection == 12 && SelectedSensors[1] == true) RFIDWarning(); 
        //if (CurrentActivitySelection == 12 && SelectedSensors[0] == true) GPSWarning(); 
        if (CurrentActivitySelection == 12 && SelectedSensors[3] == true) DroneWarning(); 
        if (CurrentActivitySelection == 12 && SelectedSensors[2] == true) LSWarning();

        //Laborer
        if (CurrentActivitySelection == 13 && (SelectedSensors[0] == true || SelectedSensors[1] == true || SelectedSensors[4] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false) { pass = true; }
        if (CurrentActivitySelection == 13 && SelectedSensors[3] == true) DroneWarning();
        if (CurrentActivitySelection == 13 && SelectedSensors[2] == true) LSWarning();
        //carpenter
        if (CurrentActivitySelection == 14 && (SelectedSensors[0] == true || SelectedSensors[1] == true || SelectedSensors[4] == true) && SelectedSensors[2] == false && SelectedSensors[3] == false) { pass = true; } 
        if (CurrentActivitySelection == 14 && SelectedSensors[3] == true) DroneWarning();
        if (CurrentActivitySelection == 14 && SelectedSensors[2] == true) LSWarning();


        if (pass)
        {
            //Dropdown 2 inactive.
            Mdropdown2.GetComponent<Button>().interactable = false;
            //disable until next activity configure, which is in NA button.
            S_confirm_button.SetActive(false);
            R_confirm_button.SetActive(true);
            //Mdropdown2.transform.Find("DisablePanel").gameObject.SetActive(true);
            //Dropdown 3 active.
            Mdropdown3.GetComponent<Button>().interactable = true;
            //Mdropdown3.transform.Find("DisablePanel").gameObject.SetActive(false);
        }
        else
            InstantiateWarningDialog();
        //else
        //{
        //dialogue warning
        //Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Warning",
        //   "Wrong sensor selected for this activity, Please correct your selection!", false);
        //reset selection
        //}
    }

            private void GPSWarning() {
        // Dialog.Open(dialogPrefabMedium, DialogButtonType.OK, "Please correct your sensor selection!",
        //"GPS: Uses satellite signals to locate tagged objects or people.", false);
        SensorWarningString = SensorWarningString + "GPS: Uses satellite signals to locate tagged objects or people. \n" ;
            }

            private void RFIDWarning()
            {
        //Dialog.Open(dialogPrefabMedium, DialogButtonType.OK, "Please correct your sensor selection!",
        // "\n\n RFID employs radio waves to collect information on tagged people, equipment or materials through an electronic reader", false);
        SensorWarningString = SensorWarningString + "RFID employs radio waves to collect information on tagged people, equipment or materials through an electronic reader. \n";
            }

            private void LSWarning()
            {
        //Dialog.Open(dialogPrefabMedium, DialogButtonType.OK, "Please correct your sensor selection!",
        // "\n\n\n\n Laser scanner: uses deflection of laser beams to capture images of assigned locations.", false);
        SensorWarningString = SensorWarningString + "Laser scanner: uses deflection of laser beams to capture images of assigned locations. \n";
            }

            private void DroneWarning()
            {
        //Dialog.Open(dialogPrefabMedium, DialogButtonType.OK, "Please correct your sensor selection!g",
        // "\n\n\n\n\n\n Drone: is an unmanned aircraft remote controlled by an operator for capturing images and videos", false);
        SensorWarningString = SensorWarningString + "Drone: is an unmanned aircraft remote controlled by an operator for capturing images and videos. \n";
            }

            private void IMUWarning()
            {
        //Dialog.Open(dialogPrefabMedium, DialogButtonType.OK, "Please correct your sensor selection!",
        //"\n\n\n\n\n\n\n\n IMU: consists of accelerometers and gyroscopes for measuring the rotation and acceleration of different body part.", false);
        SensorWarningString = SensorWarningString + "IMU: consists of accelerometers and gyroscopes for measuring the rotation and acceleration of different body part. \n";
            }

    private void InstantiateWarningDialog()
    {
            Dialog.Open(dialogPrefabMedium, DialogButtonType.OK, "Wrong sensor selection!", SensorWarningString, false);
    }

    //Resources Confirm
    public void R_Confirm()
    {
        Mdropdown3.ForceClose();
        bool pass = false;//Flag to check sensors againest resources.

        //Check Sensors Resources.
        // Which activity, correct sensor (don't need all, already determined in previouys step), correct resources?

        //A1+GPS[0]+RFID[1] + Dozer
        if (CurrentActivitySelection == 0 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && Array.Exists(SelectedResources, element => element == "Dozer") && !Array.Exists(SelectedResources, element => element == "Stockpile")) { pass = true; }
        if (CurrentActivitySelection == 0 && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "Stockpile")) Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning","Stockpile cannot be used with GPS", false);
        if (CurrentActivitySelection == 0 && SelectedSensors[1] == true && Array.Exists(SelectedResources, element => element == "Stockpile")) Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning", "Stockpile cannot be used with RFID", false);

        //A2+GPS+RFID+ Crane+SteelBeam
        if (CurrentActivitySelection == 1 && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "Steel Beam") && !Array.Exists(SelectedResources, element => element == "Crane")) { pass = true; }
        if (CurrentActivitySelection == 1 && SelectedSensors[1] == true && !Array.Exists(SelectedResources, element => element == "Steel Beam") && Array.Exists(SelectedResources, element => element == "Crane")) { pass = true; }
        if (CurrentActivitySelection == 1 && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "Crane")) Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning","Crane cannot be used with GPS", false);


        //A3+GPS+truck
        if (CurrentActivitySelection == 2 && SelectedSensors[0] == true && SelectedSensors[1] == false && Array.Exists(SelectedResources, element => element == "Truck") && !Array.Exists(SelectedResources, element => element == "Rebar")) { pass = true; }
        //A3+RFID+Rebar/ Truck
        if (CurrentActivitySelection == 2 && SelectedSensors[0] == false && SelectedSensors[1] == true && (Array.Exists(SelectedResources, element => element == "Rebar") || Array.Exists(SelectedResources, element => element == "Truck"))) { pass = true; }
        //A3+GPS+Rebar
        if (CurrentActivitySelection == 2 && SelectedSensors[0] == true && SelectedSensors[1] == false && Array.Exists(SelectedResources, element => element == "Rebar") && !Array.Exists(SelectedResources, element => element == "Truck"))
        {
            Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning","Rebar cannot be tagged with GPS.", false);
        }
        //A3+RFID+truck
        //if (CurrentActivitySelection == 2 && SelectedSensors[0] == false && SelectedSensors[1] == true && !Array.Exists(SelectedResources, element => element == "Rebar") && Array.Exists(SelectedResources, element => element == "Truck"))
        //{Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning","Truck cannot be tagged with RFID.", false);}
        //A3+gps+rfid+truck, warning: select a resource for rfid
        if (CurrentActivitySelection == 2 && SelectedSensors[0] == true && SelectedSensors[1] == true && Array.Exists(SelectedResources, element => element == "Truck") && !Array.Exists(SelectedResources, element => element == "Rebar")) Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning",
       "Select a resource for RFID.", false);
        //A3+gps+rfid+rebar, warning: select a resource for gps, vice versa.
        if (CurrentActivitySelection == 2 && SelectedSensors[0] == true && SelectedSensors[1] == true && !Array.Exists(SelectedResources, element => element == "Truck") && Array.Exists(SelectedResources, element => element == "Rebar")) Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning",
"Select a resource for GPS.", false);
        //A3+(GPS&&RFID)+(truck&&rebar)
        if (CurrentActivitySelection == 2 && SelectedSensors[0] == true && SelectedSensors[1] == true && Array.Exists(SelectedResources, element => element == "Truck") && Array.Exists(SelectedResources, element => element == "Rebar")) { pass = true; }
        //A3+gps+rfid+none
        if (CurrentActivitySelection == 2 && SelectedSensors[0] == true && SelectedSensors[1] == true && !Array.Exists(SelectedResources, element => element == "Truck") && !Array.Exists(SelectedResources, element => element == "Rebar"))
        {Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning","Please select at least one resource.", false);}

        //A4+GPS/RFID+worker
        if (CurrentActivitySelection == 3 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && Array.Exists(SelectedResources, element => element == "Worker 1")) { pass = true; }

        //A5+ GPS + RFID +loader/truck
        if (CurrentActivitySelection == 4 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && (Array.Exists(SelectedResources, element => element == "Loader") || Array.Exists(SelectedResources, element => element == "Dumptruck")) && !Array.Exists(SelectedResources, element => element == "Stockpile")) { pass = true; }
        if (CurrentActivitySelection == 4 && SelectedSensors[0] == true && Array.Exists(SelectedResources, element => element == "Stockpile")) Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning","GPS cannot be tagged on stockpile.", false);
        if (CurrentActivitySelection == 4 && SelectedSensors[1] == true && Array.Exists(SelectedResources, element => element == "Stockpile")) Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning", "RFID cannot be tagged on stockpile.", false);

        //A6+RFID+WLR
        if (CurrentActivitySelection == 5 && SelectedSensors[1] == true && (Array.Exists(SelectedResources, element => element == "Wood") || Array.Exists(SelectedResources, element => element == "Log") || Array.Exists(SelectedResources, element => element == "Rebar"))) { pass = true; }
        else { if(CurrentActivitySelection == 5 && SelectedSensors[1] == true && !Array.Exists(SelectedResources, element => element == "Wood") && !Array.Exists(SelectedResources, element => element == "Log") && !Array.Exists(SelectedResources, element => element == "Rebar")) Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning",
  "Select at least one resource!", false);
        }
        //A7+(GPS||RFID)&&(w1w2w3)
        if (CurrentActivitySelection == 6 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && (Array.Exists(SelectedResources, element => element == "Worker 1") || Array.Exists(SelectedResources, element => element == "Worker 2") || Array.Exists(SelectedResources, element => element == "Worker 3"))) { pass = true; }
        else { if(CurrentActivitySelection == 6 && (SelectedSensors[0] == true || SelectedSensors[1] == true) && !Array.Exists(SelectedResources, element => element == "Worker 1") && !Array.Exists(SelectedResources, element => element == "Worker 2") && !Array.Exists(SelectedResources, element => element == "Worker 3")) Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning",
 "Select at least one worker!", false);
        }
        //A8+LS
        if (CurrentActivitySelection == 7 && SelectedSensors[2] == true) { pass = true; }
        //A9+LS
        if (CurrentActivitySelection == 8 && SelectedSensors[2] == true) { pass = true; }
        //A10+LS+(only 1 stockpile at a time)
        if (CurrentActivitySelection == 9 && SelectedSensors[2] == true && (Array.Exists(SelectedResources, element => element == "Stockpile 1") && !Array.Exists(SelectedResources, element => element == "Stockpile 2"))) { pass = true; }
        else {
            if (CurrentActivitySelection == 9 && SelectedSensors[2] == true && Array.Exists(SelectedResources, element => element == "Stockpile 1") && Array.Exists(SelectedResources, element => element == "Stockpile 2"))
                Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning","Only 1 Laser Scanner operator available.", false);
            if (CurrentActivitySelection == 9 && SelectedSensors[2] == true && !Array.Exists(SelectedResources, element => element == "Stockpile 1") && !Array.Exists(SelectedResources, element => element == "Stockpile 2"))
                Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning", "Need to select 1 Laser Scanner object.", false);
        }
        //A11+LS/Drone+old house
        if (CurrentActivitySelection == 10 && (SelectedSensors[2] == true || SelectedSensors[3] == true) && Array.Exists(SelectedResources, element => element == "Old Building")) { pass = true; }
        //A12+drone+jobsite
        if (CurrentActivitySelection == 11 && SelectedSensors[3] == true && Array.Exists(SelectedResources, element => element == "Jobsite")) { pass = true; }
        
        //A13+IMU+w1w2w3w4
        /*
        if (CurrentActivitySelection == 12 && SelectedSensors[4] == true && (Array.Exists(SelectedResources, element => element == "Painter") || Array.Exists(SelectedResources, element => element == "Laborer") || Array.Exists(SelectedResources, element => element == "Carpenter 1") || Array.Exists(SelectedResources, element => element == "Carpenter 2"))) { pass = true; }
        else {
            if (CurrentActivitySelection == 12 && SelectedSensors[4] == true && !Array.Exists(SelectedResources, element => element == "Painter") && !Array.Exists(SelectedResources, element => element == "Laborer") && !Array.Exists(SelectedResources, element => element == "Carpenter 1") && !Array.Exists(SelectedResources, element => element == "Carpenter 2"))
                Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Resources Warning", "Select at least 1 worker to attach IMU.", false);
        }
        */

        //20201209 New version of IMU dissected to 3 activities, resources are joints. No joint selection is possible.
        //Selection of GPS and RFID with worker is also possible.
        //"Back","Neck","Shoulder","Thigh" 
        //P
        if (CurrentActivitySelection == 12 && (SelectedSensors[0] == true || SelectedSensors[1] == true || SelectedSensors[4] == true)) { pass = true; }
        //L
        if (CurrentActivitySelection == 13 && (SelectedSensors[0] == true || SelectedSensors[1] == true || SelectedSensors[4] == true)) { pass = true; }
        //C
        if (CurrentActivitySelection == 14 && (SelectedSensors[0] == true || SelectedSensors[1] == true || SelectedSensors[4] == true)) { pass = true; }

        if (pass)
        {
            //add current combo to execution command list.
            //Record new Combo command
            UpdateLUT2();

            //find next active sensor. If not, show NA.
            for (int i = CurrentSensorConfig + 1; i < 6; ++i)
            {
                // 4 is last sensor, 5 is boundary condition.
                //If selected activity is not 1, show NA button. 
                if (i == 5 && SelecedActivityNumber != 1) 
                {
                    R_confirm_button.SetActive(false);
                    ShowComboBool = true;//show combo selection panel
                    NAButton.SetActive(true); //boundary condition show next activity button.      
                    Mdropdown3.GetComponent<Button>().interactable = false;//Dropdown 3 set inactive.
                    //Mdropdown3.transform.Find("DisablePanel").gameObject.SetActive(true);
                    break;
                }
                //If selected activity is 1, don't show NA button, show select button. 
                if (i == 5 && SelecedActivityNumber == 1)
                {
                    R_confirm_button.SetActive(false);
                    ShowComboBool = true;//show combo selection panel
                    //NAButton.SetActive(true); //boundary condition show next activity button.      
                    Mdropdown3.GetComponent<Button>().interactable = false;//Dropdown 3 set inactive.
                    //Mdropdown3.transform.Find("DisablePanel").gameObject.SetActive(true);
                    SelectButton.SetActive(true);
                    break;
                }
                if (SelectedSensors[i] == true)
                {
                    CurrentSensorConfig = i; //currently configuring resource for sensor i
                    break;
                }
            }
        }
    }


    //Find next active activity, or out of bound.
    public void NextActivityButton()
    { //if reaches final activity, then show run button. 
        if (RemainingActivityNumber == 0)
        {
            //reached final activity
            NAButton.SetActive(false);
            SelectButton.SetActive(true);
            Debug.Log("Finished configuration. Can execute.");
        }
        else
        {   //has more activity to show, show activity selection canvas, also hide itself and main canvas..
            ActivitySelectionParentPanel.gameObject.SetActive(true);
            Dropdown2ShowCurrentSensor = false;
            //hide itself, and main canvas
            NAButton.SetActive(false);
            mainUICollection.SetActive(false);
            Mdropdown2.GetComponent<Button>().interactable = true; //sensor dropdown enabled
            S_confirm_button.SetActive(true);//sensor confirm button enabled
            //R_confirm_button.SetActive(true);//Resource confirm button
        }
    }


    //Used to replace original UpdateComboList().
    //Based on building original ComboList, also building a LUT2 as a look up table to correspond between ComboNumber and actual activity number.
    public void UpdateLUT2()
    {
        //this part is update ComboList()
        //Initialize as empty list
        List<string> ComboEntry = new List<string>();
        //CurrentSensorConfig, only 1!
        if (CurrentSensorConfig == 0) ComboEntry.AddRange(new string[] { "GPS" });
        if (CurrentSensorConfig == 1) ComboEntry.AddRange(new string[] { "RFID" });
        if (CurrentSensorConfig == 2) ComboEntry.AddRange(new string[] { "Laser Scanner" });
        if (CurrentSensorConfig == 3) ComboEntry.AddRange(new string[] { "Drone" });
        if (CurrentSensorConfig == 4) ComboEntry.AddRange(new string[] { "IMU" });
        //add selected resources entry
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
        //Add current ComboEntry to completed ComboList
        ComboList.Add(LUT2Index, ComboEntry);

        //this part is update LUT2
        LUT2.Add(LUT2Index, CurrentActivitySelection);
        LUT2Index = LUT2Index + 1;

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
        //Add current ComboEntry to completed ComboList
        ComboList.Add(CurrentActivitySelection, ComboEntry);
    }

    public void DroneConc()
    {
        ConcurrentSelectionMenu.SetActive(false);
        ConcurrentSelection = 1;
        if (DConcurrent == 10) ActivityManager.GetComponent<ActivityManagerScript>().select_11Drone();
        if (DConcurrent == 11) ActivityManager.GetComponent<ActivityManagerScript>().select_12();
    }

    public void LSConc()
    {
        ConcurrentSelectionMenu.SetActive(false);
        ConcurrentSelection = 2;
        if (LSConcurrent == 7) ActivityManager.GetComponent<ActivityManagerScript>().select_8();
        if (LSConcurrent == 8) ActivityManager.GetComponent<ActivityManagerScript>().select_9();
        if (LSConcurrent == 9) ActivityManager.GetComponent<ActivityManagerScript>().select_10A();
        if (LSConcurrent == 10) ActivityManager.GetComponent<ActivityManagerScript>().select_10B();
        if (LSConcurrent == 11) ActivityManager.GetComponent<ActivityManagerScript>().select_11Laser();
    }

    public void ConcurrencyResidualExe()
    {
        if (ConcurrentSelection == 1)
        {
            Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Reminder",   "You finished using drone, now we switch to Laser Scanner.", false);
            LSConc();//ConcSelec = 1 means drone conc already executed, now we need to execute residual LSConc()
        } 
        else 
        {
            Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Reminder", "You finished using Laser Scanner, now we switch to Drone.", false);
            DroneConc();//vice versa
        }

    }
    #endregion

    //----------------------------------------------------------

    #region Other small functions

    /// <summary>
    /// Show and hide button function, true means hide
    /// </summary>
    /// 
    public void ShowHide()
    {
        GameObject building = Everything.transform.Find("SceneContent").transform.Find("Construction Site").transform.Find("buildings").transform.Find("building-6").gameObject;
        GameObject LS = SensorParentNode.transform.Find("laserScanner").gameObject;
        GameObject MDrone = SensorParentNode.transform.Find("Drone").gameObject;

        if (showhidetoggle)
        {
            //Currently in hidden state, now show everything.
            showhidetoggle = false;
            mainUICollection.SetActive(true);
            building.SetActive(true);//building-6 special case shared by multiple activities
            LS.SetActive(true);
            MDrone.SetActive(true);
            foreach (Transform child in ActivityResourcesNode.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            //bool is false, set to true and hide everything
            //Switch to hide in-active assets.
            showhidetoggle = true;
            mainUICollection.SetActive(false);
            MiscAssetNode.SetActive(false);
            //building.SetActive(false); //building-6 relate to these activities:
            //LS.SetActive(false);
            //MDrone.SetActive(false);


            //if(Activity not active) {turn off;}
            if (!SelectedActivities[0]) { ActivityResourcesNode.transform.Find("Activity1").gameObject.SetActive(false); }
            if (!SelectedActivities[1]) 
            {
                if(!SelectedActivities[3])
                ActivityResourcesNode.transform.Find("Activity2").gameObject.SetActive(false); 
            }
            if (!SelectedActivities[2]) { ActivityResourcesNode.transform.Find("Activity3").gameObject.SetActive(false); }
            if (!SelectedActivities[3]) { ActivityResourcesNode.transform.Find("Activity4").gameObject.SetActive(false); }
            if (!SelectedActivities[4]) { ActivityResourcesNode.transform.Find("Activity5").gameObject.SetActive(false); }
            if (!SelectedActivities[5]) { ActivityResourcesNode.transform.Find("Activity6").gameObject.SetActive(false); }
            if (!SelectedActivities[6]) { ActivityResourcesNode.transform.Find("Activity7").gameObject.SetActive(false); }
            if (!SelectedActivities[7]) { ActivityResourcesNode.transform.Find("Activity8").gameObject.SetActive(false); }
            if (!SelectedActivities[8]) { ActivityResourcesNode.transform.Find("Activity9").gameObject.SetActive(false); }
            if (!SelectedActivities[9]) { ActivityResourcesNode.transform.Find("Activity11A").gameObject.SetActive(false); ActivityResourcesNode.transform.Find("Activity11B").gameObject.SetActive(false);  }
            if (!SelectedActivities[10]) { ActivityResourcesNode.transform.Find("Activity12").gameObject.SetActive(false); ActivityResourcesNode.transform.Find("Activity12_Laser").gameObject.SetActive(false); ActivityResourcesNode.transform.Find("Activity12_Drone").gameObject.SetActive(false);   }
            if (!SelectedActivities[11]) { ActivityResourcesNode.transform.Find("Activity13_Drone").gameObject.SetActive(false);  }
            if (!SelectedActivities[12]) { ActivityResourcesNode.transform.Find("Activity14").gameObject.SetActive(false); }
            if (!SelectedActivities[13]) { ActivityResourcesNode.transform.Find("Activity15").gameObject.SetActive(false); }
            if (!SelectedActivities[14]) { ActivityResourcesNode.transform.Find("Activity16").gameObject.SetActive(false); }

            //building-6 check
            if (!SelectedActivities[1] && !SelectedActivities[3] && !SelectedActivities[8])
            {
                building.SetActive(false);
            }

            //LS check
            if (!SelectedActivities[7] && !SelectedActivities[8] && !SelectedActivities[9] && !SelectedActivities[10])
            {
                LS.SetActive(false);
            }
            //Drone check
            if (!SelectedActivities[10] && !SelectedActivities[11])
            {
                MDrone.SetActive(false);
            }
        }
        
        
    }



    //Activate legacy menu upon selection.
    public void SelectLegacyMenu()
    {
        LegacyMainMenu.SetActive(true);
    }

    //For automatic scene 5
    public void ReloadSceneButton()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MenuHub",LoadSceneMode.Single);
        //ActivityManager.GetComponent<ActivityManagerScript>().resetScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }

    //For manual scene 6
    public void ReloadManualSceneButton()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MenuHub",LoadSceneMode.Single);
        //ActivityManager.GetComponent<ActivityManagerScript>().resetScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);
    }

    public void TutorialModeReloadSceneButton()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MenuHub",LoadSceneMode.Single);
        //ActivityManager.GetComponent<ActivityManagerScript>().resetScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void StopButton()
    {
        //reset all sensors
        //ActivityManager.GetComponent<ActivityManagerScript>().sensorSelected();
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
