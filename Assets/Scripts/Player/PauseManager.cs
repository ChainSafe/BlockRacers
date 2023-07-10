using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Time.timeScale = 0;
        paused = true;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Unpauses the game
    void Unpause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        paused = false;
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
