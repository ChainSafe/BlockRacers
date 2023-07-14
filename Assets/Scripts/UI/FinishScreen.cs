using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FinishScreen : MonoBehaviour
{
    // Global Manager
    private GlobalManager globalManager;
    
    // Audio
    private AudioManager audioManager;
    
    // Buttons
    [SerializeField] private GameObject firstButton;

    private void Awake()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Changes Bgm
        audioManager.Pause("Bgm2");
        audioManager.Play("Bgm1");
        
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
    
    public void OnMouseOverButton(GameObject button)
    {
        // Sets our selected button to what we've moused over
        EventSystem.current.SetSelectedGameObject(button);
    }
    public void MainMenuButton()
    {
        globalManager.sceneToLoad = "MainMenu";
        SceneManager.LoadScene("LoadingScreen");
    }
}
