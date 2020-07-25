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
    [SerializeField] private GameObject scannerCanvas;
    [SerializeField] private GameObject drone;
    [SerializeField] private GameObject Activity1Bulldozer;

    [SerializeField] private GameObject mainMenu; //added for main menu
    [SerializeField] private GameObject activityMenu; //added for activity menu

    [SerializeField] private GameObject scannerMenu;
    [SerializeField] private GameObject resourceMenu;
    [SerializeField] private GameObject tripodMenu;
    [SerializeField] private GameObject imuMenu;
    [SerializeField] private GameObject rfidMenu;
    [SerializeField] private Canvas droneCanvas;
    [SerializeField] private GameObject menuBackButton;
    [SerializeField] private GameObject menuBackButton2;
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
        Activity1Bulldozer.GetComponent<BullldozerActivity1>().start();
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
        droneCanvas.enabled = true;
        Vector3 newPosition = droneCanvas.transform.position;
        mainCamera.transform.position = newPosition + new Vector3(droneMove[0], droneMove[1], droneMove[2]);
        GetComponent<Canvas>().enabled = false;
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
        droneCanvas.enabled = false;
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
                //switchTag(worker);
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

}
