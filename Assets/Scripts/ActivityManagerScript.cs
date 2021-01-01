using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.Experimental.UI.BoundsControl;

public class ActivityManagerScript : MonoBehaviour
{
    #region Parameters and declaration

    //main camera
    [SerializeField] private GameObject mainCamera;

    //legacy objects
    [SerializeField] private GameObject worker;
    [SerializeField] private GameObject tripod;
    [SerializeField] private GameObject scanner;
    [SerializeField] private GameObject scannerParentNode;
    [SerializeField] private GameObject scannerCanvas;
    [SerializeField] private GameObject drone;

    //activities main actuator
    [SerializeField] private GameObject Activity1Bulldozer;
    [SerializeField] private GameObject Activity2Crane;
    [SerializeField] private GameObject Activity2CraneLoad;
    [SerializeField] private GameObject Activity3Truck;
    [SerializeField] private GameObject Activity4Worker;
    //[SerializeField] private GameObject Activity4Worker_GPSTMP;
    [SerializeField] private GameObject Activity5;
    [SerializeField] private GameObject Activity5Loader;
    [SerializeField] private GameObject Activity5dumptruck;
    [SerializeField] private GameObject Activity6_ResourcesCanvas;
    [SerializeField] private GameObject Activity7Arrow;
    //[SerializeField] private GameObject A7w1_GPSTMP;
    //[SerializeField] private GameObject A7w2_GPSTMP;
    //[SerializeField] private GameObject A7w3_GPSTMP;
    [SerializeField] private GameObject A7Worker1;
    [SerializeField] private GameObject A7Worker2;
    [SerializeField] private GameObject A7Worker3;
    [SerializeField] private GameObject Activity6;
    [SerializeField] private GameObject Activity8;
    [SerializeField] private GameObject Activity9;
    [SerializeField] private GameObject Activity11A;
    [SerializeField] private GameObject Activity11B;
    [SerializeField] private GameObject Activity12Laser;
    [SerializeField] private GameObject Activity12Drone;
    [SerializeField] private GameObject Activity12_DroneLocator;
    [SerializeField] private GameObject Activity12_MDroneCameraLocator;
    [SerializeField] private GameObject Activity13Drone;
    [SerializeField] private GameObject Activity14Canvas;
    [SerializeField] private GameObject Activity14;
    [SerializeField] private GameObject LSResetButton;


    //menus and canvas
    [SerializeField] private GameObject mainMenu; //added for main menu
    [SerializeField] private GameObject activityMenu; //added for activity menu
    [SerializeField] private GameObject scannerMenu;
    [SerializeField] private GameObject resourceMenu;
    [SerializeField] private GameObject tripodMenu;
    [SerializeField] private GameObject imuMenu;
    [SerializeField] private GameObject rfidMenu;
    [SerializeField] private GameObject droneCanvas;
    [SerializeField] private GameObject MDroneFlightCanvas;
    [SerializeField] private GameObject MDroneParentCanvas;
    [SerializeField] private GameObject Activity11Canvas;
    [SerializeField] private GameObject Activity12Canvas;
    [SerializeField] private GameObject Activity12_DroneCanvas;
    [SerializeField] private GameObject Activity13_DroneCanvas;
    [SerializeField] private GameObject LaserScannerBackButton;
    [SerializeField] private GameObject ManualDroneBackButton;
    [SerializeField] private GameObject flightCanvas;
    [SerializeField] private Slider rotateSlider;
    [SerializeField] private Slider horizontalSlider;
    [SerializeField] private Slider verticalSlider;
    [SerializeField] private float[] droneMove = new float[3];
    [SerializeField] private float[] backMove = new float[3];
    Vector3 droneResetPosition;
    Vector3 MainCameraResetPosition;
    Quaternion MainCameraResetRotation;

    //others
    bool move = false;
    bool sensors = false;

    [HideInInspector] public string A1_Dozer_GPS;
    [HideInInspector] public string A2_Load_GPS;
    [HideInInspector] public string A3_Truck_GPS;
    [HideInInspector] public string A5_Loader_GPS;
    [HideInInspector] public string A5_Dumptruck_GPS;
    [HideInInspector] public string A4_worker_GPS;
    [HideInInspector] public string A6_Wood_RFID;
    [HideInInspector] public string A6_Log_RFID;
    [HideInInspector] public string A6_Rebar_RFID;
    [HideInInspector] public bool A6_wood_flag;
    [HideInInspector] public bool A6_log_flag;
    [HideInInspector] public bool A6_rebar_flag;
    [HideInInspector] public string A3RFID;
    [HideInInspector] public bool A14_laborer;
    [HideInInspector] public bool A14_painter;
    [HideInInspector] public bool A14_c1;
    [HideInInspector] public bool A14_c2;
    [HideInInspector] public bool A4_worker_GPSRFID;
    [HideInInspector] public bool A7_worker_GPSRFID;
    [HideInInspector] public string A14_l_report;
    [HideInInspector] public string A14_p_report;
    [HideInInspector] public string A14_c1_report;
    [HideInInspector] public string A14_c2_report;
    private bool A4_worker_GPSdisplay;
    [HideInInspector] public bool A7_w1_flag;
    [HideInInspector] public bool A7_w2_flag;
    [HideInInspector] public bool A7_w3_flag;
    [HideInInspector] public string A7_w1_GPS;
    [HideInInspector] public string A7_w2_GPS;
    [HideInInspector] public string A7_w3_GPS;
    [HideInInspector] public string A14_P_GPS;
    [HideInInspector] public string A14_L_GPS;
    [HideInInspector] public string A14_C_GPS;
    [HideInInspector] public bool A7_w1_GPSdisplay;
    [HideInInspector] public bool A7_w2_GPSdisplay;
    [HideInInspector] public bool A7_w3_GPSdisplay;

    public GameObject LSConstraint;
    public GameObject AutoUI;
    public GameObject ManualUI;

    [HideInInspector] public bool ConcurencySuspension = false;

    public GameObject A5BackhoeRFID;
    public GameObject A5TruckRFID;
    public GameObject A1DozerRFID;
    public GameObject A2CraneRFID;
    public GameObject A3TruckRFID;

    public GameObject IMUPainterWorker;
    public GameObject IMULaborerWorker;
    public GameObject IMUCarpenterWorker;
    public GameObject IMUPainterWorkerTooltip;
    public GameObject IMULaborerWorkerTooltip;
    public GameObject IMUCarpenterWorkerTooltip;

    public GameObject LSAreset;
    public GameObject LSMreset;

    public GameObject LStarget1;
    public GameObject LStarget2;
    public GameObject LStarget3;
    public GameObject LStripod;
    public GameObject BubbleLeveler;
    #endregion

    #region main menu functionalities
    void Start()
    {
        MainCameraResetPosition = mainCamera.transform.position;
        MainCameraResetRotation = mainCamera.transform.rotation;
        droneResetPosition = drone.transform.position;

        ManualDroneBackButton.SetActive(false);
        mainMenu.SetActive(false);
        activityMenu.SetActive(false);
        Activity4Worker.transform.Find("Canvas").gameObject.SetActive(false);
        //Activity4Worker_GPSTMP.SetActive(false);

        A7Worker1.transform.Find("Canvas").gameObject.SetActive(false);
        //A7w1_GPSTMP.SetActive(false);

        A7Worker2.transform.Find("Canvas").gameObject.SetActive(false);
        //A7w2_GPSTMP.SetActive(false);

        A7Worker3.transform.Find("Canvas").gameObject.SetActive(false);
        //A7w3_GPSTMP.SetActive(false);

        //Addition vehicle RFID
        A1DozerRFID.SetActive(false);
        A2CraneRFID.SetActive(false);
        A3TruckRFID.SetActive(false);
        A5TruckRFID.SetActive(false);
        A5BackhoeRFID.SetActive(false);

        LStarget1.GetComponent<BoundsControl>().gameObject.SetActive(false);
        LStarget2.GetComponent<BoundsControl>().gameObject.SetActive(false);
        LStarget3.GetComponent<BoundsControl>().gameObject.SetActive(false);
        LStripod.GetComponent<BoundsControl>().gameObject.SetActive(false);

    }

    private void LSboxon()
    {
        LStarget1.GetComponent<BoundsControl>().gameObject.SetActive(true);
        LStarget2.GetComponent<BoundsControl>().gameObject.SetActive(true);
        LStarget3.GetComponent<BoundsControl>().gameObject.SetActive(true);
        LStripod.GetComponent<BoundsControl>().gameObject.SetActive(true);
    }
    private void Update()
    {
        A1_Dozer_GPS = Activity1Bulldozer.GetComponent<GenericGPS>().GGPSConent;
        A2_Load_GPS = Activity2CraneLoad.GetComponent<GenericGPS>().GGPSConent;
        A3_Truck_GPS = Activity3Truck.GetComponent<GenericGPS>().GGPSConent;
        A4_worker_GPS = Activity4Worker.GetComponent<GenericGPS>().GGPSConent;
        A5_Loader_GPS = Activity5Loader.GetComponent<GenericGPS>().GGPSConent;
        A5_Dumptruck_GPS = Activity5dumptruck.GetComponent<GenericGPS>().GGPSConent;
        A3RFID = Activity3Truck.GetComponent<Activity3Truck>().A3RFID;
        A6_Wood_RFID = Activity6.GetComponent<A6_RFID>().wood;
        A6_Log_RFID = Activity6.GetComponent<A6_RFID>().log;
        A6_Rebar_RFID = Activity6.GetComponent<A6_RFID>().rebar;
        A14_c1_report = Activity14.GetComponent<A14>().c1_string;
        A14_c2_report = Activity14.GetComponent<A14>().c2_string;
        A14_l_report = Activity14.GetComponent<A14>().l_string;
        A14_p_report = Activity14.GetComponent<A14>().p_string;
        //if(A4_worker_GPSdisplay) Activity4Worker_GPSTMP.GetComponent<TextMeshProUGUI>().text = A4_worker_GPS;
        A7_w1_GPS = A7Worker1.GetComponent<GenericGPS>().GGPSConent;
        A7_w2_GPS = A7Worker2.GetComponent<GenericGPS>().GGPSConent;
        A7_w3_GPS = A7Worker3.GetComponent<GenericGPS>().GGPSConent;
        //if (A7_w1_GPSdisplay) { A7w1_GPSTMP.GetComponent<TextMeshProUGUI>().text = A7_w1_GPS; }
        //if (A7_w2_GPSdisplay) { A7w2_GPSTMP.GetComponent<TextMeshProUGUI>().text = A7_w2_GPS; }
        //if (A7_w3_GPSdisplay) { A7w3_GPSTMP.GetComponent<TextMeshProUGUI>().text = A7_w3_GPS; }
        A14_P_GPS = IMUPainterWorker.GetComponent<GenericGPS>().GGPSConent;
        A14_L_GPS = IMULaborerWorker.GetComponent<GenericGPS>().GGPSConent;
        A14_C_GPS = IMUCarpenterWorker.GetComponent<GenericGPS>().GGPSConent;

    }

    //Functions to reset and quit scene.
    public void resetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    // Entrance interface, select sensors, which is default menu for V1.0
    public void SensorsEnabled()
    {
        activityMenu.SetActive(false); //should clear activity menu if possible double selection
        mainMenu.SetActive(true);

    }

    // Entrance interface, select activity menu
    public void ActivityEnabled()
    {
        mainMenu.SetActive(false); //should clear main menu if possible double selection
        activityMenu.SetActive(true);
    }

    //this has been re-written as:
    //Activity1Bulldozer.transform.Find("Arrow").gameObject.SetActive(false);
    private void switchTag(GameObject Tag)
    {
        if (Tag.transform.GetChild(0).gameObject.activeSelf)
            Tag.transform.GetChild(0).gameObject.SetActive(false);
        else
            Tag.transform.GetChild(0).gameObject.SetActive(true);
    }


    //stop all activities
    public void stopALL()
    {
        stop_7();//Stop activity 7
        stop_3();
        stop_4();
        stop_1();
        stop_2();
        stop_5();
        stop_10();
        stop_11_a();
        stop_12_a();
    }
    #endregion

    #region Activity button actions
    //Activity Functions
    public void select_1()
    {
        Activity1Bulldozer.transform.Find("Arrow").gameObject.SetActive(true);
        Activity1Bulldozer.GetComponent<BullldozerActivity1>().start();
    }

    public void TutorialGPS()
    {
        //Activity1Bulldozer.transform.Find("Arrow").gameObject.SetActive(true);
        Activity1Bulldozer.GetComponent<BullldozerActivity1>().start();
        Activity1Bulldozer.GetComponent<GenericGPS>().start();
    }
    public void select_1_Dozer_GPS()
    {
        Activity1Bulldozer.GetComponent<GenericGPS>().start();
    }

    public void select_1_Dozer_RFID()
    {
        A1DozerRFID.SetActive(true);
    }

    public void stop_1()
    {
        Activity1Bulldozer.transform.Find("Arrow").gameObject.SetActive(false);
        Activity1Bulldozer.GetComponent<BullldozerActivity1>().stop();
    }

    public void select_2()
    {
        Activity2Crane.GetComponent<Crane>().start();
    }

    public void select_2_crane_RFID()
    {
        A2CraneRFID.SetActive(true);
    }

    public void select_2_CraneLoad_GPS()
    {
        Activity2CraneLoad.GetComponent<GenericGPS>().start();
    }


    public void stop_2()
    {
        Activity2Crane.GetComponent<Crane>().stop();
    }

    public void select_3()
    {
        Activity3Truck.GetComponent<Activity3Truck>().start();
    }

    public void select_3_truck_RFID()
    {
        A3TruckRFID.SetActive(true);
    }

    public void select_3_truck_GPS()
    {
        Activity3Truck.GetComponent<GenericGPS>().start();
    }
    public void stop_3()
    {
        Activity3Truck.GetComponent<Activity3Truck>().stop_3();
    }

    public void select_4()
    {
        Activity4Worker.GetComponent<workerMove>().start();
        Activity4Worker.transform.Find("Arrow").gameObject.SetActive(true);
    }

    public void select_4worker_gps()
    {
        Activity4Worker.GetComponent<GenericGPS>().start();
    }

    public void select_4worker_RFID()
    {
        Activity4Worker.transform.Find("Canvas").gameObject.SetActive(true);
        if (A4_worker_GPSRFID)
        {
            //activate GPS report function
            //Activity4Worker_GPSTMP.SetActive(true);
            //A4_worker_GPSdisplay = true;

        }
    }


    public void stop_4()
    {
        Activity4Worker.GetComponent<workerMove>().stop();
        Activity4Worker.transform.Find("Arrow").gameObject.SetActive(false);
    }

    public void select_5()
    {
        Activity5.GetComponent<Activity5>().start();
    }

    public void select_5_Loader_GPS()
    {
        Activity5Loader.GetComponent<GenericGPS>().start();
    }

    public void select_5_dumptruck_GPS()
    {
        Activity5dumptruck.GetComponent<GenericGPS>().start();
    }

    public void select_5_dumptruck_RFID()
    {
        A5TruckRFID.SetActive(true);
    }
    public void select_5_backhoe_RFID()
    {
        A5BackhoeRFID.SetActive(true);
    }

    public void stop_5()
    {
        Activity5.GetComponent<Activity5>().stop_5();
    }

    //Old canvas style method
    public void select_6()
    {
        Activity6_ResourcesCanvas.SetActive(true);
    }

    public void A6_RFID()
    {
        if (A6_wood_flag) Activity6.GetComponent<A6_RFID>().WoodFlag = true;
        if (A6_log_flag) Activity6.GetComponent<A6_RFID>().LogFlag = true;
        if (A6_rebar_flag) Activity6.GetComponent<A6_RFID>().RebarFlag = true;

        Activity6.GetComponent<A6_RFID>().start();
    }

    public void select_7()
    {
        A7Worker1.GetComponent<workerMove>().start();
        A7Worker2.GetComponent<workerMove>().start();
        A7Worker3.GetComponent<workerMove>().start();
        Activity7Arrow.transform.Find("Arrow").gameObject.SetActive(true);
    }

    public void select_7_new()
    {
        if (A7_w1_flag) A7Worker1.GetComponent<workerMove>().start();
        if (A7_w2_flag) A7Worker2.GetComponent<workerMove>().start();
        if (A7_w3_flag) A7Worker3.GetComponent<workerMove>().start();
    }

    public void select_7_GPS()
    {
        if (A7_w1_flag) A7Worker1.GetComponent<GenericGPS>().start();
        if (A7_w2_flag) A7Worker2.GetComponent<GenericGPS>().start();
        if (A7_w3_flag) A7Worker3.GetComponent<GenericGPS>().start();
    }

    public void select_7_RFID()
    {
        if (A7_w1_flag) A7Worker1.transform.Find("Canvas").gameObject.SetActive(true);
        if (A7_w2_flag) A7Worker2.transform.Find("Canvas").gameObject.SetActive(true);
        if (A7_w3_flag) A7Worker3.transform.Find("Canvas").gameObject.SetActive(true);

        //GPS with RFID together, activate GPS TMP.
        if (A7_worker_GPSRFID)
        {
            //activate GPS report function
            //if (A7_w1_flag) { A7w1_GPSTMP.SetActive(true);A7_w1_GPSdisplay = true; }
            //if (A7_w2_flag) { A7w2_GPSTMP.SetActive(true); A7_w2_GPSdisplay = true; }
            //if (A7_w3_flag) { A7w3_GPSTMP.SetActive(true); A7_w3_GPSdisplay = true; }
        }
    }

    public void stop_7()
    {
        A7Worker1.GetComponent<workerMove>().stop();
        A7Worker2.GetComponent<workerMove>().stop();
        A7Worker3.GetComponent<workerMove>().stop();
        Activity7Arrow.transform.Find("Arrow").gameObject.SetActive(false);
    }

    public void select_8()
    {
        //switchTag(Activity8Arrow);
        //Activity8.transform.Find("Arrow").gameObject.SetActive(true);
        //sensorSelected();
        AutoUI.SetActive(false);
        LSboxon();
        scannerMenu.SetActive(true);
        //Vector3 ScannerPosition= Activity8.transform.position;
        //scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        scannerParentNode.transform.position = Activity8.transform.position;
        //LSConstraint.GetComponent<LSConstraint>().BeginSignal();

        //AutoUI.SetActive(false);
    }

    public void select_9()
    {
        //switchTag(Activity9Arrow);
        //Activity9.transform.Find("Arrow").gameObject.SetActive(true);
        //sensorSelected();
        AutoUI.SetActive(false);
        LSboxon();
        Vector3 ScannerPosition = Activity9.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        LSConstraint.GetComponent<LSConstraint>().BeginSignal();
        scannerMenu.SetActive(true);
    }

    //A9 scan floor, only move
    public Vector3 Explore_A9_MoveLSOnly()
    {
        Vector3 ScannerPosition = Activity9.transform.position;
        //DummyLSParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        return ScannerPosition;
    }

    public void select10()
    {
        //choose between 11A and 11B
        Activity11Canvas.SetActive(true);

    }

    //A10 scan stockpile, only move
    public Vector3 Explore_A10_MoveLSOnly()
    {
        Vector3 ScannerPosition = Activity11A.transform.position;
        //DummyLSParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        return ScannerPosition;
    }

    //laser scan stockpile 1
    public void select_10A()
    {
        //switchTag(Activity11AArrow);
        //Activity11A.transform.Find("Arrow").gameObject.SetActive(true);
        LSboxon();
        AutoUI.SetActive(false);
        Activity11Canvas.SetActive(false);
        //sensorSelected();
        Vector3 ScannerPosition = Activity11A.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        LSConstraint.GetComponent<LSConstraint>().BeginSignal();
        scannerMenu.SetActive(true);
    }

    //laser scan stockpile 2
    public void select_10B()
    {
        //switchTag(Activity11BArrow);
        //Activity11B.transform.Find("Arrow").gameObject.SetActive(true);
        LSboxon();
        AutoUI.SetActive(false);
        Activity11Canvas.SetActive(false);
        //sensorSelected();
        Vector3 ScannerPosition = Activity11B.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        LSConstraint.GetComponent<LSConstraint>().BeginSignal();
        scannerMenu.SetActive(true);
    }

    public void stop_10()
    {
        //sensorSelected();
        Activity11A.transform.Find("Arrow").gameObject.SetActive(false);
        Activity11B.transform.Find("Arrow").gameObject.SetActive(false);
    }


    public void select_11()
    {
        Activity12Canvas.SetActive(true);// choose between laser scan and drone scan.
        Activity13_DroneCanvas.SetActive(false);
    }

    public void select_11Laser()
    {
        //switchTag(Activity12Laser); //Activate laser position arrow
        //Activity12Canvas.SetActive(false);
        //sensorSelected();
        //mainCamera.transform.position = Activity12_MDroneCameraLocator.transform.position;//4, move camera.
        LSboxon();
        AutoUI.SetActive(false);
        Vector3 ScannerPosition = Activity12Laser.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        LSConstraint.GetComponent<LSConstraint>().BeginSignal();
        scannerMenu.SetActive(true);
    }

    public void select_11Drone()
    {
        //switchTag(Activity12Drone);
        //Activity12Canvas.SetActive(false);
        //sensorSelected();
        //mainCamera.transform.position = Activity12_MDroneCameraLocator.transform.position;//4, move camera.
        AutoUI.SetActive(false);
        Activity12_DroneCanvas.SetActive(true);
        //Start the drone and automatically fly around building
    }

    public void stop_11_a()
    {
        Activity12Drone.GetComponent<Drone12>().stop();
        Activity12_DroneCanvas.SetActive(false);
    }

    public void select_12()
    {
        //drone around jobsite
        //switchTag(Activity13Drone);
        //sensorSelected();
        AutoUI.SetActive(false);
        Activity13_DroneCanvas.SetActive(true);
        //Activity12Canvas.SetActive(false);
        Activity12_DroneCanvas.SetActive(false);
        //need to get rid of all canvas
    }

    //public void select_12Drone() //for manual drone purpose, close canvas for auto drone.
    //{
    //sensorSelected();
    //    Activity13_DroneCanvas.SetActive(false);
    //need to get rid of all canvas
    // }

    public void stop_12_a()
    {
        Activity13Drone.GetComponent<Drone13>().stop();
    }

    public void select_13()
    {
        //sensorSelected();
        //imuMenu.SetActive(true);
        //Debug.Log("14 SELECTED");
        //Activity14Canvas.SetActive(true);
    }

    public void select_13_new()
    {
        //initialize
        //sensorSelected();
        Activity14.GetComponent<A14>().Start();
        //select worker
        if (A14_painter) Activity14.GetComponent<A14>().painterSelect();
        if (A14_laborer) Activity14.GetComponent<A14>().laborerSelect();
        if (A14_c1) Activity14.GetComponent<A14>().carpenter1Select();
        if (A14_c2) Activity14.GetComponent<A14>().carpenter2Select();

        //execute IMU function
        Activity14.GetComponent<A14>().done();
    }

    public void select_Painter_GPS()
    {
        IMUPainterWorker.GetComponent<GenericGPS>().start();
    }

    public void select_Laborer_GPS()
    {
        IMULaborerWorker.GetComponent<GenericGPS>().start();
    }
    public void select_Carpenter_GPS()
    {
        IMUCarpenterWorker.GetComponent<GenericGPS>().start();
    }

    //Just show the tooltips
    public void select_Painter_RFID()
    {
        IMUPainterWorkerTooltip.SetActive(true);
    }
    public void select_Laborer_RFID()
    {
        IMULaborerWorkerTooltip.SetActive(true);
    }
    public void select_Carpenter_RFID()
    {
        IMUCarpenterWorkerTooltip.SetActive(true);
    }

    public void TutorialIMU()
    {
        Activity14.GetComponent<A14>().Start();
        Activity14.GetComponent<A14>().carpenter1Select();
        Activity14.GetComponent<A14>().done();
    }

    public void A13_stop()
    {
        Activity14.GetComponent<A14>().backSelected();
    }
    #endregion

    #region Sensor Related Functions


    //Legacy method to use manually controlled drone
    public void droneSelected()
    {
        //sensorSelected();
        ManualDroneBackButton.SetActive(true);
        drone.SetActive(true);
        droneCanvas.SetActive(true);
        //Vector3 newPosition = droneCanvas.transform.position;
        //mainCamera.transform.position = newPosition + new Vector3(droneMove[0], droneMove[1], droneMove[2]);
        GetComponent<Canvas>().enabled = false;
    }

    //For activity 12: Job site inspection. 
    //Mods include: Skip task, Disable camera movement.
    public void ManualDrone12JobSite()
    {
        //sensorSelected();
        Activity13_DroneCanvas.SetActive(false);
        drone.SetActive(true);
        //mainCamera.transform.position = newPosition + new Vector3(droneMove[0], droneMove[1], droneMove[2]);
        //GetComponent<Canvas>().enabled = false;
        //ManualDroneBackButton.SetActive(true);
        droneCanvas.SetActive(true);//here should be drone canvas instead of task canvas
        drone.transform.Find("Arrow").gameObject.SetActive(true);

        //MDroneParentCanvas.transform.position = Activity12_MDroneCameraLocator.transform.position;
        //drone.transform.position = Activity12_DroneLocator.transform.position;
        //drone.GetComponent<droneScript>().start();
    }

    //For activity 11: old house. 
    //Jump drone location and camera location to old house.
    public void ManualDrone11OldHouse()
    {
        //sensorSelected(); //initialization
        Activity12_DroneCanvas.SetActive(false);
        drone.SetActive(true);//2, activate drone.
        drone.transform.position = Activity12_DroneLocator.transform.position; //3, move drone to locator position.
        //mainCamera.transform.position = Activity12_MDroneCameraLocator.transform.position;//4, move camera.
        //GetComponent<Canvas>().enabled = false; //1, Disable main menu canvas.
        //ManualDroneBackButton.SetActive(true);//Backbutton active
        droneCanvas.SetActive(true);//Drone canvas active

        //drone canvas move location and rotate
        //droneCanvas.transform.eulerAngles = new Vector3(droneCanvas.transform.eulerAngles.x, droneCanvas.transform.eulerAngles.y + 50, droneCanvas.transform.eulerAngles.z);
        //move flight canvas as well
        //MDroneFlightCanvas.transform.eulerAngles = new Vector3(MDroneFlightCanvas.transform.eulerAngles.x, MDroneFlightCanvas.transform.eulerAngles.y + 50, MDroneFlightCanvas.transform.eulerAngles.z); ;

        //Move Drone Parent Node
        MDroneParentCanvas.transform.position = Activity12_MDroneCameraLocator.transform.position;
        //MDroneParentCanvas.transform.eulerAngles = new Vector3(MDroneParentCanvas.transform.eulerAngles.x, MDroneParentCanvas.transform.eulerAngles.y + 50, MDroneParentCanvas.transform.eulerAngles.z);

        drone.transform.Find("Arrow").gameObject.SetActive(true);
        //drone.GetComponent<droneScript>().start();
    }

    public void TutorialDrone()
    {
        Debug.Log("Tutorial drone executed");
        drone.SetActive(true);//activate drone.
        //drone.transform.position = Activity12_DroneLocator.transform.position; //3, move drone to locator position.
        ManualDroneBackButton.SetActive(true);//Backbutton active
        droneCanvas.SetActive(true);//Drone canvas active
        //Move Drone Parent Node
        //MDroneParentCanvas.transform.position = Activity12_MDroneCameraLocator.transform.position;
        //MDroneParentCanvas.transform.eulerAngles = new Vector3(MDroneParentCanvas.transform.eulerAngles.x, MDroneParentCanvas.transform.eulerAngles.y + 50, MDroneParentCanvas.transform.eulerAngles.z);
        //drone.transform.Find("Arrow").gameObject.SetActive(true);
    }

    public void TutorialLS()
    {
        //LSboxon();
        //LSConstraint.GetComponent<LSConstraint>().BeginSignal();
        scannerMenu.SetActive(true);     
    }
    public void GenericBackButton() //currently for all Back menus.
    {
        resetMainCam();
        LaserScannerBackButton.SetActive(false);
        ManualDroneBackButton.SetActive(false);
        GetComponent<Canvas>().enabled = true;
        gameObject.SetActive(true);
        resetScanner();
        resetDrone();
        Activity12_DroneCanvas.SetActive(false);
        Activity13_DroneCanvas.SetActive(false);
        SensorReset();
        AutoUI.SetActive(true);
        ManualUI.SetActive(false);
        LSResetButton.SetActive(false);
        ConcurencySuspension = true;
        LaserScannerBackButton.SetActive(false);
    }

    public void ManualDroneReset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);
    }
    public void AutoDroneReset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }

    public void ManualDroneManualModeBackButton()
    {
        resetMainCam();
        LaserScannerBackButton.SetActive(false);
        ManualDroneBackButton.SetActive(false);
        GetComponent<Canvas>().enabled = true;
        gameObject.SetActive(true);
        resetScanner();
        resetDrone();
        Activity12_DroneCanvas.SetActive(false);
        Activity13_DroneCanvas.SetActive(false);
        SensorReset();
        AutoUI.SetActive(false);
        ManualUI.SetActive(true);
        LSResetButton.SetActive(false);
        ConcurencySuspension = true;
        LaserScannerBackButton.SetActive(false);
    }

    public void LSBackButton()
    {
        resetMainCam();
        LaserScannerBackButton.SetActive(false);
        GetComponent<Canvas>().enabled = true;
        gameObject.SetActive(true);
    }

    public void BubbleLevelerBackButton()
    {
        //resetMainCam();
        BubbleLeveler.SetActive(false);
        GetComponent<Canvas>().enabled = true;//re-show hidden scanner initial canvas.
        gameObject.SetActive(true);
        //re-enable scaner menu
        scannerMenu.SetActive(true);
    }

    public void DroneBackButton()
    {
        resetMainCam();
        ManualDroneBackButton.SetActive(false);
        GetComponent<Canvas>().enabled = true;
        gameObject.SetActive(true);
    }

    private void resetScanner()
    {
        scannerCanvas.SetActive(false);
        scannerCanvas.GetComponent<scanScript>().resolution = 0;
        scannerCanvas.GetComponent<scanScript>().quality = 0;
        scannerCanvas.GetComponent<scanScript>().color = 0;
        scannerCanvas.GetComponent<scanScript>().profile = 0;
        scannerCanvas.GetComponent<scanScript>().coverage = false;
        scanner.GetComponent<Animator>().SetBool("spin", false);
        scannerMenu.SetActive(false);
        LSResetButton.SetActive(false);
    }
    private void resetDrone()
    {
        drone.SetActive(false);
        drone.transform.Find("Arrow").gameObject.SetActive(false);//For activity 12 back button deactive drone arrow.
        //droneCanvas.SetActive(true);
        drone.GetComponent<droneScript>().taskSelected = false;
        drone.GetComponent<droneScript>().power = false;
        drone.GetComponent<droneScript>().motor = false;
        drone.transform.position = droneResetPosition;
        drone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        flightCanvas.SetActive(false);
        droneCanvas.SetActive(false);
        rotateSlider.value = 0;
        horizontalSlider.value = 0;
        verticalSlider.value = 0;
    }
    public void resetMainCam() //currently for all Back menus.
    {
        //reset camera location to 0,0,0
        mainCamera.transform.position = MainCameraResetPosition;
        mainCamera.transform.rotation = MainCameraResetRotation;
    }
    //Function to select worker
    public void workerFunction()
    {
        if (move)
        {
            worker.GetComponent<workerMove>().start();
        }

        if (sensors)
        {
            switchTag(worker);
            //worker.GetComponent<workerMove>().switchTag();
        }
    }

    /*
    public void sensorSelected()
    {
        resetDrone();
        resetScanner();
        resourceMenu.GetComponent<gpsScript>().back();
        rfidMenu.GetComponent<rfidScript>().back();
        imuMenu.GetComponent<imuScript>().backSelected();
        scanner.GetComponent<Animator>().SetBool("spin", false);
        scannerMenu.SetActive(false);
        //resourceMenu.SetActive(false);//GPS report, can disable.
        //imuMenu.SetActive(false);//can discart
        //rfidMenu.SetActive(false);//can discart
    }
    */

    public void SensorReset()
    {
        resetDrone();
        resetScanner();
        resetMainCam();
        AutoUI.SetActive(true);
        ConcurencySuspension = true;
    }

    public void ResetForLS()
    {
        resetScanner();
        resetMainCam();
        AutoUI.SetActive(true);
        ConcurencySuspension = true;
    }

    public void ResetForLSManualMode()
    {
        resetScanner();
        resetMainCam();
        AutoUI.SetActive(false);
        ManualUI.SetActive(true);
        ConcurencySuspension = true;
    }

    public void Auto_LS_Reset()
    {
        resetScanner();
        resetMainCam();
        AutoUI.SetActive(true);
        ManualUI.SetActive(false);
        LSAreset.SetActive(false);
    }

    public void Manual_LS_Reset()
    {
        resetScanner();
        resetMainCam();
        AutoUI.SetActive(false);
        ManualUI.SetActive(true);
        LSMreset.SetActive(false);
    }
    #endregion


}
