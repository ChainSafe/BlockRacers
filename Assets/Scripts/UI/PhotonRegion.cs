using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PhotonRegion : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Dropdown regionDropdown;
    [SerializeField]
    private Button connectButton;

    // Singleton
    public static PhotonRegion instance;

    private void Start()
    {
        // Singleton
        instance = this;

        // Add our supported regions
        regionDropdown.ClearOptions();
        regionDropdown.AddOptions(new List<string> { "Australia", "Asia", "Europe", "North America" });

        // You can also set the default selected option here
        regionDropdown.value = 0;
    }


    // Here we assign regions manually based on the value the player selects
    public string GetRegionCodeFromOption(string option)
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
    public void ConnectToSelectedRegion()
    {
        string selectedOption = regionDropdown.options[regionDropdown.value].text;
        string selectedRegionCode = GetRegionCodeFromOption(selectedOption);

        Debug.Log("Connecting to region: " + selectedRegionCode);

        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = selectedRegionCode;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server in region: " + PhotonNetwork.CloudRegion);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Disconnected from server. Reason: " + cause.ToString());
    }

}
