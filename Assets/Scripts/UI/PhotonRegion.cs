using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhotonRegion : MonoBehaviourPunCallbacks
{
    // Menu items
    [SerializeField] private GameObject connectingText, searchingMenu, raceMenu, searchingBackButton;
    [SerializeField] private TMP_Dropdown regionDropdown;
    public bool faceOffMatch, wagerMatch, fiveManMatch;

    // Singleton
    public static PhotonRegion instance;

    // Audio
    private AudioManager audioManager;

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


    // Here we assign regions manually based on the value the player selects
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


    // Connect to our selected region
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
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnConnectedToMaster()
    {
        // Syncs Photon to ensure scenes are all loaded correctly
        PhotonNetwork.AutomaticallySyncScene = true;
        // Activates connecting text
        connectingText.SetActive(false);
        // Lets the user know they're connected
        Debug.Log("Connected to server in region: " + PhotonNetwork.CloudRegion);
        // Sets room options for each match, TTL set low to ensure there are no dead rooms
        if (faceOffMatch)
        {
            faceOffMatch = false;
            RoomOptions roomOps = new RoomOptions() { IsOpen = true, IsVisible = true, PlayerTtl = 300, MaxPlayers = 2 };
            PhotonNetwork.JoinOrCreateRoom("RaceLobby1v1", roomOps, TypedLobby.Default);
        }
        else if (wagerMatch)
        {
            wagerMatch = false;
            RoomOptions roomOps = new RoomOptions() { IsOpen = true, IsVisible = true, PlayerTtl = 300, MaxPlayers = 2 };
            PhotonNetwork.JoinOrCreateRoom("RaceLobby1v1Wager", roomOps, TypedLobby.Default);
        }
        else if (fiveManMatch)
        {
            fiveManMatch = false;
            RoomOptions roomOps = new RoomOptions() { IsOpen = true, IsVisible = true, PlayerTtl = 300, MaxPlayers = 5 };
            PhotonNetwork.JoinOrCreateRoom("RaceLobby5Man", roomOps, TypedLobby.Default);
        }
        // Activates the searching menu
        searchingMenu.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(searchingBackButton);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // Lets the user know we're disconnecting
        Debug.Log("Disconnected from server. Reason: " + cause.ToString());
        // Changes photon region and connects
        ConnectToSelectedRegion();
    }
}