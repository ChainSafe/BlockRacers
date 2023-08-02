using UnityEngine;

/// <summary>
/// Player controls and functions
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Fields
    
    // Stats manager
    private StatsManager statsManager;

    // Singleton access to the controller
    public static PlayerController instance;

    // Audio
    private AudioManager audioManager;
    
    // Current gear
    private int currentGear;
    
    // Speed
    private float speed, maxSpeed, speedRatio, motorForce, nosForce, breakForce;
    
    // Static for our nitrous system
    public static bool nosActive;

    // Input
    public float input, horizontalInput, verticalInput;

    // Steering and braking
    private bool isDrifting, isBraking;
    private float currentSteerAngle, currentBrakeForce, maxSteerAngle;

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
    [SerializeField] private GameObject nosParticles, tireTrailRL, tireTrailRR, driftSmoke, nftImage;

    // Taillights & headlights
    [SerializeField] private GameObject tailLights, headLights;

    // Wheel colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider, rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform, rearLeftWheelTransform, rearRightWheelTransform;
    
    #endregion

    #region Properties
    
    /// <summary>
    /// The current gear being used
    /// </summary>
    public int CurrentGear
    {
        get => currentGear;
        set => currentGear = value;
    }
    
    /// <summary>
    /// Speed of the car
    /// </summary>
    public float Speed
    {
        get => speed;
    }
    
    /// <summary>
    /// Max speed of the car
    /// </summary>
    public float MaxSpeed
    {
        get => maxSpeed;
        set => maxSpeed = value;
    }
    
    /// <summary>
    /// Speed ratio used for sounds
    /// </summary>
    public float SpeedRatio
    {
        get => speedRatio;
    }
    
    /// <summary>
    /// Motor force of the cars engine
    /// </summary>
    public float MotorForce
    {
        get => motorForce;
        set => motorForce = value;
    }
    
    /// <summary>
    /// Steering angle of the car
    /// </summary>
    public float MaxSteerAngle
    {
        get => maxSteerAngle;
        set => maxSteerAngle = value;
    }
    
    /// <summary>
    /// Checks if we're currently drifting
    /// </summary>
    public bool IsDrifting
    {
        get => isDrifting;
        set => isDrifting = value;
    }
    
    /// <summary>
    /// Checks if we're currently braking
    /// </summary>
    public bool IsBraking
    {
        get => isBraking;
        set => isBraking = value;
    }

    #endregion

    #region Methods

    private void Awake()
    {
        // Singleton
        instance = this;

        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        
        // Finds our stats manager
        statsManager = GameObject.FindWithTag("StatsManager").GetComponent<StatsManager>();

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
        if (statsManager.nftMaterial == null) return;
        nftImage.GetComponent<Renderer>().material = statsManager.nftMaterial;
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
            frontRightWheelCollider.motorTorque = 0;
        }

        // Check if braking
        currentBrakeForce = isBraking && frontLeftWheelCollider.motorTorque > 0 ? breakForce : 0f;
        
        // Brake slightly when idling
        if (input == 0)
        {
            currentBrakeForce = 50;
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
            if (DriftSystem.instance == null) return;
            tireTrailRL.SetActive(DriftSystem.instance.driftActive || isBraking);
            tireTrailRR.SetActive(DriftSystem.instance.driftActive || isBraking);
            driftSmoke.SetActive(DriftSystem.instance.driftActive || isBraking);
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
            sidewaysFrictionFL.extremumValue = 0.4f;
            sidewaysFrictionFR.extremumValue = 0.4f;
            sidewaysFrictionRL.extremumValue = 0.4f;
            sidewaysFrictionRR.extremumValue = 0.4f;
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
        if (wheelTransform.rotation != rot)
        {
            wheelTransform.rotation = Quaternion.Slerp(wheelTransform.rotation, rot, 0.2f);
        }
        wheelTransform.position = pos;
    }

    // Bool for collision sound
    private void OnCollisionEnter(Collision other)
    {
        collision = true;
    }
    
    #endregion
}