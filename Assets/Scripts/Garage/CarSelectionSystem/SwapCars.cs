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
    public void NextCar()
    {
        switch (nftTypesOwned)
        {
            case 0:
                return;
            case > 3:
                nftTypesOwned = 3;
                break;
        }
        // Destroy the current prefab
        Destroy(currentPrefab);
        // Increment the index to switch to the next prefab
        currentPrefabIndex++;
        if (currentPrefabIndex >= nftTypesOwned - 1)
        {
            currentPrefabIndex = 0;
        }

        // Instantiate the next prefab in the array
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation, transform);

        // Logic for handling the car stats and other UI information
        // Array Index is as follows: 
        // 0 = Chevrolet Camaro (Nitro Nova GTL)
        // 1 = Ford GT (Turbo Storm GT)
        // 2 = Ferrari (Star Stream S6)
        // Names are subject to change

        switch (currentPrefabIndex)
        {
            case 0:
                carName.text = "Nitro Nova GTL";
                engineSlider.value = globalManager.engineLevel;
                handlingSlider.value = globalManager.handlingLevel;
                boostSlider.value = globalManager.nosLevel;
                // Actively select this car
                globalManager.playerCar = car1;
                break;
            case 1:
                carName.text = "Turbo Storm GT";
                engineSlider.value = globalManager.engineLevel;
                handlingSlider.value = globalManager.handlingLevel;
                boostSlider.value = globalManager.nosLevel;
                // Actively select this car
                globalManager.playerCar = car2;
                break;
            case 2:
                carName.text = "Star Stream S6";
                engineSlider.value = globalManager.engineLevel;
                handlingSlider.value = globalManager.handlingLevel;
                boostSlider.value = globalManager.nosLevel;
                // Actively select this car
                globalManager.playerCar = car3;
                break;
        }

        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }

    /// <summary>
    /// Changes to the previous car in the list
    /// </summary>
    public void PreviousCar()
    {
        switch (nftTypesOwned)
        {
            case 0:
                return;
            case > 3:
                nftTypesOwned = 3;
                break;
        }

        // Destroy the current prefab
        Destroy(currentPrefab);
        // Decrement the index to switch to the next prefab  
        if (currentPrefabIndex > 0)
        {
            currentPrefabIndex--;
        }
        else
        {
            currentPrefabIndex = nftTypesOwned - 1;
        }

        // Instantiate the next prefab in the array
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation, transform);

        // Logic for handling the car stats and other UI information
        // Array Index is as follows: 
        // 0 = Chevrolet Camaro (Nitro Nova GTL)
        // 1 = Ford GT (Turbo Storm GT)
        // 2 = Ferrari (Star Stream S6)
        // Names are subject to change

        switch (currentPrefabIndex)
        {
            case 0:
                carName.text = "Nitro Nova GTL";
                engineSlider.value = globalManager.engineLevelNft1;
                handlingSlider.value = globalManager.handlingLevelNft1;
                boostSlider.value = globalManager.nosLevelNft1;
                // Actively select this car
                globalManager.playerCar = car1;
                globalManager.engineLevel = globalManager.engineLevelNft1;
                globalManager.handlingLevel = globalManager.handlingLevelNft1;
                globalManager.nosLevel = globalManager.nosLevelNft1;
                break;

            case 1:
                carName.text = "Turbo Storm GT";
                engineSlider.value = globalManager.engineLevelNft2;
                handlingSlider.value = globalManager.handlingLevelNft2;
                boostSlider.value = globalManager.nosLevelNft2;
                // Actively select this car
                globalManager.playerCar = car2;
                globalManager.engineLevel = globalManager.engineLevelNft2;
                globalManager.handlingLevel = globalManager.handlingLevelNft2;
                globalManager.nosLevel = globalManager.nosLevelNft2;
                break;

            case 2:
                carName.text = "Star Stream S6";
                engineSlider.value = globalManager.engineLevelNft3;
                handlingSlider.value = globalManager.handlingLevelNft3;
                boostSlider.value = globalManager.nosLevelNft3;
                // Actively select this car
                globalManager.playerCar = car3;
                globalManager.engineLevel = globalManager.engineLevelNft3;
                globalManager.handlingLevel = globalManager.handlingLevelNft3;
                globalManager.nosLevel = globalManager.nosLevelNft3;
                break;
        }

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
        var resultArray = response.Split(' ');
        if (response == "") return;
        Debug.Log($"Result: {response}");
        foreach (var nftId in resultArray)
        {
            Debug.Log("ADDING NFT " + nftId);
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