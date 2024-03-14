using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using Photon.Pun;
using Scripts.EVM.Token;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
    
    // Coroutine
    private Coroutine getWeb3Coroutine;

    // Menu items
    [SerializeField] private GameObject
        mainMenu,
        lootBoxesMenu,
        selectCarMenu,
        raceMenu,
        wagerMenu,
        searchingMenu,
        tutorialButton,
        oneVsOneButton,
        selectCarRaceButton,
        searchingBackButton;

    // Back buttons
    [SerializeField] private GameObject
        backButtonNormalRace,
        backButtonWager,
        playersWagerReadyObj,
        playersReady2Obj,
        playersReady5Obj;

    // UI texts
    [SerializeField] private TextMeshProUGUI
        playersReadyNumberText,
        walletAddress,
        raceTokensText;

    // Players names arrays for multiplayer
    [SerializeField] private TextMeshProUGUI[] playerWagerNames, player2v2Names, player5v5Names;

    // PHOTON - Username
    [SerializeField] private TMP_InputField usernameInput;
    private string userName;
    private bool loadingLevel;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes objects and checks if we're connected
    /// </summary>
    private async void Awake()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(tutorialButton);
        // Set our connected wallet
        walletAddress.text = await Web3Accessor.Web3.Signer.GetAddress();
        // Get race tokens
        await GetRaceTokenBalance();
    }

    public async Task GetRaceTokenBalance()
    {
        var response = await Erc20.CustomTokenBalance(Web3Accessor.Web3, ContractManager.TokenAbi, ContractManager.TokenContract);
        // Conversion with 18 decimals
        double ethConversion = double.Parse(response.ToString()) / 1000000000000000000;
        Debug.Log($"Race Tokens: {ethConversion}");
        raceTokensText.text = ethConversion.ToString();
        globalManager.raceTokens = ethConversion;
    }
    
    /// <summary>
    /// Copies the wallet address
    /// </summary>
    public void CopyWalletAddress()
    {
        audioManager.Play("MenuSelect");
        GUIUtility.systemCopyBuffer = walletAddress.text;
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
    /// Opens the tutorial area
    /// </summary>
    public void TutorialButton()
    {
        globalManager.sceneToLoad = "Tutorial";
        SceneManager.LoadScene("LoadingScreen");
        CountDownSystem.raceStarted = true;
        PlayerController.useHeadLights = true;
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Takes the user to the garage
    /// </summary>
    public void GarageButton()
    {
        globalManager.sceneToLoad = "Garage";
        SceneManager.LoadScene("LoadingScreen");
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitButton()
    {
        audioManager.Play("MenuSelect");
        Application.Quit();
    }

    #region Photon

    /// <summary>
    /// If we joined the room successfully, the scene changes based on the lobby we join
    /// </summary>
    public override void OnJoinedRoom()
    {
        // Instantiates username prefab
        PhotonNetwork.Instantiate("LobbyUserName",
            new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, 0);
        // Sets our username
        SetUsername();
        // Sets our players ready text to 1 as we join
        playersReadyNumberText.text = "1";
        // Sets race config
        PlayerController.isRacing = true;
        PlayerController.useHeadLights = false;
        // Editor debug
        // if (Application.isEditor)
        // {
        //     if (PhotonNetwork.IsMasterClient)
        //     {
        //         if (!loadingLevel)
        //         {
        //             loadingLevel = true;
        //             backButtonNormalRace.SetActive(false);
        //             // Loads level
        //             Invoke(nameof(LoadRaceTrack), 3);
        //         }
        //     }
        // }
        audioManager.Play("MenuSelect");
    }

    public void OpenSelectCarMenu()
    {
        // Opens race menu
        selectCarMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(oneVsOneButton);
        // Closes the main menu
        mainMenu.SetActive(false);
        audioManager.Play("MenuSelect");
    }
    
    public void CloseSelectCarMenu()
    {
        // Opens the main menu
        mainMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(oneVsOneButton);
        // Closes the select car menu
        selectCarMenu.SetActive(false);
        audioManager.Play("MenuSelect");
    }
    
    /// <summary>
    /// Opens lootboxes menu
    /// </summary>
    public void OpenLootboxesMenu()
    {
        mainMenu.SetActive(false);
        lootBoxesMenu.SetActive(true);
    }
    
    /// <summary>
    /// Closes lootboxes menu
    /// </summary>
    public void CloseLootboxesMenu()
    {
        lootBoxesMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Opens the race menu and connects to photon for multiplayer
    /// </summary>
    public void RaceButton()
    {
        // Closes the select car menu
        selectCarMenu.SetActive(false);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(selectCarRaceButton);
        // Opens race menu
        raceMenu.SetActive(true);
        audioManager.Play("MenuSelect");
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
        // Resets player names
        ResetPlayerLobbyNames();
        // Close username objects
        CloseUserNameObjects();
        // Disconnects from photon
        PhotonNetwork.Disconnect();
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Lets the user create or search for a 1v1 room
    /// </summary>
    public void RaceMenu1v1Button()
    {
        PhotonRegion.instance.CheckIfConnected();
        PhotonRegion.instance.faceOffMatch = true;
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Lets the user create or search for a 1v1 room
    /// </summary>
    public void RaceMenu1v1WagerButton()
    {
        PhotonRegion.instance.CheckIfConnected();
        PhotonRegion.instance.wagerMatch = true;
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Lets the user create or search for 5 man a room
    /// </summary>
    public void RaceMenu5ManButton()
    {
        PhotonRegion.instance.CheckIfConnected();
        PhotonRegion.instance.fiveManMatch = true;
        audioManager.Play("MenuSelect");
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
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Takes the user from the searching menu to the race menu
    /// </summary>
    public void SearchingMenuBackButton()
    {
        PhotonNetwork.LeaveRoom();
        // Resets player names
        ResetPlayerLobbyNames();
        // Close username objects
        CloseUserNameObjects();
        wagerMenu.SetActive(false);
        searchingMenu.SetActive(false);
        raceMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(oneVsOneButton);
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Sets the users username, a default one is chosen if none is present
    /// </summary>
    private void SetUsername()
    {
        PhotonView[] LobbyUserName = FindObjectsOfType<PhotonView>();
        if (usernameInput.text == "")
        {
            // Sets a random username is none is chosen
            int rand = Random.Range(1, 6);
            foreach (var user in LobbyUserName)
            {
                if (user.Owner.IsLocal)
                {
                    user.Owner.NickName = rand switch
                    {
                        1 => "Gary",
                        2 => "MoonCake",
                        3 => "Avocado",
                        4 => "Kevin",
                        _ => "Hue"
                    };
                }
            }
        }
        else
        {
            // Sets our user name to what we have chosen
            foreach (var user in LobbyUserName)
            {
                if (user.Owner.IsLocal)
                {
                    user.Owner.NickName = usernameInput.text;
                }
            }
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
        // Multiplayer
        if (!PhotonNetwork.InRoom) return;
        playersReadyNumberText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        switch (PhotonNetwork.CurrentRoom.Name)
        {
            // 1v1 config
            case "RaceLobby1v1":
            {
                // Finds our photon lobby objects
                PhotonView[] LobbyUserName = FindObjectsOfType<PhotonView>();
                // Set player names
                playersReady2Obj.SetActive(true);
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
                {
                    player2v2Names[i].text = LobbyUserName[i].Owner.NickName;
                }

                if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (!loadingLevel)
                        {
                            loadingLevel = true;
                            backButtonNormalRace.SetActive(false);
                            // Loads level
                            Invoke(nameof(LoadRaceTrack), 3);
                        }
                    }
                }

                break;
            }
            // 1v1 wager config
            case "RaceLobby1v1Wager":
            {
                // Finds our photon lobby objects
                PhotonView[] LobbyUserName = FindObjectsOfType<PhotonView>();
                // Set player names
                playersWagerReadyObj.SetActive(true);
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
                {
                    if(i >= playerWagerNames.Length ||  i >= LobbyUserName.Length) continue;
                    playerWagerNames[i].text = LobbyUserName[i].Owner.NickName;
                }

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
                                Invoke(nameof(LoadRaceTrack), 3);
                            }
                        }
                    }
                }

                break;
            }
            // 5man config
            case "RaceLobby5Man":
            {
                // Finds our photon lobby objects
                PhotonView[] LobbyUserName = FindObjectsOfType<PhotonView>();
                // Set player names
                playersReady5Obj.SetActive(true);
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
                {
                    player5v5Names[i].text = LobbyUserName[i].Owner.NickName;
                }

                if (PhotonNetwork.CurrentRoom.PlayerCount == 5)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (!loadingLevel)
                        {
                            loadingLevel = true;
                            backButtonNormalRace.SetActive(false);
                            // Loads level
                            Invoke(nameof(LoadRaceTrack), 3);
                        }
                    }
                }

                break;
            }
        }
    }

    /// <summary>
    /// Resets the user names for each player in the lobby
    /// </summary>
    private void ResetPlayerLobbyNames()
    {
        foreach (var player in playerWagerNames)
        {
            player.text = "";
        }

        foreach (var player in player2v2Names)
        {
            player.text = "";
        }

        foreach (var player in player5v5Names)
        {
            player.text = "";
        }
    }

    /// <summary>
    /// Closes username objects
    /// </summary>
    private void CloseUserNameObjects()
    {
        playersReady2Obj.SetActive(false);
        playersReady5Obj.SetActive(false);
        playersWagerReadyObj.SetActive(false);
    }

    #endregion

    #endregion
}