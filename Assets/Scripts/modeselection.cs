using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modeselection : MonoBehaviour
{
    public GameObject AutoCanvas;
    public GameObject ManualCanvas;
    public bool Auto;
    public bool Manual;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        AutoCanvas.SetActive(false);
        ManualCanvas.SetActive(false);
    }

    public void auto()
    {
        AutoCanvas.SetActive(true);
        gameObject.SetActive(false);
        Auto = true;
        Manual = false;
    }

    public void manual()
    {
        ManualCanvas.SetActive(true);
        gameObject.SetActive(false);
        ManualCanvas.GetComponent<ManualSelection>().SetInteractablesFalse();
        ManualCanvas.GetComponent<ManualSelection>().SetCubeFalse();
        Manual = true;
        Auto = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
