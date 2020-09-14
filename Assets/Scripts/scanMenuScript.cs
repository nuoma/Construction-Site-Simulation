using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scanMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject tripodMenu;
    [SerializeField] private GameObject tripod;
    [SerializeField] private GameObject scannerCanvas;
    [SerializeField] private GameObject scannerMenu;
    [SerializeField] private GameObject scanner;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject[] targets;
    [SerializeField] private GameObject tripodScripts;
    [SerializeField] private GameObject SelectionPrompt;
    [SerializeField] private float[] scannerMove = new float[3];

    string p1;
    string p2;
    string p3;
    string p4;
    bool targetsEnabled = false;
    bool tripodLevel = false;
    Renderer[][] targetRenderers = new Renderer[3][];

    void Start()
    {
        tripodMenu.SetActive(false);
        backButton.SetActive(false);
        tripod.GetComponent<Renderer>().enabled = false;
        scanner.GetComponent<Renderer>().enabled = false;
        //scannerCanvas.GetComponent<Canvas>().enabled = false;

        for (int i = 0; i < targets.Length; i++)
        {
            targetRenderers[i] = new Renderer[2];
            targetRenderers[i] = targets[i].GetComponentsInChildren<Renderer>();
            foreach (Renderer r in targetRenderers[i])
                r.enabled = false;
        }

        scannerMenu.SetActive(false);
    }

    public void tripodSelected()
    {
        tripodMenu.SetActive(true);
        tripod.GetComponent<Renderer>().enabled = true;
    }
    public void positionTripod()
    {
        Vector3 newPosition = tripod.transform.position;
        mainCamera.transform.position = newPosition + new Vector3(scannerMove[0], scannerMove[1], scannerMove[2]);
        backButton.SetActive(true);
        mainMenu.GetComponent<Canvas>().enabled = false;
    }
    public void levelTripod()
    {
        tripodLevel = true;
    }
    public void scannerBodySelected()
    {
        if (tripod.GetComponent<Renderer>().enabled == true)
        {
            tripodMenu.SetActive(false);
            scanner.GetComponent<Renderer>().enabled = true;
            //scanner.transform.parent.parent.GetComponent<ManipulationHandler>();
            //tripodScripts.GetComponent<ManipulationHandler>().enabled = false;
            MonoBehaviour[] scripts = tripodScripts.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
        }
    }
    public void targetsSelected()
    {
        targetsEnabled = true;

        for (int i = 0; i < targets.Length; i++)
        {
            foreach (Renderer r in targetRenderers[i])
                r.enabled = true;
        }
    }
    public void scannerInterface()
    {
        //if (tripod.GetComponent<Renderer>().enabled == true && scanner.GetComponent<Renderer>().enabled == true && targetsEnabled && tripodLevel)
        //{
            backButton.SetActive(true);
            //scannerCanvas.GetComponent<Canvas>().enabled = true;
            scannerCanvas.SetActive(true);
            
            //Vector3 newPosition = scanner.transform.position;
            //mainCamera.transform.position = newPosition + new Vector3(scannerMove[0], scannerMove[1], scannerMove[2]);
            
            mainMenu.GetComponent<Canvas>().enabled = false;
            scannerMenu.SetActive(false);
        //}
    }

    private void Update()
    {
        if (tripod.GetComponent<Renderer>().enabled == true)
            p1 = "Tripod Selected. ";
        if (scanner.GetComponent<Renderer>().enabled == true)
            p2 = "Scanner Selected. ";
        if (targetsEnabled)
            p3 = "Targets Selected. ";
        if (tripodLevel)
            p4 = "Tripod Levelled.";

        SelectionPrompt.GetComponent<TextMeshProUGUI>().text = "Selected:" +  p1+p2+p3+p4;
    }
}
