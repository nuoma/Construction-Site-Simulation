using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;

public class ExploreManager : MonoBehaviour
{
    #region parameters

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
    public GameObject A14Tooltip;
    public GameObject A15Tooltip;

    public GameObject OldHouseWindowTooltip;
    public GameObject OldHouseRoofTooltip;
    public GameObject OldHouseFloorTooltip;

    private bool showhidetoggle = false;
    public GameObject Building6;
    public GameObject Building3;
    public GameObject ActivityResourcesNode;

    public GameObject LS;
    public GameObject MDrone;

    public GameObject CAMPOS;

    [SerializeField] private GameObject PointingChevron;

    public GameObject A1DozerTooltipSpawner;
    public GameObject A1StockpileTooltipSpawner;
    public GameObject A2CraneTooltipSpawner;
    public GameObject A2BeamTooltipSpawner;
    public GameObject A3TruckTooltipSpawner;
    public GameObject A4WorkerTooltipSpawner;
    public GameObject A5TruckTooltipSpawner;
    public GameObject A5BackhoeTooltipSpawner;
    public GameObject A5StockpileTooltipSpawner;
    public GameObject A6RebarTooltipSpawner;
    public GameObject A6WoodTooltipSpawner;
    public GameObject A6LogTooltipSpawner;
    public GameObject A7w1TooltipSpawner;
    public GameObject A7w2TooltipSpawner;
    public GameObject A11Stockpile1TooltipSpawner;
    public GameObject A11Stockpile2TooltipSpawner;
    public GameObject A8CladLTooltipSpawner;
    public GameObject A8CladRTooltipSpawner;
    public GameObject A8CladFTooltipSpawner;
    public GameObject A8CladBTooltipSpawner;

    public GameObject ShowHideButton;
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
        A14Tooltip.SetActive(false);
        A15Tooltip.SetActive(false);
        NearMenuIsolate.SetActive(false);

        OldHouseWindowTooltip.SetActive(false);
        OldHouseRoofTooltip.SetActive(false);
        OldHouseFloorTooltip.SetActive(false);
        PointingChevron.SetActive(false);
        LS.SetActive(true);
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

        //pointing chevron
        //ActivityIndicator();

        ShowHide();

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
            ActivityManager.GetComponent<ActivityManagerScript>().A7_w1_flag = true;
            ActivityManager.GetComponent<ActivityManagerScript>().A7_w2_flag = true;
            ActivityManager.GetComponent<ActivityManagerScript>().A7_w3_flag = true;
            ActivityManager.GetComponent<ActivityManagerScript>().select_7_new();
        }
        //A8 scan building
        if (SelectedActivityNum == 7)
        {
            A8Tooltip.SetActive(true);
            //LS default location
        }
        //A9 scan floor
        if (SelectedActivityNum == 8)
        {
            A9Tooltip.SetActive(true);
            //Move LS to floor location
            Vector3 A9Coordinates = ActivityManager.GetComponent<ActivityManagerScript>().Explore_A9_MoveLSOnly();
            LS.transform.position = A9Coordinates;
        }
        //A10 scan stockpile
        if (SelectedActivityNum == 9)
        {
            A10Tooltip.SetActive(true);
            //move LS to stockpile
            Vector3 A10Coordinates = ActivityManager.GetComponent<ActivityManagerScript>().Explore_A10_MoveLSOnly();
            LS.transform.position = A10Coordinates;
        }
    
        //A11 scan old building
        if (SelectedActivityNum == 10)
        {
            A11Tooltip.SetActive(true);
            OldHouseWindowTooltip.SetActive(true);
            OldHouseRoofTooltip.SetActive(true);
            OldHouseFloorTooltip.SetActive(true);
        }
        //A12 jobsite inspection
        if (SelectedActivityNum == 11)
        {
            A12Tooltip.SetActive(true);
        }
        //A13 IMU
        //Painter
        if (SelectedActivityNum == 12)
        {
            A13Tooltip.SetActive(true);
        }

        //L
        if (SelectedActivityNum == 13)
        {
            A14Tooltip.SetActive(true);
        }
        //C
        if (SelectedActivityNum == 14)
        {
            A15Tooltip.SetActive(true);
        }

        //All activities
        if (SelectedActivityNum == 15)
        {
            AllActivities();
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
        ActivityManager.GetComponent<ActivityManagerScript>().A7_w1_flag = true;
        ActivityManager.GetComponent<ActivityManagerScript>().A7_w2_flag = true;
        ActivityManager.GetComponent<ActivityManagerScript>().A7_w3_flag = true;
        ActivityManager.GetComponent<ActivityManagerScript>().select_7_new();
        A8Tooltip.SetActive(true);
        A9Tooltip.SetActive(true);
        A10Tooltip.SetActive(true);
        A11Tooltip.SetActive(true);
        A12Tooltip.SetActive(true);
        A13Tooltip.SetActive(true);
        A14Tooltip.SetActive(true);
        A15Tooltip.SetActive(true);

        ShowHideButton.SetActive(false);
    }

    private void ActivityIndicator()
    {
        string name = "A" + SelectedActivityNum + "POS";
        //Activate Chevron and live for 10 seconds.
        StartCoroutine(ShowAndHide(PointingChevron, name, 10.0f));
    }

    // Activate chevron, give location, and keep it active for 5 seconds.
    public IEnumerator ShowAndHide(GameObject go, string name, float delay)
    {
        go.GetComponent<DirectionalIndicator>().DirectionalTarget = CAMPOS.transform.Find(name).transform;
        go.SetActive(true);
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }

    private void AllTooltipsOn()
    {
          A1DozerTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A1StockpileTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A2CraneTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A2BeamTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A3TruckTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A4WorkerTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A5TruckTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A5BackhoeTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A5StockpileTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A6RebarTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A6WoodTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A6LogTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A7w1TooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A7w2TooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A11Stockpile1TooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A11Stockpile2TooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A8CladLTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A8CladRTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A8CladFTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
          A8CladBTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = true;
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
            Building3.SetActive(true);
            LS.SetActive(true);
            MDrone.SetActive(true);
            foreach (Transform child in ActivityResourcesNode.transform)
            {
                child.gameObject.SetActive(true);
            }
            AllTooltipsOn();
        }
        else
        {
            //bool is false, set to true and hide everything
            //Switch to hide in-active assets.
            showhidetoggle = true;

            //All activities, hide nothing
            if (SelectedActivityNum == 15) return;
            //mainUICollection.SetActive(false);
            //MiscAssetNode.SetActive(false);
            //building.SetActive(false); //building-6 relate to these activities:
            //LS.SetActive(false);
            //MDrone.SetActive(false);


            //if(Activity not active) {turn off;}
            //A1 Dozer
            if (!(SelectedActivityNum == 0)) 
            { 
                ActivityResourcesNode.transform.Find("Activity1").gameObject.SetActive(false); 
                A1DozerTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
                A1StockpileTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
            }
            //A2 crane
            if (!(SelectedActivityNum == 1))
            {
                if (!(SelectedActivityNum == 3))
                {
                    ActivityResourcesNode.transform.Find("Activity2").gameObject.SetActive(false);
                    A2CraneTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
                    A2BeamTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
                }
                    
            }
            //A3 delivery
            if (!(SelectedActivityNum == 2)) 
            { 
                ActivityResourcesNode.transform.Find("Activity3").gameObject.SetActive(false);
                A3TruckTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
            }
            //A4 worker danger zone
            if (!(SelectedActivityNum == 3)) 
            { 
                ActivityResourcesNode.transform.Find("Activity4").gameObject.SetActive(false);
                A4WorkerTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
            }
            //A5 load haul
            if (!(SelectedActivityNum == 4)) 
            { 
                ActivityResourcesNode.transform.Find("Activity5").gameObject.SetActive(false);
                A5TruckTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
                A5BackhoeTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
                A5StockpileTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
            }
            //A6 rfid
            if (!(SelectedActivityNum == 5)) 
            { 
                ActivityResourcesNode.transform.Find("Activity6").gameObject.SetActive(false);
                A6RebarTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
                A6WoodTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
                A6LogTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
            }
            //A7 3 worker
            if (!(SelectedActivityNum == 6)) 
            { 
                ActivityResourcesNode.transform.Find("Activity7").gameObject.SetActive(false);
                A7w1TooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
                A7w2TooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
            }
            //A8 Scan building
            if (!(SelectedActivityNum == 7)) 
            { 
                ActivityResourcesNode.transform.Find("Activity8").gameObject.SetActive(false);
                A8CladLTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
                A8CladRTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
                A8CladFTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
                A8CladBTooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
            }
            //A9 floor
            if (!(SelectedActivityNum == 8)) { ActivityResourcesNode.transform.Find("Activity9").gameObject.SetActive(false); }
            //A10 scan stockpile
            if (!(SelectedActivityNum == 9)) 
            { 
                ActivityResourcesNode.transform.Find("Activity11A").gameObject.SetActive(false); 
                ActivityResourcesNode.transform.Find("Activity11B").gameObject.SetActive(false);
                A11Stockpile1TooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
                A11Stockpile2TooltipSpawner.GetComponent<ToolTipSpawner>().FocusEnabled = false;
            }
            //A11 Oldhouse
            if (!(SelectedActivityNum == 10)) 
            { 
                ActivityResourcesNode.transform.Find("Activity12").gameObject.SetActive(false); 
                ActivityResourcesNode.transform.Find("Activity12_Laser").gameObject.SetActive(false); 
                ActivityResourcesNode.transform.Find("Activity12_Drone").gameObject.SetActive(false);
                
            }
            //A12 jobsite
            if (!(SelectedActivityNum == 11)) { ActivityResourcesNode.transform.Find("Activity13_Drone").gameObject.SetActive(false); }
            //A13 paint
            if (!(SelectedActivityNum == 12)) { ActivityResourcesNode.transform.Find("Activity14").gameObject.SetActive(false); }
            //A14 labor
            if (!(SelectedActivityNum == 13)) { ActivityResourcesNode.transform.Find("Activity15").gameObject.SetActive(false); }
            //A14 carpentry
            if (!(SelectedActivityNum == 14)) { ActivityResourcesNode.transform.Find("Activity16").gameObject.SetActive(false); }

            

                //building-6 check
                if (!(SelectedActivityNum == 1) && !(SelectedActivityNum == 3) && !(SelectedActivityNum == 8))
            {
                Building6.SetActive(false);
            }

            //building-3 check
            if (!(SelectedActivityNum == 6) && !(SelectedActivityNum == 7))
            {
                Building3.SetActive(false);
            }

            //LS check
            //Only A8 LS is kept, other activities not yet moving the LS parent node,
            if (!(SelectedActivityNum == 7) && !(SelectedActivityNum == 8) && !(SelectedActivityNum == 9))
            {
                LS.SetActive(false);
            }
            //Drone check
            if ( !(SelectedActivityNum == 11))//!(SelectedActivityNum == 10) &&
            {
                MDrone.SetActive(false);
            }

        }
    }
    #endregion
}
