using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MoveVehicle : MonoBehaviour
{
    public float accelerationForce;
    public float rotationForce;
    public Rigidbody vehicle;
    
//    void Awake () { // Testing rotation issue with constant framerate, no effect
//        QualitySettings.vSyncCount = 0;  // VSync must be disabled
//        Application.targetFrameRate = 60;
//    }
    void Start()
    {
        vehicle = GetComponent<Rigidbody>();
    }

//    void FixedUpdate() // Testing rotation issue
//    {
//        Movement();
//    }

    void Update()
    {
        //ApplyGravity(vehicle);
        Movement();
        bool g = Grounded();
        Debug.Log(g);
        
    }

    private void Movement()
    {
        if (Input.GetButton("Vertical"))
        {
            vehicle.AddForce(transform.forward * accelerationForce * Input.GetAxis("Vertical"), ForceMode.Acceleration);
        }
        
        if (Input.GetButton("Horizontal"))
        {
            Vector3 localVelocity = transform.InverseTransformDirection(vehicle.velocity); //global velocity transformed to local velocity
            // Debug.Log("Z axis speed value is: " + localVelocity.z);
            float zAxisDirection = (localVelocity.z > -2f) ? 1f : -1f; //if z axis velocity is negative, invert torque direction.
            float directionInput = Input.GetAxis("Horizontal");
            float appliedRotationForce = rotationForce;
//            if (directionInput <= 0) Testing rotation issue
//            {
//                appliedRotationForce *= 0.8f;
//            }
            var torque = transform.up * directionInput * rotationForce * zAxisDirection;
            //Debug.Log(torque);
            vehicle.AddTorque(torque);
        }
    }
    
    //limit angularvelocity?

    private bool Grounded() //cast ray down from vehicle to check if it's grounded
    {
        Transform vehicleTransform;
        Vector3 down = new Vector3(0f, -0.3f, 0f);
        Vector3 pointDir = (vehicleTransform = vehicle.transform).TransformDirection(down);
        // Debug.Log("<color=yellow>pointDir - " + pointDir + "</color>");
        Vector3 vehiclePosition = vehicleTransform.position; //using centre bottom of vehicle, can implement for wheel traction if wanted in the future
        // Debug.Log("<color=red>vehiclePosition - " + vehiclePosition + "</color>");
        Debug.DrawRay(vehiclePosition, pointDir, Color.red);
        
        RaycastHit hit1;
        return Physics.Raycast(vehiclePosition, pointDir, out hit1, 0.3f);
    }

    private void ApplyGravity(Rigidbody rb) // Testing rotation issue
    {
        rb.AddForce(Vector3.down, ForceMode.VelocityChange);
    }
    
} 