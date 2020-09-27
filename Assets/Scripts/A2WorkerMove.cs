//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2WorkerMove : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float precision;
    [SerializeField] private Transform[] moveSpots;
    [SerializeField] private Animator WorkerAnimator;

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
            WorkerAnimator.SetBool("isWalk", false);
            WorkerAnimator.SetBool("isInspect", true);
            WorkerAnimator.SetBool("isIdle", false);
            transform.Find("prop").gameObject.SetActive(true);
            Debug.Log("Ground Worker is inspecting...");
            /*
            Animator.SetBool("isWalk", true);
            Animator.SetBool("isInspect", false);
            Animator.SetBool("isIdle", false);
            transform.Find("prop").gameObject.SetActive(false);
            Debug.Log("Ground Worker is walking...");
            
            //transform.position = Vector3.MoveTowards(transform.position, moveSpots[arrayPosition].position, 50 * Time.deltaTime);

            transform.position += transform.forward * Time.deltaTime * speed;

            Quaternion lookDirection = Quaternion.LookRotation(moveSpots[arrayPosition].position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, 50 * Time.deltaTime);

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
            
            if (animate)
            {
                WorkerAnimator.SetBool("moving", true);
            }
            */
            //inspect();
        }

        
    }

    public void Start() 
    {
        transform.Find("prop").gameObject.SetActive(false);
    }

    public void activate() //called by crane coroutine after step4 
    {
        enable = true;
    }

    public void stop()
    {
        enable = false;
        //WorkerAnimator.SetBool("moving", false);
        WorkerAnimator.SetBool("isWalk", false);
        WorkerAnimator.SetBool("isInspect", false);
        WorkerAnimator.SetBool("isIdle", true);
        transform.Find("prop").gameObject.SetActive(false);
        Debug.Log("Ground Worker is idle...");

    }




}
