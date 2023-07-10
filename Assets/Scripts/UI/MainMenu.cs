using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Audio
    private AudioManager audioManager;
    
    // Menu items
    [SerializeField] private GameObject ConnectMenuItems;
    [SerializeField] private GameObject MainMenuItems;

    void Start()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Change later when sdk is in
    public void ConnectButton()
    {
        ConnectMenuItems.SetActive(false);
        MainMenuItems.SetActive(true);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    public void RaceButton()
    {
        PlayerController.isRacing = true;
        SceneManager.LoadScene("RaceTrack");
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    public void TutorialButton()
    {
        SceneManager.LoadScene("Tutorial");
        CountDownSystem.raceStarted = true;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    public void GarageButton()
    {
        SceneManager.LoadScene("Garage");
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    public void QuitButton()
    {
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
        Application.Quit();
    }
}
