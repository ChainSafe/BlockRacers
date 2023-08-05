using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    // Last selected colour
    private Color lastSelectedColour;
    // Car prefabs
    public GameObject car1, car2, car3;
    // Our upgrade menu objects
    private int upgradeIndex = 0;
    // Materials
    [SerializeField] private Texture2D redSprite, blueSprite, magentaSprite;
    [SerializeField] private TextMeshProUGUI descriptionText;
    // Car Sprites
    [SerializeField] private Sprite car1Sprite, car2Sprite, car3Sprite;
    // Menu objects
    [SerializeField] private GameObject menuGarage, menuChangeCar, menuChangePaint, menuUpgrade, menuChangeNft, menuMarket, currentCarImage, currentPaintImageCar, currentPaintImagePaint;
    [SerializeField] private TextMeshProUGUI engineLevelText, handlingLevelText, nosLevelText;
    // Menu buttons
    [SerializeField] private GameObject changeCarButton, selectCarButton, selectItemButton, purchaseButton;

    #endregion

    #region Methods
    
    /// <summary>
    /// Initializes objects and sets our default colour
    /// </summary>
    private void Awake()
    {
        // Singleton
        instance = this;
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        // Set default to red
        lastSelectedColour = Color.red;
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
    /// A function to be accessed from all other scripts for button sounds to prevent referencing the audio manager everywhere
    /// </summary>
    public void PlayMenuSelect()
    {
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Purchase manager for upgrades
    /// </summary>
    public void PurchaseUpgrade()
    {
        // Play our menu select audio
        PlayMenuSelect();

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
    }

    // Closes other menus & opens the garage menu
    public void GarageMenuButton()
    {
        menuMarket.SetActive(false);
        menuUpgrade.SetActive(false);
        menuChangePaint.SetActive(false);
        menuChangeCar.SetActive(false);
        menuGarage.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(changeCarButton);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Opens the change car menu
    /// </summary>
    public void ChangeCarMenuButton()
    {
        menuGarage.SetActive(false);
        menuChangeCar.SetActive(true);
        currentPaintImageCar.GetComponent<Image>().color = lastSelectedColour;
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(selectCarButton);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Opens the change paint menu
    /// </summary>
    public void ChangePaintMenuButton()
    {
        menuChangeCar.SetActive(false);
        menuChangePaint.SetActive(true);
        currentPaintImagePaint.GetComponent<Image>().color = lastSelectedColour;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Opens the upgrade menu
    /// </summary>
    public void UpgradeMenuButton()
    {
        engineLevelText.text = $"LEVEL {globalManager.engineLevel}";
        handlingLevelText.text = $"LEVEL {globalManager.handlingLevel}";
        nosLevelText.text = $"LEVEL {globalManager.nosLevel}";
        menuGarage.SetActive(false);
        menuUpgrade.SetActive(true);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    /// <summary>
    /// Opens the nft menu
    /// </summary>
    public void NftMenuButton()
    {
        menuGarage.SetActive(false);
        menuChangeNft.SetActive(true);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
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
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Leaves garage and goes to the main menu
    /// </summary>
    public void MainMenuButton()
    {
        globalManager.sceneToLoad = "MenuScene";
        SceneManager.LoadScene("LoadingScreen");
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Change to car 1
    /// </summary>
    public void SelectCar1()
    {
        globalManager.playerCar = car1;
        currentCarImage.GetComponent<Image>().sprite = car1Sprite;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Changes to car 2
    /// </summary>
    public void SelectCar2()
    {
        globalManager.playerCar = car2;
        currentCarImage.GetComponent<Image>().sprite = car2Sprite;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    ///  Changes car 3
    /// </summary>
    public void SelectCar3()
    {
        globalManager.playerCar = car3;
        currentCarImage.GetComponent<Image>().sprite = car3Sprite;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Changes colour 1
    /// </summary>
    public void SelectColour1()
    {
        globalManager.nftSprite = redSprite;
        currentPaintImageCar.GetComponent<Image>().color = Color.red;
        currentPaintImagePaint.GetComponent<Image>().color = Color.red;

        lastSelectedColour = Color.red;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Changes colour 2
    /// </summary>
    public void SelectColour2()
    {
        globalManager.nftSprite = blueSprite;
        currentPaintImageCar.GetComponent<Image>().color = Color.blue;
        currentPaintImagePaint.GetComponent<Image>().color = Color.blue;
        lastSelectedColour = Color.blue;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Changes colour 3
    /// </summary>
    public void SelectColour3()
    {
        globalManager.nftSprite = magentaSprite;
        currentPaintImageCar.GetComponent<Image>().color = Color.magenta;
        currentPaintImagePaint.GetComponent<Image>().color = Color.magenta;
        lastSelectedColour = Color.magenta;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Upgrades car engine
    /// </summary>
    public void PurchaseEngineUpgrade()
    {
        switch (globalManager.engineLevel)
        {
            case 1:
                globalManager.engineLevel = 2;
                engineLevelText.text = $"LEVEL {globalManager.engineLevel}";
                break;
            case 2:
                globalManager.engineLevel = 3;
                engineLevelText.text = $"LEVEL {globalManager.engineLevel}";
                break;
            case 3:
                globalManager.engineLevel = 1;
                engineLevelText.text = $"LEVEL {globalManager.engineLevel}";
                break;
        }
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Upgrades car handling
    /// </summary>
    public void PurchaseHandlingUpgrade()
    {
        switch (globalManager.handlingLevel)
        {
            case 1:
                globalManager.handlingLevel = 2;
                handlingLevelText.text = $"LEVEL {globalManager.handlingLevel}";
                break;
            case 2:
                globalManager.handlingLevel = 3;
                handlingLevelText.text = $"LEVEL {globalManager.handlingLevel}";
                break;
            case 3:
                globalManager.handlingLevel = 1;
                handlingLevelText.text = $"LEVEL {globalManager.handlingLevel}";
                break;
        }
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Upgrades car NOS
    /// </summary>
    public void PurchaseNosUpgrade()
    {
        switch (globalManager.nosLevel)
        {
            case 1:
                globalManager.nosLevel = 2;
                nosLevelText.text = $"LEVEL {globalManager.nosLevel}";
                break;
            case 2:
                globalManager.nosLevel = 3;
                nosLevelText.text = $"LEVEL {globalManager.nosLevel}";
                break;
            case 3:
                globalManager.nosLevel = 1;
                nosLevelText.text = $"LEVEL {globalManager.nosLevel}";
                break;
        }
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Selects the engine upgrade
    /// </summary>
    public void SelectEngineUpgrade()
    {
        // Play our menu select audio
        PlayMenuSelect();
        // Sets index and displays description of upgrade
        upgradeIndex = 1;
        descriptionText.text = "upgrading your engine will improve your acceleration and top speed";
        // You can change a cost variable here to display how much an upgrade costs if required
    }
    
    /// <summary>
    /// Selects the handling upgrade
    /// </summary>
    public void SelectHandlingUpgrade()
    {
        // Play our menu select audio
        PlayMenuSelect();
        // Sets index and displays description of upgrade
        upgradeIndex = 2;
        descriptionText.text = "upgrading your handling will improve the agility of your car and ability to hold a drift";
        // You can change a cost variable here to display how much an upgrade costs if required
    }

    /// <summary>
    /// Selects the NOS upgrade
    /// </summary>
    public void SelectNOSUpgrade()
    {
        // Play our menu select audio
        PlayMenuSelect();
        // Sets index and displays description of upgrade
        upgradeIndex = 3;
        descriptionText.text = "upgrading your boost will allow it to drain slower and recharge faster";
        // You can change a cost variable here to display how much an upgrade costs if required
    }
    
    /// <summary>
    /// Sets description and purchase button functionality
    /// </summary>
    private void Update()
    {
        // If no upgrade is selected, we don't want the purchase button to show.
        purchaseButton.SetActive(upgradeIndex >= 1);
        // We also want to set the description based on what upgrade is selected
        descriptionText.text = (upgradeIndex == 0) ? "please select an upgrade to learn more about it" : descriptionText.text;
    }

    #endregion
}
