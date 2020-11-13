using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.Dialog;

public class ManualClickSelect : MonoBehaviour
{
    public GameObject ManualSelectionCode;
    public GameObject TagFlag;
    public bool Taggable;
    [SerializeField] [Tooltip("Assign DialogSmall_192x96.prefab")] private GameObject DialogPrefabSmall;


    // Start is called before the first frame update
    void Start()
    {
        TagFlag.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void switchTag(GameObject Tag)
    {
        if (Tag.transform.GetChild(0).gameObject.activeSelf)
            Tag.transform.GetChild(0).gameObject.SetActive(false);
        else
            Tag.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ClickAction()
    {
        if (Taggable)//taggable
        {
            Debug.Log("Clicked on" + gameObject.name);
            TagFlag.SetActive(true);
            ManualSelectionCode.GetComponent<ManualSelection>().ResourceTaggedBool = true;
        }
        else //not taggable
        {
            Dialog.Open(DialogPrefabSmall, DialogButtonType.OK, "Warning", "This is not taggable.", false);
        }

    }
}
