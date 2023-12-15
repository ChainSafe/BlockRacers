using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

/// <summary>
///  Changes the users chosen car
/// </summary>
public class SwapCars : MonoBehaviour
{
    #region Fields

    // Singleton
    public static SwapCars instance;

    // Index of the currently active prefab & livery
    public static int currentLiveryIndex;

    public int currentPrefabIndex, nftTypesOwned;

    // Array of prefabs to swap between
    public GameObject[] prefabs;

    // Our UI elements for the showroom
    public TextMeshProUGUI carName;

    public Slider engineSlider, handlingSlider, boostSlider;

    // Our available colours for each model
    public Material[] camaroLivery;
    public Material[] fordGTLivery;

    public Material[] ferrariLivery;

    // Car prefabs
    public GameObject car1, car2, car3;

    // Reference to the currently instantiated prefab
    private GameObject currentPrefab;

    // Set the spawnpoint 
    private Vector3 spawnPoint = new Vector3(89.17f, 0.4f, -9.7f);

    // Reference the global manager
    private GlobalManager globalManager;

    // Platform
    [SerializeField] private GameObject platform;

    private string[] nftArray;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes needed objects
    /// </summary>
    private void Start()
    {
        // Singleton
        instance = this;
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        // Instantiate the initial prefab
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation, transform);
        platform.transform.position = new Vector3(currentPrefab.transform.position.x,
            currentPrefab.transform.position.y - 0.45f, currentPrefab.transform.position.z);
        GetNftIds();
    }

    /// <summary>
    /// Changes to the next car in the list
    /// </summary>
    public async void NextCar()
    {
        // Destroy the current prefab
        Destroy(currentPrefab);
        currentPrefabIndex++;
        if (currentPrefabIndex >= nftTypesOwned - 1)
        {
            currentPrefabIndex = 0;
        }
        // NFT stats call based on allnfts arrays using current prefab index
        var data = await GetStruct(Web3Accessor.Web3, ContractManager.NftContract, ContractManager.NftAbi, "nftStats", new object[] { int.Parse(nftArray[currentPrefabIndex]) });
        // Instantiate the next prefab in the array
        currentPrefab = Instantiate(prefabs[int.Parse(data[0].ToString())], spawnPoint, transform.rotation, transform);
        // Set name & stats
        if (data[0] == null) return;
        switch (data[0])
        {
            case 1:
                globalManager.engineLevelNft1 = int.Parse(data[1].ToString());
                globalManager.handlingLevelNft1 = int.Parse(data[2].ToString());
                globalManager.nosLevelNft1 = int.Parse(data[3].ToString());
                break;
            case 2:
                globalManager.engineLevelNft2 = int.Parse(data[1].ToString());
                globalManager.handlingLevelNft2 = int.Parse(data[2].ToString());
                globalManager.nosLevelNft2 = int.Parse(data[3].ToString());
                break;
            case 3:
                globalManager.engineLevelNft3 = int.Parse(data[1].ToString());
                globalManager.handlingLevelNft3 = int.Parse(data[2].ToString());
                globalManager.nosLevelNft3 = int.Parse(data[3].ToString());
                break;
        }
        engineSlider.value = globalManager.engineLevel;
        handlingSlider.value = globalManager.handlingLevel;
        boostSlider.value = globalManager.nosLevel;
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }

    /// <summary>
    /// Changes to the previous car in the list
    /// </summary>
    public async void PreviousCar()
    {
        // Destroy the current prefab
        Destroy(currentPrefab);
        currentPrefabIndex--;
        if (currentPrefabIndex <= nftTypesOwned)
        {
            currentPrefabIndex = nftTypesOwned -1;
        }
        // NFT stats call based on allnfts arrays using current prefab index
        var data = await GetStruct(Web3Accessor.Web3, ContractManager.NftContract, ContractManager.NftAbi, "nftStats", new object[] { int.Parse(nftArray[currentPrefabIndex]) });
        // Instantiate the next prefab in the array
        currentPrefab = Instantiate(prefabs[int.Parse(data[0].ToString())], spawnPoint, transform.rotation, transform);
        // Set name & stats
        if (data[0] == null) return;
        switch (data[0])
        {
            case 1:
                globalManager.engineLevelNft1 = int.Parse(data[1].ToString());
                globalManager.handlingLevelNft1 = int.Parse(data[2].ToString());
                globalManager.nosLevelNft1 = int.Parse(data[3].ToString());
                break;
            case 2:
                globalManager.engineLevelNft2 = int.Parse(data[1].ToString());
                globalManager.handlingLevelNft2 = int.Parse(data[2].ToString());
                globalManager.nosLevelNft2 = int.Parse(data[3].ToString());
                break;
            case 3:
                globalManager.engineLevelNft3 = int.Parse(data[1].ToString());
                globalManager.handlingLevelNft3 = int.Parse(data[2].ToString());
                globalManager.nosLevelNft3 = int.Parse(data[3].ToString());
                break;
        }
        engineSlider.value = globalManager.engineLevel;
        handlingSlider.value = globalManager.handlingLevel;
        boostSlider.value = globalManager.nosLevel;
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }

    /// <summary>
    /// Changes the livery for each car based on the options available for each
    /// </summary>
    public void ChangeLivery()
    {
        switch (currentPrefabIndex)
        {
            case 0:
            {
                currentLiveryIndex++;
                if (currentLiveryIndex > 3)
                {
                    currentLiveryIndex = 0;
                }

                currentPrefab.GetComponentInChildren<MeshRenderer>().material = camaroLivery[currentLiveryIndex];
                break;
            }
            case 1:
            {
                currentLiveryIndex++;
                if (currentLiveryIndex > 3)
                {
                    currentLiveryIndex = 0;
                }

                currentPrefab.GetComponentInChildren<MeshRenderer>().material = fordGTLivery[currentLiveryIndex];
                break;
            }
            case 2:
            {
                currentLiveryIndex++;
                if (currentLiveryIndex > 3)
                {
                    currentLiveryIndex = 0;
                }

                currentPrefab.GetComponentInChildren<MeshRenderer>().material = ferrariLivery[currentLiveryIndex];
                break;
            }
        }

        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }
    
    /// <summary>
    /// Array duplicate so we can use a method parameter
    /// </summary>
    /// <param name="web3"></param>
    /// <param name="contractAddress"></param>
    /// <param name="abi"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static async Task<List<List<BigInteger>>> GetArray(Web3 web3, string contractAddress, string abi, string method, object[] args)
    {
        var contract = web3.ContractBuilder.Build(abi, contractAddress);
        var rawResponse = await contract.Call(method, args);
        return rawResponse.Select(raw => raw as List<BigInteger>).ToList();
    }
    
    
    /// <summary>
    /// Fetches owned Nft Ids
    /// </summary>
    private async void GetNftIds()
    {
        var account = await Web3Accessor.Web3.Signer.GetAddress();
        var data = await GetArray(Web3Accessor.Web3, ContractManager.NftContract, ContractManager.NftAbi, "getOwnerNftIds", new object[] {account});
        var response = string.Join(" ", data.Select(list => string.Join(" ", list)));
        nftArray = response.Split(' ');
        if (response == "") return;
        Debug.Log($"Result: {response}");
        foreach (var nftId in nftArray)
        {
            nftTypesOwned++;
            GetNftStats(int.Parse(nftId));
        }
    }
    
    /// <summary>
    /// Struct getter
    /// </summary>
    /// <param name="web3"></param>
    /// <param name="contractAddress"></param>
    /// <param name="abi"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static async Task<List<object>> GetStruct(Web3 web3, string contractAddress, string abi, string method, object[] args)
    {
        var contract = web3.ContractBuilder.Build(abi, contractAddress);
        var rawResponse = await contract.Call(method, args);
        return rawResponse.ToList();
    }
    
    /// <summary>
    /// Fetches NFT stats
    /// </summary>
    /// <param name="_nftId"></param>
    private async void GetNftStats(int _nftId)
    {
        var data = await GetStruct(Web3Accessor.Web3, ContractManager.NftContract, ContractManager.NftAbi, "nftStats", new object[] { _nftId });
        if (data[0] == null) return;
        switch (data[0])
        {
            case 1:
                globalManager.engineLevelNft1 = int.Parse(data[1].ToString());
                globalManager.handlingLevelNft1 = int.Parse(data[2].ToString());
                globalManager.nosLevelNft1 = int.Parse(data[3].ToString());
                break;
            case 2:
                globalManager.engineLevelNft2 = int.Parse(data[1].ToString());
                globalManager.handlingLevelNft2 = int.Parse(data[2].ToString());
                globalManager.nosLevelNft2 = int.Parse(data[3].ToString());
                break;
            case 3:
                globalManager.engineLevelNft3 = int.Parse(data[1].ToString());
                globalManager.handlingLevelNft3 = int.Parse(data[2].ToString());
                globalManager.nosLevelNft3 = int.Parse(data[3].ToString());
                break;
        }
        engineSlider.value = globalManager.engineLevel;
        handlingSlider.value = globalManager.handlingLevel;
        boostSlider.value = globalManager.nosLevel;
    }

    /// <summary>
    /// Checks for arrow key input navigation
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousCar();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextCar();
        }
    }

    #endregion
}