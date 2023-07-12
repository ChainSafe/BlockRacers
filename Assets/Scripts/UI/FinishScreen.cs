using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScreen : MonoBehaviour
{
    // Global Manager
    private GlobalManager globalManager;
    
    // Audio
    private AudioManager audioManager;

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
    }
    public void MainMenuButton()
    {
        globalManager.sceneToLoad = "MainMenu";
        SceneManager.LoadScene("LoadingScreen");
    }
}
