using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.Dialog;

public class TutorialManualClick : MonoBehaviour
{
    public GameObject SensorTutorialCode;
    public GameObject TagFlag;
    public bool Taggable;
    public bool TagStatus;
    [SerializeField] [Tooltip("Assign DialogSmall_192x96.prefab")] private GameObject DialogPrefabSmall;
    private bool WarningBool;
    private Dialog myDialog;
    private string DialogWarningString;

    void Start()
    {
        TagFlag.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (WarningBool && (myDialog == null))
        {
            myDialog = Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Warning", DialogWarningString, false);
            WarningBool = false;
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
                TagStatus = true;
            }

        }
        else //not taggable object, show warning
        {
            //Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Warning", "This is not taggable.", true);
            Debug.Log("manual click warning dialog");
            WarningBool = true;
            DialogWarningString = "This object cannot be tagged.";
        }

    }
}
