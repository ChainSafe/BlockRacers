using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GarageMenu : MonoBehaviour
{
    // Global manager
    private GlobalManager globalManager;
    
    // Audio
    private AudioManager audioManager;
    
    // Materials
    public Material redMaterial;
    public Material blueMaterial;
    public Material magentaMaterial;
    
    // Car prefabs
    public GameObject Car1;
    public GameObject Car2;
    public GameObject Car3;
    
    // Car Sprites
    public Sprite car1Sprite;
    public Sprite car2Sprite;
    public Sprite car3Sprite;
    
    // Menu objects
    [SerializeField] private GameObject menuGarage;
    [SerializeField] private GameObject menuChangeCar;
    [SerializeField] private GameObject menuChangePaint;
    [SerializeField] private GameObject menuUpgrade;
    [SerializeField] private GameObject menuMarket;
    [SerializeField] private GameObject currentCarImage;
    [SerializeField] private GameObject currentPaintImage;
    [SerializeField] private TextMeshProUGUI engineLevelText;
    [SerializeField] private TextMeshProUGUI handlingLevelText;
    [SerializeField] private TextMeshProUGUI nosLevelText;
    
    private void Awake()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
    }

    // Opens the garage menu
    public void GarageMenuButton()
    {
        menuMarket.SetActive(false);
        menuUpgrade.SetActive(false);
        menuChangePaint.SetActive(false);
        menuChangeCar.SetActive(false);
        menuGarage.SetActive(true);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Opens the change car menu
    public void ChangeCarMenuButton()
    {
        menuGarage.SetActive(false);
        menuChangeCar.SetActive(true);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Opens the change paint menu
    public void ChangePaintMenuButton()
    {
        menuChangeCar.SetActive(false);
        menuChangePaint.SetActive(true);
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
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Opens the marketplace menu
    public void MarketMenuButton()
    {
        menuGarage.SetActive(false);
        menuMarket.SetActive(true);
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
        globalManager.playerCar = Car1;
        currentCarImage.GetComponent<Image>().sprite = car1Sprite;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Change car 2
    public void SelectCar2()
    {
        globalManager.playerCar = Car2;
        currentCarImage.GetComponent<Image>().sprite = car2Sprite;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Change car 3
    public void SelectCar3()
    {
        globalManager.playerCar = Car3;
        currentCarImage.GetComponent<Image>().sprite = car3Sprite;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Change colour 1
    public void SelectColour1()
    {
        globalManager.bodyMaterial = redMaterial;
        currentPaintImage.GetComponent<Image>().color = Color.red;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Change colour 2
    public void SelectColour2()
    {
        globalManager.bodyMaterial = blueMaterial;
        currentPaintImage.GetComponent<Image>().color = Color.blue;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Change colour 3
    public void SelectColour3()
    {
        globalManager.bodyMaterial = magentaMaterial;
        currentPaintImage.GetComponent<Image>().color = Color.magenta;
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