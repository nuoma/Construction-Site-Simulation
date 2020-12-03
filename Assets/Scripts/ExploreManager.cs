using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.SceneManagement;

public class ExploreManager : MonoBehaviour
{
    #region parameters
    /*
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
    */
    public GameObject RadioButtonsParent;
    //public GameObject button14;
    public GameObject ConfirmButton;
    public GameObject ExploreLandingCanvas;
    public GameObject ActivityManager;
    public GameObject ResetButton;
    private bool[] SelectedActivities = new bool[16];
    public int SelectedActivityNum;
    public GameObject NearMenuIsolate;

    public GameObject A1Tooltip;
    public GameObject A2Tooltip;
    public GameObject A3Tooltip;
    public GameObject A4Tooltip;
    public GameObject A5Tooltip;
    public GameObject A6Tooltip;
    public GameObject A7Tooltip;
    public GameObject A8Tooltip;
    public GameObject A9Tooltip;
    public GameObject A10Tooltip;
    public GameObject A11Tooltip;
    public GameObject A12Tooltip;
    public GameObject A13Tooltip;

    private bool showhidetoggle = false;
    public GameObject Building6;
    public GameObject ActivityResourcesNode;
    #endregion

    #region Start Update
    // Start is called before the first frame update
    void Start()
    {
        A1Tooltip.SetActive(false);
        A2Tooltip.SetActive(false);
        A3Tooltip.SetActive(false);
        A4Tooltip.SetActive(false);
        A5Tooltip.SetActive(false);
        A6Tooltip.SetActive(false);
        A7Tooltip.SetActive(false);
        A8Tooltip.SetActive(false);
        A9Tooltip.SetActive(false);
        A10Tooltip.SetActive(false);
        A11Tooltip.SetActive(false);
        A12Tooltip.SetActive(false);
        A13Tooltip.SetActive(false);
        NearMenuIsolate.SetActive(false);
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
        /*
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
    */
        SelectedActivityNum = RadioButtonsParent.GetComponent<InteractableToggleCollection>().CurrentIndex;
        Debug.Log("Selected:" + SelectedActivityNum);
    }

    public void Confirm()
    {
        ExploreLandingCanvas.SetActive(false);
        ExecuteActivity();
    }

    private void ExecuteActivity()
    {
        //ResetButton.SetActive(true);
        NearMenuIsolate.SetActive(true);
        //A1 bulldozer move
        if (SelectedActivityNum == 0)
        {
            A1Tooltip.SetActive(true);
            ActivityManager.GetComponent<ActivityManagerScript>().select_1();
        }
        //A2 crane move
        if (SelectedActivityNum == 1)
        {
            A2Tooltip.SetActive(true);
            ActivityManager.GetComponent<ActivityManagerScript>().select_2();
        }
        //A3 material delivery
        if (SelectedActivityNum == 2)
        {
            A3Tooltip.SetActive(true);
            ActivityManager.GetComponent<ActivityManagerScript>().select_3();
        }
        //A4 worekr close call
        if (SelectedActivityNum == 3)
        {
            A4Tooltip.SetActive(true);
            ActivityManager.GetComponent<ActivityManagerScript>().select_4();
            ActivityManager.GetComponent<ActivityManagerScript>().select_2();
        }
        //A5 load haul
        if (SelectedActivityNum == 4)
        {
            A5Tooltip.SetActive(true);
            ActivityManager.GetComponent<ActivityManagerScript>().select_5();
        }
        //A6 material inventory
        if (SelectedActivityNum == 5)
        {
            A6Tooltip.SetActive(true);
        }
        //A7 detecting fall
        if (SelectedActivityNum == 6)
        {
            A7Tooltip.SetActive(true);
        }
        //A8 scan building
        if (SelectedActivityNum == 7)
        {
            A8Tooltip.SetActive(true);
        }
        //A9 scan floor
        if (SelectedActivityNum == 8)
        {
            A9Tooltip.SetActive(true);
        }
        //A10 scan stockpile
        if (SelectedActivityNum == 9)
        {
            A10Tooltip.SetActive(true);
        }
        //A11 scan old building
        if (SelectedActivityNum == 10)
        {
            A11Tooltip.SetActive(true);
        }
        //A12 jobsite inspection
        if (SelectedActivityNum == 11)
        {
            A12Tooltip.SetActive(true);
        }
        //A13 IMU
        if (SelectedActivityNum == 12)
        {
            A13Tooltip.SetActive(true);
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

    public void AllActivities()
    {
        //ControlPanel.SetActive(true);
        A1Tooltip.SetActive(true);
        ActivityManager.GetComponent<ActivityManagerScript>().select_1();
        A2Tooltip.SetActive(true);
        ActivityManager.GetComponent<ActivityManagerScript>().select_2();
        A3Tooltip.SetActive(true);
        ActivityManager.GetComponent<ActivityManagerScript>().select_3();
        A4Tooltip.SetActive(true);
        ActivityManager.GetComponent<ActivityManagerScript>().select_4();
        A5Tooltip.SetActive(true);
        ActivityManager.GetComponent<ActivityManagerScript>().select_5();
        A6Tooltip.SetActive(true);
        A7Tooltip.SetActive(true);
        A8Tooltip.SetActive(true);
        A9Tooltip.SetActive(true);
        A10Tooltip.SetActive(true);
        A11Tooltip.SetActive(true);
        A12Tooltip.SetActive(true);
        A13Tooltip.SetActive(true);
    }

    public void ShowHide()
    {
        //below is from menumanager.cs
        //GameObject building = Everything.transform.Find("SceneContent").transform.Find("Construction Site").transform.Find("buildings").transform.Find("building-6").gameObject;
        if (showhidetoggle)
        {
            //Currently in hidden state, now show everything.
            showhidetoggle = false;
            Building6.SetActive(true);//building-6 special case shared by multiple activities
            foreach (Transform child in ActivityResourcesNode.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            //bool is false, set to true and hide everything
            //Switch to hide in-active assets.
            showhidetoggle = true;
            //mainUICollection.SetActive(false);
            //MiscAssetNode.SetActive(false);
            //building.SetActive(false); //building-6 relate to these activities:
            //LS.SetActive(false);
            //MDrone.SetActive(false);


            //if(Activity not active) {turn off;}
            if (!(SelectedActivityNum == 1)) { ActivityResourcesNode.transform.Find("Activity1").gameObject.SetActive(false); }
            if (!(SelectedActivityNum == 2))
            {
                if (!(SelectedActivityNum == 4))
                    ActivityResourcesNode.transform.Find("Activity2").gameObject.SetActive(false);
            }
            if (!(SelectedActivityNum == 3)) { ActivityResourcesNode.transform.Find("Activity3").gameObject.SetActive(false); }
            if (!(SelectedActivityNum == 4)) { ActivityResourcesNode.transform.Find("Activity4").gameObject.SetActive(false); }
            if (!(SelectedActivityNum == 5)) { ActivityResourcesNode.transform.Find("Activity5").gameObject.SetActive(false); }
            if (!(SelectedActivityNum == 6)) { ActivityResourcesNode.transform.Find("Activity6").gameObject.SetActive(false); }
            if (!(SelectedActivityNum == 7)) { ActivityResourcesNode.transform.Find("Activity7").gameObject.SetActive(false); }
            if (!(SelectedActivityNum == 8)) { ActivityResourcesNode.transform.Find("Activity8").gameObject.SetActive(false); }
            if (!(SelectedActivityNum == 9)) { ActivityResourcesNode.transform.Find("Activity9").gameObject.SetActive(false); }
            if (!(SelectedActivityNum == 10)) { ActivityResourcesNode.transform.Find("Activity11A").gameObject.SetActive(false); ActivityResourcesNode.transform.Find("Activity11B").gameObject.SetActive(false); }
            //11.Oldhouse
            if (!(SelectedActivityNum == 11)) { ActivityResourcesNode.transform.Find("Activity12").gameObject.SetActive(false); ActivityResourcesNode.transform.Find("Activity12_Laser").gameObject.SetActive(false); ActivityResourcesNode.transform.Find("Activity12_Drone").gameObject.SetActive(false); }
            if (!(SelectedActivityNum == 12)) { ActivityResourcesNode.transform.Find("Activity13_Drone").gameObject.SetActive(false); }
            if (!(SelectedActivityNum == 13)) { ActivityResourcesNode.transform.Find("Activity14").gameObject.SetActive(false); }

            //building-6 check
            if (!(SelectedActivityNum == 2) && !(SelectedActivityNum == 4) && !(SelectedActivityNum == 9))
            {
                Building6.SetActive(false);
            }

        }
    }
    #endregion
}
