using UnityEngine;
using UnityEngine.SceneManagement;

public class GarageMenu : MonoBehaviour
{
    // Audio
    private AudioManager audioManager;
    
    void Start()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
    }
    
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MenuScene");
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
}