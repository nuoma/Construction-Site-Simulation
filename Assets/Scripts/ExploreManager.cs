using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.SceneManagement;

public class ExploreManager : MonoBehaviour
{
    #region parameters
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    public GameObject button5;
    public GameObject button6;
    public GameObject button7;
    public GameObject button8;
    public GameObject button9;
    public GameObject button10;
    public GameObject button11;
    public GameObject button12;
    public GameObject button13;
    public GameObject ButtonsParent;
    //public GameObject button14;
    public GameObject ConfirmButton;
    public GameObject ExploreLandingCanvas;
    public GameObject ActivityManager;
    private bool[] SelectedActivities = new bool[16];

    #endregion

    #region Start Update
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelected();
    }
    #endregion

    #region support function

    private void UpdateSelected()
    {
        if (button1.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[0] = true; } else { SelectedActivities[0] = false; }
        if (button2.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[1] = true; } else { SelectedActivities[1] = false; }
        if (button3.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[2] = true; } else { SelectedActivities[2] = false; }
        if (button4.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[3] = true; } else { SelectedActivities[3] = false; }
        if (button5.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[4] = true; } else { SelectedActivities[4] = false; }
        if (button6.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[5] = true; } else { SelectedActivities[5] = false; }
        if (button7.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[6] = true; } else { SelectedActivities[6] = false; }
        if (button8.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[7] = true; } else { SelectedActivities[7] = false; }
        if (button9.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[8] = true; } else { SelectedActivities[8] = false; }
        if (button10.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[9] = true; } else { SelectedActivities[9] = false; }
        if (button11.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[10] = true; } else { SelectedActivities[10] = false; }
        if (button12.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[11] = true; } else { SelectedActivities[11] = false; }
        if (button13.GetComponent<Interactable>().IsToggled == true) { SelectedActivities[12] = true; } else { SelectedActivities[12] = false; }


    }

    public void Confirm()
    {
        for (int i = 0; i < 13; ++i)
        {
            if (SelectedActivities[i] == true) Debug.Log(i + "Activated");
        }

        ExploreLandingCanvas.SetActive(false);

        ExecuteActivity();
    }

    private void ExecuteActivity()
    {
        //A1
        if (SelectedActivities[0] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_1();
        }
        //A2
        if (SelectedActivities[1] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_2();
        }
        //A3
        if (SelectedActivities[2] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_3();
        }
        //A4
        if (SelectedActivities[3] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_4();
            ActivityManager.GetComponent<ActivityManagerScript>().select_2();
        }
        //A5
        if (SelectedActivities[4] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_5();
        }
        //A6
        if (SelectedActivities[5] == true)
        {
        }
        //A7
        //A8
        //A9
        //A10
        //A11
        //A12
        //A13 IMU

    }

    public void ExitButtonFunction()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetButtonFunction()
    {
        SceneManager.LoadScene(1);
    }
    #endregion
}
