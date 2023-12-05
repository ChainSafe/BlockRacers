using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Allows the user to mint tokens
/// </summary>
public class MintMenu : MonoBehaviour
{
    #region Fields

    // Audio
    private AudioManager audioManager;

    // First button
    [SerializeField] private GameObject firstButton;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes needed objects
    /// </summary>
    private void Awake()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(firstButton);
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Sets our selected button to what we've moused over
    /// </summary>
    /// <param name="button"></param>
    public void OnMouseOverButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }

    // /// <summary>
    // /// Mints custom tokens to the users address
    // /// </summary>
    // public async void MintCustomTokens()
    // {
    //     string method = "mint";
    //     BigInteger amount = 1;
    //     string account = PlayerPrefs.GetString("PlayerAccount");
    //     object[] args =
    //     {
    //         account,
    //         amount
    //     };
    //     var data = await Evm.ContractSend(Web3Accessor.Web3, method, ContractManager.TokenAbi, ContractManager.TokenContract, args);
    //     var response = SampleOutputUtil.BuildOutputValue(data);
    //     Debug.Log($"TX: {response}");
    //     audioManager.Play("MenuSelect");
    // }

    /// <summary>
    /// Opens the faucet webpage so the user can get some gas tokens
    /// </summary>
    public void OpenGasFaucetPage()
    {
        Application.OpenURL("https://cronos.org/faucet");
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Closes the menu and gives input control back to the user
    /// </summary>
    public void CloseMenu()
    {
        // Locks the cursor so the user can resume playing normally
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
        audioManager.Play("MenuSelect");
    }
    
    /// <summary>
    /// Keeps cursor unlocked
    /// </summary>
    private void Update()
    {
        // Unlocks the cursor so the user can select things
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion
}