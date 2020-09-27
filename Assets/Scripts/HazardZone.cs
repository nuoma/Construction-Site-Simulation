using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardZone : MonoBehaviour
{
    public GameObject HazardWorker1; //manually fill in hazard worker

    private Color m_oldColor = new Color (1f,1f,1f,0f);
    private Color m_red = new Color(1f, 0f, 0f, 0.1f);

    private void OnTriggerEnter(Collider targetworker)
    {
        if (targetworker.gameObject.tag == "hazard")
        {
            //Debug.Log("Object entered the hazard zone");
            Renderer render = GetComponent<Renderer>();
            m_oldColor = render.material.color;
            render.material.color = m_red;
            //switchTag(hazard1);
            HazardWorker1.transform.Find("Cube").gameObject.SetActive(true);
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
            //Debug.Log("Object left the hazard zone");
            Renderer render = GetComponent<Renderer>();
            render.material.color = m_oldColor;
            //switchTag(hazard1);
            HazardWorker1.transform.Find("Cube").gameObject.SetActive(false);
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
