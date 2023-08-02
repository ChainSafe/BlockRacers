using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
///  Changes the users chosen car
/// </summary>
public class SwapCars : MonoBehaviour
{
    #region Fields
    
    // Array of prefabs to swap between
    public GameObject[] prefabs;

    // Index of the currently active prefab & livery
    public int currentPrefabIndex = 0;
    public static int currentLiveryIndex = 0;

    // Reference to the currently instantiated prefab
    private GameObject currentPrefab;

    // Set the spawnpoint 
    private Vector3 spawnPoint = new Vector3(89.17f, 0.4f, -9.7f);

    // Our UI elements for the showroom
    public TextMeshProUGUI carName;
    public Slider engineSlider;
    public Slider handlingSlider;
    public Slider boostSlider;

    // Our available colours for each model
    public Material[] camaroLivery;
    public Material[] fordGTLivery;
    public Material[] ferrariLivery;

    // Reference the global manager
    private GlobalManager globalManager;

    // Reference our 3 playable prefabs
    public GameObject camaro;

    // Singleton
    public static SwapCars instance;
    
    #endregion

    #region Methods

    // Start is called before the first frame update
    void Start()
    {
        // Singleton
        instance = this;

        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();

        // Instantiate the initial prefab
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation, transform);
    }

    private void Update()
    {
        // Add support for left and right arrow keys as well
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousCar();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextCar();
        }
    }
    
    /// <summary>
    /// Changes to the next car in the list
    /// </summary>
    public void NextCar()
    {
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();

        // Destroy the current prefab
        Destroy(currentPrefab);

        // Increment the index to switch to the next prefab
        currentPrefabIndex++;
        if (currentPrefabIndex >= prefabs.Length)
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
                GarageMenu.instance.SelectCar1();
                break;

            case 1:
                carName.text = "Turbo Storm GT";
                engineSlider.value = globalManager.engineLevel;
                handlingSlider.value = globalManager.handlingLevel;
                boostSlider.value = globalManager.nosLevel;

                // Actively select this car
                GarageMenu.instance.SelectCar2();
                break;

            case 2:
                carName.text = "Star Stream S6";
                engineSlider.value = globalManager.engineLevel;
                handlingSlider.value = globalManager.handlingLevel;
                boostSlider.value = globalManager.nosLevel;

                // Actively select this car
                GarageMenu.instance.SelectCar3();

                break;
        }

    }
    
    /// <summary>
    /// Changes to the previous car in the list
    /// </summary>
    public void PreviousCar()
    {
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();

        // Destroy the current prefab
        Destroy(currentPrefab);

        // Decrement the index to switch to the next prefab  
        if (currentPrefabIndex > 0)
        {
            currentPrefabIndex--;
        }
        else
        {
            currentPrefabIndex = 2;
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
                GarageMenu.instance.SelectCar1();

                break;

            case 1:
                carName.text = "Turbo Storm GT";
                engineSlider.value = globalManager.engineLevel;
                handlingSlider.value = globalManager.handlingLevel;
                boostSlider.value = globalManager.nosLevel;

                // Actively select this car
                GarageMenu.instance.SelectCar2();

                break;

            case 2:
                carName.text = "Star Stream S6";
                engineSlider.value = globalManager.engineLevel;
                handlingSlider.value = globalManager.handlingLevel;
                boostSlider.value = globalManager.nosLevel;

                // Actively select this car
                GarageMenu.instance.SelectCar3();

                break;
        }
    }
    
    /// <summary>
    /// Changes the livery for each car based on the options available for each
    /// </summary>
    public void ChangeLivery()
    {
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();

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
    }
    
    #endregion
}
