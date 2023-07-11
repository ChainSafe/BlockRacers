using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public string sceneToLoad;
    public Material bodyMaterial;
    private PlayerController playerController;
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
