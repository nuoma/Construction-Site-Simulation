using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using System;

public class bubble : MonoBehaviour
{
    public GameObject Bubble;
    public GameObject ConfirmButton;
    public float BubbleXMax;
    public float BubbleXMin;
    public float BubbleYMax;
    public float BubbleYMin;
    public float BubbleRangeXMin;
    public float BubbleRangeXMax;
    public float BubbleRangeYMin;
    public float BubbleRangeYMax;
    private float verticalMultiplier = 0;
    private float horizontalMultiplier = 0;



    // Start is called before the first frame update
    void Start()
    {
        ConfirmButton.SetActive(false);
        //var rand = new System.Random();
        //float InitialVertical = Convert.ToSingle(0.22f * (2*rand.NextDouble() - 1f));//random floating point value from 0 to 1
        //float InitialHorizontal = Convert.ToSingle(0.22f * (2 * rand.NextDouble() - 1f));
        //Debug.Log("Initial value V:"+InitialVertical +" , H:"+ InitialHorizontal);
        Bubble.transform.localPosition = new Vector3(-0.2f, -0.2f, Bubble.transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("BUbble:"+ Bubble.transform.localPosition);
        //limit button location
        if (Bubble.transform.localPosition.y < BubbleYMin)
            Bubble.transform.localPosition = new Vector3(Bubble.transform.localPosition.x, BubbleYMin, Bubble.transform.localPosition.z);
        if (Bubble.transform.localPosition.y > BubbleYMax)
            Bubble.transform.localPosition = new Vector3(Bubble.transform.localPosition.x, BubbleYMax, Bubble.transform.localPosition.z);
        if (Bubble.transform.localPosition.x < BubbleXMin)
            Bubble.transform.localPosition = new Vector3(BubbleXMin, Bubble.transform.localPosition.y, Bubble.transform.localPosition.z);
        if (Bubble.transform.localPosition.x > BubbleXMax)
            Bubble.transform.localPosition = new Vector3(BubbleXMax, Bubble.transform.localPosition.y, Bubble.transform.localPosition.z);

        //change bubble location
        if(Bubble.transform.localPosition.x < BubbleXMax && Bubble.transform.localPosition.x > BubbleXMin && Bubble.transform.localPosition.y < BubbleYMax && Bubble.transform.localPosition.y > BubbleYMin)
        Bubble.transform.localPosition = new Vector3(horizontalMultiplier, verticalMultiplier, Bubble.transform.localPosition.z);


        //if within range then show confirm button.
        if (Bubble.transform.localPosition.x < BubbleRangeXMax && Bubble.transform.localPosition.x > BubbleRangeXMin && Bubble.transform.localPosition.y < BubbleRangeYMax && Bubble.transform.localPosition.y > BubbleRangeYMin) ConfirmButton.SetActive(true); else ConfirmButton.SetActive(false);
    }

    //assign this to vertical slider
    public void verticalMoveMRTK(SliderEventData newValue)
    {
        float VValuePost = 2 * newValue.NewValue - 1.0f;//change scale from 0~1 to -1~1
        verticalMultiplier = VValuePost * 0.22f;
        //if (VValuePost > 0.15 || VValuePost < -0.15)
        //    verticalMultiplier = VValuePost;
        //else
        //   verticalMultiplier = 0;
    }

    public void horizontalMoveMRTK(SliderEventData newValue)
    {
        float HValuePost = 2 * newValue.NewValue - 1.0f;//change scale from 0~1 to -1~1
        horizontalMultiplier = HValuePost * 0.31f;
        //if (HValuePost > 0.15 || HValuePost < -0.15)
        //    horizontalMultiplier = HValuePost;
        //else
        //    horizontalMultiplier = 0;
    }
}
