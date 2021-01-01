using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class CostBenefit : MonoBehaviour
{

    //public TextMeshProUGUI testinputfield;
    public TextMeshProUGUI TestText;
    
    public GameObject R2C2;
    public GameObject R3C2;
    public GameObject R4C2;
    public GameObject R5C2;
    public GameObject R6C2;
    public GameObject R7C2;
    public TextMeshProUGUI R2C3;
    public TextMeshProUGUI R3C3;
    public TextMeshProUGUI R4C3;
    public TextMeshProUGUI R5C3;
    public TextMeshProUGUI R6C3;
    public TextMeshProUGUI R7C3;
    public TextMeshProUGUI R2C4;
    public TextMeshProUGUI R3C4;
    public TextMeshProUGUI R4C4;
    public TextMeshProUGUI R5C4;
    public TextMeshProUGUI R6C4;
    public TextMeshProUGUI R7C4;
    public GameObject R2C5;
    public GameObject R3C5;
    public GameObject R4C5;
    public GameObject R5C5;
    public GameObject R6C5;
    public GameObject R7C5;

    string filePath;
    string ReportFileName;
    string Content;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/CostBenefit";
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TestText.GetComponent<TextMeshProUGUI>().text = "Value R2C3:" +testinputfield.GetComponent<TextMeshProUGUI>().text +". R3C2"+ R3C2.GetComponent<Interactable>().IsToggled;
        TestText.GetComponent<TextMeshProUGUI>().text = "File save path:" + filePath;
    }

    public void SaveExit()
    {
        GetContent();
        //File.WriteAllText(ReportFileName, "Cost Benefit Report: ");
        //File.AppendAllText(ReportFileName, Content);
        ReportFileName = string.Format("{0}/CostBenefitReport_{1}.txt", filePath, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        File.WriteAllText(ReportFileName, Content);
        SceneManager.LoadScene(0);
    }

    public void Scene4Reset()
    {
        SceneManager.LoadScene(4);
    }

    private void GetContent()
    {
        Content = "";
        Content += "Activites,Sensors,Workers Needed,Cost($),ProcessingTime(min),ActivitiesDisturbance\n";
        Content += "Material Delivery,GPS,"+ R2C2.GetComponent<Interactable>().IsToggled + ","+ R2C3.GetComponent<TextMeshProUGUI>().text + ","+ R2C4.GetComponent<TextMeshProUGUI>().text + ","+ R2C5.GetComponent<Interactable>().IsToggled + "\n";
        Content += "Material Delivery,RFID," + R3C2.GetComponent<Interactable>().IsToggled + "," + R3C3.GetComponent<TextMeshProUGUI>().text + "," + R3C4.GetComponent<TextMeshProUGUI>().text + "," + R3C5.GetComponent<Interactable>().IsToggled + "\n";
        Content += "Scan Old Building,Laser Scanner," + R4C2.GetComponent<Interactable>().IsToggled + "," + R4C3.GetComponent<TextMeshProUGUI>().text + "," + R4C4.GetComponent<TextMeshProUGUI>().text + "," + R4C5.GetComponent<Interactable>().IsToggled + "\n";
        Content += "Scan Old Building,Drone," + R5C2.GetComponent<Interactable>().IsToggled + "," + R5C3.GetComponent<TextMeshProUGUI>().text + "," + R5C4.GetComponent<TextMeshProUGUI>().text + "," + R5C5.GetComponent<Interactable>().IsToggled + "\n";
        Content += "Worker Ergonomics,IMU," + R6C2.GetComponent<Interactable>().IsToggled + "," + R6C3.GetComponent<TextMeshProUGUI>().text + "," + R6C4.GetComponent<TextMeshProUGUI>().text + "," + R6C5.GetComponent<Interactable>().IsToggled + "\n";
        Content += "Fall Detection,RFID," + R7C2.GetComponent<Interactable>().IsToggled + "," + R7C3.GetComponent<TextMeshProUGUI>().text + "," + R7C4.GetComponent<TextMeshProUGUI>().text + "," + R7C5.GetComponent<Interactable>().IsToggled + "\n";

        /*
                 Content = "";
        Content += "Activites,Sensors,Resources,Workers Needed,Cost($),ProcessingTime(min),ActivitiesDisturbance\n";
        Content += "Material Delivery,GPS,Truck,"+ R2C2.GetComponent<Interactable>().IsToggled + ","+ R2C3.GetComponent<TextMeshProUGUI>().text + ","+ R2C4.GetComponent<TextMeshProUGUI>().text + ","+ R2C5.GetComponent<Interactable>().IsToggled + "\n";
        Content += "Material Delivery,RFID,Rebar in truck," + R3C2.GetComponent<Interactable>().IsToggled + "," + R3C3.GetComponent<TextMeshProUGUI>().text + "," + R3C4.GetComponent<TextMeshProUGUI>().text + "," + R3C5.GetComponent<Interactable>().IsToggled + "\n";
        Content += "Scan Old Building,Laser Scanner,old building," + R4C2.GetComponent<Interactable>().IsToggled + "," + R4C3.GetComponent<TextMeshProUGUI>().text + "," + R4C4.GetComponent<TextMeshProUGUI>().text + "," + R4C5.GetComponent<Interactable>().IsToggled + "\n";
        Content += "Scan Old Building,Drone,old building," + R5C2.GetComponent<Interactable>().IsToggled + "," + R5C3.GetComponent<TextMeshProUGUI>().text + "," + R5C4.GetComponent<TextMeshProUGUI>().text + "," + R5C5.GetComponent<Interactable>().IsToggled + "\n";
        Content += "Worker Ergonomics,IMU,Carpenter Painter and floorer Avatars," + R6C2.GetComponent<Interactable>().IsToggled + "," + R6C3.GetComponent<TextMeshProUGUI>().text + "," + R6C4.GetComponent<TextMeshProUGUI>().text + "," + R6C5.GetComponent<Interactable>().IsToggled + "\n";
        Content += "Fall Detection,RFID, worker 1and2and3," + R7C2.GetComponent<Interactable>().IsToggled + "," + R7C3.GetComponent<TextMeshProUGUI>().text + "," + R7C4.GetComponent<TextMeshProUGUI>().text + "," + R7C5.GetComponent<Interactable>().IsToggled + "\n";

         */
    }
}
