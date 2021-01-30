using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVehicle : MonoBehaviour
{
    // Visible Params
    public float accelerationForce = 1f;
    public Rigidbody vehicle;
    
    void Start()
    {
        // Assign component first
        vehicle = GetComponent<Rigidbody>();
    }

    void FixedUpdate() // consistent 0.2 ms physics updates
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Vertical"))
        {
            // Can call this from Update() for good input detection, and the effects will take place on the next FixedUpdate()
            vehicle.AddForce(transform.forward * accelerationForce * Input.GetAxis("Vertical"));
            // Unfortunately friction causes this to rotate on its Y axis and topple over forwards/backwards
            // Temp workaround of freezing the Y axis rotation on the object for now. Maybe look into using wheel colliders instead.
        }
        
        if (Input.GetButton("Horizontal"))
        {
            /* Need to improve so it turns the object, not moves, it left or right. Perhaps change both these methods to use
            Rigidbody AddForce of type acceleration and AddTorque */
            transform.position += Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
    }
} 