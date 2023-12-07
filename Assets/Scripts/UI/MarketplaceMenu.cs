using System;
using System.Numerics;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Marketplace functionality
/// </summary>
public class MarketplaceMenu : MonoBehaviour
{
    #region Fields

    // Global manager
    private GlobalManager globalManager;

    // The base prefab we're using to display nfts
    [SerializeField] private GameObject nftPrefab;

    // Our nft image array
    [SerializeField] private Texture2D[] nfts;

    // Our nft object array
    [SerializeField] private GameObject[] nftPrefabs;
    
    [SerializeField] private GameObject purchaseButton1, purchaseButton2, purchaseButton3, selectButton1, selectButton2, selectButton3;

    // NFT sprites
    [SerializeField] private Texture2D Nft1, Nft2, Nft3;

    // The canvas to populate
    [SerializeField] private RectTransform scrollCanvas;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes objects and calls data
    /// </summary>
    private void Awake()
    {
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        //CallData();
    }

    /// <summary>
    /// Calls nft data
    /// </summary>
    private void CallData()
    {
        InitializeArrays();
    }

    /// <summary>
    /// Initializes our prefab arrays
    /// </summary>
    private void InitializeArrays()
    {
        // Initialize arrays by size
        nfts = new Texture2D[2];
        nftPrefabs = new GameObject[2];
        InstantiatePrefabs();
    }

    /// <summary>
    /// Spawns our nft prefabs
    /// </summary>
    private void InstantiatePrefabs()
    {
        // Instantiate prefabs and set parent to scroll canvas
        foreach (var nftPrefabItem in nftPrefabs)
        {
            // Instantiate prefabs
            Instantiate(nftPrefab);
            // Set parent to scroll canvas
            nftPrefabItem.transform.SetParent(scrollCanvas);
            // Populates the prefabs
            PopulatePrefabs(nftPrefabItem);
        }
    }

    /// <summary>
    /// Populates the nft prefab base with images
    /// </summary>
    private void PopulatePrefabs(GameObject nftPrefab)
    {
        foreach (Texture2D nftImage in nfts)
        {
            Image image = nftPrefab.GetComponent<Image>();
            image.sprite = nftImage.GetComponent<Image>().sprite;
        }
    }

    public async void PurchaseNft(int _nftType)
    {
        try
        {
            // Sign nonce and set voucher
            BigInteger amount = (BigInteger)(50*1e18);
            await ContractManager.Approve(ContractManager.NftContract, amount);
            var account = await Web3Accessor.Web3.Signer.GetAddress();
            // Mint
            object[] args =
            {
                amount,
                _nftType,
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