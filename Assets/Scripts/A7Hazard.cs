using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A7Hazard : MonoBehaviour
{
    public GameObject HazardWorker1; //manually fill in hazard worker
    public GameObject HazardWorker2;
    public GameObject HazardWorker3;



    private void OnTriggerEnter(Collider targetworker)
    {
        if (targetworker.gameObject.tag == "hazard")
        {
            if (targetworker.gameObject == HazardWorker1)
            {
                switchTag(HazardWorker1);
                Debug.Log("Worker 1 in hazard");
            }
            if (targetworker.gameObject == HazardWorker2)
            {
                switchTag(HazardWorker2);
                Debug.Log("Worker 2 in hazard");
            }
            if (targetworker.gameObject == HazardWorker3)
            {
                switchTag(HazardWorker3);
                Debug.Log("Worker 3 in hazard");
            }
            //switchTag(HazardWorker2);
            //switchTag(HazardWorker3);
            //targetworker.transform.GetChild(0).gameObject.SetActive(true);
            //HazardWorker1.transform.GetChild(0).gameObject.SetActive(true);
            //HazardWorker2.transform.GetChild(0).gameObject.SetActive(true);
            //HazardWorker3.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider targetworker)
    {
        if (targetworker.gameObject.tag == "hazard")
        {
            //Debug.Log("Object is within the hazard zone");
        }
    }

    private void OnTriggerExit(Collider targetworker)
    {
        if (targetworker.gameObject.tag == "hazard")
        {
            if (targetworker.gameObject == HazardWorker1)
            {
                switchTag(HazardWorker1);
            }
            if (targetworker.gameObject == HazardWorker2)
            {
                switchTag(HazardWorker2);
            }
            if (targetworker.gameObject == HazardWorker3)
            {
                switchTag(HazardWorker3);
            }
            //switchTag(HazardWorker1);
            //switchTag(HazardWorker2);
            //switchTag(HazardWorker3);
            //targetworker.transform.GetChild(0).gameObject.SetActive(false);
            //HazardWorker1.transform.GetChild(0).gameObject.SetActive(false);
            //HazardWorker2.transform.GetChild(0).gameObject.SetActive(false);
            //HazardWorker3.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void switchTag(GameObject Tag)
    {
        if (Tag.transform.GetChild(0).gameObject.activeSelf)
            Tag.transform.GetChild(0).gameObject.SetActive(false);
        else
            Tag.transform.GetChild(0).gameObject.SetActive(true);
    }

}
