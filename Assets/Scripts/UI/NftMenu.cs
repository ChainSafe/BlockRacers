using UnityEngine;

/// <summary>
/// NFT object functionality for the car
/// </summary>
public class NftMenu : MonoBehaviour
{
    #region Fields
    
    // Global manager
    private GlobalManager globalManager;
    // NFT sprites
    [SerializeField] private Texture2D Nft1, Nft2, Nft3;

    #endregion

    #region Methods
    
    /// <summary>
    /// Initializes objects
    /// </summary>
    private void Awake()
    {
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
    }

    /// <summary>
    /// Changes to NFT 1
    /// </summary>
    public void SelectNft1()
    {
        globalManager.nftSprite = Nft1;
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }

    /// <summary>
    /// Changes to NFT 2
    /// </summary>
    public void SelectNft2()
    {
        globalManager.nftSprite = Nft2;
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }

    /// <summary>
    /// Changes to NFT 3
    /// </summary>
    public void SelectNft3()
    {
        globalManager.nftSprite = Nft3;
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
    }
    
    #endregion
}