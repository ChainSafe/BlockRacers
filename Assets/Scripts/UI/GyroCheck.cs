using UnityEngine;

public class GyroCheck : MonoBehaviour
{
    private GlobalManager globalManager;
    private AudioManager audioManager;
    public GameObject GyroCheckMenu;

    private void Awake()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        // Find our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        if (Application.isMobilePlatform)
        {
            GyroCheckMenu.SetActive(true);
        }
    }

    public void EnableGyro()
    {
        globalManager.gyroEnabled = true;
        GyroCheckMenu.SetActive(false);
        // Plays pause sound
        audioManager.Play("MenuSelect");
    }
    
    public void DisableGyro()
    {
        globalManager.gyroEnabled = false;
        GyroCheckMenu.SetActive(false);
        // Plays pause sound
        audioManager.Play("MenuSelect");
    }
}
