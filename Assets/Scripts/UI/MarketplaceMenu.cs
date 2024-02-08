using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;

/// <summary>
/// Marketplace functionality
/// </summary>
public class MarketplaceMenu : MonoBehaviour
{
    #region Fields

    // Global manager
    private GlobalManager globalManager;

    [SerializeField] private GameObject mintButton1, mintButton2, mintButton3;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes objects and calls data
    /// </summary>
    private void Awake()
    {
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        GetOwnerIds();
    }

    /// <summary>
    /// Calls owned nfts
    /// </summary>
    private async void GetOwnerIds()
    {
        var method = "getOwnerNftIds";
        // Call nft array
        var data = await Evm.GetArray(Web3Accessor.Web3, ContractManager.NftContract, ContractManager.NftAbi, method);
        // Clear the ownedNftIds array before adding new members
        ((IList)globalManager.ownedNftIds)?.Clear();
        globalManager.ownedNftIds ??= new List<int>();
        foreach (var member in data.SelectMany(list => list))
        {
            // Add id to players array
            ((IList)globalManager.ownedNftIds)?.Add(int.Parse(member));
            Debug.Log($"Getting stats for nftId {member}");
            GetNftStats(int.Parse(member));
        }
        var responseString = string.Join(",\n", data.Select((list, i) => $"{string.Join((string)", ", (IEnumerable<string>)list)}"));
        SampleOutputUtil.PrintResult(responseString, nameof(Evm), nameof(Evm.GetArray));
        // Set selected NFT to first ID in the array
        if (globalManager.ownedNftIds != null) globalManager.selectedNftId = globalManager.ownedNftIds.FirstOrDefault();
        GetUnlockedNfts();
    }

    /// <summary>
    /// Checks unlocked nfts to be used in upgrades
    /// </summary>
    private async void GetUnlockedNfts()
    {
        var method = "getUnlockedNfts";
        var data = await Evm.GetArray(Web3Accessor.Web3, ContractManager.NftContract, ContractManager.NftAbi, method);
        var responseString = string.Join(",\n", data.Select((list, i) => $"{string.Join((string)", ", (IEnumerable<string>)list)}"));
        SampleOutputUtil.PrintResult(responseString, nameof(Evm), nameof(Evm.GetArray));
        string[] values = responseString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (values.All(string.IsNullOrWhiteSpace))
        {
            Debug.Log("No unlocked nfts, please mint 1");
            mintButton1.SetActive(true);
            mintButton2.SetActive(true);
            mintButton3.SetActive(true);
            return;
        }
        for (int i = 0; i < values.Length; i++)
        {
            bool isActive = bool.Parse(values[i].Trim());
            switch (i)
            {
                case 0:
                    mintButton1.SetActive(!isActive);
                    if (isActive)
                    {
                        globalManager.unlockedNftCount++;
                    }
                    break;
                case 1:
                    mintButton2.SetActive(!isActive);
                    if (isActive)
                    {
                        globalManager.unlockedNftCount++;
                    }
                    break;
                case 2:
                    mintButton3.SetActive(!isActive);
                    if (isActive)
                    {
                        globalManager.unlockedNftCount++;
                    }
                    break;
            }
        }
        Debug.Log("Unlocked nfts found, mint buttons disabled for owned nfts");
    }

    private async void GetNftStats(int _nftId)
    {
        string method = "getNftStats";
        object[] args =
        {
            _nftId
        };
        var data = await Evm.ContractCall(Web3Accessor.Web3, method, ContractManager.NftAbi, ContractManager.NftContract, args);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"NFTSTATS: {response}");
        string[] values = response.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        var nftType = int.Parse(values[0]);
        var engineLevel = int.Parse(values[1]);
        var handlingLevel = int.Parse(values[2]);
        var nosLevel = int.Parse(values[3]);
        switch (nftType)
        {
            case 1:
                globalManager.engineLevelNft1 = engineLevel;
                globalManager.handlingLevelNft1 = handlingLevel;
                globalManager.nosLevelNft1 = nosLevel;
                break;
            case 2:
                globalManager.engineLevelNft2 = engineLevel;
                globalManager.handlingLevelNft2 = handlingLevel;
                globalManager.nosLevelNft2 = nosLevel;
                break;
            case 3:
                globalManager.engineLevelNft3 = engineLevel;
                globalManager.handlingLevelNft3 = handlingLevel;
                globalManager.nosLevelNft3 = nosLevel;
                break;
            default:
                break;
        }
    }
    
    // TODO: typecast to ints or rather a tuple to deal with different data types & update main
    // public static async Task<List<List<string>>> GetArray(Web3 web3, string contractAddress, string abi, string method)
    // {
    //     var contract = web3.ContractBuilder.Build(abi, contractAddress);
    //     var rawResponse = await contract.Call(method);
    //     return rawResponse.Select(raw => raw as List<string>).ToList();
    // }

    public async void PurchaseNft(int _nftType)
    {
        try
        {
            // TODO:Sign nonce and set voucher for ECDSA
            BigInteger amount = (BigInteger)(50*1e18);
            await ContractManager.Approve(ContractManager.NftContract, amount);
            var account = await Web3Accessor.Web3.Signer.GetAddress();
            object[] args =
            {
                account,
                amount,
                _nftType
            };
            var data = await Evm.ContractSend(Web3Accessor.Web3, "mintNft", ContractManager.NftAbi, ContractManager.NftContract, args);
            var response = SampleOutputUtil.BuildOutputValue(data);
            Debug.Log($"TX: {response}");
            // Play our menu select audio
            GarageMenu.instance.PlayMenuSelect();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion
}