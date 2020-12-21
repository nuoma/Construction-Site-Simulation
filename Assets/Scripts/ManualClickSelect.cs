using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.Dialog;

public class ManualClickSelect : MonoBehaviour
{
    public GameObject ManualSelectionCode;
    public GameObject TagFlag;
    public bool Taggable;
    public bool TagStatus = false;
    [SerializeField] [Tooltip("Assign DialogSmall_192x96.prefab")] private GameObject DialogPrefabSmall;
    private bool WarningBool;
    private Dialog myDialog;
    private string DialogWarningString;

    // Start is called before the first frame update
    void Start()
    {
        TagFlag.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (WarningBool) 
        {
            if (myDialog == null)
            {
                myDialog = Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Warning", DialogWarningString, false);
                WarningBool = false;
            } 
        }

        //currently configuring as A2 crane activity + GPS
        if (ManualSelectionCode.GetComponent<ManualSelection>().ActualActivityNumber == 2 && ManualSelectionCode.GetComponent<ManualSelection>().SelectedSensorIndex == 0)
        {
            //identify self by name
            if (gameObject.name == "Steel Beam")
            { Taggable = true; }
            if (gameObject.name == "Crane")
            { Taggable = false; }
        }
        //currently configuring as A2 crane activity + RFID
        if (ManualSelectionCode.GetComponent<ManualSelection>().ActualActivityNumber == 2 && ManualSelectionCode.GetComponent<ManualSelection>().SelectedSensorIndex == 1)
        {
            //identify self by name
            if (gameObject.name == "Steel Beam")
            { Taggable = false; }
            if (gameObject.name == "Crane")
            { Taggable = true; }
        }
    }

    public void ClickAction()
    {
        if (Taggable)//taggable
        {
            if (TagStatus)
            {
                //Already tagged, toggle off
                Debug.Log("Cancel selection on: " + gameObject.name);
                TagFlag.SetActive(false);
                //ManualSelectionCode.GetComponent<ManualSelection>().ResourceTaggedBool = false;
                TagStatus = false;
            }
            else
            {
                //Not tagged, toggle on
                Debug.Log("Clicked on: " + gameObject.name);
                TagFlag.SetActive(true);
                ManualSelectionCode.GetComponent<ManualSelection>().ResourceTaggedBool = true;
                TagStatus = true;
            }
            
        }
        else //not taggable object, show warning
        {
            //Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Warning", "This is not taggable.", true);
            if (myDialog == null)
            {
                WarningBool = true;
            }
            
            DialogWarningString = gameObject.name + " cannot be tagged with " + ManualSelectionCode.GetComponent<ManualSelection>().CurrentSensor;
        }

    }
}
