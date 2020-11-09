using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualClickSelect : MonoBehaviour
{
    public GameObject ManualSelectionCode;
    public GameObject A1DozerFlag;

    // Start is called before the first frame update
    void Start()
    {
        A1DozerFlag.SetActive(false);
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
        Debug.Log("Clicked on"+gameObject.name);
        A1DozerFlag.SetActive(true);
        ManualSelectionCode.GetComponent<ManualSelection>().ResourceTaggedBool = true;
    }
}
