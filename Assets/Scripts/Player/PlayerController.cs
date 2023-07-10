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
    
    // steering and braking
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private bool isDrifting;

    // static for our nitrous system
    public static bool nosActive;

    // static to enabled / disable our headlights for the race & freeroam.
    public static bool useHeadLights;

    // used for letting the game know if we're racing or in freeroam
    public static bool isRacing;

    // particles
    [SerializeField] private GameObject nosParticles;
    [SerializeField] private GameObject tireTrailRL;
    [SerializeField] private GameObject tireTrailRR;
    
    // rigidbody
    [SerializeField] private Rigidbody rigidBody;

    // tailights & headlights
    [SerializeField] private GameObject tailLights;
    [SerializeField] private GameObject headLights;


    // settings
    [SerializeField] private float motorForce, nosForce, breakForce, maxSteerAngle;

    // wheel colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void Awake()
    {
        // lock our cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        // finds our audio manager and plays idle sound
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("EngineIdle");
    }

    void Update()
    {
        // speed derived from wheel speed
        speedRatio = GetSpeedRatio();
        speed = rigidBody.velocity.magnitude * 3.6f;

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

        // head lights
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
        
        // engine idle if no input
        if (input != 0)
        {
            if (audioManager == null) return;
            FindObjectOfType<AudioManager>().Pause("EngineIdle");
        }
        
        // check braking
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }
    
    // used for engine sounds
    public float GetSpeedRatio()
    {
        var gas = Mathf.Clamp(verticalInput, 0.5f, 1f);
        return (speed*gas)/maxSpeed;
    }


    // nos
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
    
    // braking
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