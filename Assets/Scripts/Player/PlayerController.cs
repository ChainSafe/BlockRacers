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

    // Settings
    [SerializeField] private float motorForce, nosForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void FixedUpdate() {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        HandleNos();
        HandleDrift();
    }

    private void GetInput() {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
        
        // NOS Input
        nosActive = Input.GetKey(KeyCode.LeftShift);
    }
    
    // engine
    private void HandleMotor()
    {
        float input = verticalInput * (motorForce * nosForce);
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
    
    // NOS
    private void HandleNos()
    {
        // if nos active set power to 10, else it's 1. Applied to motor force above
        nosForce = nosActive ? 10 : 1;
    }

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
        }
        else
        {
            suspensionSpringFL.damper = 5000;
            suspensionSpringFR.damper = 5000;
            suspensionSpringRL.damper = 5000;
            suspensionSpringRR.damper = 5000;
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