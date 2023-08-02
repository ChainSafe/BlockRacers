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

    private void Awake()
    {
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        
        // Gets our body material object from the global manager which can be changed via garage/marketplace
        nftSprite = globalManager.nftSprite;
        Debug.Log("Creating material from: " + nftSprite);
        nftMaterial.SetTexture("_MainTex", nftSprite);

        // Set our stats
        engineLevel = globalManager.engineLevel;
        handlingLevel = globalManager.handlingLevel;
        nosLevel = globalManager.nosLevel;
    }

    public void UpdateStats()
    {
        // Finds our player controller if its active and sets stats
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        if (playerController == null) return;

        playerController.MaxSpeed = engineLevel switch
        {
            1 => 180f,
            2 => 230f,
            _ => 280f
        };
        
        playerController.MotorForce = engineLevel switch
        {
            1 => 3500,
            2 => 4000,
            _ => 4500
        };

        playerController.MaxSteerAngle = handlingLevel switch
        {
            1 => 40,
            2 => 45,
            _ => 50
        };

        NitrousManager.boostRate = nosLevel switch
        {
            1 => 100f,
            2 => 60f,
            _ => 20f
        };
    }
    
    #endregion
}
