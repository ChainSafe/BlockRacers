using Photon.Pun;
using Photon.Realtime;
using TMPro;
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
    [SerializeField] private GameObject connectMenu, mainMenu, raceMenu, wagerMenu, searchingMenu, connectButton, tutorialButton, oneVsOneButton, searchingBackButton, connectingText;
    // Back buttons
    [SerializeField] private GameObject backButtonNormalRace, backButtonWager; 
    // Players text for multiplayer
    [SerializeField] private TextMeshProUGUI playersReadyNumberText;
    // PHOTON - Are we connected to the master server?
    public static bool connectedToMaster;
    // PHOTON - Username
    [SerializeField] private TMP_InputField usernameInput;
    private string userName;
    private bool loadingLevel;

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

    /// <summary>
    /// Connects a users wallet to the game
    /// </summary>
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

    /// <summary>
    /// Opens the tutorial area
    /// </summary>
    public void TutorialButton()
    {
        globalManager.sceneToLoad = "Tutorial";
        SceneManager.LoadScene("LoadingScreen");
        CountDownSystem.raceStarted = true;
        PlayerController.useHeadLights = true;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

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

    /////////////////////////////////////////
    /// PHOTON LOBBY CONFIGURATION STARTS ///
    /////////////////////////////////////////
    
    /// <summary>
    /// PHOTON Fires once a user is connected to proceed and let's the user know we're connected
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // Syncs Photon to ensure scenes are all loaded correctly
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Connected To Master Server!");
        connectedToMaster = true;
        // Deactivates connecting text
        connectingText.SetActive(false);
        // Opens race menu
        raceMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(oneVsOneButton);
    }
    
    /// <summary>
    /// If we joined the room successfully, the scene changes based on the lobby we join
    /// </summary>
    public override void OnJoinedRoom() 
    {
        // Sets our username
        SetUsername();
        Debug.Log($"Player Index {PhotonNetwork.LocalPlayer.ActorNumber}");
        // Sets our players ready text to 1 as we join
        playersReadyNumberText.text = "1";
        // Sets race config
        PlayerController.isRacing = true;
        PlayerController.useHeadLights = false;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    /// <summary>
    /// Opens the race menu and connects to photon for multiplayer
    /// </summary>
    public void RaceButton()
    {
        // PHOTON If we're not connected to photon, connect
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            // Activates connecting text
            connectingText.SetActive(true);
        }
        else
        {
            // Opens race menu
            raceMenu.SetActive(true);
            // Sets our first selected button
            EventSystem.current.SetSelectedGameObject(oneVsOneButton);
        }
        // Closes the menu to stop duplicate connections
        mainMenu.SetActive(false);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Takes the user from the race menu to the main menu and disconnects from photon
    /// </summary>
    public void RaceMenuBackButton()
    {
        wagerMenu.SetActive(false);
        raceMenu.SetActive(false);
        mainMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(tutorialButton);
        // Disconnects from photon
        PhotonNetwork.Disconnect();
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    /// <summary>
    /// Lets the user create or search for a 1v1 room
    /// </summary>
    public void RaceMenu1v1Button()
    {
        SearchingMenu();
        // The Official Room Options for 1v1, TTL set low to ensure there are no dead rooms
        RoomOptions roomOps = new RoomOptions() { IsOpen = true, IsVisible = true, PlayerTtl = 300, MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom("RaceLobby1v1", roomOps, TypedLobby.Default);
    }
    
    /// <summary>
    /// Lets the user create or search for a 1v1 room
    /// </summary>
    public void RaceMenu1v1WagerButton()
    {
        SearchingMenu();
        // The Official Room Options for 1v1, TTL set low to ensure there are no dead rooms
        RoomOptions roomOps = new RoomOptions() { IsOpen = true, IsVisible = true, PlayerTtl = 300, MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom("RaceLobby1v1Wager", roomOps, TypedLobby.Default);
    }
    
    /// <summary>
    /// Lets the user create or search for 5 man a room
    /// </summary>
    public void RaceMenu5ManButton()
    {
        SearchingMenu();
        // The Official Room Options for 5 man races, TTL set low to ensure there are no dead rooms
        RoomOptions roomOps = new RoomOptions() { IsOpen = true, IsVisible = true, PlayerTtl = 300, MaxPlayers = 5 };
        PhotonNetwork.JoinOrCreateRoom("RaceLobby5Man", roomOps, TypedLobby.Default);
    }

    /// <summary>
    /// Opens the wager menu
    /// </summary>
    public void WagerButton()
    {
        raceMenu.SetActive(false);
        wagerMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(oneVsOneButton);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    /// <summary>
    /// Opens the searching menu
    /// </summary>
    private void SearchingMenu()
    {
        raceMenu.SetActive(false);
        searchingMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(searchingBackButton);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Takes the user from the searching menu to the race menu
    /// </summary>
    public void SearchingMenuBackButton()
    {
        PhotonNetwork.LeaveRoom();
        searchingMenu.SetActive(false);
        raceMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(oneVsOneButton);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    /// <summary>
    /// Sets the users username, a default one is chosen if none is present
    /// </summary>
    private void SetUsername()
    {
        if (usernameInput.text == "")
        {
            // Sets a random username is none is chosen
            int rand = Random.Range(1, 6);
            PhotonNetwork.LocalPlayer.NickName = rand switch
            {
                1 => "Gary",
                2 => "MoonCake",
                3 => "Avocado",
                4 => "Kevin",
                _ => "Hue"
            };
        }
        else
        {
            // Sets our user name to what we have chosen
            PhotonNetwork.LocalPlayer.NickName = usernameInput.text;
        }
    }
    
    /// <summary>
    /// Loads the race track for everyone
    /// </summary>
    private void LoadRaceTrack()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Loads level
            PhotonNetwork.LoadLevel("RaceTrack");
        }
    }

    /// <summary>
    /// Updates players in room and checks if we're ready to race
    /// </summary>
    private void Update()
    {
        if (!PhotonNetwork.InRoom) return;
        playersReadyNumberText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        switch (PhotonNetwork.CurrentRoom.Name)
        {
            // 1v1 config
            case "RaceLobby1v1":
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (!loadingLevel)
                        {
                            loadingLevel = true;
                            backButtonNormalRace.SetActive(false);
                            // Loads level
                            Invoke("LoadRaceTrack", 3);
                        }
                    }
                }
                break;
            }
            // 1v1 wager config
            case "RaceLobby1v1Wager":
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                {
                    searchingMenu.SetActive(false);
                    wagerMenu.SetActive(true);
                    if (globalManager.wagerAccepted)
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            if (!loadingLevel)
                            {
                                loadingLevel = true;
                                backButtonWager.SetActive(false);
                                // Loads level
                                Invoke("LoadRaceTrack", 3);
                            }
                        }
                    }
                }
                break;
            }
            // 5man config
            case "RaceLobby5Man":
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount == 5)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (!loadingLevel)
                        {
                            loadingLevel = true;
                            backButtonNormalRace.SetActive(false);
                            // Loads level
                            Invoke("LoadRaceTrack", 3);
                        }
                    }
                }
                break;
            }
        }
    }

    /////////////////////////////////////////
    /// PHOTON LOBBY CONFIGURATION ENDS /////
    /////////////////////////////////////////

    #endregion
}
