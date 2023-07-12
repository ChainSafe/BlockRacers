using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    // Stats manager
    public StatsManager statsManager;
    
    // String for loading screen
    public string sceneToLoad;
    
    // Player material
    public Material bodyMaterial;
    
    // Player stats
    public int engineLevel;
    public int handlingLevel;
    public int nosLevel;
    
    private void Awake()
    {
        // Makes object global and doesnt destroy it when changing scenes
        DontDestroyOnLoad(this);
    }
    
    // Updates our players body material to what's on this object
    public void UpdateBodyMaterial()
    {
        // Finds our stats manager
        statsManager = GameObject.FindWithTag("StatsManager").GetComponent<StatsManager>();
        statsManager.bodyMaterial = bodyMaterial;
    }
    
}
