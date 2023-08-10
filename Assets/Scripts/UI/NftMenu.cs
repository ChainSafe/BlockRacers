using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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

    // Our nft image array
    [SerializeField] private Texture2D[] nfts;

    // Our nft object array
    [SerializeField] private GameObject[] nftPrefabs;

    // NFT sprites
    [SerializeField] private Texture2D Nft1, Nft2, Nft3;

    // The canvas to populate
    [SerializeField] private GameObject scrollCanvas;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes objects and calls data
    /// </summary>
    private void Awake()
    {
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        CallData();
    }

    /// <summary>
    /// Calls nft data
    /// </summary>
    private void CallData()
    {
        InitializeArrays();
    }

    /// <summary>
    /// Initializes our prefab arrays
    /// </summary>
    private void InitializeArrays()
    {
        // Initialize arrays by size
        nfts = new Texture2D[2];
        nftPrefabs = new GameObject[2];
        InstantiatePrefabs();
    }

    /// <summary>
    /// Spawns our nft prefabs
    /// </summary>
    private void InstantiatePrefabs()
    {
        // Instantiate prefabs and set parent to scroll canvas
        foreach (var nftPrefabItem in nftPrefabs)
        {
            // Instantiate prefabs
            Instantiate(nftPrefab);
            // Set parent to scroll canvas
            nftPrefab.transform.SetParent(scrollCanvas.gameObject.transform);
            // Populates the prefabs
            PopulatePrefabs(nftPrefab);
        }
    }

    /// <summary>
    /// Populates the nft prefab base with images
    /// </summary>
    private void PopulatePrefabs(GameObject nftPrefab)
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