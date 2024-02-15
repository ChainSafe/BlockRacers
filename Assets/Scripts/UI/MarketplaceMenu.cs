using System;
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
    public async void GetOwnerIds()
    {
        var method = "getOwnerNftIds";
        // Call nft array
        var data = await Evm.GetArray<BigInteger>(Web3Accessor.Web3, ContractManager.NftContract, ContractManager.NftAbi, method);
        // Clear the ownedNftIds array before adding new members
        globalManager.ownedNftIds?.Clear();
        globalManager.ownedNftIds ??= new List<int>();
        if (data != null)
        {
            foreach (var list in data)
            {
                foreach (int member in list)
                {
                    // Add id to players array
                    globalManager.ownedNftIds.Add(member);
                    Debug.Log($"Getting stats for nftId {member}");
                    GetNftStats(member);
                }
            }
        }
        // Set selected NFT to first ID in the array
        globalManager.selectedNftId = globalManager.ownedNftIds.FirstOrDefault();
        GetUnlockedNfts();
    }

    /// <summary>
    /// Checks unlocked nfts to be used in upgrades
    /// </summary>
    private async void GetUnlockedNfts()
    {
        var method = "getUnlockedNfts";
        var data = await Evm.GetArray<bool>(Web3Accessor.Web3, ContractManager.NftContract, ContractManager.NftAbi, method);
    
        // Flatten the list of lists into a single list of bool values
        var boolValues = data.SelectMany(innerList => innerList).ToList();
    
        if (boolValues.All(value => !value))
        {
            Debug.Log("No unlocked nfts, please mint 1");
            mintButton1.SetActive(true);
            mintButton2.SetActive(true);
            mintButton3.SetActive(true);
            return;
        }
        
        // Initialize unlockedNfts array with size 3
        globalManager.unlockedNfts = new bool[3];
    
        for (int i = 0; i < boolValues.Count; i++)
        {
            bool isActive = boolValues[i];
            switch (i)
            {
                case 0:
                    mintButton1.SetActive(!isActive);
                    if (isActive)
                    {
                        globalManager.unlockedNfts[0] = true;
                    }
                    break;
                case 1:
                    mintButton2.SetActive(!isActive);
                    if (isActive)
                    {
                        globalManager.unlockedNfts[1] = true;
                    }
                    break;
                case 2:
                    mintButton3.SetActive(!isActive);
                    if (isActive)
                    {
                        globalManager.unlockedNfts[2] = true;
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
        }
    }

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