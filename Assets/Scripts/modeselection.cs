using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modeselection : MonoBehaviour
{
    public GameObject AutoCanvas;
    public GameObject ManualCanvas;
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
    }

    public void manual()
    {
        ManualCanvas.SetActive(true);
        gameObject.SetActive(false);
        ManualCanvas.GetComponent<ManualSelection>().SetInteractablesFalse();
        ManualCanvas.GetComponent<ManualSelection>().SetCubeFalse();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
