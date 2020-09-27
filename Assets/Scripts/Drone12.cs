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
            DroneModel.SetActive(true);
            DroneParent.transform.Find("Arrow").gameObject.SetActive(false);
            DroneModel.GetComponent<Animator>().SetBool("fly", true);
            transform.Rotate(0, speed * Time.deltaTime, 0);
        }
        else
        DroneModel.SetActive(false);

    }

    public void stop()
    {
        DroneParent.transform.Find("Arrow").gameObject.SetActive(false);
        startRot = false;
    }

}
