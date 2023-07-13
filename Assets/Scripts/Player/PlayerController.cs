using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Stats manager
    private StatsManager statsManager;

    // Singleton access to the controller
    public static PlayerController instance;

    // Audio
    private AudioManager audioManager;
    
    // Current gear
    [Header("Speed")]
    public int currentGear;
    
    // Speed
    [SerializeField] public float speed, maxSpeed, speedRatio, motorForce;
    [SerializeField] private float nosForce, breakForce;
    
    // Static for our nitrous system
    public static bool nosActive;

    // Input
    public float input;
    private float horizontalInput, verticalInput;
    
    // Steering and braking
    [Header("Steering & Braking")]
    public bool isDrifting;
    private bool isBraking;
    public float currentSteerAngle, currentBrakeForce, maxSteerAngle;

    // Reset bool
    public bool resetActive;

    // Static to enabled / disable our headlights for the race & tutorial
    public static bool useHeadLights;

    // Used for letting the game know if we're racing or tutorial
    public static bool isRacing;

    // collision bool for sounds
    public bool collision;
    
    // Rigidbody
    [Header("Objects & Particle Effects")]
    [SerializeField] public Rigidbody rigidBody;

    // Particles
    [SerializeField] private GameObject nosParticles, tireTrailRL, tireTrailRR, driftSmoke;

    // Taillights & headlights
    [SerializeField] private GameObject tailLights, headLights;

    // Body Material
    [Header("Wheels & Colliders")]
    [SerializeField] private GameObject carBody;
    
    // Wheel colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider, rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform, rearLeftWheelTransform, rearRightWheelTransform;

    // Player Input
    private PlayerInputActions playerInput;

    private void Awake()
    {
        // Singleton
        instance = this;

        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        
        // Finds our stats manager
        statsManager = GameObject.FindWithTag("StatsManager").GetComponent<StatsManager>();

        // Initialize player input actions
        playerInput = new PlayerInputActions();
        playerInput.Game.Move.started += OnMovementInput;
        playerInput.Game.Move.canceled += OnMovementInput;
        playerInput.Game.Move.performed += OnMovementInput;
        playerInput.Game.Nos.started += OnNosInput;
        playerInput.Game.Nos.canceled += OnNosInput;
        playerInput.Game.Nos.performed += OnNosInput;
        playerInput.Game.Accelerate.started += OnAccelerateInput;
        playerInput.Game.Accelerate.canceled += OnAccelerateInput;
        playerInput.Game.Accelerate.performed += OnAccelerateInput;
        playerInput.Game.Brake.started += OnBrakeInput;
        playerInput.Game.Brake.canceled += OnBrakeInput;
        playerInput.Game.Brake.performed += OnBrakeInput;
        playerInput.Game.Drift.started += OnDriftInput;
        playerInput.Game.Drift.canceled += OnDriftInput;
        playerInput.Game.Drift.performed += OnDriftInput;
        playerInput.Game.Reset.performed += OnResetInput;

        // Lock our cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        // Changes Bgm
        audioManager.Pause("Bgm1");
        audioManager.Play("Bgm2");
        
        // Updates our stats
        statsManager.UpdateStats();
        
        // Updates body material
        if (statsManager.bodyMaterial == null) return;
        carBody.GetComponent<Renderer>().material = statsManager.bodyMaterial;
    }

    // used for player movement, call this to enable or disable player input detection
    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
    
    // Steering input
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        var currentMovementInput = context.ReadValue<Vector2>();
        horizontalInput = currentMovementInput.x;
        //verticalInput = currentMovementInput.y;
    }
    
    // Accelerate input
    private void OnAccelerateInput(InputAction.CallbackContext context)
    {
        verticalInput = context.ReadValue<float>();
    }
    
    // Brake input
    private void OnBrakeInput(InputAction.CallbackContext context)
    {
        isBraking = Convert.ToBoolean(context.ReadValue<float>());
        verticalInput = context.ReadValue<float>() * -1;
    }

    // Drift input
    private void OnDriftInput(InputAction.CallbackContext context)
    {
        isDrifting = Convert.ToBoolean(context.ReadValue<float>());
    }
    
    // Nos input
    private void OnNosInput(InputAction.CallbackContext context)
    {
        nosActive = Convert.ToBoolean(context.ReadValue<float>());
    }
    
    // Reset input
    private void OnResetInput(InputAction.CallbackContext context)
    {
        resetActive = Convert.ToBoolean(context.ReadValue<float>());
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
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        HandleNos();
        HandleDrift();
        HandleTireTrails();
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
        currentBrakeForce = isBraking && frontLeftWheelCollider.motorTorque > 0 ? breakForce : 0f;
        
        // Brake slightly when idling
        if (input == 0)
        {
            currentBrakeForce = 100;
        }
        
        // Apply the above
        ApplyBraking();
    }
    
    // Used for engine sounds
    private float GetSpeedRatio()
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
    
    // Tire trails and smoke
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
            currentBrakeForce = 100;
        }
        else
        {
            sidewaysFrictionFL.extremumValue = 0.7f;
            sidewaysFrictionFR.extremumValue = 0.7f;
            sidewaysFrictionRL.extremumValue = 0.7f;
            sidewaysFrictionRR.extremumValue = 0.7f;
            frontLeftWheelCollider.sidewaysFriction = sidewaysFrictionFL;
            frontRightWheelCollider.sidewaysFriction = sidewaysFrictionFR;
            rearLeftWheelCollider.sidewaysFriction = sidewaysFrictionRL;
            rearRightWheelCollider.sidewaysFriction = sidewaysFrictionRR;
        }
    }
    
    // Braking
    private void ApplyBraking() {
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
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