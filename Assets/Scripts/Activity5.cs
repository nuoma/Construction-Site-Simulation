using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.Vehicles;
using WSMGameStudio.HeavyMachinery;

public class Activity5 : MonoBehaviour
{

    #region parameters
    public GameObject DumpTruck;
    public GameObject Backhoe;

    private DumpTruckController _DumptruckController;
    private BackhoeController _BackhoeController;
    private WSMVehicleController _DumptruckVC;
    private WSMVehicleController _BackhoeVC;

    private bool enable = false; //enable flag for entire activity 5, trigger from menu button click.
    private bool step0 = true;
    private bool step1 = false;
    private bool step2 = false;
    private bool step3 = false;
    private bool step4 = false;
    private bool step5 = false;
    private bool step6 = false;
    private bool step7 = false;
    private bool Step3MoveEnable = false;
    private bool Step7MoveEnable = false;

    [SerializeField] private Transform[] DT_Trip1_Spots;
    [SerializeField] private Transform[] DT_Trip2_Spots;
    [SerializeField] private Transform[] CameraSite2Position;


    private float step1time = 3f;
    private int arrayPosition = 0;
    private float speed = 0.1f;
    private float speedBack = 0.1f;
    private float precision = 0.2f;

    //main camera
    [SerializeField] private GameObject mainCamera;
    Vector3 MainCameraResetPosition;
    #endregion

    #region main function body

    // Start is called before the first frame update
    public void start()
    {
        MainCameraResetPosition = mainCamera.transform.position;
        //Move the GetComponent to the Start method and cache the components on your private variables
        _DumptruckVC = DumpTruck.GetComponent<WSMVehicleController>();
        _DumptruckController = DumpTruck.GetComponent<DumpTruckController>();

        //only one backhoe in actual test
        _BackhoeVC = Backhoe.GetComponent<WSMVehicleController>();
        _BackhoeController = Backhoe.GetComponent<BackhoeController>();

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
            StartCoroutine(ActionSequence());
            //activate indicator arrow
            DumpTruck.transform.Find("Arrow").gameObject.SetActive(true);
        }
        
    }


    public void stop_5()
    {
        //deactivate indicator arrow
        DumpTruck.transform.Find("Arrow").gameObject.SetActive(false);
        //disable all coroutines
        StopAllCoroutines();
    }
    #endregion


    #region define coroutine

    IEnumerator ActionSequence()
    {
        //move camera to new location
        //mainCamera.transform.position = CameraSite2Position[0].transform.position;

        //Necessary for stone and backhoe front bucket initialize correctly.
        yield return new WaitForSeconds(1);

        //Step 0, collect stone from ground, with high mass rock as constraint.
        while (step0)
        {
            _BackhoeVC.AccelerationInput = 0.01f; //-1f, 1f
            yield return new WaitForSeconds(3);

            _BackhoeVC.AccelerationInput = 0; //reset for brake 
            _BackhoeVC.BrakesInput = 1; //_brakes = Mathf.Clamp01(value)
            yield return new WaitForSeconds(1);

            _BackhoeVC.BrakesInput = 0; //vehicle reset
            yield return new WaitForSeconds(1);

            step0 = false;
            step1 = true;
            yield return null;
        }   

        //Step 1,  raise backhoe frame and bucket
        //loader frame speed slowed to 0.2 to avoid rock bouncing
        while (step1) 
        {
            
            float duration = Time.time + 5f;

            while (Time.time < duration)
            {
                _BackhoeController.MoveFrontBucket(1, 1);//(int bucketInput, int frameInput)
                _BackhoeController.MoveLoaderFrame(1);//-1 = down | 0 = none | 1 = up
                yield return null;
            }
            if (Time.time > duration)
            {
                step1 = false;
                step2 = true;
                yield return new WaitForSeconds(2);
            }
        }

        
        //Step 2: backhoe backup
        while (step2)
        {
            //Backhoe steer and backup to 180 degree
            _BackhoeVC.AccelerationInput = -0.01f;
            _BackhoeVC.SteeringInput = 1; //-1 to 1
            yield return new WaitForSeconds(2);

            
            Debug.Log("Backhoe rotate:"+Backhoe.transform.eulerAngles.y);
            _BackhoeVC.BrakesInput = 1;
            yield return new WaitForSeconds(1);

            //Release brake, drive straight towards truck
            _BackhoeVC.BrakesInput = 0;
            _BackhoeVC.SteeringInput = 0; //-1 to 1
            _BackhoeVC.AccelerationInput = 0.001f;
            yield return new WaitForSeconds(3.5f);

            //Stop and brake
            _BackhoeVC.BrakesInput = 1;
            _BackhoeVC.AccelerationInput = 0;
            yield return new WaitForSeconds(1);

            //reset for next backhoe action
            _BackhoeVC.BrakesInput = 0;

            step2 = false;
            step3 = true;
            yield return new WaitForSeconds(3);
        }
        
        
        //Step 3: backhoe dump bucket
        while (step3)
        {
            float duration = Time.time + 5f;
            while (Time.time < duration)
            {
                _BackhoeController.MoveFrontBucket(-1, 0);//(int bucketInput, int frameInput)
                yield return null;
            }
            if (Time.time > duration)
            {
                step3 = false;
                step4 = true;
                yield return new WaitForSeconds(1);
            }
        }

        //Step 4: backhoe backup after dropping rock
        while (step4)
        {
            _BackhoeVC.AccelerationInput = -0.01f;
            yield return new WaitForSeconds(2);
            _BackhoeVC.BrakesInput = 1;
            _BackhoeVC.AccelerationInput = 0;
            yield return new WaitForSeconds(1);

            step4 = false;
            step5 = true;
            yield return null;
        }

        //Step 5: dumptruck drive and arrive at location
        while (step5)
        {
            
            Step3MoveEnable = true;

            while (Step3MoveEnable)
            {
                //Debug.Log("running!");
                DumpTruck.transform.position = Vector3.MoveTowards(DumpTruck.transform.position, DT_Trip1_Spots[arrayPosition].position, speed * Time.deltaTime);
                //DumpTruck.transform.position += transform.forward * Time.deltaTime * speed;

                Quaternion lookDirection = Quaternion.LookRotation(DT_Trip1_Spots[arrayPosition].position - DumpTruck.transform.position);
                DumpTruck.transform.rotation = Quaternion.RotateTowards(DumpTruck.transform.rotation, lookDirection, 30 * Time.deltaTime);
                //Debug.Log("Distance to WPT: " + Vector3.Distance(DumpTruck.transform.position, DT_Trip1_Spots[arrayPosition].position));
                if (Vector3.Distance(DumpTruck.transform.position, DT_Trip1_Spots[arrayPosition].position) < precision)
                {
                    if (arrayPosition < DT_Trip1_Spots.Length - 1)
                    {
                        arrayPosition++;
                        //Debug.Log("ArrayPosition: " + arrayPosition);
                    }
                    else
                    {
                        Step3MoveEnable = false; // Stop vehicle when reached to final point, and exit out of this movement loop
                    }
                }
                yield return null; //in while loop
            }

            step5 = false;
            step6 = true;
            yield return null;
        }

        //Step6: truck raise bed, vehicle forward and brake, then lower bed
        while (step6)
        {
            float Step4aDuration = Time.time + 5f;
            while (Time.time < Step4aDuration)
            {
                _DumptruckController.MoveDumpBed(1); //The MoveDumpBed method needs a parameter to indicate the direction of the movement: -1 = down | 0 = none | 1 = up
                yield return null;
            }

            _DumptruckVC.BrakesInput = 0;
            _DumptruckVC.AccelerationInput = 0.001f; 
            yield return new WaitForSeconds(3);
            _DumptruckVC.BrakesInput = 0.1f;
            _DumptruckVC.AccelerationInput = 0;
            yield return new WaitForSeconds(3);

            float Step4bDuration = Time.time + 6f;
            while (Time.time < Step4bDuration)
            {
                _DumptruckController.MoveDumpBed(-1);
                yield return null;
            }

            step6 = false;
            step7 = true;
            yield return null;
        }

        //Step7, dump truck trip back
        while (step7)
        {
            Step7MoveEnable = true;
            arrayPosition = 0;//reset from previous truck trip

            while (Step7MoveEnable)
            {
                //Debug.Log("running!");
                DumpTruck.transform.position = Vector3.MoveTowards(DumpTruck.transform.position, DT_Trip2_Spots[arrayPosition].position, speedBack * Time.deltaTime);
                //DumpTruck.transform.position += transform.forward * Time.deltaTime * speed;

                Quaternion lookDirection = Quaternion.LookRotation(DT_Trip2_Spots[arrayPosition].position - DumpTruck.transform.position);
                DumpTruck.transform.rotation = Quaternion.RotateTowards(DumpTruck.transform.rotation, lookDirection, 30 * Time.deltaTime);
                //Debug.Log("Distance to WPT: " + Vector3.Distance(DumpTruck.transform.position, DT_Trip2_Spots[arrayPosition].position));
                if (Vector3.Distance(DumpTruck.transform.position, DT_Trip2_Spots[arrayPosition].position) < precision)
                {
                    if (arrayPosition < DT_Trip2_Spots.Length - 1)
                    {
                        arrayPosition++;
                        //Debug.Log("ArrayPosition: " + arrayPosition);
                    }
                    else
                    {
                        Step7MoveEnable = false; // Stop vehicle when reached to final point, and exit out of this movement loop
                    }
                }
                yield return null; //in while loop
            }

            //reset main camera position
            //mainCamera.transform.position = MainCameraResetPosition;

            step7 = false;
            yield return null;
        }

        yield return null;
    }

    #endregion

}
