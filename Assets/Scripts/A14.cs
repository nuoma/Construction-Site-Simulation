using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class A14 : MonoBehaviour
{

	#region parameters
	[SerializeField] private GameObject laborer;
	[SerializeField] private GameObject painter;
	[SerializeField] private GameObject carpenter1;
	[SerializeField] private GameObject carpenter2;
	[SerializeField] private GameObject WorkerSelectPage;
	[SerializeField] private GameObject ReportPage;
	[SerializeField] private GameObject SelectedWorkerText;
/*
	[SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject laborerPage;
    [SerializeField] private GameObject painterPage;
    [SerializeField] private GameObject carpenter1Page;
    [SerializeField] private GameObject carpenter2Page;
    
    [SerializeField] private GameObject resultsPage;
*/
	private bool laborerSelected;
	private bool painterSelected;
	private bool carpenter1Selected;
	private bool carpenter2Selected;

	private string worker1;
	private string worker2;
	private string worker3;
	private string worker4;

	string filePath;
    //string fileName; 
    string LFileName;
    string PFileName;
    string C1FileName;
    string C2FileName;

    public string l_string;
    public string p_string;
    public string c1_string;
    public string c2_string;
	#endregion
    
    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("IMU persistent data path:"+Application.persistentDataPath);
        //Debug.Log("IMU app data path:" + Application.dataPath);

        filePath = Application.persistentDataPath + "/imuReports";

        //filePath = Application.dataPath + "/imuReports";

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        //fileName = string.Format("{0}/imuReport_{1}.txt",filePath,System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

        PFileName = string.Format("{0}/imuReport_Painter_{1}.txt",filePath,System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")); 
        LFileName = string.Format("{0}/imuReport_Laborer_{1}.txt",filePath,System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        C1FileName = string.Format("{0}/imuReport_Carpenter1_{1}.txt",filePath,System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        C2FileName = string.Format("{0}/imuReport_Carpenter2_{1}.txt",filePath,System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));                

        //WorkerSelectPage.SetActive(false);
        //ReportPage.SetActive(false);
    }

    public void laborerSelect()
    {
    	laborerSelected = true;
    	worker1 = "Laborer, ";

        File.WriteAllText(LFileName, "IMU Report: ");
        File.AppendAllText(LFileName, "Laborer\n");    
    }

    public void painterSelect()
    {
		painterSelected = true;
		worker2 = "Painter, ";

        File.WriteAllText(PFileName, "IMU Report: ");
        File.AppendAllText(PFileName, "Painter\n");
       
        
    }
    public void carpenter1Select()
    {
    	carpenter1Selected = true;
    	worker3 = "Carpenter 1, ";

        File.WriteAllText(C1FileName, "IMU Report: ");
        File.AppendAllText(C1FileName, "Carpenter 1\n");
        
        
    }
    public void carpenter2Select()
    {
    	carpenter2Selected = true;
    	worker4 = "Carpenter 2, ";

        File.WriteAllText(C2FileName, "IMU Report: ");
        File.AppendAllText(C2FileName, "Carpenter 2\n");
    }

    //rewrite. Back button on report canvas. End all file write.
    public void backSelected()
    {
    	//reset workers
        laborer.GetComponent<workerScript>().reset();
        painter.GetComponent<workerScript>().reset();
        carpenter1.GetComponent<workerScript>().reset();
        carpenter2.GetComponent<workerScript>().reset();

        //reset file name using new date and time.
        //fileName = string.Format("{0}/imuReport_{1}.txt",filePath,System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

        //Close all canvas
        //WorkerSelectPage.SetActive(false);
        //ReportPage.SetActive(false);
        //SelectedWorkerText.GetComponent<TextMeshProUGUI>().text = "";
    }

    //Finish selection of worker, proceed to report
    public void done()
    {
    	//activate report canvas
    	//ReportPage.SetActive(true);

    	//activate workers selected
    	if(laborerSelected)
    	{
			laborer.GetComponent<workerScript>().active = true;
        	laborer.GetComponent<workerScript>().fileName = LFileName;
    	}
    	if(painterSelected)
    	{
			painter.GetComponent<workerScript>().active = true;
        	painter.GetComponent<workerScript>().fileName = PFileName;
    	}
    	if(carpenter1Selected)
    	{
			carpenter1.GetComponent<workerScript>().active = true;
        	carpenter1.GetComponent<workerScript>().fileName = C1FileName;
    	}
    	if(carpenter2Selected)
    	{
			carpenter2.GetComponent<workerScript>().active = true;
        	carpenter2.GetComponent<workerScript>().fileName = C2FileName;
    	}
    }

    
    void Update()
    {
        //Deprecated, old canvas display style.
        //SelectedWorkerText.GetComponent<TextMeshProUGUI>().text = "Selected:" + worker1 + worker2 + worker3 + worker4 +".";
        
        l_string = laborer.GetComponent<workerScript>().ReportString;
        p_string = painter.GetComponent<workerScript>().ReportString;
        c1_string = carpenter1.GetComponent<workerScript>().ReportString;
        c2_string = carpenter2.GetComponent<workerScript>().ReportString;
}
    
}
