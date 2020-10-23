using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSConstraint : MonoBehaviour
{
    //prevent laser scanner from falling under ground
    public GameObject tripod;
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;
    private float yMin; //they all share the same vertical min value
    public bool begin = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (tripod.transform.localPosition.y < yMin)
            tripod.transform.localPosition = new Vector3(tripod.transform.localPosition.x, yMin, tripod.transform.localPosition.z);
        if (target1.transform.localPosition.y < yMin)
            target1.transform.localPosition = new Vector3(target1.transform.localPosition.x, yMin, target1.transform.localPosition.z);
        if (target2.transform.localPosition.y < yMin)
            target2.transform.localPosition = new Vector3(target2.transform.localPosition.x, yMin, target2.transform.localPosition.z);
        if (target3.transform.localPosition.y < yMin)
            target3.transform.localPosition = new Vector3(target3.transform.localPosition.x, yMin, target3.transform.localPosition.z);
    }

    public void BeginSignal() //after entire LS activity moved location already
    {
        yMin = tripod.transform.localPosition.y;
    }
}
