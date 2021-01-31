using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVehicle : MonoBehaviour
{
    // Visible Params
    public float accelerationForce;
    public float rotationForce;
    public Rigidbody vehicle;
    
    void Start()
    {
        // Assign component first
        vehicle = GetComponent<Rigidbody>();
        //vehicle.angularDrag = 10000; //angular drag affects torque rotation so I have changed the object shape to a rectangle
        // this will naturally prevent the object from toppling over as it will have a much lower centre of mass
    }

    void FixedUpdate() // consistent 0.2 ms physics updates
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Vertical")) // AddForce forwards and backwards
        {
            // Can call this from Update() for good input detection, and the effects will take place on the next FixedUpdate()
            vehicle.AddForce(transform.forward * accelerationForce * Input.GetAxis("Vertical"), ForceMode.Acceleration);
        }
        
        if (Input.GetButton("Horizontal")) // AddTorque to rotate on Y axis
        {
            vehicle.AddTorque(transform.up * rotationForce * Input.GetAxis("Horizontal"));
        }
    }
} 