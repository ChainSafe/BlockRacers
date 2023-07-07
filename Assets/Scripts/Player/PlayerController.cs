using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // audio
    private AudioManager audioManager;
    // speed
    public float speed;
    public float maxSpeed = 280f;
    private float speedRatio;
    // input
    private float horizontalInput, verticalInput;
    // steering and breaking
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private bool isDrifting;
    public static bool nosActive;
    // particles
    [SerializeField] private GameObject nosParticles;
    [SerializeField] private GameObject tireTrailRL;
    [SerializeField] private GameObject tireTrailRR;
    
    // rigidbody
    [SerializeField] private Rigidbody rigidBody;
    
    // Tailights
    [SerializeField] private GameObject tailLights;

    // settings
    [SerializeField] private float motorForce, nosForce, breakForce, maxSteerAngle;

    // wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("EngineIdle");
    }

    void Update()
    {
        // remove later
        
        // speed derived from wheel speed
        speedRatio = GetSpeedRatio();
        speed = rearLeftWheelCollider.rpm * rearLeftWheelCollider.radius * 2f * Mathf.PI / 10f;

        // nos and brakes
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (audioManager == null) return;
            FindObjectOfType<AudioManager>().Play("Nos");
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (audioManager == null) return;
            FindObjectOfType<AudioManager>().Play("EngineAccelerate");
        }
        
        // brake lights
        if ((isBreaking) || (isDrifting))
        {
            tailLights.SetActive(true);
        }
        else
        {
            tailLights.SetActive(false);
        }
    }

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
        isBreaking = Input.GetKey(KeyCode.S);
        
        // drifting Input
        isDrifting = Input.GetKey(KeyCode.Space);
        
        // nos Input
        nosActive = Input.GetKey(KeyCode.LeftShift);
    }
    
    // engine
    private void HandleMotor()
    {
        // get input for accleration
        float input = verticalInput * motorForce;
        
        // if less than max speed
        if (speed < maxSpeed)
        {
            frontLeftWheelCollider.motorTorque = input;
            frontRightWheelCollider.motorTorque = input;
        }
        else
        {
            frontLeftWheelCollider.motorTorque = 0;
            frontRightWheelCollider.motorTorque = 0;
        }
        
        if (input != 0)
        {
            if (audioManager == null) return;
            FindObjectOfType<AudioManager>().Pause("EngineIdle");
        }

        currentbreakForce = isBreaking ? breakForce : 0f;
        
        ApplyBreaking();
    }
    
    // used four sounds
    public float GetSpeedRatio()
    {
        var gas = Mathf.Clamp(verticalInput, 0.5f, 1f);
        return (speed*gas)/maxSpeed;
    }
    
    // nos
    private void HandleNos()
    {
        // If we're using and our current boost amount is more than 0
        if (nosActive && NitrousManager.currentBoost > 0)
        {
            nosParticles.SetActive(true);
            rigidBody.AddForce(transform.forward * nosForce);
        }
        else
        {
            nosParticles.SetActive(false);
        }
    }
    
    // tire trails
    private void HandleTireTrails()
    {
        if (rearLeftWheelCollider.isGrounded && rearRightWheelCollider.isGrounded)
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
    }
    
    // drifting
    private void HandleDrift()
    {
        JointSpring suspensionSpringFL = frontLeftWheelCollider.suspensionSpring;
        JointSpring suspensionSpringFR = frontRightWheelCollider.suspensionSpring;
        JointSpring suspensionSpringRL = rearLeftWheelCollider.suspensionSpring;
        JointSpring suspensionSpringRR = rearRightWheelCollider.suspensionSpring;
        if (isDrifting)
        {
            suspensionSpringFL.damper = 2000;
            suspensionSpringFR.damper = 2000;
            suspensionSpringRL.damper = 2000;
            suspensionSpringRR.damper = 2000;
            maxSteerAngle = 60;
        }
        else
        {
            suspensionSpringFL.damper = 1000;
            suspensionSpringFR.damper = 1000;
            suspensionSpringRL.damper = 1000;
            suspensionSpringRR.damper = 1000;
            maxSteerAngle = 40;
        }
    }
    
    // breaking
    private void ApplyBreaking() {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
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