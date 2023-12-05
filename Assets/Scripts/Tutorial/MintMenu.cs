using System.Linq;
using System.Numerics;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
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

    /// <summary>
    /// Mints custom tokens to the users address
    /// </summary>
    public async void MintCustomTokens()
    {
        // Sign nonce and set voucher
        string ecdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
        string account = PlayerPrefs.GetString("PlayerAccount");
        var amount = 500*1e18;
        var nonceData = await Evm.ContractCall(Web3Accessor.Web3, "nonce", ContractManager.TokenAbi, ContractManager.TokenContract, new object[] {account});
        var nonceResponse = SampleOutputUtil.BuildOutputValue(nonceData);
        int nonce = int.Parse(nonceResponse);
        string message = $"{nonce}{account}{amount}";
        var signature = Evm.EcdsaSignMessage(ecdsaKey, message);
        // Mint
        object[] args =
        {
            account,
            amount,
            signature
        };
        var data = await Evm.ContractSend(Web3Accessor.Web3, "mint", ContractManager.TokenAbi, ContractManager.TokenContract, args);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
        audioManager.Play("MenuSelect");
    }

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