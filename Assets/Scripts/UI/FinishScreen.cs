using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScreen : MonoBehaviour
{
    // Global Manager
    private GlobalManager globalManager;

    private void Awake()
    {
        // Find our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void MainMenuButton()
    {
        globalManager.sceneToLoad = "MainMenu";
        SceneManager.LoadScene("LoadingScreen");
    }
}
