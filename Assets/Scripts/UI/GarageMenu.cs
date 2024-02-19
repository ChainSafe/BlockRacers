using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the garage menu functions
/// </summary>
public class GarageMenu : MonoBehaviour
{
    #region Fields

    // Singleton to access functions
    public static GarageMenu instance;

    // Audio
    private AudioManager audioManager;

    // Global manager
    private GlobalManager globalManager;

    // Our upgrade menu objects
    private int upgradeIndex;

    [SerializeField] private TextMeshProUGUI descriptionText;

    // Menu objects
    [SerializeField] private GameObject menuGarage, menuShowRoom, menuUpgrade, menuChangeNft, menuMarket, mintNftDescription;

    [SerializeField] private TextMeshProUGUI engineLevelText, handlingLevelText, nosLevelText;

    // Menu buttons
    [SerializeField] private GameObject changeCarButton, selectItemButton, purchaseButton;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes objects
    /// </summary>
    private void Awake()
    {
        // Singleton
        instance = this;
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(changeCarButton);
    }

    /// <summary>
    /// Sets our selected button to what we've moused over
    /// </summary>
    /// <param name="button"></param>
    public void OnMouseOverButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }

    /// <summary>
    /// A function to play the menu selection sound
    /// </summary>
    public void PlayMenuSelect()
    {
        audioManager.Play("MenuSelect");
    }

    // Closes other menus & opens the garage menu
    public void GarageMenuButton()
    {
        if (menuShowRoom.activeSelf)
        {
            // Rotate our camera back
            CameraController.instance.RotateCamera(-95f, 0.5f);
        }
        menuMarket.SetActive(false);
        menuUpgrade.SetActive(false);
        menuShowRoom.SetActive(false);
        menuChangeNft.SetActive(false);
        menuGarage.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(changeCarButton);
        // Play our menu select audio
        PlayMenuSelect();
    }

    /// <summary>
    /// Takes the user to the showroom
    /// </summary>
    public void ShowRoomMenuButton()
    {
        // Rotates our camera to the showroom cars
        CameraController.instance.RotateCamera(95f, 0.5f);
        // Changes menu displays
        menuGarage.SetActive(false);
        menuShowRoom.SetActive(true);
        // Play our menu select audio
        PlayMenuSelect();
    }

    /// <summary>
    /// Opens the car upgrade menu for the player
    /// </summary>
    public void UpgradesMenuButton()
    {
        // Rotates our camera back
        CameraController.instance.RotateCamera(-95f, 0.5f);
        // Changes menu displays
        menuUpgrade.SetActive(true);
        menuShowRoom.SetActive(false);
        // Play our menu select audio
        PlayMenuSelect();
    }
    
    /// <summary>
    /// Opens market menu from the upgrades area for the player
    /// </summary>
    public void MintNftFromUpgrades()
    {
        if (menuShowRoom.activeSelf)
        {
            // Rotate our camera back
            CameraController.instance.RotateCamera(-95f, 0.5f);
            menuShowRoom.SetActive(false);
            menuUpgrade.SetActive(false);
            menuMarket.SetActive(true);
        }
    }


    /// <summary>
    /// Opens the nft menu
    /// </summary>
    public void NftMenuButton()
    {
        // Rotates our camera back
        CameraController.instance.RotateCamera(-95f, 0.5f);
        menuShowRoom.SetActive(false);
        menuChangeNft.SetActive(true);
        // Play our menu select audio
        PlayMenuSelect();
    }

    /// <summary>
    /// Opens the marketplace menu
    /// </summary>
    public void MarketMenuButton()
    {
        menuGarage.SetActive(false);
        menuMarket.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(selectItemButton);
        // Play our menu select audio
        PlayMenuSelect();
    }

    /// <summary>
    /// Leaves garage and goes to the main menu
    /// </summary>
    public void MainMenuButton()
    {
        globalManager.sceneToLoad = "MenuScene";
        SceneManager.LoadScene("LoadingScreen");
        // Play our menu select audio
        PlayMenuSelect();
    }

    /// <summary>
    /// Purchase manager for upgrades
    /// </summary>
    public void PurchaseUpgrade()
    {
        // Based on which upgrade is currently selected, we invoke the required method
        switch (upgradeIndex)
        {
            case 0:
                break;
            case 1:
                PurchaseEngineUpgrade();
                break;
            case 2:
                PurchaseHandlingUpgrade();
                break;
            case 3:
                PurchaseNosUpgrade();
                break;
        }

        // Play our menu select audio
        PlayMenuSelect();
    }

    /// <summary>
    /// Selects the engine upgrade
    /// </summary>
    public void SelectEngineUpgrade()
    {
        // Sets index and displays description of upgrade
        upgradeIndex = 1;
        descriptionText.text = "upgrading your engine will improve your acceleration and top speed";
        // You can change a cost variable here to display how much an upgrade costs if required
        // Play our menu select audio
        PlayMenuSelect();
    }

    /// <summary>
    /// Selects the handling upgrade
    /// </summary>
    public void SelectHandlingUpgrade()
    {
        // Sets index and displays description of upgrade
        upgradeIndex = 2;
        descriptionText.text =
            "upgrading your handling will improve the agility of your car and ability to hold a drift";
        // You can change a cost variable here to display how much an upgrade costs if required
        // Play our menu select audio
        PlayMenuSelect();
    }

    /// <summary>
    /// Selects the NOS upgrade
    /// </summary>
    public void SelectNOSUpgrade()
    {
        // Sets index and displays description of upgrade
        upgradeIndex = 3;
        descriptionText.text = "upgrading your boost will allow it to drain slower and recharge faster";
        // You can change a cost variable here to display how much an upgrade costs if required
        // Play our menu select audio
        PlayMenuSelect();
    }

    private async void PurchaseUpgrade(int enumValue)
    {
        var response = await ContractManager.PurchaseUpgrade(globalManager.selectedNftId, enumValue);
        Debug.Log(response);
        // Play our menu select audio
        PlayMenuSelect();
    }

    /// <summary>
    /// Upgrades car engine
    /// </summary>
    private void PurchaseEngineUpgrade()
    {
        switch (globalManager.engineLevel)
        {
            case 1:
                try
                {
                    PurchaseUpgrade(1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                globalManager.engineLevel = 2;
                engineLevelText.text = $"LEVEL 2";
                break;
            case 2:
                try
                {
                    PurchaseUpgrade(1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                globalManager.engineLevel = 3;
                engineLevelText.text = $"LEVEL 3";
                break;
            case 3:
                Debug.Log("Engine Level Max!");
                break;
        }

        // Play our menu select audio
        PlayMenuSelect();
    }

    /// <summary>
    /// Upgrades car handling
    /// </summary>
    private void PurchaseHandlingUpgrade()
    {
        switch (globalManager.handlingLevel)
        {
            case 1:
                try
                {
                    PurchaseUpgrade(2);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                handlingLevelText.text = $"LEVEL 2";
                break;
            case 2:
                try
                {
                    PurchaseUpgrade(2);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                globalManager.handlingLevel = 3;
                handlingLevelText.text = $"LEVEL 3";
                break;
            case 3:
                Debug.Log("Handling Level Max!");
                break;
        }

        // Play our menu select audio
        PlayMenuSelect();
    }

    /// <summary>
    /// Upgrades car NOS
    /// </summary>
    private void PurchaseNosUpgrade()
    {
        switch (globalManager.nosLevel)
        {
            case 1:
                try
                {
                    PurchaseUpgrade(3);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                globalManager.nosLevel = 2;
                nosLevelText.text = $"LEVEL 2";
                break;
            case 2:
                try
                {
                    PurchaseUpgrade(3);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                globalManager.nosLevel = 3;
                nosLevelText.text = $"LEVEL 3";
                break;
            case 3:
                Debug.Log("Nos Level Max!");
                break;
        }
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Sets description and purchase button functionality
    /// </summary>
    private void Update()
    {
        // If no upgrade is selected, we don't want the purchase button to show.
        purchaseButton.SetActive(upgradeIndex >= 1);
        // We also want to set the description based on what upgrade is selected
        descriptionText.text =
            (upgradeIndex == 0) ? "please select an upgrade to learn more about it" : descriptionText.text;
    }

    #endregion
}