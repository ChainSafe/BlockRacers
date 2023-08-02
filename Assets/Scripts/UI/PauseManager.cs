using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Pause manager functionality
/// </summary>
public class PauseManager : MonoBehaviour
{
    #region Fields

    // Our on-screen race UI to disable when we pause
    public GameObject[] raceUI;
    // Audio
    private AudioManager audioManager;
    // Pause menu
    [SerializeField] private GameObject pauseMenu;
    // Buttons
    [SerializeField] private GameObject firstButton;
    // Paused bool
    private bool paused;
    // Player Input
    private PlayerInputActions playerInput;
    // Global manager
    private GlobalManager globalManager;

    #endregion

    #region Methods

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
    
    /// <summary>
    /// Enables player input
    /// </summary>
    private void OnEnable()
    {
        playerInput.Enable();
    }
    
    /// <summary>
    /// Disables player input
    /// </summary>
    private void OnDisable()
    {
        playerInput.Disable();
    }
    
    /// <summary>
    /// Sets our selected button to what we've moused over
    /// </summary>
    /// <param name="button"></param>
    public void OnMouseOverButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }
    
    /// <summary>
    /// Pause input
    /// </summary>
    /// <param name="context"></param>
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

    /// <summary>
    /// Pauses the game
    /// </summary>
    private void Pause()
    {
        // Unlocks the cursor so the user can select things
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Sets our paused bool
        pauseMenu.SetActive(true);
        paused = true;
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(firstButton);
        if (raceUI == null) return;
        foreach(GameObject raceUI in raceUI)
        {
            raceUI.SetActive(false);
        }
        // Plays pause sound
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    /// <summary>
    /// Unpauses the game
    /// </summary>
    private void Unpause()
    {
        // Locks the cursor so the user can resume playing normally
        Cursor.lockState =  CursorLockMode.Locked;
        Cursor.visible = false;
        // Sets our paused bool
        pauseMenu.SetActive(false);
        paused = false;
        if (raceUI == null) return;
        foreach (GameObject raceUI in raceUI)
        {
            raceUI.SetActive(true);
        }
        // Plays pause sound
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    /// <summary>
    /// Goes to the main menu
    /// </summary>
    public void MainMenuButton()
    {
        audioManager.Pause("Bgm2");
        audioManager.Play("Bgm1");
        globalManager.sceneToLoad = "MenuScene";
        SceneManager.LoadScene("LoadingScreen");
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    #endregion
}
