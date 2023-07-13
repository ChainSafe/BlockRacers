using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GarageMenu : MonoBehaviour
{
    // Global manager
    private GlobalManager globalManager;
    
    // Audio
    private AudioManager audioManager;
    
    // Materials
    public Material redMaterial, blueMaterial, magentaMaterial;

    // Car prefabs
    public GameObject car1, car2, car3;

    // Car Sprites
    public Sprite car1Sprite, car2Sprite, car3Sprite;
    private Color lastSelectedColour;

    // Menu objects
    [SerializeField] private GameObject menuGarage, menuChangeCar, menuChangePaint, menuUpgrade, menuMarket, currentCarImage, currentPaintImageCar, currentPaintImagePaint;
    [SerializeField] private TextMeshProUGUI engineLevelText, handlingLevelText, nosLevelText;
    
    // Menu buttons
    [SerializeField] private GameObject changeCarButton, selectCarButton, selectPaintButton, selectUpgradeButton, selectItemButton;
        
    private void Awake()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        // Set default to red
        lastSelectedColour = Color.red;
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(changeCarButton);
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
    
    // Opens the change car menu
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
    
    // Opens the change paint menu
    public void ChangePaintMenuButton()
    {
        menuChangeCar.SetActive(false);
        menuChangePaint.SetActive(true);
        currentPaintImagePaint.GetComponent<Image>().color = lastSelectedColour;
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(selectPaintButton);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Opens the upgrade menu
    public void UpgradeMenuButton()
    {
        engineLevelText.text = $"LEVEL {globalManager.engineLevel}";
        handlingLevelText.text = $"LEVEL {globalManager.handlingLevel}";
        nosLevelText.text = $"LEVEL {globalManager.nosLevel}";
        menuGarage.SetActive(false);
        menuUpgrade.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(selectUpgradeButton);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Opens the marketplace menu
    public void MarketMenuButton()
    {
        menuGarage.SetActive(false);
        menuMarket.SetActive(true);
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(selectItemButton);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Leaves garage and goes to the main menu
    public void MainMenuButton()
    {
        globalManager.sceneToLoad = "MenuScene";
        SceneManager.LoadScene("LoadingScreen");
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Change car 1
    public void SelectCar1()
    {
        globalManager.playerCar = car1;
        currentCarImage.GetComponent<Image>().sprite = car1Sprite;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Change car 2
    public void SelectCar2()
    {
        globalManager.playerCar = car2;
        currentCarImage.GetComponent<Image>().sprite = car2Sprite;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Change car 3
    public void SelectCar3()
    {
        globalManager.playerCar = car3;
        currentCarImage.GetComponent<Image>().sprite = car3Sprite;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Change colour 1
    public void SelectColour1()
    {
        globalManager.bodyMaterial = redMaterial;
        currentPaintImageCar.GetComponent<Image>().color = Color.red;
        currentPaintImagePaint.GetComponent<Image>().color = Color.red;
        lastSelectedColour = Color.red;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Change colour 2
    public void SelectColour2()
    {
        globalManager.bodyMaterial = blueMaterial;
        currentPaintImageCar.GetComponent<Image>().color = Color.blue;
        currentPaintImagePaint.GetComponent<Image>().color = Color.blue;
        lastSelectedColour = Color.blue;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Change colour 3
    public void SelectColour3()
    {
        globalManager.bodyMaterial = magentaMaterial;
        currentPaintImageCar.GetComponent<Image>().color = Color.magenta;
        currentPaintImagePaint.GetComponent<Image>().color = Color.magenta;
        lastSelectedColour = Color.magenta;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    // Upgrading engine
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
    
    // Upgrading handling
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
    
    // Upgrading nos
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
}