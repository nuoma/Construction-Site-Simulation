using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuScript : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private GameObject worker;
    [SerializeField] private GameObject tripod;
    [SerializeField] private GameObject scanner;
    [SerializeField] private GameObject scannerParentNode;
    [SerializeField] private GameObject scannerCanvas;
    [SerializeField] private GameObject drone;

    //activities main actuator
    [SerializeField] private GameObject Activity1Bulldozer;
    [SerializeField] private GameObject Activity4Worker;
    [SerializeField] private GameObject Activity2Crane;
    [SerializeField] private GameObject Activity3Truck;
    [SerializeField] private GameObject Activity7Arrow;
    [SerializeField] private GameObject Activity8Arrow;
    [SerializeField] private GameObject Activity9Arrow;
    [SerializeField] private GameObject Activity11AArrow;
    [SerializeField] private GameObject Activity11BArrow;
    [SerializeField] private GameObject Activity12Laser;
    [SerializeField] private GameObject Activity12Drone;
    [SerializeField] private GameObject Activity13Drone;
    [SerializeField] private GameObject Activity6_ResourcesCanvas;

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
    [SerializeField] private GameObject menuBackButton2;
    [SerializeField] private GameObject A7Worker1;
    [SerializeField] private GameObject A7Worker2;
    [SerializeField] private GameObject A7Worker3;
    [SerializeField] private Canvas flightCanvas;
    [SerializeField] private Slider rotateSlider;
    [SerializeField] private Slider horizontalSlider;
    [SerializeField] private Slider verticalSlider;

    [SerializeField] private float[] droneMove = new float[3];
    [SerializeField] private float[] backMove = new float[3];

    bool move = false;
    bool sensors = false;
    Vector3 droneResetPosition;

    void Start()
    {
        menuBackButton2.SetActive(false);
        droneResetPosition = drone.transform.position;
        mainMenu.SetActive(false);
        activityMenu.SetActive(false);
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
           //switchTag(Activity7Arrow);
           Activity7Arrow.transform.Find("Arrow").gameObject.SetActive(true);
    }

    public void stop_7()
    {
        A7Worker1.GetComponent<workerMove>().stop();
        A7Worker2.GetComponent<workerMove>().stop();
        A7Worker3.GetComponent<workerMove>().stop();
        //switchTag(Activity7Arrow);
        Activity7Arrow.transform.Find("Arrow").gameObject.SetActive(false);
    }

    public void select_8()
    {
        switchTag(Activity8Arrow);
        sensorSelected();
        Vector3 ScannerPosition= Activity8Arrow.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        scannerMenu.SetActive(true);
    }

    public void select_9()
    {
        switchTag(Activity9Arrow);
        sensorSelected();
        Vector3 ScannerPosition = Activity9Arrow.transform.position;
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
    public void select_11A()
    {
        switchTag(Activity11AArrow);
        Activity11Canvas.SetActive(false);
        sensorSelected();
        Vector3 ScannerPosition = Activity11AArrow.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        scannerMenu.SetActive(true);
    }

    public void select_11B()
    {
        switchTag(Activity11BArrow);
        Activity11Canvas.SetActive(false);
        sensorSelected();
        Vector3 ScannerPosition = Activity11BArrow.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        scannerMenu.SetActive(true);
    }

    public void select_12()
    {
        Activity12Canvas.SetActive(true);// choose between laser scan and drone scan.
    }

    public void select_12Laser()
    {
        switchTag(Activity12Laser); //Activate laser position arrow
        Activity12Canvas.SetActive(false);
        sensorSelected();
        Vector3 ScannerPosition = Activity12Laser.transform.position;
        scannerParentNode.transform.position = ScannerPosition;//new Vector3(droneMove[0], droneMove[1], droneMove[2]); ;// change scanner parent node position to building 2.
        scannerMenu.SetActive(true);
    }

    public void select_12Drone()
    {
        switchTag(Activity12Drone);
        Activity12Canvas.SetActive(false);
        sensorSelected();
        Activity12_DroneCanvas.SetActive(true);
        //Start the drone and automatically fly around building
    }

    public void select_13()
    {
        //drone around jobsite
        //switchTag(Activity13Drone);
        sensorSelected();
        Activity13_DroneCanvas.SetActive(true);
        //need to get rid of all canvas
    }

    public void select_13Drone()
    {
        sensorSelected();
        Activity13_DroneCanvas.SetActive(false);
        //need to get rid of all canvas
    }


    public void select_14()
    {
        sensorSelected();
        imuMenu.SetActive(true);
    }

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
    public void droneSelected()
    {
        sensorSelected();
        menuBackButton2.SetActive(true);
        drone.SetActive(true);
        droneCanvas.SetActive(true);
        //Vector3 newPosition = droneCanvas.transform.position;
        //mainCamera.transform.position = newPosition + new Vector3(droneMove[0], droneMove[1], droneMove[2]);
        GetComponent<Canvas>().enabled = false;
    }

    //modified for task12, skip task, disable camera movement
    public void droneMSelected()
    {
        sensorSelected();
        menuBackButton2.SetActive(true);
        drone.SetActive(true);
        droneCanvas.SetActive(true);//here should be drone canvas instead of task canvas
        //Vector3 newPosition = droneCanvas.transform.position;
        //mainCamera.transform.position = newPosition + new Vector3(droneMove[0], droneMove[1], droneMove[2]);
        GetComponent<Canvas>().enabled = false;
        drone.GetComponent<droneScript>().Start();
    }

    public void backMenu()
    {
        Vector3 newPosition = this.transform.position;
        mainCamera.transform.position = newPosition + new Vector3(backMove[0], backMove[1], backMove[2]);
        menuBackButton.SetActive(false);
        menuBackButton2.SetActive(false);
        GetComponent<Canvas>().enabled = true;
        resetScanner();
        resetDrone();
    }
    private void resetScanner()
    {
        scannerCanvas.GetComponent<Canvas>().enabled = false;
        scannerCanvas.GetComponent<scanScript>().resolution = 0;
        scannerCanvas.GetComponent<scanScript>().quality = 0;
        scannerCanvas.GetComponent<scanScript>().color = 0;
        scannerCanvas.GetComponent<scanScript>().profile = 0;
        scannerCanvas.GetComponent<scanScript>().coverage = false;
    }
    private void resetDrone()
    {
        drone.SetActive(false);
        droneCanvas.SetActive(true);
        drone.GetComponent<droneScript>().taskSelected = false;
        drone.GetComponent<droneScript>().power = false;
        drone.GetComponent<droneScript>().motor = false;
        drone.transform.position = droneResetPosition;
        drone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        flightCanvas.enabled = false;
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

    private void switchTag(GameObject Tag)
    {
        if (Tag.transform.GetChild(0).gameObject.activeSelf)
            Tag.transform.GetChild(0).gameObject.SetActive(false);
        else
            Tag.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void stopALL()
    {
        stop_7();//Stop activity 7
        stop_3();
        stop_4();
        stop_1();
        stop_2();
    }

}
