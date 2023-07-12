using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Stats manager
    public StatsManager statsManager;

    // Audio
    private AudioManager audioManager;

    // Speed
    public float speed;
    public float maxSpeed = 280f;
    public float speedRatio;

    // Input
    public float input;
    private float horizontalInput, verticalInput;
    
    // Steering and braking
    private float currentSteerAngle, currentbrakeForce;
    private bool isBraking;
    private bool isDrifting;
    
    // Current gear
    public int currentGear;

    // Static for our nitrous system
    public static bool nosActive;

    // Static to enabled / disable our headlights for the race & tutorial
    public static bool useHeadLights;

    // Used for letting the game know if we're racing or tutorial
    public static bool isRacing;

    // collision bool for sounds
    public bool collision;

    // Particles
    [SerializeField] private GameObject nosParticles;
    [SerializeField] private GameObject tireTrailRL;
    [SerializeField] private GameObject tireTrailRR;
    [SerializeField] private GameObject driftSmoke;
    
    // Rigidbody
    [SerializeField] private Rigidbody rigidBody;

    // Tailights & headlights
    [SerializeField] private GameObject tailLights;
    [SerializeField] private GameObject headLights;

    // Settings
    [SerializeField] private float motorForce, nosForce, breakForce;
    public float maxSteerAngle;

    // Wheel colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;
    
    // Body Material
    [SerializeField] private GameObject carBody;

    private void Awake()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        
        // Finds our stats manager
        statsManager = GameObject.FindWithTag("StatsManager").GetComponent<StatsManager>();

        // Updates body material
        carBody.GetComponent<Renderer>().material = statsManager.bodyMaterial;
        
        // Updates our stats
        statsManager.UpdateStats();

        // Lock our cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        // Changes Bgm
        audioManager.Pause("Bgm1");
        audioManager.Play("Bgm2");
    }
    
    private void Update()
    {
        // Speed derived from wheel speed
        speedRatio = GetSpeedRatio();
        speed = rigidBody.velocity.magnitude * 3.6f;
        
        // Brake lights
        tailLights.SetActive(isBraking || isDrifting);

        // Head lights
        headLights.SetActive(useHeadLights);
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
        // Gets input for acceleration
        input = verticalInput * motorForce / currentGear;

        // If speed less than max speed, add input to motor torque
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

        // Check if braking
        currentbrakeForce = isBraking && frontLeftWheelCollider.motorTorque > 0 ? breakForce : 0f;

        ApplyBraking();
    }
    
    // Used for engine sounds
    public float GetSpeedRatio()
    {
        var gas = Mathf.Clamp(verticalInput, 0.5f, 1f);
        return (speed*gas) / maxSpeed;
    }
    
    // Nos
    private void HandleNos()
    {
        // If we're using nos and our current boost amount is more than 0
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
            tireTrailRL.SetActive(currentSteerAngle > 25 || currentSteerAngle < -25 || isBraking);
            tireTrailRR.SetActive(currentSteerAngle > 25 || currentSteerAngle < -25 || isBraking);
            driftSmoke.SetActive(currentSteerAngle > 25 || currentSteerAngle < -25 || isBraking);
        }
    }
    
    // Drifting
    private void HandleDrift()
    {
        WheelFrictionCurve sidewaysFrictionFL = frontLeftWheelCollider.sidewaysFriction;
        WheelFrictionCurve sidewaysFrictionFR = frontRightWheelCollider.sidewaysFriction;
        WheelFrictionCurve sidewaysFrictionRL = rearLeftWheelCollider.sidewaysFriction;
        WheelFrictionCurve sidewaysFrictionRR = rearRightWheelCollider.sidewaysFriction;
        if (isDrifting)
        {
            sidewaysFrictionFL.extremumValue = 0.2f;
            sidewaysFrictionFR.extremumValue = 0.2f;
            sidewaysFrictionRL.extremumValue = 0.2f;
            sidewaysFrictionRR.extremumValue = 0.2f;
            frontLeftWheelCollider.sidewaysFriction = sidewaysFrictionFL;
            frontRightWheelCollider.sidewaysFriction = sidewaysFrictionFR;
            rearLeftWheelCollider.sidewaysFriction = sidewaysFrictionRL;
            rearRightWheelCollider.sidewaysFriction = sidewaysFrictionRR;
            currentbrakeForce = 100;
        }
        else
        {
            sidewaysFrictionFL.extremumValue = 0.5f;
            sidewaysFrictionFR.extremumValue = 0.5f;
            sidewaysFrictionRL.extremumValue = 0.5f;
            sidewaysFrictionRR.extremumValue = 0.5f;
            frontLeftWheelCollider.sidewaysFriction = sidewaysFrictionFL;
            frontRightWheelCollider.sidewaysFriction = sidewaysFrictionFR;
            rearLeftWheelCollider.sidewaysFriction = sidewaysFrictionRL;
            rearRightWheelCollider.sidewaysFriction = sidewaysFrictionRR;
        }
    }
    
    // Braking
    private void ApplyBraking() {
        frontRightWheelCollider.brakeTorque = currentbrakeForce;
        frontLeftWheelCollider.brakeTorque = currentbrakeForce;
        rearLeftWheelCollider.brakeTorque = currentbrakeForce;
        rearRightWheelCollider.brakeTorque = currentbrakeForce;
    }
    
    // Steering
    private void HandleSteering()
    {
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

    // Bool for collision sound
    private void OnCollisionEnter(Collision other)
    {
        collision = true;
    }
}