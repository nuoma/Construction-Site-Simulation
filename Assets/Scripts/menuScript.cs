using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuScript : MonoBehaviour
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
    [SerializeField] private GameObject Activity3Truck;
    [SerializeField] private GameObject Activity4Worker;
    [SerializeField] private GameObject Activity5;
    [SerializeField] private GameObject Activity6_ResourcesCanvas;
    [SerializeField] private GameObject Activity7Arrow;
    [SerializeField] private GameObject A7Worker1;
    [SerializeField] private GameObject A7Worker2;
    [SerializeField] private GameObject A7Worker3;
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



    //menus and canvas
    [SerializeField] private GameObject mainMenu; //added for main menu
    [SerializeField] private GameObject activityMenu; //added for activity menu
    [SerializeField] private GameObject scannerMenu;
    [SerializeField] private GameObject resourceMenu;
    [SerializeField] private GameObject tripodMenu;
    [SerializeField] private GameObject imuMenu;
    [SerializeField] private GameObject rfidMenu;
    [SerializeField] private GameObject droneCanvas;
    [SerializeField] private GameObject Activity11Canvas;
    [SerializeField] private GameObject Activity12Canvas;
    [SerializeField] private GameObject Activity12_DroneCanvas;
    [SerializeField] private GameObject Activity13_DroneCanvas;
    [SerializeField] private GameObject menuBackButton;
    [SerializeField] private GameObject ManualDroneBackButton;
    [SerializeField] private GameObject flightCanvas;
    [SerializeField] private Slider rotateSlider;
    [SerializeField] private Slider horizontalSlider;
    [SerializeField] private Slider verticalSlider;
    [SerializeField] private float[] droneMove = new float[3];
    [SerializeField] private float[] backMove = new float[3];
    Vector3 droneResetPosition;
    Vector3 MainCameraResetPosition;

    //others
    bool move = false;
    bool sensors = false;


    #endregion

    #region main menu functionalities
    void Start()
    {
        ManualDroneBackButton.SetActive(false);
        droneResetPosition = drone.transform.position;
        mainMenu.SetActive(false);
        activityMenu.SetActive(false);
        MainCameraResetPosition = mainCamera.transform.position;
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
        stop_11();
        stop_12_a();
        stop_13_a();
    }
#endregion

    #region Activity button actions
    //Activity Functions
    public void select_1()
    {
        Activity1Bulldozer.transform.Find("Arrow").gameObject.SetActive(true);
        Activity1Bulldozer.GetComponent<BullldozerActivity1>().start();
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

    public void stop_2()
    {
        Activity2Crane.GetComponent<Crane>().stop();
    }

    public void select_3()
    {
        Activity3Truck.GetComponent<Activity3Truck>().start();
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

    public void stop_4()
    {
        Activity4Worker.GetComponent<workerMove>().stop();
        Activity4Worker.transform.Find("Arrow").gameObject.SetActive(false);
    }

    public void select_5()
    {
        Activity5.GetComponent<Activity5>().start();
    }

    public void stop_5()
    {
        Activity5.GetComponent<Activity5>().stop_5();
    }

    public void select_6()
    {
        Activity6_ResourcesCanvas.SetActive(true);
    }

    public void select_7()
    {
           A7Worker1.GetComponent<workerMove>().start();
           A7Worker2.GetComponent<workerMove>().start();
           A7Worker3.GetComponent<workerMove>().start();
           Activity7Arrow.transform.Find("Arrow").gameObject.SetActive(true);
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
        sensorSelected();
        Vector3 ScannerPosition= Activity8.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        scannerMenu.SetActive(true);
    }

    public void select_9()
    {
        //switchTag(Activity9Arrow);
        //Activity9.transform.Find("Arrow").gameObject.SetActive(true);
        sensorSelected();
        Vector3 ScannerPosition = Activity9.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        scannerMenu.SetActive(true);
    }

    public void select_10()
    {
        //is skipped
    }

    public void select11()
    {
        //choose between 11A and 11B
        Activity11Canvas.SetActive(true);

    }

    //laser scan stockpile 1
    public void select_11A()
    {
        //switchTag(Activity11AArrow);
        //Activity11A.transform.Find("Arrow").gameObject.SetActive(true);
        Activity11Canvas.SetActive(false);
        sensorSelected();
        Vector3 ScannerPosition = Activity11A.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        scannerMenu.SetActive(true);
    }

    //laser scan stockpile 2
    public void select_11B()
    {
        //switchTag(Activity11BArrow);
        //Activity11B.transform.Find("Arrow").gameObject.SetActive(true);
        Activity11Canvas.SetActive(false);
        sensorSelected();
        Vector3 ScannerPosition = Activity11B.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        scannerMenu.SetActive(true);
    }

    public void stop_11()
    {
        sensorSelected();
        Activity11A.transform.Find("Arrow").gameObject.SetActive(false);
        Activity11B.transform.Find("Arrow").gameObject.SetActive(false);
    }


    public void select_12()
    {
        Activity12Canvas.SetActive(true);// choose between laser scan and drone scan.
        Activity13_DroneCanvas.SetActive(false);
    }

    public void select_12Laser()
    {
        //switchTag(Activity12Laser); //Activate laser position arrow
        Activity12Canvas.SetActive(false);
        sensorSelected();
        Vector3 ScannerPosition = Activity12Laser.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        scannerMenu.SetActive(true);
    }

    public void select_12Drone()
    {
        //switchTag(Activity12Drone);
        Activity12Canvas.SetActive(false);
        sensorSelected();
        Activity12_DroneCanvas.SetActive(true);
        //Start the drone and automatically fly around building
    }

    public void stop_12_a()
    {
        Activity12Drone.GetComponent<Drone12>().stop();
        Activity12_DroneCanvas.SetActive(false);
    }

    public void select_13()
    {
        //drone around jobsite
        //switchTag(Activity13Drone);
        sensorSelected();
        Activity13_DroneCanvas.SetActive(true);
        Activity12Canvas.SetActive(false);
        Activity12_DroneCanvas.SetActive(false);
        //need to get rid of all canvas
    }

    public void select_13Drone()
    {
        sensorSelected();
        Activity13_DroneCanvas.SetActive(false);
        //need to get rid of all canvas
    }

    public void stop_13_a()
    {
        Activity13Drone.GetComponent<Drone13>().stop();
    }


    public void select_14()
    {
        sensorSelected();
        //imuMenu.SetActive(true);
        Debug.Log("14 SELECTED");
        Activity14Canvas.SetActive(true);
    }
    #endregion

    #region Legacy Sensor Functions
    //Sensor Functions
    public void gpsSelected()
    {
        sensorSelected();
        resourceMenu.SetActive(true);
    }
    public void laserScannerSelected()
    {
        sensorSelected();
        scannerMenu.SetActive(true);
    }
    public void imuSelected()
    {
        sensorSelected();
        imuMenu.SetActive(true);
    }
    public void rfidSelected()
    {
        sensorSelected();
        rfidMenu.SetActive(true);
    }

    //Legacy method to use manually controlled drone
    public void droneSelected()
    {
        sensorSelected();
        ManualDroneBackButton.SetActive(true);
        drone.SetActive(true);
        droneCanvas.SetActive(true);
        //Vector3 newPosition = droneCanvas.transform.position;
        //mainCamera.transform.position = newPosition + new Vector3(droneMove[0], droneMove[1], droneMove[2]);
        GetComponent<Canvas>().enabled = false;
    }

    //modified for activity 13. Skip task, Disable camera movement.
    public void droneMSelected()
    {
        sensorSelected();
        ManualDroneBackButton.SetActive(true);
        drone.SetActive(true);
        droneCanvas.SetActive(true);//here should be drone canvas instead of task canvas
        //Vector3 newPosition = droneCanvas.transform.position;
        //mainCamera.transform.position = newPosition + new Vector3(droneMove[0], droneMove[1], droneMove[2]);
        GetComponent<Canvas>().enabled = false;
        drone.transform.Find("Arrow").gameObject.SetActive(true);
        //flightCanvas.SetActive(true);
        drone.GetComponent<droneScript>().start();
    }

    //For activity 12. Jump drone location and camera location to old house.
    public void ManualDrone12()
    {
        sensorSelected(); //initialization
        GetComponent<Canvas>().enabled = false; //1, Disable main menu canvas.
        drone.SetActive(true);//2, activate drone.
        drone.transform.position = Activity12_DroneLocator.transform.position; //3, move drone to locator position.
        mainCamera.transform.position = Activity12_MDroneCameraLocator.transform.position;//4, move camera.
        ManualDroneBackButton.SetActive(true);//Backbutton active
        droneCanvas.SetActive(true);//Drone canvas active
        drone.transform.Find("Arrow").gameObject.SetActive(true);
        drone.GetComponent<droneScript>().start();
    }

    public void backMenu() //currently for all Back menus.
    {
        mainCamera.transform.position = MainCameraResetPosition;
        menuBackButton.SetActive(false); 
        ManualDroneBackButton.SetActive(false);
        GetComponent<Canvas>().enabled = true;
        resetScanner();
        resetDrone();
        Activity12_DroneCanvas.SetActive(false);
        Activity13_DroneCanvas.SetActive(false);
    }
    private void resetScanner()
    {
        //scannerCanvas.GetComponent<Canvas>().enabled = false;
        scannerCanvas.SetActive(false); 
        scannerCanvas.GetComponent<scanScript>().resolution = 0;
        scannerCanvas.GetComponent<scanScript>().quality = 0;
        scannerCanvas.GetComponent<scanScript>().color = 0;
        scannerCanvas.GetComponent<scanScript>().profile = 0;
        scannerCanvas.GetComponent<scanScript>().coverage = false;
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

    //Function to select worker
    public void workerFunction()
    {
        if(move)
        {
            worker.GetComponent<workerMove>().start();
        }

        if(sensors)
        {
                switchTag(worker);
                //worker.GetComponent<workerMove>().switchTag();
        }
    }

    private void sensorSelected()
    {
        resetDrone();
        resetScanner();
        resourceMenu.GetComponent<gpsScript>().back();
        rfidMenu.GetComponent<rfidScript>().back();
        imuMenu.GetComponent<imuScript>().backSelected();
        scanner.GetComponent<Animator>().SetBool("spin", false);
        scannerMenu.SetActive(false);
        resourceMenu.SetActive(false);
        imuMenu.SetActive(false);
        rfidMenu.SetActive(false);
    }
    #endregion


}
