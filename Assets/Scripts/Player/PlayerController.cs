using UnityEngine;

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
    public float input, horizontalInput, verticalInput;

    // Steering and braking
    [Header("Steering & Braking")]
    public bool isDrifting;
    public bool isBraking;
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
    [SerializeField] private GameObject nosParticles, tireTrailRL, tireTrailRR, driftSmoke, nftImage;

    // Taillights & headlights
    [SerializeField] private GameObject tailLights, headLights;

    // Wheel colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider, rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform, rearLeftWheelTransform, rearRightWheelTransform;

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
        if (statsManager.nftImage == null) return;
        nftImage.GetComponent<Renderer>().material = statsManager.nftImage;
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
<<<<<<< HEAD
            if (DriftSystem.instance == null) return;
=======
>>>>>>> main
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