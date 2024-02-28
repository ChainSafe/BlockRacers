using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    
    // Reference the global manager
    private GlobalManager globalManager;

    // Index of the currently active prefab & livery
    public static int currentLiveryIndex;

    public int currentPrefabIndex;

    // Array of prefabs to swap between
    public GameObject[] prefabs;

    // Our UI elements for the showroom
    public TextMeshProUGUI carName, liveriesOwnedText;

    public Slider engineSlider, handlingSlider, boostSlider;

    // Our available colours for each model
    public Material[] camaroLivery;
    public Material[] fordGTLivery;
    public Material[] ferrariLivery;
    
    // Nft stat fetch display
    [SerializeField]
    private GameObject fetchingStatsDisplay;

    // Car prefabs
    public GameObject car1, car2, car3, liveryLockedDisplay;

    // Reference to the currently instantiated prefab
    private GameObject currentPrefab;

    // Set the spawn point 
    private Vector3 spawnPoint = new Vector3(89.17f, 0.4f, -9.7f);

    // Platform that the cars spawn on
    [SerializeField] private GameObject platform;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes needed objects
    /// </summary>
    private void Awake()
    {
        // Singleton
        instance = this;
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        // Instantiate the initial prefab
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation, transform);
        platform.transform.position = new Vector3(currentPrefab.transform.position.x,
            currentPrefab.transform.position.y - 0.45f, currentPrefab.transform.position.z);
    }
    
    /// <summary>
    /// Calls unlocked nfts whenever the menu is opened
    /// </summary>
    private void OnEnable()
    {
        fetchingStatsDisplay.SetActive(true);
        // Reset the prefab index so the first car is displayed
        currentPrefabIndex = 0;
        currentLiveryIndex = 0;
        // Destroy the current prefab
        Destroy(currentPrefab);
        // Instantiate the next prefab in the array
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation, transform);
        // Call nft data
        GetNftData();
    }

    /// <summary>
    /// Changes to the next car in the list
    /// </summary>
    public void NextCar()
    {
        // Reset livery index
        currentLiveryIndex = 0;
        // Destroy the current prefab
        Destroy(currentPrefab);
        currentPrefabIndex = (currentPrefabIndex <= 1) ? currentPrefabIndex + 1 : 0;
        // Instantiate the next prefab in the array
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation, transform);
        // Display stats
        DisplayNftStats();
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }

    /// <summary>
    /// Changes to the previous car in the list
    /// </summary>
    public void PreviousCar()
    {
        // Reset livery index
        currentLiveryIndex = 0;
        // Destroy the current prefab
        Destroy(currentPrefab);
        currentPrefabIndex = (currentPrefabIndex >= 1) ? currentPrefabIndex - 1 : 2;
        // Instantiate the next prefab in the array
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation, transform);
        // Display stats
        DisplayNftStats();
        globalManager.playerCar = currentPrefab;
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }

    /// <summary>
    /// Changes the livery for each car based on the options available for each
    /// </summary>
    public void ChangeLivery()
    {
        fetchingStatsDisplay.SetActive(true);
        GetNftSkinsData();
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
    /// Fetches NFT data from the chain
    /// </summary>
    /// <param name="_nftId"></param>
    private async void GetNftData()
    {
        Debug.Log($"Getting stats");
        // Contract call
        var values = await ContractManager.GetNftStatsWithType();
        List<List<BigInteger>> nestedList = new List<List<BigInteger>>(values[0]);
        // Parse values
        for (int i = 0; i < nestedList.Count; i++)
        {
            globalManager.ownedNftIds[i] = (int)nestedList[i][0];
            globalManager.engineLevelStats[i] = (int)nestedList[i][1] + 1;
            globalManager.handlingLevelStats[i] = (int)nestedList[i][2] + 1;
            globalManager.nosLevelStats[i] = (int)nestedList[i][3] + 1;
        }
        // Display the stats on the sliders
        DisplayNftStats();
    }
    
    /// <summary>
    /// Displays NFT stats
    /// </summary>
    /// <param name="_nftId"></param>
    private void DisplayNftStats()
    {
        // Set our selected car
        globalManager.playerCar = currentPrefabIndex switch
        {
            0 => car1,
            1 => car2,
            2 => car3,
            _ => globalManager.playerCar
        };
        // Set slider values to stored data values
        engineSlider.value = globalManager.engineLevelStats[currentPrefabIndex];
        handlingSlider.value = globalManager.handlingLevelStats[currentPrefabIndex];
        boostSlider.value = globalManager.nosLevelStats[currentPrefabIndex];
        // // Set selected type & nft stats for use in race
        globalManager.engineLevel = (int)engineSlider.value;
        globalManager.handlingLevel = (int)handlingSlider.value;
        globalManager.nosLevel = (int)boostSlider.value;
        // Set selected NftType
        globalManager.selectedNftType = currentPrefabIndex;
        // Sets our selected NFT ID
        globalManager.selectedNftId = globalManager.ownedNftIds[currentPrefabIndex];
        fetchingStatsDisplay.SetActive(false);
    }
    
    /// <summary>
    /// Gets unlocked Nft skins
    /// </summary>
    private async void GetNftSkinsData()
    {
        // Check skins
        Debug.Log("Getting Nft skins data");
        var unlockedSkins = await ContractManager.GetNftSkins();
        List<List<BigInteger>> nestedSkinsList = new List<List<BigInteger>>(unlockedSkins[0]);
        Dictionary<BigInteger, BigInteger> skinsDictionary = new Dictionary<BigInteger, BigInteger>();
        // Initialize dictionary with default values as 0
        for (int i = 0; i < 9; i++)
        {
            skinsDictionary.Add(i, 0);
        }
        // Populate dictionary with provided values
        foreach (var pair in nestedSkinsList)
        {
            if (pair.Count == 2 && pair[0] >= 0 && pair[0] <= 8)
            {
                skinsDictionary[pair[0]] = pair[1];
            }
        }
        DisplayLiveryLock(skinsDictionary);
    }
    
    /// <summary>
    /// // Display livery lock and owned livery text number
    /// </summary>
    /// <param name="result"></param>
    private void DisplayLiveryLock(Dictionary<BigInteger, BigInteger> skinsDictionary)
    {
        switch (currentPrefabIndex)
        {
            case 0 when currentLiveryIndex == 1:
                liveryLockedDisplay.SetActive(skinsDictionary[0] == 0);
                liveriesOwnedText.text = skinsDictionary[0].ToString();
                break;
            case 0 when currentLiveryIndex == 2:
                liveryLockedDisplay.SetActive(skinsDictionary[1] == 0);
                liveriesOwnedText.text = skinsDictionary[1].ToString();
                break;
            case 0:
            {
                if (currentLiveryIndex == 3)
                {
                    liveryLockedDisplay.SetActive(skinsDictionary[2] == 0);
                    liveriesOwnedText.text = skinsDictionary[2].ToString();
                }
                break;
            }
            case 1 when currentLiveryIndex == 1:
                liveryLockedDisplay.SetActive(skinsDictionary[3] == 0);
                liveriesOwnedText.text = skinsDictionary[3].ToString();
                break;
            case 1 when currentLiveryIndex == 2:
                liveryLockedDisplay.SetActive(skinsDictionary[4] == 0);
                liveriesOwnedText.text = skinsDictionary[4].ToString();
                break;
            case 1:
            {
                if (currentLiveryIndex == 3)
                {
                    liveryLockedDisplay.SetActive(skinsDictionary[5] == 0);
                    liveriesOwnedText.text = skinsDictionary[5].ToString();
                }
                break;
            }
            case 2 when currentLiveryIndex == 1:
                liveryLockedDisplay.SetActive(skinsDictionary[6] == 0);
                liveriesOwnedText.text = skinsDictionary[6].ToString();
                break;
            case 2 when currentLiveryIndex == 2:
                liveryLockedDisplay.SetActive(skinsDictionary[7] == 0);
                liveriesOwnedText.text = skinsDictionary[7].ToString();
                break;
            case 2:
            {
                if (currentLiveryIndex == 3)
                {
                    liveryLockedDisplay.SetActive(skinsDictionary[8] == 0);
                    liveriesOwnedText.text = skinsDictionary[8].ToString();
                }
                break;
            }
        }
        fetchingStatsDisplay.SetActive(false);
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