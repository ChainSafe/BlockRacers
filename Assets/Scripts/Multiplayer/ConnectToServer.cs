using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Connects the users to the multiplayer server
/// </summary>
public class ConnectToServer : MonoBehaviourPunCallbacks
{
    #region Fields
    
    // Are we connected to the master server?
    public static bool connectedToMaster;
    
    #endregion

    #region Methods

    void Start()
    {
        // Connects to Photon Master servers.

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    /// <summary>
    /// Fires once a user is connected
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master Server!");

        SceneManager.LoadScene("MenuScene"); // Once connected we transition to the Server Menu to Join
        connectedToMaster = true;
    }
    
    #endregion
}
