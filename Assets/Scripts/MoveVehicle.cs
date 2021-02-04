using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVehicle : MonoBehaviour
{
    public float accelerationForce;
    public float rotationForce;
    public Rigidbody vehicle;
    
    void Start()
    {
        vehicle = GetComponent<Rigidbody>();
    }

    void FixedUpdate() // consistent 0.2 ms physics updates
    {

    }

    void Update()
    {
        if (Input.GetButton("Vertical"))
        {
            vehicle.AddForce(transform.forward * accelerationForce * Input.GetAxis("Vertical"), ForceMode.Acceleration);
        }
        
        if (Input.GetButton("Horizontal"))
        {
            Vector3 localVelocity = transform.InverseTransformDirection(vehicle.velocity); //global velocity transformed to local velocity
            Debug.Log("Z axis speed value is: " + localVelocity.z);
            float direction = (localVelocity.z > 0) ? 1f : -1f; //if z axis velocity is negative, invert torque direction.
            vehicle.AddRelativeTorque(transform.up * rotationForce * Input.GetAxis("Horizontal") * direction);
        }
        
        
    }
} 