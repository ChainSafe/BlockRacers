using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    // This small script rotates the showroom platform

    // The speed at which the platform rotates
    public float rotationSpeed = 10f; 

    public void Update()
    {
        // Rotate the platform around the Y-axis based on the rotation speed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
