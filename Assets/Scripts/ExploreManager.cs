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
    public GameObject ResetButton;
    private bool[] SelectedActivities = new bool[16];

    public GameObject A1Parent;

    public GameObject A5Panel;
    public GameObject A6Panel;
    public GameObject A7Panel;
    public GameObject A8Panel;
    public GameObject A9Panel;
    public GameObject A10Panel;
    public GameObject A11Panel;
    public GameObject A12Panel;
    public GameObject A13Panel;

    #endregion

    #region Start Update
    // Start is called before the first frame update
    void Start()
    {
        A5Panel.SetActive(false);
        A6Panel.SetActive(false);
        A7Panel.SetActive(false);
        A8Panel.SetActive(false);
        A9Panel.SetActive(false);
        A10Panel.SetActive(false);
        A11Panel.SetActive(false);
        A12Panel.SetActive(false);
        A13Panel.SetActive(false);
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
        ResetButton.SetActive(true);

        //A1 bulldozer move
        if (SelectedActivities[0] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_1();
        }
        //A2 crane move
        if (SelectedActivities[1] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_2();
        }
        //A3 material delivery
        if (SelectedActivities[2] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_3();
        }
        //A4 worekr close call
        if (SelectedActivities[3] == true)
        {
            ActivityManager.GetComponent<ActivityManagerScript>().select_4();
            ActivityManager.GetComponent<ActivityManagerScript>().select_2();
        }
        //A5 load haul
        if (SelectedActivities[4] == true)
        {
            A5Panel.SetActive(true);
            ActivityManager.GetComponent<ActivityManagerScript>().select_5();
        }
        //A6 material inventory
        if (SelectedActivities[5] == true)
        {
            A6Panel.SetActive(true);
        }
        //A7 detecting fall
        if (SelectedActivities[6] == true)
        {
            A7Panel.SetActive(true);
        }
        //A8 scan building
        if (SelectedActivities[7] == true)
        {
            A8Panel.SetActive(true);
        }
        //A9 scan floor
        if (SelectedActivities[8] == true)
        {
            A9Panel.SetActive(true);
        }
        //A10 scan stockpile
        if (SelectedActivities[9] == true)
        {
            A10Panel.SetActive(true);
        }
        //A11 scan old building
        if (SelectedActivities[10] == true)
        {
            A11Panel.SetActive(true);
        }
        //A12 jobsite inspection
        if (SelectedActivities[11] == true)
        {
            A12Panel.SetActive(true);
        }
        //A13 IMU
        if (SelectedActivities[12] == true)
        {
            A13Panel.SetActive(true);
        }

    }

    //All show hide buttons.
    //A1
    public void showhide5()
    {
        bool flag = false;
        Renderer[] renderers = A1Parent.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            // Do something with the renderer here...
            if (flag)
            {
                r.enabled = false; // like disable it for example. 
                flag = false;
                Debug.Log("Render off");
            }
            else
            {
                r.enabled = true; // like disable it for example. 
                flag = true;
            }
        }
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
