using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Main menu functionality
/// </summary>
public class MainMenu : MonoBehaviourPunCallbacks
{
    #region Fields
    
    // Global Manager
    private GlobalManager globalManager;

    // Audio
    private AudioManager audioManager;

    // Menu items
    [SerializeField] private GameObject connectMenuItems, mainMenuItems, connectButton, tutorialButton;

    #endregion

    #region Methods

    void Awake()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();

        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();

        // By passes connection screen if connected
        if (globalManager.connected)
        {
            connectMenuItems.SetActive(false);
            mainMenuItems.SetActive(true);
            // Sets our first selected button
            EventSystem.current.SetSelectedGameObject(tutorialButton);
        }
        else
        {
            // Sets our first selected button
            EventSystem.current.SetSelectedGameObject(connectButton);
        }
    }

    public void OnMouseOverButton(GameObject button)
    {
        // Sets our selected button to what we've moused over
        EventSystem.current.SetSelectedGameObject(button);
    }

    // Change later when sdk is in
    public void ConnectButton()
    {
        connectMenuItems.SetActive(false);
        mainMenuItems.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(tutorialButton);
        globalManager.connected = true;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /////////////////////////////////////////
    /// PHOTON LOBBY CONFIGURATION STARTS ///
    /////////////////////////////////////////

    // This will somehow need to be merged with the Photon.LoadScene method and the function below..
    public void RaceButton()
    {
        PlayerController.isRacing = true;
        PlayerController.useHeadLights = false;
        globalManager.sceneToLoad = "RaceTrack";
        SceneManager.LoadScene("LoadingScreen");
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    // The lobby configuration for our race. Needs to be merged with the above RaceButton Function
    public void JoinRace()
    {
        // The Official Room Options
        RoomOptions roomOps = new RoomOptions() { IsOpen = true, IsVisible = true, PlayerTtl = 7200 };

        PhotonNetwork.JoinOrCreateRoom("RaceLobby", roomOps, TypedLobby.Default);
    }


    // The lobby configuration for our tutorial. We can keep this single player if you want though. 
    public void JoinTutorial()
    {
        // The Official Room Options
        RoomOptions roomOps = new RoomOptions() { IsOpen = true, IsVisible = true, PlayerTtl = 7200 };

        PhotonNetwork.JoinOrCreateRoom("Tutorial", roomOps, TypedLobby.Default);
    }

    // This will somehow need to be merged with the Photon.LoadScene method and the function above..
    public void TutorialButton()
    {
        globalManager.sceneToLoad = "Tutorial";
        SceneManager.LoadScene("LoadingScreen");
        CountDownSystem.raceStarted = true;
        PlayerController.useHeadLights = true;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    // If we joined the room successfully, the scene changes based on the lobby we join.
    public override void OnJoinedRoom() 
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;

        if (currentRoom.Name == "RaceLobby")
        {
            PhotonNetwork.LoadLevel("RaceTrack");
        }
        else if (currentRoom.Name == "Tutorial")
        {
            PhotonNetwork.LoadLevel("Tutorial");
        }
    }


    /////////////////////////////////////////
    /// PHOTON LOBBY CONFIGURATION ENDS /////
    /////////////////////////////////////////


    public void GarageButton()
    {
        globalManager.sceneToLoad = "Garage";
        SceneManager.LoadScene("LoadingScreen");
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    public void QuitButton()
    {
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
        Application.Quit();
    }
    
    #endregion
}
