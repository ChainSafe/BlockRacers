using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of our NFT variables
/// </summary>
public class GlobalManager : MonoBehaviour
{
    #region Fields

    // String for loading screen
    public string sceneToLoad;

    // Player car
    public GameObject playerCar;
    
    // Gyro input
    public bool gyroEnabled;

    // Player material
    public Texture2D nftSprite;
    
    // Race tokens
    public double raceTokens;
    
    // Player nft ids
    public List<int> ownedNftIds;
    
    // Player nfts unlocked
    public bool[] unlockedNfts;
    
    // Player stats & default values
    public int selectedNftId,
        selectedNftType,
        engineLevel,
        handlingLevel,
        nosLevel;

    public int[] engineLevelStats, nosLevelStats, handlingLevelStats;

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
        // Initialize data arrays so they dont error
        unlockedNfts = new bool[3];
        engineLevelStats = new int[3];
        nosLevelStats = new int[3];
        handlingLevelStats = new int[3];
        // Temp array to assign initial values
        int[] initialValues = { 1, 1, 1 };
        // Iterate over arrays and set values
        int index = 0;
        foreach (int[] array in new[] { engineLevelStats, nosLevelStats, handlingLevelStats })
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = initialValues[index];
            }
            index++;
        }
    }

    #endregion
}