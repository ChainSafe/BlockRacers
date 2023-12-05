using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// An example of how to transfer tokens using Chainsafe's SDK
/// </summary>
public class TransferMenu : MonoBehaviour
{
    #region Fields

    // Audio
    private AudioManager audioManager;

    // Input field
    [SerializeField] private TMP_InputField inputField;

    // Address to send to
    private string walletAddress;

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
    // /// Transfers custom tokens to an address
    // /// </summary>
    // public async void Transfer()
    // {
    //     walletAddress = inputField.text;
    //     string amount = "1000000000000000";
    //     var data = await Erc20.TransferErc20(Web3Accessor.Web3, ContractManager.TokenContract, walletAddress, amount);
    //     var response = SampleOutputUtil.BuildOutputValue(data);
    //     Debug.Log($"TX: {response}");
    //     audioManager.Play("MenuSelect");
    // }

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