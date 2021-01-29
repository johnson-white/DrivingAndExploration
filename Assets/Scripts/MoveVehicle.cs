using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVehicle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If button pressed. Keys (e.g. "Vertical") are managed in Unity/Edit/Project Settings/Input Manager
        if (Input.GetButton("Vertical"))
        {
            /* Changing the position of the script's parent object by getting the positive or negative Vertical input,
            which is a float, then multiplying this float with a Forward Z axis value that Unity has abstracted in place
            of (0, 0, 1) */
            transform.position += Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime;
        }
        
        if (Input.GetButton("Horizontal"))
        {
            /* Need to improve so it turns the object, not moves, it left or right. Perhaps change both these methods to use
            Rigidbody AddForce of type acceleration and AddTorque */
            transform.position += Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
    }
} 