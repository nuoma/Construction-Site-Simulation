using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreSelection : MonoBehaviour
{

    public GameObject ExploreIsolateCanvas;
    public GameObject ModeSelectionPanel;
    public GameObject ExploreManager;
    public GameObject NearMenu;
    public GameObject NearMenuIsolate;
    // Start is called before the first frame update
    void Start()
    {
        ModeSelectionPanel.SetActive(true);
        ExploreIsolateCanvas.SetActive(false);
        NearMenu.SetActive(false);
        NearMenuIsolate.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Isolate()
    {
        ModeSelectionPanel.SetActive(false);
        ExploreIsolateCanvas.SetActive(true);
        //NearMenuIsolate.SetActive(true);
    }

    public void AllActivities()
    {
        ModeSelectionPanel.SetActive(false);
        ExploreManager.GetComponent<ExploreManager>().AllActivities();
        NearMenu.SetActive(true);
    }
}
