using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//this function will auto start the engines of the drone, and rotate camera (drone object) around designated object.

public class Drone12 : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private GameObject DroneModel;
    [SerializeField] private GameObject DroneParent;

    private bool startRot;

    public void Start()
    {
        //start drone animation
        DroneModel.GetComponent<Animator>().SetBool("fly", true);
        startRot = false;
    }

    public void SetStart()
    {
        startRot = true;
    }

    public void Update()
    {
        if(startRot)
        {
            DroneParent.transform.Find("Arrow").gameObject.SetActive(true);
            DroneModel.SetActive(true);
            transform.Rotate(0, speed * Time.deltaTime, 0);
        }

    }

        public void stop()
    {
        DroneModel.SetActive(false);
        DroneParent.transform.Find("Arrow").gameObject.SetActive(false);
        startRot = false;
    }
}
/*
public class Drone12 : MonoBehaviour
{
    [SerializeField] private Canvas droneCanvas;
    [SerializeField] private Canvas tasksCanvas;
    [SerializeField] private Canvas flightCanvas;
    [SerializeField] private GameObject flyButton;
    [SerializeField] private Camera droneCamera;
    [SerializeField] private float speed = 1;

    private float zMax = 5.15f;
    private float zMin = -1.178f;
    private float xMax = 4.111f;
    private float xMin = -2.563f;
    private float yMax = 1.25f;
    private float yMin = -0.25f;

    private float verticalMultiplier = 0;
    private float horizontalMultiplier = 0;
    private float rotationMultiplier = 0;

    [HideInInspector] public bool taskSelected = false;
    [HideInInspector] public bool power = false;
    [HideInInspector] public bool motor = false;

    private float speed; 
    private void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0)

    }
    // Start is called before the first frame update
    void Start()
    {
        tasksCanvas.enabled = false;
        droneCanvas.enabled = false;
        flightCanvas.enabled = false;
        droneCamera.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotate = new Vector3(0, rotationMultiplier, 0);
        Vector3 move = new Vector3(0, verticalMultiplier, horizontalMultiplier);
        transform.Rotate(20*rotate*Time.deltaTime);
        transform.Translate(move*speed*Time.deltaTime);

        if (verticalMultiplier != 0 || horizontalMultiplier != 0)
        {
            if (transform.localPosition.y < yMin)
                transform.localPosition = new Vector3(transform.localPosition.x, yMin, transform.localPosition.z);
            else if (transform.localPosition.y > yMax)
                transform.localPosition = new Vector3(transform.localPosition.x, yMax, transform.localPosition.z);
            if (transform.localPosition.x < xMin)
                transform.localPosition = new Vector3(xMin, transform.localPosition.y, transform.localPosition.z);
            else if (transform.localPosition.x > xMax)
                transform.localPosition = new Vector3(xMax, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.z < zMin)
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zMin);
            else if (transform.localPosition.z > zMax)
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zMax);
        }

    }

    //Start Drone
    public void powerOn()
    {
        power = true;
    }
    public void enginesOn()
    {
        if(power)
        {
            GetComponent<Animator>().SetBool("fly", true);
            motor = true;

            ColorBlock colorVar = flyButton.GetComponent<Button>().colors;
            colorVar.highlightedColor = new Color32(138, 255, 114, 255);
            colorVar.pressedColor = new Color32(17, 101, 0, 255);
            flyButton.GetComponent<Button>().colors = colorVar;
        }
    }
    public void selectTask()
    {
        taskSelected = true;
        tasksCanvas.enabled = false;
        droneCanvas.enabled = true;
    }

    //Fly Drone Functions
    public void fly()
    {
        if(motor)
        {
            flightCanvas.enabled = true;
            droneCanvas.enabled = false;
            droneCamera.depth = 1;
            droneCamera.gameObject.SetActive(true);
        }
    }
    public void verticalMove(Slider newValue)
    {
        if(newValue.value > 0.15 || newValue.value < -0.15)
            verticalMultiplier = newValue.value;
        else
            verticalMultiplier = 0;
    }
    public void horizontalMove(Slider newValue)
    {
        if(newValue.value > 0.15 || newValue.value < -0.15)
            horizontalMultiplier = newValue.value;
        else
            horizontalMultiplier = 0;
    }
    public void rotationMove(Slider newValue)
    {
        if(newValue.value > 0.15 || newValue.value < -0.15)
            rotationMultiplier = newValue.value;
        else
            rotationMultiplier = 0;
    }
}
*/





