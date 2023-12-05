using System;
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
        ShowNftButtons();
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
    
    /// <summary>
    /// Loops through Nfts and enables the select buttons if the user owns the nft type
    /// </summary>
    private async void ShowNftButtons()
    {
        var playerAccount = await Web3Accessor.Web3.Signer.GetAddress();
        string account = playerAccount;
        var data1 = await Evm.ContractCall(Web3Accessor.Web3, "getOwnerNftIds", ContractManager.NftAbi, ContractManager.NftContract, new object[] {account});
        var response1 = SampleOutputUtil.BuildOutputValue(data1);
        Debug.Log($"OWNER NFTS IDS: {response1}");
        foreach (var nftId in response1)
        {
            Debug.Log($"Checking NFT ID {nftId}");
            var data2 = await Evm.ContractCall(Web3Accessor.Web3, "nftType", ContractManager.NftAbi, ContractManager.NftContract, new object[] {nftId});
            var response2 = SampleOutputUtil.BuildOutputValue(data2);
            if (int.Parse(response2) == 1)
            {
                purchaseButton1.SetActive(false);
                selectButton1.SetActive(true);
            }
            else if (int.Parse(response2) == 2)
            {
                purchaseButton2.SetActive(false);
                selectButton2.SetActive(true);
            }
            else
            {
                purchaseButton3.SetActive(false);
                selectButton3.SetActive(true);
            }
            // Stops looping if all 3 are true
            if (selectButton1.activeSelf && selectButton2.activeSelf && selectButton3.activeSelf)
            {
                return;
            }
        }
    }

    private async void PurchaseNft(int _nftType)
    {
        // Sign nonce and set voucher
        string ecdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
        string account = PlayerPrefs.GetString("PlayerAccount");
        var amount = 50*1e18;
        var nonceData = await Evm.ContractCall(Web3Accessor.Web3, "nonce", ContractManager.NftAbi, ContractManager.NftContract, new object[] {account});
        var nonceResponse = SampleOutputUtil.BuildOutputValue(nonceData);
        int nonce = int.Parse(nonceResponse);
        string message = $"{nonce}{amount}{account}{_nftType}";
        var signature = Evm.EcdsaSignMessage(ecdsaKey, message);
        // Mint
        object[] args =
        {
            amount,
            _nftType,
            signature
        };
        var data = await Evm.ContractSend(Web3Accessor.Web3, "mintNft", ContractManager.NftAbi, ContractManager.NftContract, args);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }

    /// <summary>
    /// Changes to NFT 1
    /// </summary>
    public void SelectNft1()
    {
        globalManager.nftSprite = Nft1;
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }
    
    /// <summary>
    /// Purchase NFT 1
    /// </summary>
    public void PurchaseNft1()
    {
        try
        {
            PurchaseNft(1);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Changes to NFT 2
    /// </summary>
    public void SelectNft2()
    {
        globalManager.nftSprite = Nft2;
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }
    
    /// <summary>
    /// Purchase NFT 2
    /// </summary>
    public void PurchaseNft2()
    {
        try
        {
            PurchaseNft(2);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Changes to NFT 3
    /// </summary>
    public void SelectNft3()
    {
        globalManager.nftSprite = Nft3;
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }
    
    /// <summary>
    /// Purchase NFT 1
    /// </summary>
    public void PurchaseNft3()
    {
        try
        {
            PurchaseNft(3);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion
}