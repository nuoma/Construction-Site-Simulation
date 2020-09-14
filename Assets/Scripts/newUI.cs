using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class newUI : MonoBehaviour
{

    #region Parameters
    [SerializeField] private GameObject LegacyMainMenu; //LegacyMainMenu
    [SerializeField] private GameObject DropDown1;
    [SerializeField] private GameObject DropDown2;
    [SerializeField] private GameObject DropDown3;
    public TMPro.TMP_Dropdown ActivityDropdown;
    public TMPro.TMP_Dropdown ActivityDropdown2;
    public TMPro.TMP_Dropdown ActivityDropdown3;
    Dropdown m_Dropdown;
    [SerializeField] private GameObject m_Text;
    #endregion

    #region main functions
    // Start is called before the first frame update
    void Start()
    {
        //By default turn off legacy menu entrance
        LegacyMainMenu.SetActive(false);

        var dropdown1 = DropDown1.GetComponent<Dropdown>();
        dropdown1.options.Clear();
        dropdown1.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown1); });

    }

    // Update is called once per frame
    void Update()
    {
        //Add listener for when the value of the Dropdown changes, to take action


        //Initialise the Text to say the first value of the Dropdown
        m_Text.GetComponent<TextMeshProUGUI>().text = "First Value : " + m_Dropdown.value;
    }


    #endregion

    public void DropdownValueChanged(Dropdown dropdown)
    {
        int index = dropdown.value;
        m_Text.GetComponent<TextMeshProUGUI>().text = dropdown.options[index].text;
    }

    //Activate legacy menu upon selection.
    public void SelectLegacyMenu()
    {
        LegacyMainMenu.SetActive(true);
    }
}

