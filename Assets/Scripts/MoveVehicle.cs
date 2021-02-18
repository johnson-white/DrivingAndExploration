using System.Collections;
using UnityEngine;

public class MoveVehicle : MonoBehaviour
{
    public float accelerationForce;
    public float rotationForce;
    public float strafeForce;
    public Rigidbody vehicle;
    private GameObject FrontLeftWheel, FrontRightWheel;
    private Vector3 FLWpos;
    private Vector3 FRWpos;
    private Transform centreOfMass;
    public Vector3 boxShape = new Vector3(0.25f, 0.04f, 0.27f);
    
    void Start()
    {
        vehicle = GetComponent<Rigidbody>();
        vehicle.maxAngularVelocity = 2f;
        
        FrontLeftWheel = vehicle.transform.Find("FrontLeftWheel").gameObject; // Get specific child component
        FrontRightWheel = vehicle.transform.Find("FrontRightWheel").gameObject;
        FLWpos = FrontLeftWheel.transform.position;
        FRWpos = FrontRightWheel.transform.position;
        
        centreOfMass = vehicle.transform.Find("CentreOfMass").gameObject.transform;
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
        Movement();
    }

    public IEnumerator ResistDrag()
    {
        Debug.Log("<color=yellow>Resist Drag called </color>");
        var resistAcceleration = accelerationForce;
        Debug.Log("<color=red>resistAcceleration outside loop"+resistAcceleration+" </color>");
        
        //loop does not run
        for (var i = 0; i < 50; i++) // run 5 seconds
        {
            Debug.Log("<color=cyan>"+i+"</color>");

            if (Input.GetAxis("Vertical") > 0.1f)
            {
                Debug.Log("yield break");
                yield break; // if player starts accelerating again, stop coroutine here
            }
            vehicle.AddForce(transform.forward * resistAcceleration);
            //resistAcceleration *= 0.95f;
            yield return new WaitForSeconds(0.1f); // runs once every 0.1 seconds
            
            Debug.Log("<color=red>Loop: "+ i +"</color>");
            Debug.Log("<color=blue>resistAcceleration inside loop: "+ resistAcceleration +"</color>");
        }
        
        //this runs
        Debug.Log("<color=green>After loop </color>"); 
    }

    private void Movement()
    {
        if (Input.GetButtonUp("Vertical"))
        {
            StartCoroutine(ResistDrag());
        }
        
        if (Input.GetButton("Vertical"))
        {
            if (IsGrounded())
            {
                var v = Input.GetAxis("Vertical");
                var applyForce = (v >= 0.1f) ? accelerationForce : accelerationForce * 0.8f; // if input is negative, make the deacceleration force weaker
                //Debug.Log("<color=yellow>applyForce is: "+applyForce+"</color>");
                vehicle.AddForceAtPosition(transform.forward * applyForce * v, centreOfMass.position);
            }
        }
        
        if (Input.GetButton("Horizontal"))
        {
            if (IsGrounded())
            {
                float localVelocity = transform.InverseTransformDirection(vehicle.velocity).z; // velocity
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
                vehicle.AddTorque(torque);
                
                // adding horizontal force
                var strafe = (h > 0) ? vehicle.transform.right : -vehicle.transform.right;
                strafe *= strafeForce;
                vehicle.AddForceAtPosition(strafe, centreOfMass.position, ForceMode.Acceleration);
            }
        }
    }

    private bool IsGrounded()
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