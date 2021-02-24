using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVehicle : MonoBehaviour
{
    public float accelerationForce;
    public float brakingForce;
    public float rotationForce;
    public float strafeForce;
    public Rigidbody vehicle;
    private GameObject FrontLeftWheel, FrontRightWheel;
    private Vector3 FLWpos;
    private Vector3 FRWpos;
    private Transform customCenterPoint;
    public Vector3 boxShape = new Vector3(0.25f, 0.04f, 0.27f);

    private float maxResistAcceleration;
    private float sumResistAcceleration = 0f;
    
    void Start()
    {
        vehicle = GetComponent<Rigidbody>();
        vehicle.maxAngularVelocity = 2f;
        
        FrontLeftWheel = vehicle.transform.Find("FrontLeftWheel").gameObject; // Get specific child component
        FrontRightWheel = vehicle.transform.Find("FrontRightWheel").gameObject;
        FLWpos = FrontLeftWheel.transform.position;
        FRWpos = FrontRightWheel.transform.position;

        maxResistAcceleration = accelerationForce * 20f;
        
        customCenterPoint = vehicle.transform.Find("customCenterPoint").gameObject.transform;
    }

    private void OnDrawGizmos()
    {
        //visualising FWD boxcast
        Gizmos.color = new Color(0, 0, 255);
        Gizmos.DrawWireCube(FLWpos, boxShape);
        Gizmos.color = new Color(255, 0, 0);
        Gizmos.DrawWireCube(FRWpos, boxShape);
    }

    void Update()
    {
        float localVelocity = transform.InverseTransformDirection(vehicle.velocity).z;
        Debug.Log("<color=red>Velocity "+localVelocity+"</color>");
        
        movement();
        constrainResistDrag();
    }

    private void constrainResistDrag()
    {
            if (coroutinesList.Count > 1) // if coroutine already running, stop all coroutines except the latest one
            {
                Debug.Log("<color=yellow>Identified multiple coroutines.</color>");
                for (var c = 0; c < coroutinesList.Count; c++)
                {
                    Debug.Log("<color=blue>Starting closure. Loop: "+c+"</color><color=blue> coroutinesList Length: "+coroutinesList.Count+"</color>");
                    StopCoroutine(coroutinesList[c]);
                    Debug.Log("<color=red>Removing index "+c+" from list</color>");
                    coroutinesList.RemoveAt(c);
                    Debug.Log("<color=red>Removed index. coroutinesList length is: "+coroutinesList.Count+"</color>");
                }
                Debug.Log("<color=green>Closure loop finished. coroutinesList length is: "+coroutinesList.Count+"</color>");
            }
    }

    private IEnumerator resistDrag()
    {
        Debug.Log("<color=yellow>Resist Drag called </color>");
        
        var resistAcceleration = maxResistAcceleration; //reducing the acceleration force as its unlikely the vehicle is at max acceleration
        //Debug.Log("<color=blue>resistAcceleration outside loop: "+ resistAcceleration +" </color>");

        for (var i = 0; i < 65; i++) // run for 2.5 seconds
        {
//            if (coroutinesList.Count > 1) // if coroutine already running, stop all coroutines except this one
//            {
//                Debug.Log("<color=yellow>Identified multiple coroutines.</color>");
//                for (var c = 0; c < coroutinesList.Count; c++)
//                {
//                    Debug.Log("<color=blue>Starting closure. Loop: "+c+"</color><color=blue> coroutinesList Length: "+coroutinesList.Count+"</color>");
//                    StopCoroutine(coroutinesList[c]);
//                    Debug.Log("<color=red>Removing index "+c+" from list</color>");
//                    coroutinesList.RemoveAt(c);
//                    Debug.Log("<color=red>Removed index. coroutinesList length is: "+coroutinesList.Count+"</color>");
//                }
//                Debug.Log("<color=green>Closure loop finished. coroutinesList length is: "+coroutinesList.Count+"</color>");
//            }
            
            if (Input.GetAxis("Vertical") == 1f) // if player starts accelerating again, stop coroutine here
            {
                //Debug.Log(Input.GetAxis("Vertical"));
                Debug.Log("<color=red>Yield break</color>");
                
                yield break; 
            }
            if (Input.GetAxis("Vertical") == -1f) // if player brakes fully, stop coroutine here
            {
                //Debug.Log(Input.GetAxis("Vertical"));
                Debug.Log("<color=red>Yield break -ve</color>");
                
                yield break; 
            }
            
            // to account for multiple instances of this coroutine running and adding exponentially more accelerationForce
            // implement a max accelerationForce limit here
            sumResistAcceleration += resistAcceleration; // keeping record of sum of acceleration force across all instances of this coroutine
            
            if ( sumResistAcceleration > maxResistAcceleration)
            {
                resistAcceleration = 0;
            } else if (maxResistAcceleration > sumResistAcceleration)
            {
                resistAcceleration = Mathf.Abs(maxResistAcceleration - sumResistAcceleration);
            }
            vehicle.AddForce(vehicle.transform.forward * resistAcceleration * Time.deltaTime, ForceMode.Acceleration);
            resistAcceleration *= 0.95f;
            sumResistAcceleration = 0f;
            //Debug.Log("<color=purple>resistAcceleration inside loop: "+ resistAcceleration +"</color>");

            Debug.Log("Yielding OUT "+i);
            yield return new WaitForSeconds(0.05f); // runs once every 0.1 seconds
        }
        Debug.Log("<color=green>ResistDrag loop finished.</color>");
        
        coroutinesList.Clear();
        Debug.Log("<color=green>After loop. coroutinesList length is: </color>"+coroutinesList.Count); 
    }
    
    private List<Coroutine> coroutinesList = new List<Coroutine>();
    private void movement()
    {
        //StartCoroutine(Speedometer());
        
        if (Input.GetButtonUp("Vertical") && Input.GetAxis("Vertical") > 0f)
        {
            coroutinesList.Add(StartCoroutine(resistDrag()));
            Debug.Log("<color=lime>In Movement() coroutineList length is: </color>"+coroutinesList.Count);
//            if (coroutinesList.Count > 1) // if coroutine already running, stop all coroutines except this one
//            {
//                Debug.Log("<color=yellow>Identified multiple coroutines.</color>");
//                for (var c = 0; c < coroutinesList.Count; c++)
//                {
//                    Debug.Log("<color=blue>Starting closure. Loop: "+c+"</color><color=blue> coroutinesList Length: "+coroutinesList.Count+"</color>");
//                    StopCoroutine(coroutinesList[c]);
//                    Debug.Log("<color=red>Removing index "+c+" from list</color>");
//                    coroutinesList.RemoveAt(c);
//                    Debug.Log("<color=red>Removed index. coroutinesList length is: "+coroutinesList.Count+"</color>");
//                }
//                Debug.Log("<color=green>Closure loop finished. coroutinesList length is: "+coroutinesList.Count+"</color>");
//            }
        }
        
        if (Input.GetButton("Vertical"))
        {
            if (isGrounded())
            {
                var v = Input.GetAxis("Vertical");
                //var applyForce = (v >= 0.1f) ? accelerationForce : accelerationForce * 0.7f; // if input is negative, make the deacceleration force weaker

                var applyForce = accelerationForce;
                //Debug.Log("<color=blue>Velocity: "+ localVelocity +"</color>");
                float localVelocity = transform.InverseTransformDirection(vehicle.velocity).z; // velocity
                if (localVelocity > 0f && v < 0f) // if driving forward and input is reverse
                {
                    applyForce = brakingForce;
                }
                else
                {
                    applyForce = accelerationForce * 1.4f; //applying a little extra force to further constrain ResistDrag() exponential speed issue
                }
                
                vehicle.AddForceAtPosition(transform.forward * applyForce * v * Time.deltaTime, customCenterPoint.position);
            }
        }
        
        if (Input.GetButton("Horizontal"))
        {
            if (isGrounded())
            {
                float localVelocity = transform.InverseTransformDirection(vehicle.velocity).z;
                float zAxisDirection;
                if (localVelocity > 2f)
                {
                    zAxisDirection = 1f;
                } else if (localVelocity < -2f) 
                {
                    zAxisDirection = -1f;
                }
                else // if at very low speeds, do not turn. Prevents turning fluctuating between forward and reverse directions.
                     // Also acts as a full steering lock 
                {
                    zAxisDirection = 0f;
                }

                var h = Input.GetAxis("Horizontal");
                
                var torque = transform.up * h * rotationForce * zAxisDirection;
                vehicle.AddTorque(torque * Time.deltaTime);
                
                // adding horizontal force
                var strafe = (h > 0) ? vehicle.transform.right : -vehicle.transform.right;
                strafe *= strafeForce;
                vehicle.AddForceAtPosition(strafe * Time.deltaTime, customCenterPoint.position, ForceMode.Acceleration);
            }
        }
    }
    
    private IEnumerator speedometer()
    {
        float localVelocity2 = transform.InverseTransformDirection(vehicle.velocity).z; // velocity
        Debug.Log("<color=blue>Velocity: "+ localVelocity2 +"</color>");
        yield return new WaitForSeconds(0.5f);
    }

    private bool isGrounded()
    {
        // Grabbing global coords of temp objects so I can use gizmos to draw a wire box, will use local position and remove temp objects later
        FLWpos = FrontLeftWheel.transform.position;
        FRWpos = FrontRightWheel.transform.position;
        // Now that we have local positions, use them to create two BoxCasts
        
        int FLWOverlap = Physics.OverlapBox(FLWpos, boxShape).Length; //always returns at least 1 as it is overlapping with the vehicle's collision mesh
        //Debug.Log("<color=blue>FLWOverlap count: " + FLWOverlap + "</color>");
        int FRWOverlap = Physics.OverlapBox(FRWpos, boxShape).Length;
        //Debug.Log("<color=red>FRWOverlap: count" + FRWOverlap + "</color>");

        return (FLWOverlap + FRWOverlap) > 2;
    }
} 