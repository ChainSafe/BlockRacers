using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Changes our photon region
/// </summary>
public class PhotonRegion : MonoBehaviourPunCallbacks
{
    // Menu items
    [SerializeField] private GameObject connectingText, searchingMenu, raceMenu, searchingBackButton;
    [SerializeField] private TMP_Dropdown regionDropdown;
    private bool changingRegions;
    public bool faceOffMatch, wagerMatch, fiveManMatch;

    // Singleton
    public static PhotonRegion instance;

    // Audio
    private AudioManager audioManager;

    /// <summary>
    /// Initialize objects
    /// </summary>
    private void Start()
    {
        // Singleton
        instance = this;

        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();

        // Add our supported regions
        regionDropdown.ClearOptions();
        regionDropdown.AddOptions(new List<string> { "Australia", "Asia", "Europe", "North America" });

        // You can also set the default selected option here
        regionDropdown.value = 0;
    }

    /// <summary>
    /// Assigns regions manually based on the value the player selects
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    private string GetRegionCodeFromOption(string option)
    {
        // Use conditional statements to map the selected option to a region code
        switch (option)
        {
            case "Australia":
                return "au";

            case "Asia":
                return "asia";

            case "Europe":
                return "eu";

            case "North America":
                return "usw";

            default:
                return "asia"; // Default region code
        }
    }

    /// <summary>
    /// Connects to the selected region
    /// </summary>
    private void ConnectToSelectedRegion()
    {
        // Sets region
        string selectedOption = regionDropdown.options[regionDropdown.value].text;
        string selectedRegionCode = GetRegionCodeFromOption(selectedOption);
        Debug.Log("Connecting to region: " + selectedRegionCode);
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = selectedRegionCode;
        // Hides race menu
        raceMenu.SetActive(false);
        // Connects
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Checks if we're connected to and changes regions
    /// </summary>
    public void CheckIfConnected()
    {
        // PHOTON If we're not connected to photon, connect
        if (!PhotonNetwork.IsConnected)
        {
            // Changes photon region and connects
            ConnectToSelectedRegion();
            // Activates connecting text
            connectingText.SetActive(true);
        }
        else
        {
            changingRegions = true;
            PhotonNetwork.Disconnect();
        }
    }

    /// <summary>
    /// When the user is connected to the master server
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // Syncs Photon to ensure scenes are all loaded correctly
        PhotonNetwork.AutomaticallySyncScene = true;
        // Deactivates connecting text
        connectingText.SetActive(false);
        // Lets the user know they're connected
        Debug.Log("Connected to server in region: " + PhotonNetwork.CloudRegion);
        // Sets room options for each match, TTL set low to ensure there are no dead rooms
        if (faceOffMatch)
        {
            faceOffMatch = false;
            RoomOptions roomOps = new RoomOptions()
                { IsOpen = true, IsVisible = true, PlayerTtl = 300, MaxPlayers = 2 };
            PhotonNetwork.JoinOrCreateRoom("RaceLobby1v1", roomOps, TypedLobby.Default);
        }
        else if (wagerMatch)
        {
            wagerMatch = false;
            RoomOptions roomOps = new RoomOptions()
                { IsOpen = true, IsVisible = true, PlayerTtl = 300, MaxPlayers = 2 };
            PhotonNetwork.JoinOrCreateRoom("RaceLobby1v1Wager", roomOps, TypedLobby.Default);
        }
        else if (fiveManMatch)
        {
            fiveManMatch = false;
            RoomOptions roomOps = new RoomOptions()
                { IsOpen = true, IsVisible = true, PlayerTtl = 300, MaxPlayers = 5 };
            PhotonNetwork.JoinOrCreateRoom("RaceLobby5Man", roomOps, TypedLobby.Default);
        }

        // Activates the searching menu
        searchingMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(searchingBackButton);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// When the user changes regions, disconnect and reconnect
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        if (!changingRegions)
        {
            // Lets the user know we're disconnecting
            Debug.Log("Disconnected from server. Reason: " + cause.ToString());
        }
        else
        {
            changingRegions = false;
            // Changes photon region and connects
            ConnectToSelectedRegion();
        }
    }
}