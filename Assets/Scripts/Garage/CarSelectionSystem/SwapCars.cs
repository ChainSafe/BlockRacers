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
    public TextMeshProUGUI carName;

    public Slider engineSlider, handlingSlider, boostSlider;

    // Our available colours for each model
    public Material[] camaroLivery;
    public Material[] fordGTLivery;
    public Material[] ferrariLivery;
    
    // Nft stat fetch display
    [SerializeField]
    private GameObject fetchingStatsDisplay;

    // Car prefabs
    public GameObject car1, car2, car3;

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
        // Destroy the current prefab
        Destroy(currentPrefab);
        currentPrefabIndex = (currentPrefabIndex >= 1) ? currentPrefabIndex - 1 : 2;
        // Instantiate the next prefab in the array
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation, transform);
        // Display stats
        DisplayNftStats();
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