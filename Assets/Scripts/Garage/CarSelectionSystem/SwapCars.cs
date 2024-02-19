using System.Collections.Generic;
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
        fetchingStatsDisplay.SetActive(true);
        // Call nft data
        GetOwnerIds();
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
        // If owned, fetch stats
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
        // Call stats at start of garage, If nft type owned, change stats else stats go back to default
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
    /// Gets a users owned nft ids array
    /// </summary>
    private async void GetOwnerIds()
    {
        // Contract call
        var values = await ContractManager.GetOwnerNftIds();
        // Clear the ownedNftIds list before adding new members
        globalManager.ownedNftIds?.Clear();
        globalManager.ownedNftIds ??= new List<int>();
        if (values != null)
        {
            foreach (var list in values)
            {
                foreach (int member in list)
                {
                    // Add id to players array
                    globalManager.ownedNftIds.Add(member);
                }
            }
            // Set the first nftID as selected
            globalManager.selectedNftId = globalManager.ownedNftIds[0];
            GetNftData();
        }
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
        foreach (var member in values)
        {
            foreach (var value in member)
            {
                Debug.Log("ID: " + value);
            }
        }
        // Parse values
        for (int i = 1; i < values.Count; i++)
        {
            globalManager.engineLevelStats[i] = int.Parse(values[i][1]);
            globalManager.handlingLevelStats[i] = int.Parse(values[i][2]);
            globalManager.nosLevelStats[i] = int.Parse(values[i][3]);
        }
        // Get the initial nft stats
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
        // Set selected type & nft stats for use in race
        globalManager.engineLevel = (int)engineSlider.value;
        globalManager.handlingLevel = (int)handlingSlider.value;
        globalManager.nosLevel = (int)boostSlider.value;
        // +1 here as we're converting from index to type which starts at 1
        globalManager.selectedNftType = currentPrefabIndex + 1;
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