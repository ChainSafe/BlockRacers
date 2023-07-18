using UnityEngine;

public class GlobalManager : MonoBehaviour
{
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

    private void Awake()
    {
        // Locks framerate to 60 FPS
        Application.targetFrameRate = 60;
        
        // Makes object global and doesnt destroy it when changing scenes
        DontDestroyOnLoad(this);
    }
}
