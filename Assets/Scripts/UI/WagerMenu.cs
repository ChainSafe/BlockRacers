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

    // /// <summary>
    // /// Sets our wager amount
    // /// </summary>
    // public async void SetWager()
    // {
    //     Debug.Log("Setting Wager!");
    //     if (int.Parse(wagerInput.text) > 100)
    //     {
    //         wagerAmount = 100;
    //     }
    //     else
    //     {
    //         wagerAmount = int.Parse(wagerInput.text);
    //     }
    //     
    //     // Chain call to set wager
    //     string ecdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
    //     string message = "secretmessage";
    //     var signature = Evm.EcdsaSignMessage(ecdsaKey, message);
    //     Debug.Log($"Signed Message: {signature}");
    //     string method = "setPvpWager";
    //     object[] args =
    //     {
    //         wagerAmount,
    //         signature
    //     };
    //     var data = await Evm.ContractSend(Web3Accessor.Web3, method, ContractManager.ArrayAndTotalAbi, ContractManager.ArrayAndTotalContract, args);
    //     var response = SampleOutputUtil.BuildOutputValue(data);
    //     Debug.Log($"TX: {response}");
    //     
    //     Debug.Log($"Wager set at: {wagerAmount}");
    //     globalManager.wagerAmount = wagerAmount;
    //     photonView.RPC("RPCWagerSet", RpcTarget.Others, wagerAmount);
    // }
    //
    // /// <summary>
    // /// Accepts wager
    // /// </summary>
    // public async void AcceptWager()
    // {
    //     // Chain call to set wager
    //     string ecdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
    //     string message = "secretmessage";
    //     var signature = Evm.EcdsaSignMessage(ecdsaKey, message);
    //     Debug.Log($"Signed Message: {signature}");
    //     string method = "acceptPvpWager";
    //     string opponent = ""; // TO DO SET
    //     object[] args =
    //     {
    //         opponent,
    //         wagerAmount,
    //         signature
    //     };
    //     var data = await Evm.ContractSend(Web3Accessor.Web3, method, ContractManager.ArrayAndTotalAbi, ContractManager.ArrayAndTotalContract, args);
    //     var response = SampleOutputUtil.BuildOutputValue(data);
    //     Debug.Log($"TX: {response}");
    //     
    //     // Set wagering to true
    //     globalManager.wagering = true;
    //     globalManager.wagerAccepted = true;
    //     globalManager.wagerAmount = wagerAmount;
    //     photonView.RPC("RPCWagerAccept", RpcTarget.Others);
    //}

    // /// <summary>
    // /// RPC to set wager
    // /// </summary>
    // [PunRPC]
    // private void RPCWagerSet(int wagerAmount)
    // {
    //     globalManager.wagerAmount = wagerAmount;
    //     acceptWagerButton.SetActive(true);
    //     wagerText.text = $"WAGER: {wagerAmount}";
    // }
    //
    // /// <summary>
    // /// RPC to accept wager
    // /// </summary>
    // [PunRPC]
    // private void RPCWagerAccept()
    // {
    //     // Set wagering to true
    //     globalManager.wagering = true;
    //     // Loads level
    //     if (PhotonNetwork.IsMasterClient)
    //     {
    //         // Loads level
    //         PhotonNetwork.LoadLevel("RaceTrack");
    //     }
    // }

    #endregion
}