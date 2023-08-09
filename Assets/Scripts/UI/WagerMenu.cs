using Photon.Pun;
using TMPro;
using UnityEngine;

/// <summary>
/// An example of how wagering tokens would work
/// </summary>
public class WagerMenu : MonoBehaviourPunCallbacks
{
    #region Fields
    
    // Global Manager
    private GlobalManager globalManager;
    // Wager config
    [SerializeField] private TMP_InputField wagerInput;
    [SerializeField] private GameObject setWagerObject;
    [SerializeField] private GameObject acceptWagerButton;
    [SerializeField] private TextMeshProUGUI wagerText;
    private int wagerAmount;
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// Initializes needed objects
    /// </summary>
    void Awake()
    {
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        if (PhotonNetwork.IsMasterClient)
        {
            setWagerObject.SetActive(true);
            wagerText.text = "SET WAGER";
        }
    }
    
    /// <summary>
    /// Sets our wager amount
    /// </summary>
    public void SetWager()
    {
        Debug.Log("Setting Wager!");
        if (int.Parse(wagerInput.text) > 100)
        {
            wagerAmount = 100;
        }
        else
        {
            wagerAmount = int.Parse(wagerInput.text);
        }
        // Chain call here to set wager
        Debug.Log($"Wager set at: {wagerAmount}");
        // Change this later to check both ends have accepted
        globalManager.wagerAccepted = true;
        photonView.RPC("RPCWagerSet", RpcTarget.All, wagerAmount);
    }
    
    /// <summary>
    /// Accepts wager
    /// </summary>
    public void AcceptWager()
    {
        globalManager.wagerAccepted = true;
        photonView.RPC("RPCWagerAccept", RpcTarget.All);
    }
    
    /// <summary>
    /// RPC to set wager
    /// </summary>
    [PunRPC]
    private void RPCWagerSet(int wagerAmount)
    {
        globalManager.wagerAmount = wagerAmount;
        acceptWagerButton.SetActive(true);
        wagerText.text = $"WAGER: {wagerAmount}";
    }
    
    /// <summary>
    /// RPC to accept wager
    /// </summary>
    [PunRPC]
    private void RPCWagerAccept()
    {
        // Set wagering to true
        globalManager.wagering = true;
        // Loads level
        if (PhotonNetwork.IsMasterClient)
        {
            // Loads level
            PhotonNetwork.LoadLevel("RaceTrack");
        }
    }

    #endregion
}
