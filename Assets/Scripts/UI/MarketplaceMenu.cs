using UnityEngine;

/// <summary>
/// Marketplace functionality
/// </summary>
public class MarketplaceMenu : MonoBehaviour
{
    #region Fields

    // Global manager
    private GlobalManager globalManager;
    
    // Mint buttons to enable/disable
    [SerializeField]
    private GameObject mintButton1, mintButton2, mintButton3;
    
    // Nft stat fetch display
    [SerializeField]
    private GameObject fetchingStatsDisplay;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes objects and calls data
    /// </summary>
    private void Awake()
    {
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
    }
    
    /// <summary>
    /// Calls unlocked nfts whenever the menu is opened
    /// </summary>
    private void Start()
    {
        //fetchingStatsDisplay.SetActive(true);
        GetUnlockedNfts();
    }

    /// <summary>
    /// Checks unlocked nfts to be used in upgrades
    /// </summary>
    private async void GetUnlockedNfts()
    {
        // Contract call
        var values = await ContractManager.GetUnlockedNfts();
        Debug.Log("Checking unlocked Nfts");
        // Disable the mint button for an NFT if already purchased
        for (int i = 0; i < values.Count; i++)
        {
            bool isActive = values[i];
            switch (i)
            {
                case 0:
                    mintButton1.SetActive(!isActive);
                    if (isActive)
                    {
                        globalManager.unlockedNfts[0] = true;
                    }
                    break;
                case 1:
                    mintButton2.SetActive(!isActive);
                    if (isActive)
                    {
                        globalManager.unlockedNfts[1] = true;
                    }
                    break;
                case 2:
                    mintButton3.SetActive(!isActive);
                    if (isActive)
                    {
                        globalManager.unlockedNfts[2] = true;
                    }
                    break;
            }
        }
        Debug.Log("Unlocked nfts found, mint buttons disabled for owned nfts");
        fetchingStatsDisplay.SetActive(false);
    }
    
    /// <summary>
    /// Purchases an NFT based on the nft type
    /// </summary>
    /// <param name="_nftType"></param>
    public async void PurchaseNft(int _nftType)
    {
        var response = await ContractManager.PurchaseNft(_nftType);
        Debug.Log($"Response: {response}");
        GarageMenu.instance.PlayMenuSelect();
    }
    
    /// <summary>
    /// Mints race tokens to an account, good for testing purposes
    /// </summary>
    public async void MintRaceTokens()
    {
        var response = await ContractManager.MintRaceTokens();
        Debug.Log($"Response: {response}");
        GarageMenu.instance.PlayMenuSelect();
    }

    #endregion
}