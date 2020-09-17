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

    private int index;
    private int activity; // can have multiples
    #endregion

//****************************************************************************************************

    #region main functions
    // Start is called before the first frame update
    void Start()
    {
        //By default turn off legacy menu entrance
        LegacyMainMenu.SetActive(false);

        //used to store activity list
        List<string> ActivityList = new List<string>();
        List<string> ResourcesList = new List<string>();
        List<string> SensorsList = new List<string>();

        //initialize drop down menu 1: acitivities
        var dropdown1 = DropDown1.GetComponent<TMP_Dropdown>();
        //update drop down listings
        /*
        List<string> list = new List<string> { "option1", "option2" };
        dropdown1.options.Clear();
        foreach (string option in list)
        {
            dropdown1.options.Add(new TMP_Dropdown.OptionData(option));
        }
        */
        
        //monitor dropdown value changes.
        dropdown1.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown1); });

        //Dropdown 2 is resources.

        //Dropdown 3 is sensors.

    }

    // Update is called once per frame
    void Update()
    {

    }

//****************************************************************************************************

    public void DropdownValueChanged(TMP_Dropdown dropdown)
    {
        index = dropdown.value;
        m_Text.GetComponent<TextMeshProUGUI>().text = "Selected: "+ dropdown.options[index].text;
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

