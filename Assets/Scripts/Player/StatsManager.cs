using UnityEngine;

/// <summary>
/// Manages the stats for our chosen NFT
/// </summary>
public class StatsManager : MonoBehaviour
{
    #region Fields

    // Global
    private GlobalManager globalManager;
    // Stats
    private int engineLevel;
    private int handlingLevel;
    private int nosLevel;
    // Body Material
    public Material nftMaterial;
    private Texture2D nftSprite;
    // Player objects
    [SerializeField] private PlayerController playerController;

    #endregion

    #region Methods
    
    /// <summary>
    /// Initializes needed objects and sets our levels based on our selected NFT
    /// </summary>
    private void Awake()
    {
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        // Gets our body material object from the global manager which can be changed via garage/marketplace
        nftSprite = globalManager.nftSprite;
        nftMaterial.SetTexture("_MainTex", nftSprite);
        // Set our stats
        engineLevel = globalManager.engineLevel;
        handlingLevel = globalManager.handlingLevel;
        nosLevel = globalManager.nosLevel;
    }
    
    /// <summary>
    /// Updates our stats based on our chosen NFT
    /// </summary>
    public void UpdateStats()
    {
        // Finds our player controller if its active and sets stats
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (playerController == null) return;
        // Engine based on upgraded level
        playerController.MaxSpeed = engineLevel switch
        {
            1 => 180f,
            2 => 230f,
            _ => 280f
        };
        // Motor force based on engine level
        playerController.MotorForce = engineLevel switch
        {
            1 => 3500,
            2 => 4000,
            _ => 4500
        };
        // Steering angle based on upgrade level
        playerController.MaxSteerAngle = handlingLevel switch
        {
            1 => 40,
            2 => 45,
            _ => 50
        };
        // Nos boost rate based on level
        NitrousManager.boostRate = nosLevel switch
        {
            1 => 100f,
            2 => 60f,
            _ => 20f
        };
    }
    
    #endregion
}
