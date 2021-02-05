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
    private GameObject FrontLeftWheel, FrontRightWheel;
    
    void Start()
    {
        vehicle = GetComponent<Rigidbody>();
        FrontLeftWheel = vehicle.transform.Find("FrontLeftWheel").gameObject; // Get specific child component
        FrontRightWheel = vehicle.transform.Find("FrontRightWheel").gameObject;
    }

    void FixedUpdate() // Testing rotation issue
    {
        
    }

    void Update()
    {
        Movement();
        bool g = Grounded();
        Debug.Log(g);
        
        // Grabbing local coords of objects
        Vector3 FLWpos = FrontLeftWheel.transform.localPosition;
        Vector3 FRWpos = FrontRightWheel.transform.localPosition;
        Debug.Log("<color=blue>FrontLeftWheel local position is - " + FLWpos + "</color>");
        Debug.Log("<color=red>FrontRightWheel local position is - " + FRWpos + "</color>");
        // Now that we have local positions, use them to create two BoxCasts
    }

    private void Movement()
    {
        if (Input.GetButton("Vertical"))
        {
            vehicle.AddForce(transform.forward * accelerationForce * Input.GetAxis("Vertical"), ForceMode.Acceleration);
        }
        
        if (Input.GetButton("Horizontal"))
        {
            Vector3 localVelocity = transform.InverseTransformDirection(vehicle.velocity);
            float zAxisDirection = (localVelocity.z > -2f) ? 1f : -1f;
            var torque = transform.up * Input.GetAxis("Horizontal") * rotationForce * zAxisDirection;
            vehicle.AddTorque(torque);
        }
    }

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
} 