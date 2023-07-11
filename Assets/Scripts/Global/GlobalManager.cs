using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    // String for loading screen
    public string sceneToLoad;
    
    // Player objects
    private PlayerController playerController;
    public Material bodyMaterial;
    
    // Player stats
    public int engineLevel = 1;
    public int handlingLevel = 1;
    public int nosLevel = 1;
    
    private void Awake()
    {
        // Makes object global and doesnt destroy it when changing scenes
        DontDestroyOnLoad(this);
    }
    
    // Updates our players body material to what's on this object
    public void UpdateBodyMaterial()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.bodyMaterial = bodyMaterial;
    }
    
}
