using UnityEngine;
using UnityEngine.SceneManagement;

public class GarageMenu : MonoBehaviour
{
    // Audio
    private AudioManager audioManager;

    [SerializeField] private GameObject MenuGarage;
    [SerializeField] private GameObject MenuUpgrade;
    [SerializeField] private GameObject MenuMarket;
    
    void Start()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
    }
    
    // Opens the garage menu
    public void GarageMenuButton()
    {
        MenuMarket.SetActive(false);
        MenuUpgrade.SetActive(false);
        MenuGarage.SetActive(true);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Opens the upgrade menu
    public void UpgradeMenuButton()
    {
        MenuGarage.SetActive(false);
        MenuUpgrade.SetActive(true);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Opens the marketplace menu
    public void MarketMenuButton()
    {
        MenuGarage.SetActive(false);
        MenuMarket.SetActive(true);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    // Leaves garage and goes to the main menu
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MenuScene");
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
}