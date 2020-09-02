using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2WorkerMove : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float precision;
    [SerializeField] private Transform[] moveSpots;
    [SerializeField] private Animator Animator;

    private bool enable = false;
    private bool animate = false;
    private bool goBack = false;
    [HideInInspector] public bool tagged = false;
    private int arrayPosition = 0;


    // Update is called once per frame
    void Update()
    {
        //if (enable && tagged)
        if (enable)
        {
            Animator.SetBool("isWalk", true);
            Animator.SetBool("isInspect", false);
            Animator.SetBool("isIdle", false);
            transform.Find("prop").gameObject.SetActive(false);
            Debug.Log("Ground Worker is walking...");

            //transform.position = Vector3.MoveTowards(transform.position, moveSpots[arrayPosition].position, 50 * Time.deltaTime);
            transform.position += transform.forward * Time.deltaTime * speed;

           Quaternion lookDirection = Quaternion.LookRotation(moveSpots[arrayPosition].position - transform.position);
           transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, 30 * Time.deltaTime);

            if (Vector3.Distance(transform.position, moveSpots[arrayPosition].position) < precision)
            {
                if (arrayPosition < moveSpots.Length - 1)
                    arrayPosition++;
                else
                {
                    arrayPosition = 0;
                    enable = false;
                    animate = true;
                }
            }
        }

        if (animate)
        {
            Animator.SetBool("isInspect", true);
            Animator.SetBool("isWalk", false);
            Animator.SetBool("isIdle", false);
            transform.Find("prop").gameObject.SetActive(true);
            Debug.Log("Ground Worker Inspecting...");
        }
    }

    public void start() //called by crane coroutine after step4 
    {
        transform.Find("prop").gameObject.SetActive(false); //writing pad in hand shouldn't appear
        enable = true;
    }

    public void stop()
    {
        enable = false;
        Animator.SetBool("isIdle", true);
        Animator.SetBool("isWalk", false);
        Animator.SetBool("isInspect", false);
        transform.Find("prop").gameObject.SetActive(false);
        Debug.Log("Ground Worker Idle.");
    }

    public void inspect()
    {
        Animator.SetBool("isInspect", true);
        Animator.SetBool("isWalk", false);
        Animator.SetBool("isIdle", false);
        transform.Find("prop").gameObject.SetActive(true);
        Debug.Log("Ground Worker Inspecting...");
    }




}
