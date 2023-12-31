using UnityEngine;

/// <summary>
/// Keeps track of our NFT variables
/// </summary>
public class GlobalManager : MonoBehaviour
{
    #region Fields

    // String for loading screen
    public string sceneToLoad;
    
    // Security integer for contracts
    public int walletInt;

    // Player car
    public GameObject playerCar;

    // Player material
    public Texture2D nftSprite;

    // Player stats
    public int engineLevel, handlingLevel, nosLevel, engineLevelNft1 = 1, handlingLevelNft1 = 1, nosLevelNft1 = 1, engineLevelNft2 = 1, handlingLevelNft2 = 1, nosLevelNft2 = 1, engineLevelNft3 = 1, handlingLevelNft3 = 1, nosLevelNft3 = 1;

    // Connected bool
    public bool connected;

    // Race won bool for wagering
    public bool raceWon;

    // Player who won the race
    public string winningPlayer;

    // Bool to check if wagering is active or not
    public bool wagering;
    public bool wagerAccepted;
    public string wagerOpponent;
    public int wagerAmount;

    // Private key for the auth wallet 0x0d9566FcE2513cBD388DCD7749a873900033401C
    public string ERC20PrivateKey = "a78313605b1ded96c3f2c991b8cbe883924035f4e338fe9f5a860429c2777b2f";

    #endregion

    #region Methods

    /// <summary>
    /// Locks our frame rate to stop devices overheating and keeps things uniform
    /// </summary>
    private void Awake()
    {
        // Locks framerate to 60 FPS
        Application.targetFrameRate = 60;
        // Makes object global and doesnt destroy it when changing scenes
        DontDestroyOnLoad(this);
    }

    #endregion
}