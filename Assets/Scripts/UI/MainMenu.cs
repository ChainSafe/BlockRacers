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
    [SerializeField] private GameObject connectMenu, mainMenu, raceMenu, connectButton, tutorialButton, oneVsOneButton;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes objects and checks if we're connected
    /// </summary>
    private void Awake()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        // By passes connection screen if connected
        if (globalManager.connected)
        {
            connectMenu.SetActive(false);
            mainMenu.SetActive(true);
            // Sets our first selected button
            EventSystem.current.SetSelectedGameObject(tutorialButton);
        }
        else
        {
            // Sets our first selected button
            EventSystem.current.SetSelectedGameObject(connectButton);
        }
    }
    
    /// <summary>
    /// Sets our selected button to what we've moused over
    /// </summary>
    /// <param name="button">The button being moused over</param>
    public void OnMouseOverButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }

    // Change later when sdk is in
    public void ConnectButton()
    {
        connectMenu.SetActive(false);
        mainMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(tutorialButton);
        globalManager.connected = true;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    public void RaceButton()
    {
        mainMenu.SetActive(false);
        raceMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(oneVsOneButton);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    public void RaceMenuBackButton()
    {
        raceMenu.SetActive(false);
        mainMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(tutorialButton);
        globalManager.connected = true;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    public void TutorialButton()
    {
        globalManager.sceneToLoad = "Tutorial";
        SceneManager.LoadScene("LoadingScreen");
        CountDownSystem.raceStarted = true;
        PlayerController.useHeadLights = true;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /////////////////////////////////////////
    /// PHOTON LOBBY CONFIGURATION STARTS ///
    /////////////////////////////////////////

    // This will somehow need to be merged with the Photon.LoadScene method and the function below..
    
    public void RaceMenu1v1Button()
    {
        // The Official Room Options for 1v1
        RoomOptions roomOps = new RoomOptions() { IsOpen = true, IsVisible = true, PlayerTtl = 7200 };
        PhotonNetwork.JoinOrCreateRoom("RaceLobby1v1", roomOps, TypedLobby.Default);
    }
    
    public void RaceMenu5ManButton()
    {
        // The Official Room Options for 5 man races
        RoomOptions roomOps = new RoomOptions() { IsOpen = true, IsVisible = true, PlayerTtl = 7200 };
        PhotonNetwork.JoinOrCreateRoom("RaceLobby5Man", roomOps, TypedLobby.Default);
    }

    // If we joined the room successfully, the scene changes based on the lobby we join
    // 1v1 & 5 man config duplicated whilst building
    public override void OnJoinedRoom() 
    {
        // Sets our current room
        Room currentRoom = PhotonNetwork.CurrentRoom;
        // 1v1 config
        if (PhotonNetwork.CurrentRoom.Name == "RaceLobby1v1")
        {
            // Set to start race for now whilst we build functionality for photon
            PlayerController.isRacing = true;
            PlayerController.useHeadLights = false;
            if (audioManager == null) return;
            FindObjectOfType<AudioManager>().Play("MenuSelect");
            PhotonNetwork.LoadLevel("RaceTrack");   
        }
        // 5man config
        else if (PhotonNetwork.CurrentRoom.Name == "RaceLobby5Man")
        {
            // Set to start race for now whilst we build functionality for photon
            PlayerController.isRacing = true;
            PlayerController.useHeadLights = false;
            if (audioManager == null) return;
            FindObjectOfType<AudioManager>().Play("MenuSelect");
            PhotonNetwork.LoadLevel("RaceTrack"); 
        }
    }


    /////////////////////////////////////////
    /// PHOTON LOBBY CONFIGURATION ENDS /////
    /////////////////////////////////////////

    /// <summary>
    /// Takes the user to the garage
    /// </summary>
    public void GarageButton()
    {
        globalManager.sceneToLoad = "Garage";
        SceneManager.LoadScene("LoadingScreen");
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitButton()
    {
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
        Application.Quit();
    }
    
    #endregion
}
