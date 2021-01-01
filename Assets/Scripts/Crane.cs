using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{
    #region parameters
    // speeds
    public float turnSpeed;             // rate at which the crane can rotate on the Y axis
    public float hookVerticalSpeed;     // rate at which the hook can be raised and lowered
    public float hookHorizontalSpeed;   // rate at which the hook can be moved horizontally along the crane

    // hook
    public float hookRaiseLimit;        // the highest the hook can be raised
    public float hookLowerLimit;        // the lowest the hook can be lowered
    public float hookForwardsLimit;     // the furthest forward the hook can be moved
    public float hookBackwardsLimit;    // the furthest backwards the hook can be moved
    public float step8limit;

    // components
    public GameObject craneTop;         // top of the crane which rotates
    public GameObject hook;             // the hook object
    public GameObject container;        // the container or load
    public GameObject TargetArea;            // truck, target region
    public GameObject WorkerGround;
    public GameObject WorkerTop;

    private int arrayPosition = 0; //added to seek target position
    private bool step2flag = true; //flag for step 2
    private bool step3flag = true; //flag for step 2
    private bool step6flag = true; //flag for step 6
    private bool step7flag = true; //flag for step 7
    private bool GroundWorkerApproach = true;// Insert ground worker tie load
    [SerializeField] private Transform[] moveSpots;
    [SerializeField] private float precision;

    private bool enable = false;
    #endregion

    #region Crane action definition
    // rotates the crane clockwise along the Y axis
    public void TurnClockwise ()
    {
        craneTop.transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
    }

    // rotates the crane anti-clockwise along the Y axiss
    public void TurnAntiClockwise ()
    {
        craneTop.transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
    }

    // moves the hook down
    public void LowerHook ()
    {
        if(hook.transform.localPosition.y > hookLowerLimit)
        {
            hook.transform.localPosition += Vector3.down * hookVerticalSpeed * Time.deltaTime;
        }
    }

    // moves the hook up
    public void RaiseHook ()
    {
        if(hook.transform.localPosition.y < hookRaiseLimit)
        {
            hook.transform.localPosition += Vector3.up * hookVerticalSpeed * Time.deltaTime;
        }
    }

    // moves the hook forwards horizontally along the crane
    public void MoveHookForward ()
    {
        if(hook.transform.localPosition.x > hookForwardsLimit)
        {
            hook.transform.localPosition += Vector3.left * hookHorizontalSpeed * Time.deltaTime;
        }
    }

    // moves the hook backwards horizontally along the crane
    public void MoveHookBackwards ()
    {
        if(hook.transform.localPosition.x < hookBackwardsLimit)
        {
            hook.transform.localPosition += Vector3.right * hookHorizontalSpeed * Time.deltaTime;
        }
    }
    #endregion

    #region Coroutine steps for crane
    private IEnumerator Sequence()
    {
        //0.test hook
        /*
        while (hook.transform.localPosition.y > hookLowerLimit)
        {
            LowerHook();
            yield return null;
        }
        */

        //0. activate ground worker animation
        WorkerGround.GetComponent<A2WorkerMove>().activate();
        WorkerTop.GetComponent<A2WorkerMove>().activate();

        //1.hook go up top
        while (hook.transform.localPosition.y < hookRaiseLimit) 
        {
            RaiseHook();
            yield return null;
        }
        
        //2.rotate angle to load 
        while (step2flag)
        {
            TurnClockwise();
            //calculate look angle from crane top pivot point to target location
            Quaternion lookDirection = Quaternion.LookRotation(moveSpots[0].position - transform.position); //moveSpots is first target location, transform position is top tower crane
            float angle = Quaternion.Angle(lookDirection, craneTop.transform.rotation); //the angle between look direction and actual crane top pivot angle
            //Debug.Log(Mathf.Abs(angle));
            bool sameRotation = Mathf.Abs(angle) > 90f; //after test, desire angle is 90, due to top crane model coordinate initial direction. Container placed directly before crane, use orthognal view to adjust.
            if (sameRotation) //rotate to desired location.
            {
                step2flag = false; //set flag to terminate step 2 rotation
            }
            yield return null;
        }

        //3.move hook to load, hack is alway move hook backwards. Same idea based on step 2
        while (step3flag)
        {
            MoveHookBackwards();// let hook move backwards
            //hook.transform.position.x - truck.transform.position.x //calculate distance between load and hook.
            float distance = Vector2.Distance(new Vector2(hook.transform.position.x, hook.transform.position.z), new Vector2(moveSpots[0].transform.position.x, moveSpots[0].transform.position.z));
            /*
             * Debug.Log(distance);
            Debug.Log("hook original" + hook.transform.position);
            Debug.Log("container original" + moveSpots[0].transform.position);
            Debug.Log("hook"+ new Vector2(hook.transform.localPosition.x, hook.transform.localPosition.z));
            Debug.Log("container"+ new Vector2(moveSpots[0].position.x, moveSpots[0].position.z));
            */
            Debug.Log("Step3 Distance:"+distance);
            bool distancebool = Mathf.Abs(distance) < 0.003f; //distance precision value
            if ( distancebool ) //rotate to desired location.
            {
                step3flag = false; //set flag to terminate step 3 rotation
            }
            yield return null;
        }
        //4.hook go down bottom
        while (hook.transform.localPosition.y > hookLowerLimit)
        {
            LowerHook();    
            yield return null;
        }

        //4+: insert action so that worker move towards load, perform animation and backup.
        while (GroundWorkerApproach) //default is true
        {
            //WorkerGround.GetComponent<A2WorkerMove>().activate();//jump to A2WorkerMove.cs
            // first move, then stop and animate, then move back, then continue coroutine?
            yield return new WaitForSeconds(2);

            //WorkerGround.GetComponent<A2WorkerMove>().inspect();
            //yield return new WaitForSeconds(6);

            //WorkerGround.GetComponent<A2WorkerMove>().stop();
            //yield return new WaitForSeconds(1);

            GroundWorkerApproach = false;
            yield return null;
        }

        //5.hook go up top
        while (hook.transform.localPosition.y < hookRaiseLimit)
        {
            RaiseHook();
            yield return null;
        }
        //6.rotate to target area, mod based on step 2, change moveDpots to 1 for next target location, also change target location angle to 270 and use orthognal view to adjuist truck on right side of crane, also change step 6 flag
        while (step6flag)
        {
            TurnAntiClockwise();
            //calculate look angle from crane top pivot point to target location
            Quaternion lookDirection = Quaternion.LookRotation(moveSpots[1].position - transform.position); //moveSpots is first target location, transform position is top tower crane
            float angle = Quaternion.Angle(lookDirection, craneTop.transform.rotation); //the angle between look direction and actual crane top pivot angle
            Debug.Log("Step 6 angle:"+angle);
            bool sameRotation = Mathf.Abs(angle) < 95f; //after test, desire angle is 90, due to top crane model coordinate initial direction. Container placed directly before crane, use orthognal view to adjust.
            if (sameRotation) //rotate to desired location.
            {
                step6flag = false; //set flag to terminate step 2 rotation
            }
            yield return null;
        }
        //7.move hook to target area, same idea as step 3. Hack, container close to crane, target area far from crane.
        while (step7flag)
        {
            MoveHookForward();// let hook move forward, due to small hack mentioned above 
            float distance = Vector2.Distance(new Vector2(hook.transform.position.x, hook.transform.position.z), new Vector2(moveSpots[1].transform.position.x, moveSpots[1].transform.position.z));//calculate distance between load and target area.
            bool distancebool = Mathf.Abs(distance) < 0.005f; //distance precision value
            Debug.Log("Step7 distance: "+distance);
            //Debug.Log("hook(x,z):" + hook.transform.position.x + hook.transform.position.z);
            //Debug.Log("spot(x,z):" + moveSpots[1].transform.position.x + moveSpots[1].transform.position.z);
            //Debug.Log("hook transform raw:"+hook.transform.position);
            //Debug.Log("Distance(hook(x,z),sport(x,z))"+distance);
            if (distancebool) //rotate to desired location.
            {
                step7flag = false; //set flag to terminate step 7 rotation
            }
            yield return null;
        }
        //8.move hook down to bottom
        while (hook.transform.localPosition.y > step8limit)
        {
            LowerHook();
            yield return null;
        }
        //9.move hook up to top
        while (hook.transform.localPosition.y < hookRaiseLimit)
        {
            RaiseHook();
            yield return null;
        }

        //TODO: insert action so that worker move towards load, perform animation and backup.


    }
#endregion

    #region Main Function
    public void start()
    {
        container.transform.Find("Arrow").gameObject.SetActive(true);
        if (enable)
        {
            enable = false;
        }
        else
        {
            enable = true;
        }

        if (enable)
        {
            StartCoroutine(Sequence());
        }
            
    }

    public void stop()
    {
        container.transform.Find("Arrow").gameObject.SetActive(false);
        StopCoroutine(Sequence());
    }
    #endregion
}



