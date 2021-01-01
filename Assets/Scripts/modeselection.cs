using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class modeselection : MonoBehaviour
{
    public GameObject AutoCanvas;
    public GameObject ManualCanvas;
    public bool Auto;
    public bool Manual;
    public GameObject manualselection;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        //AutoCanvas.SetActive(false);
        //ManualCanvas.SetActive(false);
        manualselection.GetComponent<ManualSelection>().SetInteractablesFalse();
        manualselection.GetComponent<ManualSelection>().SetCubeFalse();
        Debug.Log("SceneName:" + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "3_auto(5)") { Auto = true; Manual = false; } 
        if (SceneManager.GetActiveScene().name == "3_manual(6)") { Auto = false; Manual = true; }
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

    public void exitbutton()
    {
        SceneManager.LoadScene(0);
    }

    public void Backbutton()
    {
        SceneManager.LoadScene(3);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
