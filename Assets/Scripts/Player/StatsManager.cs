using UnityEngine;

public class StatsManager : MonoBehaviour
{
    // Global
    private GlobalManager globalManager;
    
    // Stats
    private int engineLevel;
    private int handlingLevel;
    private int nosLevel;
    
    // Body Material
    public Material bodyMaterial;
    
    // Player objects
    [SerializeField] private PlayerController playerController;
    
    private void Awake()
    {
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        
        // Gets our body material object from the global manager which can be changed via garage/marketplace
        globalManager.UpdateBodyMaterial();

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

        playerController.maxSpeed = engineLevel switch
        {
            1 => 180f,
            2 => 230f,
            _ => 280f
        };

        playerController.maxSteerAngle = handlingLevel switch
        {
            1 => 40,
            2 => 50,
            _ => 60
        };

        NitrousManager.boostRate = nosLevel switch
        {
            1 => 80f,
            2 => 60f,
            _ => 45f
        };
    }
}
