using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.UI;

public class sceneSwitcher : MonoBehaviour
{
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    private bool b1;
    private bool b2;

    private void Start()
    {
        //button2.GetComponent<Interactable>().enabled = false;
        //button3.GetComponent<Interactable>().enabled = false;
        //button4.GetComponent<Interactable>().enabled = false;
    }

    private void Update()
    {
        //if(b1) button2.GetComponent<Interactable>().enabled = true;

        //if (b2) 
        //{ 
        //    button3.GetComponent<Interactable>().enabled = true; button4.GetComponent<Interactable>().enabled = true; }
    }
    public void Scene1()
    {
        SceneManager.LoadScene(1);
        b1 = true;
    }
    public void Scene2()
    {
        SceneManager.LoadScene(2);
        b2 = true;
    }
    public void Scene3()
    {
        SceneManager.LoadScene(3);
    }
    public void Scene4()
    {
        SceneManager.LoadScene(4);
    }

    public void AutoScene()
    {
        SceneManager.LoadScene(5);
    }

    public void ManualScene()
    {
        SceneManager.LoadScene(6);
    }
}
