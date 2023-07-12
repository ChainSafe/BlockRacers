using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GarageMenu : MonoBehaviour
{
    // Global manager
    private GlobalManager globalManager;
    
    // Audio
    private AudioManager audioManager;
    
    // Menu objects
    [SerializeField] private GameObject menuGarage;
    [SerializeField] private GameObject menuChangePaint;
    [SerializeField] private GameObject menuUpgrade;
    [SerializeField] private GameObject menuMarket;
    [SerializeField] private TextMeshProUGUI engineLevelText;
    [SerializeField] private TextMeshProUGUI handlingLevelText;
    [SerializeField] private TextMeshProUGUI nosLevelText;
    
    private void Awake()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        // Find our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
    }

    // Opens the garage menu
    public void GarageMenuButton()
    {
        menuMarket.SetActive(false);
        menuUpgrade.SetActive(false);
        menuChangePaint.SetActive(false);
        menuGarage.SetActive(true);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Opens the change car menu
    public void ChangePaintMenuButton()
    {
        menuGarage.SetActive(false);
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
}