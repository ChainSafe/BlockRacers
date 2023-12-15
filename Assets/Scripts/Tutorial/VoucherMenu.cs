using System.Numerics;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// An example of how Chainsafe's SDK voucher functionality works
/// </summary>
public class VoucherMenu : MonoBehaviour
{
    #region Fields

    // Audio
    private AudioManager audioManager;

    // First button
    [SerializeField] private GameObject firstButton;
    
    // Voucher
    private string voucher;

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
    /// Generates a voucher to be used to almost anything
    /// </summary>
    public async void GenerateVoucher()
    {
        // Sign nonce and set voucher
        string ecdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
        var account = await Web3Accessor.Web3.Signer.GetAddress(); 
        BigInteger amount = (BigInteger)(50*1e18);
        int nftType = 1;
        string message = (account + amount + nftType);
        var signatureResponse = Evm.EcdsaSignMessage(ecdsaKey, message);
        Debug.Log($"Signed Message: {signatureResponse}");
        voucher = signatureResponse;
        audioManager.Play("MenuSelect");
    }

    /// <summary>
    /// Redeems the generated voucher
    /// </summary>
    public async void RedeemVoucher()
    {
        string method = "mintNft";
        BigInteger amount = (BigInteger)(50*1e18);
        object[] args =
        {
            amount,
            voucher
        };
        var data = await Evm.ContractSend(Web3Accessor.Web3, method, ContractManager.NftAbi, ContractManager.NftContract, args);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
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