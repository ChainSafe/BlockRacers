using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // Audio
    private AudioManager audioManager;
    
    // Pause menu
    [SerializeField] private GameObject pauseMenu;
    
    // Paused bool
    private bool paused;

    void Start()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
    }
    
    
    // Pauses the game
    void Pause()
    {
        pauseMenu.SetActive(true);
        paused = true;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Unpauses the game
    void Unpause()
    {
        pauseMenu.SetActive(false);
        paused = false;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Goes to the main menu
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MenuScene");
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
    }
}
