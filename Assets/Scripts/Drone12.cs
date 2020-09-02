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
