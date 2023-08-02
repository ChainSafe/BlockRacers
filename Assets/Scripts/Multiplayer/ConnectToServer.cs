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
    
    /// <summary>
    /// Connects to Photon Master servers to allow multiplayer
    /// </summary>
    private void Start()
    {
        // If we're not connected, connect
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    /// <summary>
    /// Fires once a user is connected to proceeed and let's the user know we're connected
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master Server!");
        // Once connected we transition to the Server Menu to Join
        SceneManager.LoadScene("MenuScene");
        connectedToMaster = true;
    }
    
    #endregion
}
