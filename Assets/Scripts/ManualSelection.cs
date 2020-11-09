using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;
using Microsoft.MixedReality.Toolkit.Experimental.Dialog;
using Microsoft.MixedReality.Toolkit.UI;

public class ManualSelection : MonoBehaviour
{

    #region Parameters
    [SerializeField]
    [Tooltip("Assign DialogSmall_192x96.prefab")]
    private GameObject dialogPrefabSmall;
    [SerializeField] private GameObject PointingChevron;

    public GameObject A1Dozer;
    public GameObject RunButton;
    public GameObject ManualSelectionListPanel;
    public GameObject UIMenuManager;
    public GameObject ActivityManagerScript;
    public GameObject TagButton;

    public CustomDropdown Dropdown1;
    public CustomDropdown Dropdown2;

    List<string> ActivityList = new List<string> { "Activities" };
    List<string> SensorsList;

    private int SelectedActivityIndex;
    private int SelectedSensorIndex;
    private int ActualActivityNumber;

    private bool onSensorChanged;
    public bool ResourceTaggedBool = false;
    #endregion

    #region Start Update
    // Start is called before the first frame update
    void Start()
    {
        //create sensors list
        CreateActivityInitialDropdown();
        CreateSensorsDropdown();
        TagButton.SetActive(false);
        ManualSelectionListPanel.SetActive(false);
        RunButton.SetActive(false);
        //A1Dozer.GetComponent<BoundingBox>().gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateSensorSelected();

        if (onSensorChanged)
        {
            UpdateActivityList();
            ClearActivitiesDropdown();
            CreateActivitiesDropdown();
            onSensorChanged = false;
        }

        //UpdateActivitySelected();
        //after selection is made, activate run button.
        if (ResourceTaggedBool == true)
        { RunButton.SetActive(true); ResourceTaggedBool = false; }
    }
    #endregion


    #region supporting functions

    private void CreateSensorsDropdown()//Dropdown 1
    {
        SensorsList = new List<string> { "Sensors","GPS", "RFID", "Laser Scanner", "Drone", "IMU" };
        foreach (string option in SensorsList)
        {
            Dropdown1.CreateNewItem(option, null);
        }
        Dropdown1.dropdownEvent.AddListener(delegate { DropdownValueChanged(Dropdown1); });
        Dropdown1.SetupDropdown();
    }

    private void CreateActivityInitialDropdown()
    {
        foreach (string option in ActivityList)
        {
            Dropdown2.CreateNewItem(option, null);
        }
        //Dropdown2.dropdownEvent.AddListener(delegate { DropdownValueChanged(Dropdown2); });
        Dropdown2.SetupDropdown();
    }

    void DropdownValueChanged(CustomDropdown dropdown)
    {
        //Debug.Log("Dropdown Value Changed : " + dropdown.selectedItemIndex);
        //for (int i = 0; i < 5; ++i){ SelectedSensors[i] = false; }
        //SelectedSensors[dropdown.selectedItemIndex] = true;
        SelectedSensorIndex = dropdown.selectedItemIndex;
        onSensorChanged = true;
    }

    void DropdownValueChangedActivity(CustomDropdown dropdown)
    {
        //Debug.Log("Dropdown Value Changed : " + dropdown.selectedItemIndex);
        //for (int i = 0; i < 12; ++i) { SelectedActivity[i] = false; }
        //SelectedActivity[dropdown.selectedItemIndex] = true;
        SelectedActivityIndex = dropdown.selectedItemIndex;
        Debug.Log("Dropdown 2 value change:"+dropdown.selectedItemIndex);
        TagButton.SetActive(true);
    }

    private void LUT()
    {
        if (SelectedSensorIndex == 0)//GPS
        {
            if (SelectedActivityIndex == 0) ActualActivityNumber = 1;
            if (SelectedActivityIndex == 1) ActualActivityNumber = 2;
            if (SelectedActivityIndex == 2) ActualActivityNumber = 3;
            if (SelectedActivityIndex == 3) ActualActivityNumber = 4;
            if (SelectedActivityIndex == 4) ActualActivityNumber = 5;
            if (SelectedActivityIndex == 5) ActualActivityNumber = 7;
        }
        if (SelectedSensorIndex == 1)//RFID
        {
            if (SelectedActivityIndex == 0) ActualActivityNumber = 3;
            if (SelectedActivityIndex == 1) ActualActivityNumber = 4;
            if (SelectedActivityIndex == 2) ActualActivityNumber = 6;
            if (SelectedActivityIndex == 3) ActualActivityNumber = 7;
        }
        if (SelectedSensorIndex == 2)//LS
        {
            if (SelectedActivityIndex == 0) ActualActivityNumber = 8;
            if (SelectedActivityIndex == 1) ActualActivityNumber = 9;
            if (SelectedActivityIndex == 2) ActualActivityNumber = 10;
            if (SelectedActivityIndex == 3) ActualActivityNumber = 11;
        }
        if (SelectedSensorIndex == 3)//Drone
        {
            if (SelectedActivityIndex == 0) ActualActivityNumber = 11;
            if (SelectedActivityIndex == 1) ActualActivityNumber = 12;
        }
        if (SelectedSensorIndex == 4)//IMU
        {
            if (SelectedActivityIndex == 0) ActualActivityNumber = 13;
        }
    }


    private void ClearActivitiesDropdown()
    {
        //To clear existing items.
        Dropdown2.dropdownItems.Clear(); 
        Dropdown2.SetupDropdown();
    }

    private void CreateActivitiesDropdown()
    {
        
         foreach(var human in ActivityList)
        {
            Debug.Log(human);
        }
        //Create using updated ResourcesList
        foreach (string option in ActivityList)
        {
            Dropdown2.CreateNewItem(option,null);
        }
        Dropdown2.dropdownEvent.AddListener(delegate { DropdownValueChangedActivity(Dropdown2); });
        Dropdown2.SetupDropdown();   
    }

    public void TagButtonAction()
    {
        Debug.Log("Final selection:");

        LUT();
        //disable canvas
        //ERROR: cannot be disabled due to coroutine.
        //gameObject.SetActive(false);

        //which activity is selected?
        //SelectedActivityIndex

        //pointing arrow
        ActivityIndicator();

        UpdateResourceText();
       
        //activate canvas that shows a list of resources.
        ManualSelectionListPanel.SetActive(true);

        //activate corresponding resources bbox
        A1Dozer.GetComponent<BoundingBox>().gameObject.SetActive(true);

       
        
    }

    //List resources correspond to selected activity.
    private void UpdateResourceText()
    {
        string Text = "Dummy test resource for A1. \n A1R2. \n A1R3.";

        if (ActualActivityNumber == 1) Text = "Dozer \n Stockpile \n";
        if (ActualActivityNumber == 2) Text = "Crane \n Steel Beam \n";
        if (ActualActivityNumber == 3) Text = "Truck \n Rebar \n";
        if (ActualActivityNumber == 4) Text = "Worker 1 \n";
        if (ActualActivityNumber == 5) Text = "Loaser \n Dumptruck \n Stockpile \n";
        if (ActualActivityNumber == 6) Text = "Wood \n Log \n Rebar \n";
        if (ActualActivityNumber == 7) Text = "Worker 1 \n Worker 2 \n Worker 3 \n";
        if (ActualActivityNumber == 8) Text = "8? \n";
        if (ActualActivityNumber == 9) Text = "9? \n";
        if (ActualActivityNumber == 10) Text = "Stockpile 1 \n Stockpile 2 \n";
        if (ActualActivityNumber == 11) Text = "Old Building \n";
        if (ActualActivityNumber == 12) Text = "12? \n";
        if (ActualActivityNumber == 13) Text = " Painter \n Laborer \n Carpenter 1 \n Carpenter 2 \n";

        ManualSelectionListPanel.transform.Find("ResourceList").GetComponent<TextMeshProUGUI>().text = Text;

    }

    public void RunButtonFunction()
    {
        Debug.Log("Confirm selection and execute.");

        //disable canvas
        gameObject.SetActive(false);
        ManualSelectionListPanel.SetActive(false);

        //execute command
        if (ActualActivityNumber == 1) ActivityManagerScript.GetComponent<ActivityManagerScript>().select_1();
        if (ActualActivityNumber == 2) ;
    }

    private void ActivityIndicator()
    {

        string name = "A" + ActualActivityNumber + "POS";
        //TODO: this index is very very wrong!!!! needs LUT.

        //Activate Chevron and live for 5 seconds.
        //UIMenuManager.GetComponent<MenuManager>().ShowAndHide(PointingChevron, name, 8.0f);
        StartCoroutine(ShowAndHide(PointingChevron, name, 10.0f));

    }

    // Activate chevron, give location, and keep it active for 5 seconds.
    public IEnumerator ShowAndHide(GameObject go, string name, float delay)
    {
        go.GetComponent<DirectionalIndicator>().DirectionalTarget = GameObject.Find(name).transform;
        go.SetActive(true);
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }

    /// <summary>
    /// Update activity list based on selected sensors.
    /// </summary>
    private void UpdateActivityList()
    {
        ActivityList.Clear();
        Debug.Log("Activity list updated");
        //Activity correspond to GPS
        if (SelectedSensorIndex == 0) { ActivityList.AddRange(new string[] { "Activities","Dozer backfilling", "Crane Loading", "Material Delivery", "Worker's Close Call", "Load & Haul",
             "Detecting Fall" }); }
        if (SelectedSensorIndex == 1) { ActivityList.AddRange(new string[] {  "Activities","Material Delivery", "Worker's Close Call", 
            "Material Inventory", "Detecting Fall" }); }
        if (SelectedSensorIndex == 2) { ActivityList.AddRange(new string[] { "Activities", "Scan Building", "Scan Floor", "Scan Stockpile", "Scan Old Building" }); }
        if (SelectedSensorIndex == 3) { ActivityList.AddRange(new string[] { "Activities", "Scan Old Building", "Jobsite Inspection" }); }
        if (SelectedSensorIndex == 4) { ActivityList.AddRange(new string[] { "Activities", "Worker Ergonomics" }); }
    }

    public void ReloadSceneButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


    //Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Confirmation Dialog, Small, Far", "This is an example of a small dialog with only one button, placed at near interaction range", false);

    #endregion
}
