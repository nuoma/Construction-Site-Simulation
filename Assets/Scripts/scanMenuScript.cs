using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

public class scanMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject tripodMenu;
    [SerializeField] private GameObject tripod;
    [SerializeField] private GameObject scannerCanvas;
    [SerializeField] private GameObject scannerMenu;
    [SerializeField] private GameObject TargetsMenu;
    [SerializeField] private GameObject scanner;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject AutoResetButton;
    [SerializeField] private GameObject ManualResetButton;
    [SerializeField] private GameObject ModeSelection;
    [SerializeField] private GameObject[] targets;
    [SerializeField] private GameObject tripodScripts;
    [SerializeField] private GameObject SelectionPrompt;
    [SerializeField] private float[] scannerMove = new float[3];
    [SerializeField] public GameObject ScannerInterfaceButton;

    public GameObject Arrow;

    string p1;
    string p2;
    string p3;
    string p4;
    bool targetsEnabled = false;
    bool targetsMoved = false;
    bool tripodLevel = false;
    Renderer[][] targetRenderers = new Renderer[3][];
    public GameObject TripodParentNode;

    public GameObject BubbleLeveler;

    void Start()
    {
        BubbleLeveler.SetActive(false);
        ScannerInterfaceButton.GetComponent<Button>().interactable = false;

        tripodMenu.SetActive(false);
        backButton.SetActive(false);
        ManualResetButton.SetActive(false);
        AutoResetButton.SetActive(false);
        tripod.GetComponent<Renderer>().enabled = false;
        scanner.GetComponent<Renderer>().enabled = false;
        Arrow.SetActive(false);


        for (int i = 0; i < targets.Length; i++)
        {
            targetRenderers[i] = new Renderer[2];
            targetRenderers[i] = targets[i].GetComponentsInChildren<Renderer>();
            foreach (Renderer r in targetRenderers[i])
                r.enabled = false;
        }

        //scannerMenu.SetActive(false);
    }

    public void tripodSelected()
    {
        tripodMenu.SetActive(true);
        tripod.GetComponent<Renderer>().enabled = true;
        Arrow.SetActive(true);
    }
    public void positionTripod()
    {
        //Vector3 newPosition = tripod.transform.position;
        //mainCamera.transform.position = newPosition + new Vector3(scannerMove[0], scannerMove[1], scannerMove[2]);
        backButton.SetActive(true);
        //mainMenu.GetComponent<Canvas>().enabled = false;
        mainMenu.SetActive(false);
    }
    public void levelTripod()
    {
        //new with bubble leveler
        BubbleLeveler.SetActive(true);
        //mainMenu.SetActive(false);
        //old implementation disable entire activity manager
        //we only need to hide scanner menu itself, while tripod menu is disabled completely
        scannerMenu.SetActive(false);
        //old
        tripodLevel = true;
        tripodMenu.SetActive(false);
        TripodParentNode.GetComponent<NearInteractionGrabbable>().enabled = false;
        TripodParentNode.GetComponent<ObjectManipulator>().enabled = false;
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

    //activate targets, show prompt
    public void targetsSelected()
    {
        targetsEnabled = true;

        for (int i = 0; i < targets.Length; i++)
        {
            foreach (Renderer r in targetRenderers[i])
                r.enabled = true;
        }

        TargetsMenu.SetActive(true);
    }

    public void targetMove()
    {
        //show canvas, move camera to move targets
        targetsMoved = true;
        TargetsMenu.SetActive(false);
        mainMenu.SetActive(false);
        //Vector3 newPosition = tripod.transform.position;
        //mainCamera.transform.position = newPosition + new Vector3(scannerMove[0], scannerMove[1], scannerMove[2]);
        backButton.SetActive(true);
        //mainMenu.GetComponent<Canvas>().enabled = false;
    }

    public void scannerInterface()
    {
        //this condition is now used to change button interactable.
        //if (tripod.GetComponent<Renderer>().enabled == true && scanner.GetComponent<Renderer>().enabled == true && targetsEnabled && tripodLevel)
        Arrow.SetActive(false);
        TargetsMenu.SetActive(false);
        //backButton.SetActive(true);
        //if auto selection menu mode
        if (ModeSelection.GetComponent<modeselection>().Auto == true)
        {
            AutoResetButton.SetActive(true);
            ManualResetButton.SetActive(false);
        }
        //if manual selection menu mode
        if (ModeSelection.GetComponent<modeselection>().Manual == true)
        {
            AutoResetButton.SetActive(false);
            ManualResetButton.SetActive(true);
        }
        
        //scannerCanvas.GetComponent<Canvas>().enabled = true;
        scannerCanvas.SetActive(true);// substitute above function.
        Vector3 newPosition = scanner.transform.position;
        //mainCamera.transform.position = newPosition + new Vector3(scannerMove[0], scannerMove[1], scannerMove[2]);
        //mainMenu.GetComponent<Canvas>().enabled = false;
        scannerMenu.SetActive(false);

        //targets cannot move
        for (int i = 0; i < targets.Length; i++)
        {
            //targets[i].GetComponent<NearInteractionGrabbable>().gameObject.SetActive(false);
            targets[i].GetComponent<NearInteractionGrabbable>().enabled = false;
            targets[i].GetComponent<ObjectManipulator>().enabled = false;
        }
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
            p4 = "Tripod Levelled. ";

        SelectionPrompt.GetComponent<TextMeshProUGUI>().text = "Selected:" +  p1+p2+p3+p4;

        if (tripod.GetComponent<Renderer>().enabled == true && scanner.GetComponent<Renderer>().enabled == true && targetsEnabled && tripodLevel && targetsMoved)
        { ScannerInterfaceButton.GetComponent<Button>().interactable = true; }
            
    }
}
