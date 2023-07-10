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

    private void Start()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
    }
    
    // Pauses the game
    private void Pause()
    {
        Cursor.lockState =  CursorLockMode.None;
        pauseMenu.SetActive(true);
        paused = true;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Unpauses the game
    private void Unpause()
    {
        Cursor.lockState =  CursorLockMode.Locked;
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
    
    private void Update()
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
