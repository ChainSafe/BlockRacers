using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Are we connected to the master server?
    public static bool connectedToMaster;

    void Start()
    {
        // Connects to Photon Master servers.

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master Server!");

        SceneManager.LoadScene("MenuScene"); // Once connected we transition to the Server Menu to Join
        connectedToMaster = true;
    }
}
