using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Audio
    private AudioManager audioManager;
    [SerializeField] private AudioSource nosSound;
    [SerializeField] private AudioSource idleSound;
    [SerializeField] private AudioSource accelerateSound;
    [SerializeField] private AudioSource deaccelerateSound;
    public float idleMaxVolume;
    public float accelerateMaxVolume;
    public float accelerateMaxPitch;

    // Speed
    public float speed;
    public float maxSpeed = 280f;
    private float speedRatio;

    // Input
    private float horizontalInput, verticalInput;
    
    // Steering and braking
    private float currentSteerAngle, currentbreakForce;
    private bool isBraking;
    private bool isDrifting;

    // Static for our nitrous system
    public static bool nosActive;

    // Static to enabled / disable our headlights for the race & tutorial
    public static bool useHeadLights;

    // Used for letting the game know if we're racing or tutorial
    public static bool isRacing;

    // Particles
    [SerializeField] private GameObject nosParticles;
    [SerializeField] private GameObject tireTrailRL;
    [SerializeField] private GameObject tireTrailRR;
    
    // Rigidbody
    [SerializeField] private Rigidbody rigidBody;

    // Tailights & headlights
    [SerializeField] private GameObject tailLights;
    [SerializeField] private GameObject headLights;

    // Settings
    [SerializeField] private float motorForce, nosForce, breakForce, maxSteerAngle;

    // Wheel colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void Awake()
    {
        // Lock our cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Speed derived from wheel speed
        speedRatio = GetSpeedRatio();
        speed = rigidBody.velocity.magnitude * 3.6f;
        
        // Engine sounds
        idleSound.volume = Mathf.Lerp(0.1f, idleMaxVolume, speedRatio);
        accelerateSound.volume = Mathf.Lerp(0.3f, accelerateMaxVolume, speedRatio);
        accelerateSound.pitch = Mathf.Lerp(0.3f, accelerateMaxPitch, speedRatio);
        deaccelerateSound.volume = Mathf.Lerp(0.3f, accelerateMaxVolume, speedRatio);
        deaccelerateSound.pitch = Mathf.Lerp(0.3f, accelerateMaxPitch, speedRatio);

        // Nos and brakes
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            nosSound.Play();
        }

        // Brake lights
        if ((isBraking) || (isDrifting))
        {
            tailLights.SetActive(true);
        }
        else
        {
            tailLights.SetActive(false);
        }

        // Head lights
        if (useHeadLights)
        {
            headLights.SetActive(true);
        }
        else
        {
            headLights.SetActive(false);
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

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBraking = Input.GetKey(KeyCode.S);

        // Drifting Input
        isDrifting = Input.GetKey(KeyCode.Space);

        // Nos Input
        nosActive = Input.GetKey(KeyCode.LeftShift);
    }

    // Engine
    private void HandleMotor()
    {
        // Gets input for accleration
        float input = verticalInput * motorForce;
        
        // If speed less than max speed, stop motor torqu
        if (speed < maxSpeed && CountDownSystem.raceStarted)
        {
            frontLeftWheelCollider.motorTorque = input;
            frontRightWheelCollider.motorTorque = input;
        }
        else
        {
            frontLeftWheelCollider.motorTorque = 0;
            rearLeftWheelCollider.motorTorque = 0;
        }
        
        // Sounds
        if (input == 0)
        {
            if (!idleSound.isPlaying)
            {
                idleSound.Play();
            }
        }
        else
        {
            if (frontLeftWheelCollider.motorTorque != 0)
            {
                if (!accelerateSound.isPlaying)
                {
                    idleSound.Pause();
                    accelerateSound.Play();
                }
            }
            else if (frontLeftWheelCollider.motorTorque == 0)
            {
                if (!deaccelerateSound.isPlaying)
                {
                    accelerateSound.Pause();
                    deaccelerateSound.Play();
                }
            }
        }

        // Check if braking
        currentbreakForce = isBraking ? breakForce : 0f;
        ApplyBreaking();
    }
    
    // Used for engine sounds
    public float GetSpeedRatio()
    {
        var gas = Mathf.Clamp(verticalInput, 0.5f, 1f);
        return (speed*gas)/maxSpeed;
    }


    // Nos
    private void HandleNos()
    {
        // If we're using and our current boost amount is more than 0
        if (nosActive && NitrousManager.currentBoost > 0 && CountDownSystem.raceStarted)
        {
            nosParticles.SetActive(true);
            rigidBody.AddForce(transform.forward * nosForce);
        }
        else
        {
            nosParticles.SetActive(false);
        }
    }
    
    // Tire trails
    private void HandleTireTrails()
    {
        if (rearLeftWheelCollider.isGrounded && rearRightWheelCollider.isGrounded)
        {
            if ((currentSteerAngle > 25) || (currentSteerAngle < -25) || (isBraking))
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
    
    // Drifting
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
    
    // Braking
    private void ApplyBreaking() {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }
    
    // Steering
    private void HandleSteering() {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }
    
    // Wheel colliders & transforms
    private void UpdateWheels() {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }
    
    // Wheel position & rotation
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}