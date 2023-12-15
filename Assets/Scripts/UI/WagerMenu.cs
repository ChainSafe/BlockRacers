using System;
using ChainSafe.Gaming.UnityPackage;
using Photon.Pun;
using Scripts.EVM.Token;
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
     public async void SetWager()
     {
         Debug.Log("Setting Wager!");
         var account = await Web3Accessor.Web3.Signer.GetAddress();
         
         if (int.Parse(wagerInput.text) > 100)
         {
             wagerAmount = 100;
         }
         else
         {
             wagerAmount = int.Parse(wagerInput.text);
         }

         try
         {
             // Set wager
             object[] args =
             {
                 wagerAmount
             };
             var data = await Evm.ContractSend(Web3Accessor.Web3, "setPvpWager", ContractManager.WagerAbi, ContractManager.WagerContract, args);
             var response = SampleOutputUtil.BuildOutputValue(data);
             Debug.Log($"TX: {response}");
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             throw;
         }
         
         Debug.Log($"Wager set at: {wagerAmount}");
         globalManager.wagerAmount = wagerAmount;
         photonView.RPC("RPCWagerSet", RpcTarget.Others, wagerAmount, account);
     }
    
     /// <summary>
     /// Accepts wager
     /// </summary>
     public async void AcceptWager()
     {
         // Chain call to set wager
         var account = await Web3Accessor.Web3.Signer.GetAddress();
         try
         {
             // TO DO SET OPPONENT
             string opponent = "";
             object[] args =
             {
                 opponent,
                 wagerAmount,
             };
             var data = await Evm.ContractSend(Web3Accessor.Web3, "acceptPvpWager", ContractManager.WagerAbi, ContractManager.WagerContract, args);
             var response = SampleOutputUtil.BuildOutputValue(data);
             Debug.Log($"TX: {response}");
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             throw;
         }
         
         // Set wagering to true
         globalManager.wagering = true;
         globalManager.wagerAccepted = true;
         globalManager.wagerAmount = wagerAmount;
         photonView.RPC("RPCWagerAccept", RpcTarget.Others, account);
    }

     /// <summary>
     /// RPC to set wager
     /// </summary>
     [PunRPC]
     private void RPCWagerSet(int _wagerAmount, string _account)
     {
         // Set wager values
         globalManager.wagerAmount = _wagerAmount;
         globalManager.wagerOpponent = _account;
         wagerText.text = $"WAGER: {_wagerAmount}";
         acceptWagerButton.SetActive(true);
     }
    
     /// <summary>
     /// RPC to accept wager
     /// </summary>
     [PunRPC]
     private void RPCWagerAccept(string _account)
     {
         // Set wager values
         globalManager.wagerOpponent = _account;
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