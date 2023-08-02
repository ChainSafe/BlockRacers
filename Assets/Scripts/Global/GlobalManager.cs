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
    
    #endregion

    #region Methods

    private void Awake()
    {
        // Locks framerate to 60 FPS
        Application.targetFrameRate = 60;
        
        // Makes object global and doesnt destroy it when changing scenes
        DontDestroyOnLoad(this);
    }
    
    #endregion
}
