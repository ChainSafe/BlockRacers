using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    // string for loading screen
    public string sceneToLoad;
    // player objects
    private PlayerController playerController;
    public Material bodyMaterial;
    // player stats
    public int engineLevel = 1;
    public int handlingLevel = 1;
    public int nosLevel = 1;
    private void Awake()
    {
        // Makes object global and doesnt destroy it when changing scenes
        DontDestroyOnLoad(this);
    }

    public void UpdateBodyMaterial()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.bodyMaterial = bodyMaterial;
    }
    
}
