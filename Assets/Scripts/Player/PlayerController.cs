using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Player controls and functions
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Fields
    
    // Singleton access to the controller
    public static PlayerController instance;
    // Static to enabled / disable our headlights for the race & tutorial
    public static bool useHeadLights;
    // Used for letting the game know if we're racing or tutorial
    public static bool isRacing;
    // Static for our nitrous system
    public static bool nosActive;
    // Reset bool
    public bool resetActive;
    // collision bool for sounds
    public bool collision;
    // Stats manager
    private StatsManager statsManager;
    // Audio
    private AudioManager audioManager;
    // Current gear
    private int currentGear;
    // Speed
    private float speed, maxSpeed, speedRatio, motorForce, nosForce, breakForce;
    // Input
    private float input, horizontalInput, verticalInput;
    // Steering and braking
    private bool isDrifting, isBraking;
    private float currentSteerAngle, currentBrakeForce, maxSteerAngle;
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
    // Photon
    [SerializeField] private PhotonView PV;
    [SerializeField] private PhotonTransformView PVTransformView;
    [SerializeField] private PhotonRigidbodyView PVRigidBody;
    // Lap config
    [SerializeField] private GameObject lapCanvas;
    // Lap config
    public TextMeshProUGUI lapCountText;
    public GameObject finalLapReminder;
    private int lapCount;

    #endregion

    #region Properties
    
    /// <summary>
    /// The current lap
    /// </summary>
    public int LapCount
    {
        get => lapCount;
        set => lapCount = value;
    }
    
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
    /// Input
    /// </summary>
    public float Input
    {
        get => input;
        set => input = value;
    }
    
    /// <summary>
    /// Horizontal Input
    /// </summary>
    public float HorizontalInput
    {
        get => horizontalInput;
        set => horizontalInput = value;
    }
    
    /// <summary>
    /// Vertical Input
    /// </summary>
    public float VerticalInput
    {
        get => verticalInput;
        set => verticalInput = value;
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
    
    /// <summary>
    /// Initializes instance and needed objects
    /// </summary>
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
        // Disables photon components if we're in the tutorial as it bugs out
        if (SceneManager.GetActiveScene().name != "Tutorial") return;
        PVTransformView.enabled = false;
        PVRigidBody.enabled = false;
        lapCanvas.SetActive(false);
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
        // WARNING ``````````````````````
        // Set car layer, car1 L6, car 2 L7, car3 L8, car4 L9, car5 L10, change later to for photon
        //gameObject.layer = 6;
    }

    /// <summary>
    /// Engine power determined via input
    /// </summary>
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
    
    /// <summary>
    /// Used for engine sounds
    /// </summary>
    /// <returns></returns>
    private float GetSpeedRatio()
    {
        var gas = Mathf.Clamp(verticalInput, 0.5f, 1f);
        return (speed*gas) / maxSpeed;
    }
    
    /// <summary>
    /// Emits Nos particles and applies force if nos active
    /// </summary>
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
    
    /// <summary>
    /// Tire trails and smoke
    /// </summary>
    private void HandleTireTrails()
    {
        // Checks if both wheels are grounded & if we're drifting or braking
        if (rearLeftWheelCollider.isGrounded && rearRightWheelCollider.isGrounded)
        {
            if (DriftSystem.instance == null) return;
            tireTrailRL.SetActive(DriftSystem.instance.driftActive || isBraking);
            tireTrailRR.SetActive(DriftSystem.instance.driftActive || isBraking);
            driftSmoke.SetActive(DriftSystem.instance.driftActive || isBraking);
        }
    }
    
    /// <summary>
    /// Handles drift friction when enabled
    /// </summary>
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
    
    /// <summary>
    /// Applies braking when active
    /// </summary>
    private void ApplyBraking()
    {
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
    }
    
    /// <summary>
    /// Vehicle steering
    /// </summary>
    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }
    
    /// <summary>
    /// Wheel colliders & transforms
    /// </summary>
    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }
    
    /// <summary>
    /// Wheel position & rotation
    /// </summary>
    /// <param name="wheelCollider">The collider of the wheel</param>
    /// <param name="wheelTransform">The transform of the wheel</param>
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        if (wheelTransform.rotation != rot)
        {
            wheelTransform.rotation = Quaternion.Slerp(wheelTransform.rotation, rot, 0.2f);
        }
        wheelTransform.position = pos;
    }

    /// <summary>
    /// Used to enable collision sound
    /// </summary>
    /// <param name="other">The other object we're colliding with</param>
    private void OnCollisionEnter(Collision other)
    {
        collision = true;
    }
    
    /// <summary>
    /// Tracks speed ratio and car lights
    /// </summary>
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
    
    /// <summary>
    /// Tracks needed vehicle functions
    /// </summary>
    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        HandleNos();
        HandleDrift();
        HandleTireTrails();
    }
    
    #endregion
}