using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{
    [SerializeField] private Animator TestAnimator;
   // [SerializeField] private GameObject;

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("prop").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))//walk
        {
            //transform.Rotate(0.0f, -90.0f, 0.0f, Space.World);
            TestAnimator.SetBool("isWalk",true);
            TestAnimator.SetBool("isInspect", false);
            TestAnimator.SetBool("isIdle", false);
            transform.Find("prop").gameObject.SetActive(false);
            Debug.Log("walk");
        }
        if (Input.GetKeyDown("w"))//inspect
        {
            TestAnimator.SetBool("isInspect", true);
            TestAnimator.SetBool("isWalk", false);
            TestAnimator.SetBool("isIdle", false);
            transform.Find("prop").gameObject.SetActive(true);
            Debug.Log("inspect");
        }
        if (Input.GetKeyDown("e"))
        {
            TestAnimator.SetBool("isIdle", true);
            TestAnimator.SetBool("isWalk", false);
            TestAnimator.SetBool("isInspect", false);
            transform.Find("prop").gameObject.SetActive(false);
            Debug.Log("idle");
        }
    }
}
