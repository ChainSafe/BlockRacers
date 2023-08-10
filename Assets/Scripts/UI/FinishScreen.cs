using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads the finish screen once the race is over
/// </summary>
public class FinishScreen : MonoBehaviour
{
    #region Fields

    // Global Manager
    private GlobalManager globalManager;

    // Audio
    private AudioManager audioManager;

    // Buttons
    [SerializeField] private GameObject menuButton, claimButton;

    // Winning player text
    [SerializeField] private TextMeshProUGUI winningPlayerText;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes needed objects, locks our cursor and changes BGM
    /// </summary>
    private void Awake()
    {
        // Stops scene sync
        PhotonNetwork.AutomaticallySyncScene = false;
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
        EventSystem.current.SetSelectedGameObject(menuButton);
        PhotonNetwork.LeaveRoom();
        // Enables the claim button if we've won
        if (globalManager.raceWon && globalManager.wagering)
        {
            claimButton.SetActive(true);
        }

        // Resets our race won and wagering bools for the next match
        globalManager.raceWon = false;
        globalManager.wagering = false;
        globalManager.wagerAccepted = false;
        // Sets the winning player text
        winningPlayerText.text = globalManager.winningPlayer;
    }

    /// <summary>
    /// Sets our selected button to what we've moused over
    /// </summary>
    /// <param name="button"></param>
    public void OnMouseOverButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void ClaimWinnings()
    {
        // Claim functionality goes here
        Debug.Log("Claiming Winnings! Congratulations!");
        globalManager.wagerAmount = 0;
    }

    /// <summary>
    /// Our main menu button, disconnects us from photon
    /// </summary>
    public void MainMenuButton()
    {
        PhotonNetwork.Disconnect();
        globalManager.sceneToLoad = "MenuScene";
        SceneManager.LoadScene("LoadingScreen");
    }

    #endregion
}