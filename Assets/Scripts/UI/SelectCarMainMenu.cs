using System;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SelectCarMainMenu : MonoBehaviour
{
    #region Fields

    private GlobalManager globalManager;
    private AudioManager audioManager;
    [SerializeField] private GameObject car1, car2, car3, carSpawnPoint, fetchingDataPopup, liveryLockedDisplay;
    public Slider engineSlider, handlingSlider, boostSlider;
    private int currentPrefabIndex, currentLiveryIndex;
    // Our available colours for each model
    public Material[] camaroLivery;
    public Material[] fordGTLivery;
    public Material[] ferrariLivery;
    // Reference to the currently instantiated prefab
    private GameObject currentPrefab;
    // Array of prefabs to swap between
    public GameObject[] prefabs;
    // Set the spawn point 
    private Vector3 spawnPoint;
    // Texts & Inputs
    [SerializeField] private TextMeshProUGUI carName;
    [SerializeField] private TMP_InputField bbTxInput;

    #endregion

    #region Methods
    
    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Awake()
    {
        // Finds our managers
        audioManager = FindObjectOfType<AudioManager>();
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        // Set the spawn point 
        spawnPoint = new Vector3(carSpawnPoint.transform.position.x, carSpawnPoint.transform.position.y, carSpawnPoint.transform.position.z);
    }
    
    /// <summary>
    /// Resets selected on menu open
    /// </summary>
    private void OnEnable()
    {
        currentPrefabIndex = 0;
        currentLiveryIndex = 0;
        carName.text = "Camaro";
        // Instantiate the initial prefab
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation);
        currentPrefab.transform.rotation = Quaternion.Euler(0,45,0);
        fetchingDataPopup.SetActive(true);
        GetCarStats();
    }
    
    /// <summary>
    /// Destroys car prefab on menu close
    /// </summary>
    private void OnDisable()
    {
        Destroy(currentPrefab);
    }

    /// <summary>
    /// Gets Nft car stats from chain
    /// </summary>
    private async void GetCarStats()
    {
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
        DisplayCarStats();
    }
    
    /// <summary>
    /// Displays Nft car stats
    /// </summary>
    private void DisplayCarStats()
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
        // Changes car text name
        switch (currentPrefabIndex)
        {
            case 0:
                carName.text = "Camaro";
                break;
            case 1:
                carName.text = "Ford GT";
                break;
            case 2:
                carName.text = "Ferrari";
                break;
            default:
                carName.text = "Camaro";
                break;
        }
        GetNftSkinsData();
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
                break;
            case 0 when currentLiveryIndex == 2:
                liveryLockedDisplay.SetActive(skinsDictionary[1] == 0);
                break;
            case 0:
            {
                if (currentLiveryIndex == 3)
                {
                    liveryLockedDisplay.SetActive(skinsDictionary[2] == 0);
                }
                break;
            }
            case 1 when currentLiveryIndex == 1:
                liveryLockedDisplay.SetActive(skinsDictionary[3] == 0);
                break;
            case 1 when currentLiveryIndex == 2:
                liveryLockedDisplay.SetActive(skinsDictionary[4] == 0);
                break;
            case 1:
            {
                if (currentLiveryIndex == 3)
                {
                    liveryLockedDisplay.SetActive(skinsDictionary[5] == 0);
                }
                break;
            }
            case 2 when currentLiveryIndex == 1:
                liveryLockedDisplay.SetActive(skinsDictionary[6] == 0);
                break;
            case 2 when currentLiveryIndex == 2:
                liveryLockedDisplay.SetActive(skinsDictionary[7] == 0);
                break;
            case 2:
            {
                if (currentLiveryIndex == 3)
                {
                    liveryLockedDisplay.SetActive(skinsDictionary[8] == 0);
                }
                break;
            }
        }
        fetchingDataPopup.SetActive(false);
    }
    
    /// <summary>
    /// Changes to the next car in the list
    /// </summary>
    public void NextCar()
    {
        // Disable lock display as the first skin is free
        liveryLockedDisplay.SetActive(false);
        // Reset livery index
        currentLiveryIndex = 0;
        // Destroy the current prefab
        Destroy(currentPrefab);
        currentPrefabIndex = (currentPrefabIndex <= 1) ? currentPrefabIndex + 1 : 0;
        // Instantiate the next prefab in the array
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation);
        currentPrefab.transform.rotation = Quaternion.Euler(0,45,0);
        // Display stats
        DisplayCarStats();
        // Play our menu select audio
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Changes to the previous car in the list
    /// </summary>
    public void PreviousCar()
    {
        // Disable lock display as the first skin is free
        liveryLockedDisplay.SetActive(false);
        // Reset livery index
        currentLiveryIndex = 0;
        // Destroy the current prefab
        Destroy(currentPrefab);
        currentPrefabIndex = (currentPrefabIndex >= 1) ? currentPrefabIndex - 1 : 2;
        // Instantiate the next prefab in the array
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation, transform);
        currentPrefab.transform.rotation = Quaternion.Euler(0,45,0);
        // Display stats
        DisplayCarStats();
        globalManager.playerCar = currentPrefab;
        // Play our menu select audio
        audioManager.Play("MenuSelect");
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
        // Sets livery index for use in game
        SwapCars.currentLiveryIndex = currentLiveryIndex;
    }
    
    /// <summary>
    /// Maxes out car stats with Block Blasterz TX
    /// </summary>
    public void GetMaxxedOutCar()
    {
        var _txHash = bbTxInput.text;
        if (_txHash != "0x1f7f4194916fe5e4a28e2365429c99ecff70fc869558b458fe8c2488bde230h3") return;
        globalManager.engineLevel = 3;
        globalManager.nosLevel = 3;
        globalManager.handlingLevel = 3;
        engineSlider.value = globalManager.engineLevel;
        handlingSlider.value = globalManager.nosLevel;
        boostSlider.value = globalManager.handlingLevel;
    }

    #endregion
}
