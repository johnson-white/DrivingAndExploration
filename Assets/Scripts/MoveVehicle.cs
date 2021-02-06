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
    private Vector3 FLWpos;
    private Vector3 FRWpos;
    
    void Start()
    {
        vehicle = GetComponent<Rigidbody>();
        FrontLeftWheel = vehicle.transform.Find("FrontLeftWheel").gameObject; // Get specific child component
        FrontRightWheel = vehicle.transform.Find("FrontRightWheel").gameObject;
        
        FLWpos = FrontLeftWheel.transform.position;
        FRWpos = FrontRightWheel.transform.position;
    }

    private void OnDrawGizmos()
    {
        //visualising boxcast
        Gizmos.color = new Color(0, 0, 255);
        Gizmos.DrawWireCube(FLWpos, new Vector3(0.25f, 0.025f, 0.27f));
        Gizmos.color = new Color(255, 0, 0);
        Gizmos.DrawWireCube(FRWpos, new Vector3(0.25f, 0.025f, 0.27f));
    }

    void FixedUpdate() // Testing rotation issue
    {
        
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (Input.GetButton("Vertical"))
        {
            if (IsGrounded())
            {
                            vehicle.AddForce(transform.forward * accelerationForce * Input.GetAxis("Vertical"), ForceMode.Acceleration);
            }
        }
        
        if (Input.GetButton("Horizontal"))
        {
            if (IsGrounded())
            {
                Vector3 localVelocity = transform.InverseTransformDirection(vehicle.velocity);
                float zAxisDirection = (localVelocity.z > -2f) ? 1f : -1f;
                var torque = transform.up * Input.GetAxis("Horizontal") * rotationForce * zAxisDirection;
                vehicle.AddTorque(torque);
            }
        }
    }

    private bool IsGrounded()
    {
        // Grabbing global coords of temp objects so I can use gizmos to draw a wire box, will use local position and remove temp objects later
        FLWpos = FrontLeftWheel.transform.position;
        FRWpos = FrontRightWheel.transform.position;
        //Debug.Log("<color=blue>FrontLeftWheel local position is - " + FLWpos + "</color>");
        //Debug.Log("<color=red>FrontRightWheel local position is - " + FRWpos + "</color>");
        // Now that we have local positions, use them to create two BoxCasts
        Vector3 boxShape = new Vector3(0.25f, 0.025f, 0.27f);
        int FLWOverlap = Physics.OverlapBox(FLWpos, boxShape).Length; //always returns at least 1 as it is overlapping with the vehicle's collision mesh
        Debug.Log("<color=blue>FLWOverlap count: " + FLWOverlap + "</color>");
        int FRWOverlap = Physics.OverlapBox(FRWpos, boxShape).Length;
        Debug.Log("<color=red>FRWOverlap: count" + FRWOverlap + "</color>");

        return (FLWOverlap + FRWOverlap) > 2;
    }
} 