using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class LegacyWorkerScript : MonoBehaviour
{
    [SerializeField] private GameObject shoulder;
    [SerializeField] private GameObject thigh;
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject neck;
    [SerializeField] private GameObject shoulderText;
    [SerializeField] private GameObject thighText;
    [SerializeField] private GameObject backText;
    [SerializeField] private GameObject neckText;

    [SerializeField] private GameObject currentMenu;
    [SerializeField] private GameObject resultsMenu;

    public bool active = false;
    public string fileName;

    private bool shoulderBool = false;
    private bool thighBool = false;
    private bool backBool = false;
    private bool neckBool = false;
    private bool recordData = false;
    private float timer = 0;
    private int timeCount = 0;
    private string shoulderContent = "\nShoulder Data\n\n";
    private string thighContent = "\nThigh Data\n\n";
    private string backContent = "\nBack Data\n\n";
    private string neckContent = "\nNeck Data\n\n";


    private void Update()
    {
        if(recordData)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                timeCount++;
                timer = 0;                

                if (shoulderBool)
                {
                    shoulderContent += "Time: " + timeCount.ToString() + " seconds ";
                    shoulderText.GetComponent<TextMeshProUGUI>().text = (shoulder.transform.eulerAngles.y % 140).ToString("#.00");
                    shoulderContent += "  x:" + shoulder.transform.eulerAngles.x.ToString("#.00") +
                        "  y:" + (shoulder.transform.eulerAngles.y % 140).ToString("#.00") +
                        "  z:" + shoulder.transform.eulerAngles.z.ToString("#.00") + "\n";
                }
                else
                    shoulderText.GetComponent<TextMeshProUGUI>().text = "";

                if (thighBool)
                {
                    thighContent += "Time: " + timeCount.ToString() + " seconds ";
                    thighText.GetComponent<TextMeshProUGUI>().text = (thigh.transform.eulerAngles.y % 60).ToString("#.00");
                    thighContent += "Thigh:  x:" + thigh.transform.eulerAngles.x.ToString("#.00") +
                        "  y:" + (thigh.transform.eulerAngles.y % 60).ToString("#.00") +
                        "  z:" + thigh.transform.eulerAngles.z.ToString("#.00") + "\n";
                }
                else
                    thighText.GetComponent<TextMeshProUGUI>().text = "";

                if (backBool)
                {
                    backContent += "Time: " + timeCount.ToString() + " seconds ";
                    backText.GetComponent<TextMeshProUGUI>().text = (back.transform.eulerAngles.y % 120).ToString("#.00");
                    backContent += "Back:  x:" + back.transform.eulerAngles.x.ToString("#.00") +
                        "  y:" + (back.transform.eulerAngles.y % 120).ToString("#.00") +
                        "  z:" + back.transform.eulerAngles.z.ToString("#.00") + "\n";
                }
                else
                    backText.GetComponent<TextMeshProUGUI>().text = "";

                if (neckBool)
                {
                    neckContent += "Time: " + timeCount.ToString() + " seconds ";
                    neckText.GetComponent<TextMeshProUGUI>().text = (neck.transform.eulerAngles.y % 40).ToString("#.00");
                    neckContent += "Neck:  x:" + neck.transform.eulerAngles.x.ToString("#.00") +
                        "  y:" + (neck.transform.eulerAngles.y % 40).ToString("#.00") +
                        "  z:" + neck.transform.eulerAngles.z.ToString("#.00") + "\n";
                }
                else
                    neckText.GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void shoulderSelect()
    {
        shoulderBool = !shoulderBool;
    }
    public void thighSelect()
    {
        thighBool = !thighBool;
    }
    public void backSelect()
    {
        backBool = !backBool;
    }
    public void neckSelect()
    {
        neckBool = !neckBool;
    }
    public void reset()
    {
        if (shoulderBool)
        {
            File.AppendAllText(fileName, shoulderContent);
            shoulderContent = "\nShoulder Data\n\n";
        }
        if (thighBool)
        {
            File.AppendAllText(fileName, thighContent);
            thighContent = "\nThigh Data\n\n";
        }
        if (backBool)
        {
            File.AppendAllText(fileName, backContent);
            backContent = "\nBack Data\n\n";
        }
        if (neckBool)
        {
            File.AppendAllText(fileName, neckContent);
            neckContent = "\nNeck Data\n\n";
        }
        shoulderBool = false;
        thighBool = false;
        backBool = false;
        neckBool = false;
        active = false;
        recordData = false;
        timer = 0;
        timeCount = 0;
    }
    public void select()
    {
        currentMenu.SetActive(false);
        resultsMenu.SetActive(true);
        recordData = true;
    }
}
