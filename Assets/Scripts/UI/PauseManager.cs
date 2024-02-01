using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Pause manager functionality
/// </summary>
public class PauseManager : MonoBehaviourPunCallbacks
{
    #region Fields

    // Our on-screen race UI to disable when we pause
    public GameObject[] raceUI;

    // Audio
    private AudioManager audioManager;

    // Pause menu
    [SerializeField] private GameObject pauseMenu;

    // Options menu
    [SerializeField] private GameObject optionsMenu;

    // Controls menu
    [SerializeField] private GameObject controlsMenu;

    // Buttons
    [SerializeField] private GameObject firstButton, gyroButton;

    [SerializeField] private Toggle gyroToggle;

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
        if (!Application.isMobilePlatform) return;
        gyroToggle.isOn = globalManager.gyroEnabled;
        gyroButton.SetActive(true);
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
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(firstButton);
        // Unlocks the cursor so the user can select things
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Sets our paused bool
        pauseMenu.SetActive(true);
        paused = true;
        if (raceUI == null) return;
        foreach (GameObject raceUI in raceUI)
        {
            raceUI.SetActive(false);
        }
        // Plays pause sound
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Unpauses the game
    /// </summary>
    private void Unpause()
    {
        // Locks the cursor so the user can resume playing normally
        Cursor.lockState = CursorLockMode.Locked;
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
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Goes to the main menu
    /// </summary>
    public void MainMenuButton()
    {
        audioManager.Pause("Bgm2");
        audioManager.Play("Bgm1");
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            PhotonNetwork.LeaveRoom();
        }
        globalManager.sceneToLoad = "MenuScene";
        SceneManager.LoadScene("LoadingScreen");
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Opens the controls menu
    /// </summary>
    public void OpenControlsButton()
    {
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(true);
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Closes the controls menu
    /// </summary>
    public void CloseControlsButton()
    {
        controlsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Opens the options menu
    /// </summary>
    public void OpenOptionsButton()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Closes the options menu
    /// </summary>
    public void CloseOptionsButton()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Toggles fullscreen mode
    /// </summary>
    public void FullScreenButton()
    {
        Screen.fullScreen = !Screen.fullScreen;
        audioManager.Play("MenuSelect");
    }
    
    /// <summary>
    /// Toggles gyro movement
    /// </summary>
    public void ToggleGyro()
    {
        globalManager.gyroEnabled = !globalManager.gyroEnabled;
        gyroToggle.isOn = globalManager.gyroEnabled;
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Enables player input
    /// </summary>
    public override void OnEnable()
    {
        playerInput.Enable();
    }

    /// <summary>
    /// Disables player input
    /// </summary>
    public override void OnDisable()
    {
        playerInput.Disable();
    }

    #endregion
}