using Unity.VisualScripting;
using UnityEngine;
using Image = UnityEngine.UIElements.Image;

/// <summary>
/// NFT object functionality for the car ---Work in progress
/// </summary>
public class NftMenu : MonoBehaviour
{
    #region Fields
    
    // Global manager
    private GlobalManager globalManager;
    // The base prefab we're using to display nfts
    [SerializeField] private GameObject nftPrefab;
    // Our nft object array
    [SerializeField] private Texture2D[] nfts;
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
        // Initialize array by size
        nfts = new Texture2D[1];
        // Add cars to array
        for (int i = 0; i < nfts.Length; i++)
        {
            nfts[i] = Nft1;
        }
        // Populates the prefabs
        PopulatePrefab(nftPrefab);
    }
    
    /// <summary>
    /// Spawns and populates our nft prefab base with images
    /// </summary>
    private void PopulatePrefab(GameObject nftPrefab)
    {
        foreach (Texture2D nftImage in nfts)
        {
            GameObject image = GameObject.Find("Image");
            image.GetComponent<Image>().sprite = nftImage.GetComponent<Image>().sprite;
        }
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