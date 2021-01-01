using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class workerScript : MonoBehaviour
{
    [SerializeField] private GameObject shoulder;
    [SerializeField] private GameObject thigh;
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject neck;

    [SerializeField] private GameObject workerText;
    [SerializeField] private GameObject ReportText;

    //[SerializeField] private GameObject shoulderText;
    //[SerializeField] private GameObject thighText;
    //[SerializeField] private GameObject backText;
    //[SerializeField] private GameObject neckText;

    //[SerializeField] private GameObject currentMenu;
    //[SerializeField] private GameObject resultsMenu;

    public bool active = false;
    public string fileName;
    public string filePath;
    public string ReportString = "";
    public string teststring = "test";

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

    private void Start()
    {
        //default file name to resolve unity error, if no start function it probably work.
        Debug.Log("IMU persistent data path:" + Application.persistentDataPath);
        filePath = Application.persistentDataPath + "/imuReports";
        

        fileName = string.Format("{0}/imuReport_{1}.txt",filePath,System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        workerText.SetActive(true);
    }

    private void Update()
    {
        if(active)
        {
            //activate worker name display
            workerText.SetActive(true);

            timer += Time.deltaTime;
            if (timer >= 1)
            {
                timeCount++;
                timer = 0;
                ReportString = gameObject.name + ", Time: " + System.DateTime.UtcNow.ToLocalTime() + ". ";
                if (shoulderBool)
                {
                    shoulderContent += "Time: " + timeCount.ToString() + " seconds ";
                    ReportString = ReportString + "Shoulder: " + (shoulder.transform.eulerAngles.y % 140).ToString("#.0")+ "\u00B0".ToString() + ". ";
                    //shoulderText.GetComponent<TextMeshProUGUI>().text = "Shoulder:" + (shoulder.transform.eulerAngles.y % 140).ToString("#.00");
                    shoulderContent += "  x:" + shoulder.transform.eulerAngles.x.ToString("#.0") +
                        "  y:" + (shoulder.transform.eulerAngles.y % 140).ToString("#.0") +
                        "  z:" + shoulder.transform.eulerAngles.z.ToString("#.0") + "\n";
                }
                //else
                //shoulderText.GetComponent<TextMeshProUGUI>().text = "";

                if (thighBool)
                {
                    thighContent += "Time: " + timeCount.ToString() + " seconds ";
                    ReportString = ReportString + "Thigh: " + (thigh.transform.eulerAngles.y % 60).ToString("#.00") + "\u00B0".ToString() + ". ";
                    //thighText.GetComponent<TextMeshProUGUI>().text = "Thigh:" + (thigh.transform.eulerAngles.y % 60).ToString("#.00");
                    thighContent += "Thigh:  x:" + thigh.transform.eulerAngles.x.ToString("#.00") +
                        "  y:" + (thigh.transform.eulerAngles.y % 60).ToString("#.00") +
                        "  z:" + thigh.transform.eulerAngles.z.ToString("#.00") + "\n";
                }
                //else
                //thighText.GetComponent<TextMeshProUGUI>().text = "";

                if (backBool)
                {
                    backContent += "Time: " + timeCount.ToString() + " seconds ";
                    ReportString = ReportString + "Back: " + (back.transform.eulerAngles.y % 120).ToString("#.00") + "\u00B0".ToString() + ". ";
                    //backText.GetComponent<TextMeshProUGUI>().text = "Back: " + (back.transform.eulerAngles.y % 120).ToString("#.00");
                    backContent += "Back:  x:" + back.transform.eulerAngles.x.ToString("#.00") +
                        "  y:" + (back.transform.eulerAngles.y % 120).ToString("#.00") +
                        "  z:" + back.transform.eulerAngles.z.ToString("#.00") + "\n";
                }
                //else
                //backText.GetComponent<TextMeshProUGUI>().text = "";

                if (neckBool)
                {
                    neckContent += "Time: " + timeCount.ToString() + " seconds ";
                    ReportString = ReportString + "Neck: " + (neck.transform.eulerAngles.y % 40).ToString("#.00") + "\u00B0".ToString() + ". ";  //+ ". \n \n";
                    //neckText.GetComponent<TextMeshProUGUI>().text = "Neck: " + (neck.transform.eulerAngles.y % 40).ToString("#.00");
                    neckContent += "Neck:  x:" + neck.transform.eulerAngles.x.ToString("#.00") +
                        "  y:" + (neck.transform.eulerAngles.y % 40).ToString("#.00") +
                        "  z:" + neck.transform.eulerAngles.z.ToString("#.00") + "\n";
                }
                //else
                //neckText.GetComponent<TextMeshProUGUI>().text = "";

                ReportString = ReportString + "\n \n";
                //ReportText.GetComponent<TextMeshProUGUI>().text = ReportString;
                //ReportString = ""; //Reset report string to empty after each frame
            }
            
        }
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
        
        //shoulderBool = false;
        //thighBool = false;
        //backBool = false;
        //neckBool = false;
        
        active = false;
        recordData = false;
        timer = 0;
        timeCount = 0;
    }
    /*
       public void select()
       {
           currentMenu.SetActive(false);
           resultsMenu.SetActive(true);
           recordData = true;
       }

       */
    public void TutorialInitial()
    {
        shoulderBool = false;
        thighBool = false;
        backBool = false;
        neckBool = false;
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
        //neckBool = true;
    }
    
}
