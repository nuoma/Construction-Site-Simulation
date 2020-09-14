using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class imuScript : MonoBehaviour
{
    [SerializeField] private GameObject laborer;
    [SerializeField] private GameObject painter;
    [SerializeField] private GameObject carpenter1;
    [SerializeField] private GameObject carpenter2;
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject laborerPage;
    [SerializeField] private GameObject painterPage;
    [SerializeField] private GameObject carpenter1Page;
    [SerializeField] private GameObject carpenter2Page;
    [SerializeField] private GameObject resultsPage;

    [SerializeField] private GameObject imuPage;

    string filePath;
    string fileName;    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("IMU persistent data path:"+Application.persistentDataPath);
        //Debug.Log("IMU app data path:" + Application.dataPath);

        filePath = Application.persistentDataPath + "/imuReports";

        //filePath = Application.dataPath + "/imuReports";

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        fileName = string.Format("{0}/imuReport_{1}.txt",
            filePath,
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));                

        mainPage.SetActive(true);
        laborerPage.SetActive(false);
        painterPage.SetActive(false);
        carpenter1Page.SetActive(false);
        carpenter2Page.SetActive(false);
        resultsPage.SetActive(false);

        imuPage.SetActive(false);
    }

    public void laborerSelect()
    {
        File.WriteAllText(fileName, "IMU Report: ");
        File.AppendAllText(fileName, "Laborer\n");
        mainPage.SetActive(false);
        laborerPage.SetActive(true);
        laborer.GetComponent<workerScript>().active = true;
        laborer.GetComponent<workerScript>().fileName = fileName;
    }
    public void painterSelect()
    {
        File.WriteAllText(fileName, "IMU Report: ");
        File.AppendAllText(fileName, "Painter\n");
        mainPage.SetActive(false);
        painterPage.SetActive(true);
        painter.GetComponent<workerScript>().active = true;
        painter.GetComponent<workerScript>().fileName = fileName;
    }
    public void carpenter1Select()
    {
        File.WriteAllText(fileName, "IMU Report: ");
        File.AppendAllText(fileName, "Carpenter 1\n");
        mainPage.SetActive(false);
        carpenter1Page.SetActive(true);
        carpenter1.GetComponent<workerScript>().active = true;
        carpenter1.GetComponent<workerScript>().fileName = fileName;
    }
    public void carpenter2Select()
    {
        File.WriteAllText(fileName, "IMU Report: ");
        File.AppendAllText(fileName, "Carpenter 2\n");
        mainPage.SetActive(false);
        carpenter2Page.SetActive(true);
        carpenter2.GetComponent<workerScript>().active = true;
        carpenter2.GetComponent<workerScript>().fileName = fileName;
    }
    public void backSelected()
    {
        laborer.GetComponent<workerScript>().reset();
        painter.GetComponent<workerScript>().reset();
        carpenter1.GetComponent<workerScript>().reset();
        carpenter2.GetComponent<workerScript>().reset();

        resultsPage.SetActive(false);
        laborerPage.SetActive(false);
        painterPage.SetActive(false);
        carpenter1Page.SetActive(false);
        carpenter2Page.SetActive(false);
        mainPage.SetActive(true);

        fileName = string.Format("{0}/imuReport_{1}.txt",
            filePath,
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

        imuPage.SetActive(false);
    }

}
