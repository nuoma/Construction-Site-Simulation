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
    private bool startRot;

    public void Start()
    {
        //start drone animation
        switchTag(DroneModel);
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
            DroneModel.SetActive(true);
            DroneModel.transform.Find("Arrow").gameObject.SetActive(true);
            transform.Rotate(0, speed * Time.deltaTime, 0);
            //when reach one lap, stop save
        }

    }


    private void switchTag(GameObject Tag)
    {
        if (Tag.transform.GetChild(0).gameObject.activeSelf)
            Tag.transform.GetChild(0).gameObject.SetActive(false);
        else
            Tag.transform.GetChild(0).gameObject.SetActive(true);



    public void stop()
    {
        DroneModel.SetActive(false);
        DroneModel.transform.Find("Arrow").gameObject.SetActive(false);
        startRot = false;

    }
}
