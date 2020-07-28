using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class rfidScript : MonoBehaviour
{
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject resultsPage;
    [SerializeField] private GameObject rfidPage;
    [SerializeField] private GameObject logFlag;
    [SerializeField] private GameObject rebarFlag;
    [SerializeField] private GameObject woodFlag;
    [SerializeField] private GameObject logReport;
    [SerializeField] private GameObject rebarReport;
    [SerializeField] private GameObject woodReport;

    private bool log;
    private bool wood;
    private bool rebar;

    // Start is called before the first frame update
    void Start()
    {
        log = false;
        wood = false;
        rebar = false;
        logReport.SetActive(false);
        woodReport.SetActive(false);
        rebarReport.SetActive(false);
        mainPage.SetActive(true);
        resultsPage.SetActive(false);
        rfidPage.SetActive(false);
    }

    public void logSelected()
    {
        if (log)
        {
            log = false;
            logFlag.SetActive(false);
        }
        else
        {
            log = true;
            logFlag.SetActive(true);
        }
    }


    public void WoodSelected()
    {
        if (wood)
        {
            wood = false;
            woodFlag.SetActive(false);
        }
        else
        {
            wood = true;
            woodFlag.SetActive(true);
        }
    }

    public void rebarSelected()
    {
        if (rebar)
        {
            rebar = false;
            rebarFlag.SetActive(false);
        }
        else
        {
            rebar = true;
            rebarFlag.SetActive(true);
        }
    }

    public void done()
    {
        mainPage.SetActive(false);
        resultsPage.SetActive(true);
        if (log)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Log") as GameObject[];
            int LogNumber = objectsWithTag.Length;
            logReport.SetActive(true);
            logReport.GetComponent<TextMeshProUGUI>().text = "Log Number:" + LogNumber;
        }
        if (wood)
        {
            // All wood object should have a tag with "wood"
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("wood") as GameObject[];
            int WoodNumber = objectsWithTag.Length;
            woodReport.SetActive(true);
            woodReport.GetComponent<TextMeshProUGUI>().text = "Wood Number:" + WoodNumber;
        }
        if (rebar)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("rebar") as GameObject[];
            int RebarNumber = objectsWithTag.Length;
            rebarReport.SetActive(true);
            rebarReport.GetComponent<TextMeshProUGUI>().text = "Rebar Number:" + RebarNumber;
        }
    }

    public void back()
    {
        log = false;
        wood = false;
        rebar = false;
        logFlag.SetActive(false);
        woodFlag.SetActive(false);
        rebarFlag.SetActive(false);
        logReport.SetActive(false);
        woodReport.SetActive(false);
        rebarReport.SetActive(false);
        mainPage.SetActive(true);
        resultsPage.SetActive(false);
        rfidPage.SetActive(false);
    }
}

