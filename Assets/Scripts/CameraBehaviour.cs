using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform targetTransform;
    public Transform cameraTransform;
    private Camera vehicleCamera;

    public float currentZ = 4f;
    public float currentX = 0f;
    public float currentY = 0f;
    public float mouseXSensitivity = 5f;
    public float mouseYSensitivity = 5f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        cameraTransform = this.transform;
        vehicleCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X");
        currentY -= Input.GetAxis("Mouse Y");
        currentY = Mathf.Clamp(currentY, 0f, 80f);
    }

    void LateUpdate()
    {
        Vector3 initialCameraPosition = new Vector3(0, 0f, -currentZ);
        Quaternion cameraRotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 targetPosition = targetTransform.position;
        cameraTransform.position = targetPosition + cameraRotation * initialCameraPosition;
        cameraTransform.LookAt(targetPosition);
    }
}
