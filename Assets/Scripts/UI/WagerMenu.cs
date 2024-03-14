using System;
using System.Numerics;
using System.Text;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Photon.Pun;
using Scripts.EVM.Token;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject acceptWagerButton, setWagerObject, wagerExistsPopup;
    [SerializeField] private TextMeshProUGUI wagerText;
    [SerializeField] private GameObject spinner;
    [SerializeField] private Button setWagerButton;
    
    private int wagerAmount;
    private bool wagering;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes needed objects
    /// </summary>
    private void Awake()
    {
        setWagerButton.interactable = false;
        wagerInput.onValueChanged.AddListener(value => setWagerButton.interactable = !string.IsNullOrEmpty(value));
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
    }

    private void OnDestroy()
    {
        wagerInput.onValueChanged.RemoveAllListeners();
    }

    /// <summary>
    /// Starts wager check
    /// </summary>
    private new void OnEnable()
    {
        // Check if wager is active
        WagerCheck();
        
    }
 
    /// <summary>
    /// Checks if we're wagering and enables button
    /// </summary>
    private async void WagerCheck()
    {
        try
        {
            spinner.SetActive(true);
            setWagerObject.SetActive(false);
            wagerText.SetText("Checking if wager exists");
            var account = await Web3Accessor.Web3.Signer.GetAddress();
            object[] args =
            {
                account
            };
            var data = await Evm.ContractCall(Web3Accessor.Web3, "getWager", ContractManager.WagerAbi,
                ContractManager.WagerContract, args);
            var response = SampleOutputUtil.BuildOutputValue(data);
            Debug.Log($"Output: {response}");
            // Display wager cancel button if wager is set as you can't have 2 wagers at once
            if (response[1] != 0)
            {
                wagering = true;
            }

            wagerExistsPopup.SetActive(wagering);
            // Enable wager object if master client to set wager
            if (PhotonNetwork.IsMasterClient)
            {
                setWagerObject.SetActive(true);
                wagerText.text = "SET WAGER";
            }
            else
            {
                spinner.SetActive(true);
                wagerText.SetText("Waiting for opponent to set wager");
            }
        }
        catch (Web3Exception e)
        {
            // Dirty fallback if a wager hasn't been set
            // Enable wager object if master client to set wager
            if (PhotonNetwork.IsMasterClient)
            {
                setWagerObject.SetActive(true);
                wagerText.text = "SET WAGER";
                spinner.SetActive(false);
            }
            else
            {
                spinner.SetActive(true);
                wagerText.SetText("Waiting for opponent to set wager");
            }

            Console.WriteLine(e);
            throw;
        }
        finally
        {
            spinner.SetActive(false);
        }
    }

     /// <summary>
     /// Sets our wager amount
     /// </summary>
     public async void SetWager()
     {
         var previousText = wagerText.text;
         try
         {
             setWagerObject.SetActive(false);
             spinner.SetActive(true);
             wagerText.SetText("Sending wager amount");
             
             var account = await Web3Accessor.Web3.Signer.GetAddress();
             if (int.Parse(wagerInput.text) > 100)
             {
                 wagerAmount = 100;
             }
             else
             {
                 wagerAmount = int.Parse(wagerInput.text);
             }

             // Approve transfer amount
             BigInteger wager = BigInteger.Multiply(wagerAmount, BigInteger.Pow(10, 18));
             await ContractManager.Approve(ContractManager.WagerContract, wager);
             // Convert the current time to Unix timestamp & add 15 minutes so it doesn't expire
             BigInteger deadline = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + 900;
             // Additional function parameters
             BigInteger nonce = 1;
             var message = $"{account}{wager}{nonce}{deadline}{ContractManager.WagerContract}{338}";
             // Sign & send sig to opponent over rpc
             wagerText.SetText("Waiting for opponent to approve");
             string signature = await Evm.SignMessage(Web3Accessor.Web3, message);
             Debug.Log($"Wager set at: {wagerAmount}");
             globalManager.wagerAmount = wagerAmount;
             photonView.RPC("RPCWagerSet", RpcTarget.Others, wagerAmount, account, signature, deadline.ToString());
         }
         catch (Web3Exception e)
         {
             spinner.SetActive(false);
             wagerText.text = previousText;
             Console.WriteLine(e);
             throw;
         }
     }
     
     /// <summary>
     /// Accepts wager
     /// </summary>
     public async void AcceptWager()
     {
         try
         {
             acceptWagerButton.SetActive(false);
             wagerText.SetText("Accepting wager");
             spinner.SetActive(true);
             var account = await Web3Accessor.Web3.Signer.GetAddress();
             // Approve transfer amount
             BigInteger wager = BigInteger.Multiply(globalManager.wagerAmount, BigInteger.Pow(10, 18));
             await ContractManager.Approve(ContractManager.WagerContract, wager);
             // Additional function parameters
             BigInteger nonce = 1;
             byte[] opponentSig = Encoding.UTF8.GetBytes(globalManager.opponentSignature);
             object[] args =
             {
                 globalManager.wagerOpponent,
                 wager,
                 nonce,
                 globalManager.deadline,
                 opponentSig
             };
             var data = await Evm.ContractSend(Web3Accessor.Web3, "startWager", ContractManager.WagerAbi, ContractManager.WagerContract, args);
             var response = SampleOutputUtil.BuildOutputValue(data);
             wagerText.SetText("Loading scene");
             Debug.Log($"TX: {response}");
             wagerText.SetText("Wager accepted loading scene");
             // Set wagering to true
             globalManager.wagering = true;
             globalManager.wagerAccepted = true;
             photonView.RPC("RPCWagerAccept", RpcTarget.Others, account);
         }
         catch (Web3Exception e)
         {
             spinner.SetActive(false);
             Console.WriteLine(e);
             throw;
         }
         
    }
     
     /// <summary>
     /// Accepts wager
     /// </summary>
     public async void CancelWager()
     {
         try
         {
             // Additional function parameters
             BigInteger nonce = 1;
             byte[] opponentSig = Encoding.UTF8.GetBytes(globalManager.opponentSignature);
             object[] args =
             {
                 nonce,
                 globalManager.deadline,
                 opponentSig
             };
             var data = await Evm.ContractSend(Web3Accessor.Web3, "cancelWager", ContractManager.WagerAbi,
                 ContractManager.WagerContract, args);
             var response = SampleOutputUtil.BuildOutputValue(data);
             Debug.Log($"TX: {response}");
             // Set wagering to false

         }
         catch (Web3Exception e)
         {
             Console.WriteLine(e);
             throw;
         }
         finally
         {
             globalManager.wagering = false;
             globalManager.wagerAccepted = false;
             wagerExistsPopup.SetActive(false);
         }
         
     }

     /// <summary>
     /// RPC to set wager
     /// </summary>
     [PunRPC]
     private void RPCWagerSet(int _wagerAmount, string _account, string _opponentSignature, string _deadline)
     {
         // Set wager values
         globalManager.wagerAmount = _wagerAmount;
         globalManager.wagerOpponent = _account;
         globalManager.opponentSignature = _opponentSignature;
         globalManager.deadline = BigInteger.Parse(_deadline);
         wagerText.text = $"WAGER: {_wagerAmount}";
         acceptWagerButton.SetActive(true);
     }
    
     /// <summary>
     /// RPC to accept wager
     /// </summary>
     [PunRPC]
     private void RPCWagerAccept(string _account)
     {
         wagerText.SetText("Loading scene");
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