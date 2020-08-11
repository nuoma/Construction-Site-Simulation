using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2WorkerMove : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float precision;
    [SerializeField] private Transform[] moveSpots;
    [SerializeField] private Animator workAnimator;
    [SerializeField] private Animator walkAnimator;

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
            //transform.position = Vector3.MoveTowards(transform.position, moveSpots[arrayPosition].position, speed * Time.deltaTime);
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
        }

        if (animate)
        {
            workAnimator.SetBool("moving", true);
        }
    }

    public void start() //called by crane coroutine after step4 
    {
        walkAnimator.SetBool("moving", true);
        enable = true;
    }

    public void stop()
    {
        enable = false;
        walkAnimator.SetBool("moving", false);

    }




}
