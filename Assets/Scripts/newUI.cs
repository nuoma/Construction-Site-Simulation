using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class newUI : MonoBehaviour
{

    #region Parameters
    [SerializeField] private GameObject LegacyMainMenu; //LegacyMainMenu
    [SerializeField] private GameObject DropDown1;//Dropdown 1 is for activities.
    [SerializeField] private GameObject DropDown2;
    [SerializeField] private GameObject DropDown3;

    [SerializeField] private GameObject m_Text;

    //used to store activity list
    List<string> ActivityList = new List<string>();
    List<string> ResourcesList = new List<string>();
    List<string> SensorsList = new List<string>();

    private int index;
    int index1;
    int index2;
    int index3;
    private int activity; // can have multiples
    #endregion

//****************************************************************************************************

    #region main functions
    // Start is called before the first frame update
    void Start()
    {
        //By default turn off legacy menu entrance
        LegacyMainMenu.SetActive(false);



        //initialize drop down menu 1: acitivities
        var dropdown1 = DropDown1.GetComponent<TMP_Dropdown>();
        //update drop down listings
        ActivityList = new List<string> { "Activities", "1","2","3","4","5","6","7","8","9","10","11","12","13","14","StopAll","Reset"};
        dropdown1.options.Clear();
        foreach (string option in ActivityList)
        {
            dropdown1.options.Add(new TMP_Dropdown.OptionData(option));
        }
        
        
        //monitor dropdown value changes.
        dropdown1.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown1); });

        //Dropdown 2 is sensors.
        var dropdown2 = DropDown2.GetComponent<TMP_Dropdown>();
        //update drop down listings
        SensorsList = new List<string> { "Sensors", "GPS","RFID","LaserScanner","Drone","IMU" };
        dropdown2.options.Clear();
        foreach (string option in SensorsList)
        {
            dropdown2.options.Add(new TMP_Dropdown.OptionData(option));
        }

        //Dropdown 2 monitor change without text change
        dropdown2.onValueChanged.AddListener(delegate { DropdownValueChanged2(dropdown2); });


        //Dropdown 3 is resources.
        var dropdown3 = DropDown3.GetComponent<TMP_Dropdown>();
        //update drop down listings
        ResourcesList = new List<string> { "Resources", "1.Dozer", "1.Stockpile", "2.Crane", "2.Load", "3.Truck","3.Rebar","4.Worker 1","5.Loader","5.dumptruck","5.stockpile","6.wood","6.Log","6.Rebar" };
        dropdown3.options.Clear();
        foreach (string option in ResourcesList)
        {
            dropdown3.options.Add(new TMP_Dropdown.OptionData(option));
        }

        //Dropdown 2 monitor change without text change
        dropdown3.onValueChanged.AddListener(delegate { DropdownValueChanged3(dropdown3); });
    }

    // Update is called once per frame
    void Update()
    {
        m_Text.GetComponent<TextMeshProUGUI>().text = "Selected: " + DropDown1.GetComponent<TMP_Dropdown>().options[index].text+" , "+ DropDown2.GetComponent<TMP_Dropdown>().options[index2].text+ " , " + DropDown3.GetComponent<TMP_Dropdown>().options[index3].text;
    }

//****************************************************************************************************

    public void DropdownValueChanged(TMP_Dropdown dropdown)
    {
        index = dropdown.value;
    }

    public void DropdownValueChanged2(TMP_Dropdown dropdown)
    {
        index2 = dropdown.value;
    }

    public void DropdownValueChanged3(TMP_Dropdown dropdown)
    {
        index3 = dropdown.value;
    }

    //Activate legacy menu upon selection.
    public void SelectLegacyMenu()
    {
        LegacyMainMenu.SetActive(true);
    }


    private void updateResources()
    {
        //Example: A1, rock and bulldozer
        //If activity contains 1, then list of resources.
        if (index == 1)
        {

        }
    }

    private void updateSensors()
    {
        //Example: A1, RFID and GPS.
    }
    #endregion

}

