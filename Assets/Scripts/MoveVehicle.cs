﻿using System.Collections;
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
    
    private bool resistingDrag = false;
    private IEnumerator ResistDrag()
    {
        Debug.Log("<color=yellow>Resist Drag called </color>");
        
        if (resistingDrag) // if coroutine already running, stop all coroutines except this one
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
        resistingDrag = true;
        Debug.Log("resistingDrag <color=cyan>TRUE</color>");
        
        var resistAcceleration = accelerationForce * 0.7f; //reducing the acceleration force as its unlikely the vehicle is at max acceleration
        //Debug.Log("<color=blue>resistAcceleration outside loop: "+ resistAcceleration +" </color>");

        for (var i = 0; i < 25; i++) // run for 2.5 seconds
        {
            if (Input.GetAxis("Vertical") == 1f) // if player starts accelerating again, stop coroutine here
            {
                //Debug.Log(Input.GetAxis("Vertical"));
                Debug.Log("<color=red>Yield break</color>");
                resistingDrag = false;
                Debug.Log("resistingDrag <color=green>FALSE</color>");
                yield break; 
            }
            vehicle.AddForce(vehicle.transform.forward * resistAcceleration, ForceMode.Acceleration);
            resistAcceleration *= 0.95f;
            //Debug.Log("<color=purple>resistAcceleration inside loop: "+ resistAcceleration +"</color>");

            yield return new WaitForSeconds(0.05f); // runs once every 0.1 seconds
        }
        Debug.Log("<color=green>ResistDrag loop finished.</color>");

        resistingDrag = false;
        Debug.Log("resistingDrag <color=green>FALSE</color>");
        coroutinesList.Clear();
        Debug.Log("<color=green>After loop. coroutinesList length is: </color>"+coroutinesList.Count); 
    }
    
    private List<Coroutine> coroutinesList = new List<Coroutine>();
    private void Movement()
    {
        if (Input.GetButtonUp("Vertical") && Input.GetAxis("Vertical") > 0f)
        {
            coroutinesList.Add(StartCoroutine(ResistDrag()));
            //Debug.Log("<color=lime>In Movement() coroutineList length is: </color>"+coroutinesList.Count);
        }
        
        if (Input.GetButton("Vertical"))
        {
            if (IsGrounded())
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
                    applyForce = accelerationForce;
                }
                
                vehicle.AddForceAtPosition(transform.forward * applyForce * v, centreOfMass.position);
            }
        }
        
        if (Input.GetButton("Horizontal"))
        {
            if (IsGrounded())
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