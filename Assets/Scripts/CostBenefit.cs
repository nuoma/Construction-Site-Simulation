using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

public class CostBenefit : MonoBehaviour
{

    public TextMeshProUGUI testinputfield;
    public TextMeshProUGUI TestText;
    public GameObject R3C2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TestText.GetComponent<TextMeshProUGUI>().text = "Value R2C3:" +testinputfield.GetComponent<TextMeshProUGUI>().text +". R3C2"+ R3C2.GetComponent<Interactable>().IsToggled;
    }
}
