using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//this function will auto start the engines of the drone, and rotate camera (drone object) around designated object.
//disable all canvas object, rotate for 1 loop, record image sequence, and reset canvas objects.

public class Drone13 : MonoBehaviour
{
    [SerializeField] private float speed = 15;
    [SerializeField] private GameObject DroneModel;
    [SerializeField] private GameObject DroneCamera;
    private bool startRot;
    private int FrmCount = 0;
    public GameObject ManualModeButton;

    public void Start()
    {
        startRot = false;
    }

    public void SetStart()
    {
        startRot = true;
    }

    public void Update()
    {
        if (startRot)
        {
            DroneModel.SetActive(true);
            DroneModel.GetComponent<Animator>().SetBool("fly", true);
            DroneModel.transform.Find("Arrow").gameObject.SetActive(true);
            transform.Rotate(0, speed * Time.deltaTime, 0);
            ManualModeButton.SetActive(false);
            //capture image every 5 frames.
            if (FrmCount % 5 == 0) DroneCamera.GetComponent<DroneCapture>().capture = true;
            FrmCount++;
        }
        else
        { 
           DroneModel.SetActive(false);
            ManualModeButton.SetActive(true);
        }
            
    }

    public void stop()
    {
        DroneModel.SetActive(false);
        DroneModel.transform.Find("Arrow").gameObject.SetActive(false);
        startRot = false;
        FrmCount = 0;
    }
}
