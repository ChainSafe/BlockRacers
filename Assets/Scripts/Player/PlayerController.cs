using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // speed
    public float speed;
    public float speedMax = 280f;
    // input
    private float horizontalInput, verticalInput;
    // steering and breaking
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private bool nosActive;
    // particles
    [SerializeField] private GameObject nosParticles;
    [SerializeField] private GameObject tireTrailRL;
    [SerializeField] private GameObject tireTrailRR;
    
    // rigidbody
    [SerializeField] private Rigidbody rb;

    // settings
    [SerializeField] private float motorForce, nosForce, breakForce, maxSteerAngle;

    // wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void FixedUpdate() {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        HandleNos();
        HandleDrift();
        HandleTireTrails();
    }

    private void GetInput() {
        // steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
        
        // nos Input
        nosActive = Input.GetKey(KeyCode.LeftShift);
    }
    
    // engine
    private void HandleMotor()
    {
        float input = verticalInput * motorForce;
        frontLeftWheelCollider.motorTorque = input;
        frontRightWheelCollider.motorTorque = input;
        if (input != 0)
        {
            speed += (input / 100) * Time.deltaTime;
        }
        else
        {
            speed -= 2 * Time.deltaTime;
        }
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }
    
    // nos
    private void HandleNos()
    {
        if (nosActive)
        {
            nosParticles.SetActive(true);
            rb.AddForce(0,0, nosForce);
        }
        else
        {
            nosParticles.SetActive(false);
        }
    }
    
    // tire trails
    private void HandleTireTrails()
    {
        if ((currentSteerAngle > 25) || (currentSteerAngle < -25) || (isBreaking))
        {
            tireTrailRL.SetActive(true);
            tireTrailRR.SetActive(true);
        }
        else
        {
            tireTrailRL.SetActive(false);
            tireTrailRR.SetActive(false);
        }
    }
    
    // drifting
    private void HandleDrift()
    {
        JointSpring suspensionSpringFL = frontLeftWheelCollider.suspensionSpring;
        JointSpring suspensionSpringFR = frontRightWheelCollider.suspensionSpring;
        JointSpring suspensionSpringRL = rearLeftWheelCollider.suspensionSpring;
        JointSpring suspensionSpringRR = rearRightWheelCollider.suspensionSpring;
        if ((isBreaking) || (nosActive))
        {
            suspensionSpringFL.damper = 10000;
            suspensionSpringFR.damper = 10000;
            suspensionSpringRL.damper = 10000;
            suspensionSpringRR.damper = 10000;
            maxSteerAngle = 60;
        }
        else
        {
            suspensionSpringFL.damper = 6000;
            suspensionSpringFR.damper = 6000;
            suspensionSpringRL.damper = 6000;
            suspensionSpringRR.damper = 6000;
            maxSteerAngle = 40;
        }
    }
    
    // breaking
    private void ApplyBreaking() {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
        speed -= (currentbreakForce / 100) * Time.deltaTime;
    }
    
    // steering
    private void HandleSteering() {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }
    
    // wheel colliders & transforms
    private void UpdateWheels() {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }
    
    // wheel position & rotation
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}