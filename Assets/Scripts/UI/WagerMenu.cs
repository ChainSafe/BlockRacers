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
         string account = PlayerPrefs.GetString("PlayerAccount");
         
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
             // Sign nonce and set voucher
             string ecdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
             var nonceData = await Evm.ContractCall(Web3Accessor.Web3, "nonce", ContractManager.WagerAbi, ContractManager.WagerContract, new object[] {account});
             var nonceResponse = SampleOutputUtil.BuildOutputValue(nonceData);
             int nonce = int.Parse(nonceResponse);
             string message = $"{nonce}{wagerAmount}{account}";
             var signature = Evm.EcdsaSignMessage(ecdsaKey, message);
             // Set wager
             object[] args =
             {
                 wagerAmount,
                 signature
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
         string account = PlayerPrefs.GetString("PlayerAccount");
         try
         {
             // Sign nonce and set voucher
             string ecdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
             var nonceData = await Evm.ContractCall(Web3Accessor.Web3, "nonce", ContractManager.WagerAbi, ContractManager.WagerContract, new object[] {account});
             var nonceResponse = SampleOutputUtil.BuildOutputValue(nonceData);
             int nonce = int.Parse(nonceResponse);
             string opponent = ""; // TO DO SET
             string message = $"{nonce}{wagerAmount}{account}{opponent}";
             var signature = Evm.EcdsaSignMessage(ecdsaKey, message);
             object[] args =
             {
                 opponent,
                 wagerAmount,
                 signature
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