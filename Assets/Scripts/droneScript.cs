using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

public class droneScript : MonoBehaviour
{
    [SerializeField] private GameObject droneCanvas;
    [SerializeField] public GameObject DronIMGPathTMP;
    [SerializeField] private GameObject flightCanvas;
    [SerializeField] private GameObject flyButton;
    [SerializeField] private Camera droneCamera;
    [SerializeField] private float speed = 1;
    [SerializeField] private GameObject Backbutton;

    private int FrmCount = 0;
    [HideInInspector] public bool startRot;

    private float zMax = 7.8f;
    private float zMin = -6f;
    private float xMax = 6.6f;
    private float xMin = -5f;
    private float yMax = 1.25f;
    private float yMin = -0.25f;

    private float verticalMultiplier = 0;
    private float horizontalMultiplier = 0;
    private float rotationMultiplier = 0;

    [HideInInspector] public bool taskSelected = false;
    [HideInInspector] public bool power = false;
    [HideInInspector] public bool motor = false;

    public GameObject FlightCanvasBackButton;
    public GameObject FlightCanvasManualBackButton;
    public GameObject ModeSelection;

    // Start is called before the first frame update
    public void Start()
    {
        //tasksCanvas.enabled = false;
        //droneCanvas.SetActive(true);
        //flightCanvas.SetActive(false);
        //droneCamera.gameObject.SetActive(false);
        //this.gameObject.SetActive(false);
        //Debug.Log("drone start flag");
        flyButton.GetComponent<Button>().interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(startRot)
        {
            Vector3 rotate = new Vector3(0, rotationMultiplier, 0);
            Vector3 move = new Vector3(0, verticalMultiplier, horizontalMultiplier);
            transform.Rotate(20 * rotate * Time.deltaTime);
            transform.Translate(move * speed * Time.deltaTime);

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

            DronIMGPathTMP.GetComponent<TextMeshProUGUI>().text = "Saved Image Path: " + Application.persistentDataPath + "/DroneImages";
            //capture image every 5 frames.
            if (FrmCount % 5 == 0) droneCamera.GetComponent<DroneCapture>().capture = true;
            FrmCount++;
        }
    }

    //Start Drone
    public void powerOn()
    {
        if (power)
        { power = false;  }
        else
        { power = true; }

        if (power && motor)
        {
            GetComponent<Animator>().SetBool("fly", true);
            //ColorBlock colorVar = flyButton.GetComponent<Button>().colors;
            //colorVar.highlightedColor = new Color32(138, 255, 114, 255);
            //colorVar.pressedColor = new Color32(17, 101, 0, 255);
            //flyButton.GetComponent<Button>().colors = colorVar;
            flyButton.GetComponent<Button>().interactable = true;
        }
        else
        { flyButton.GetComponent<Button>().interactable = false; }
    }

    public void enginesOn()
    {
        if(motor)
        { motor = false; }
        else
        { motor = true; }

        if (power && motor)
        {
            GetComponent<Animator>().SetBool("fly", true);
            //ColorBlock colorVar = flyButton.GetComponent<Button>().colors;
            //colorVar.highlightedColor = new Color32(138, 255, 114, 255);
            //colorVar.pressedColor = new Color32(17, 101, 0, 255);
            //flyButton.GetComponent<Button>().colors = colorVar;
            flyButton.GetComponent<Button>().interactable = true;
        }
        else
        { flyButton.GetComponent<Button>().interactable = false; }
    }


    //Fly Drone Functions
    public void fly()
    {
        if(motor)
        {
            //Debug.Log("Activate flight canvas, disable drone canvas");
            flightCanvas.SetActive(true);
            droneCanvas.SetActive(false);
            //droneCamera.depth = 1;
            //droneCamera.gameObject.SetActive(true);
            startRot = true;
            Backbutton.SetActive(false);

            //if auto selection menu mode
            if (ModeSelection.GetComponent<modeselection>().Auto == true)
            {
                FlightCanvasBackButton.SetActive(true);
                FlightCanvasManualBackButton.SetActive(false);
            }
            //if manual selection menu mode
            if (ModeSelection.GetComponent<modeselection>().Manual == true)
            {
                FlightCanvasManualBackButton.SetActive(true);
                FlightCanvasBackButton.SetActive(false);
            }      
        }
    }

    public void verticalMoveMRTK(SliderEventData newValue)
    {
        float VValuePost = 2 * newValue.NewValue - 1.0f;
        if (VValuePost > 0.15 || VValuePost < -0.15)
            verticalMultiplier = VValuePost;
        else
            verticalMultiplier = 0;
    }

    public void horizontalMoveMRTK(SliderEventData newValue)
    {
        float HValuePost = 2 * newValue.NewValue - 1.0f;
        if (HValuePost > 0.15 || HValuePost < -0.15)
            horizontalMultiplier = HValuePost;
        else
            horizontalMultiplier = 0;
    }

    public void rotationMoveMRTK(SliderEventData newValue)
    {
        float RValuePost = 2 * newValue.NewValue - 1.0f;
        if (RValuePost > 0.15 || RValuePost < -0.15)
            rotationMultiplier = RValuePost;
        else
            rotationMultiplier = 0;
    }


    /*
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
    */
    public void stop()
    {
        startRot = false;
        FrmCount = 0;
    }
}
