using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // Global manager
    private GlobalManager globalManager;
    
    // Audio
    private AudioManager audioManager;
    
    // Pause menu
    [SerializeField] private GameObject pauseMenu;

    // Paused bool
    private bool paused;
    
    // Player Input
    private PlayerInputActions playerInput;

    // Our on-screen race UI to disable when we pause
    public GameObject[] raceUI;

    private void Awake()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        
        // Find our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        
        // Initialize player input actions
        playerInput = new PlayerInputActions();
        playerInput.Game.Pause.performed += OnPauseInput;
    }
    
    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
    
    // Pause input
    private void OnPauseInput(InputAction.CallbackContext context)
    {
        if (!paused && CountDownSystem.raceStarted)
        {
            Pause();
        }
        else if (paused && CountDownSystem.raceStarted)
        {
            Unpause();
        }
    }

    // Pauses the game
    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        
        foreach(GameObject raceUI in raceUI)
        {
            raceUI.SetActive(false);
        }

        paused = true;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Unpauses the game
    private void Unpause()
    {
        Cursor.lockState =  CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);

        foreach (GameObject raceUI in raceUI)
        {
            raceUI.SetActive(true);
        }

        paused = false;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Goes to the main menu
    public void MainMenuButton()
    {
        audioManager.Pause("Bgm2");
        audioManager.Play("Bgm1");
        globalManager.sceneToLoad = "MenuScene";
        SceneManager.LoadScene("LoadingScreen");
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
}
