using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cameraToRotate; // Reference to the camera you want to rotate
    public Vector3 desiredRotation; // Desired rotation for the camera
    private Quaternion initialRotation; // Initial rotation of the camera
    private Quaternion targetRotation; // Target rotation of the camera
    private float rotationTimer; // Timer for tracking the rotation progress
    private bool isRotating; // Flag to check if rotation is in progress
    public float rotationDuration; // The duration of the rotation in seconds

    public static CameraController instance;    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        initialRotation = Quaternion.Euler(0f, -45f, 0f);
        targetRotation = Quaternion.Euler(desiredRotation);
        cameraToRotate.transform.rotation = initialRotation;
        isRotating = false;
    }

    // Rotate the camera with specified rotation amount and duration
    public void RotateCamera(float rotationAmount, float duration)
    {
        if (!isRotating)
        {
            isRotating = true;
            initialRotation = cameraToRotate.transform.rotation;
            targetRotation = Quaternion.Euler(cameraToRotate.transform.eulerAngles + new Vector3(0f, rotationAmount, 0f));
            rotationTimer = 0f;
            rotationDuration = duration;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isRotating)
        {
            rotationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(rotationTimer / rotationDuration);
            cameraToRotate.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            if (rotationTimer >= rotationDuration)
            {
                isRotating = false;
            }
        }
    }
}
