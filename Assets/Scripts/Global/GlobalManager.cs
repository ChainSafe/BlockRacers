using UnityEngine;

/// <summary>
/// Keeps track of our NFT variables
/// </summary>
public class GlobalManager : MonoBehaviour
{
    #region Fields
    
    // String for loading screen
    public string sceneToLoad;
    // Player car
    public GameObject playerCar;
    // Player material
    public Texture2D nftSprite;
    // Player stats
    public int engineLevel, handlingLevel, nosLevel;
    // Connected bool
    public bool connected;
    // Race won bool for wagering
    public bool raceWon;
    // Bool to check if wagering is active or not
    public bool wagering;
    public bool wagerAccepted;
    
    #endregion

    #region Methods
    
    /// <summary>
    /// Locks our frame rate to stop devices overheating and keeps things uniform
    /// </summary>
    private void Awake()
    {
        // Locks framerate to 60 FPS
        Application.targetFrameRate = 60;
        // Makes object global and doesnt destroy it when changing scenes
        DontDestroyOnLoad(this);
    }
    
    #endregion
}
