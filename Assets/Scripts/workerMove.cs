using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class workerMove : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float precision;
    [SerializeField] private Transform[] moveSpots;
    [SerializeField] private Animator workerAnimator;

    private bool enable;
    [HideInInspector] public bool tagged;
    private int arrayPosition;

    // Start is called before the first frame update
    void Start()
    {
        enable = false;
        tagged = false;
        arrayPosition = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (enable && tagged)
        if (enable)
        {
            //transform.position = Vector3.MoveTowards(transform.position, moveSpots[arrayPosition].position, 0.05f * Time.deltaTime);
            transform.position += transform.forward * Time.deltaTime * speed;

            Quaternion lookDirection = Quaternion.LookRotation(moveSpots[arrayPosition].position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, 50 * Time.deltaTime);

            if (Vector3.Distance(transform.position, moveSpots[arrayPosition].position) < precision)
            {
                if (arrayPosition < moveSpots.Length - 1)
                    arrayPosition++;
                else
                    arrayPosition = 0;
            }
        }
    }

    public void start()
    {

        enable = true;
        workerAnimator.SetBool("moving", true);
        /*
        if (enable)
        {
            workerAnimator.SetBool("moving", false);
            enable = false;
        }
        else
        {
            workerAnimator.SetBool("moving", true);
            enable = true;
        }
        */
    }

    public void stop()
    {
        enable = false;
        workerAnimator.SetBool("moving", false);
    }
}
