using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Custom call SDK functions
/// </summary>
public class CustomCallMenu : MonoBehaviour
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
    /// Reads a variable from a contract
    /// </summary>
    public async void ReadContract()
    {
        string method = "myTotal";
        object[] args =
        {
            await Web3Accessor.Web3.Signer.GetAddress()
        };
        var data = await Evm.ContractCall(Web3Accessor.Web3, method, ContractManager.ArrayAndTotalAbi, ContractManager.ArrayAndTotalContract, args);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"Output: {response}");
        audioManager.Play("MenuSelect");
    }
    
    /// <summary>
    /// Writes a variable to a contract
    /// </summary>
    public async void WriteContract()
    {
        string method = "addTotal";
        int increaseAmount = 1;
        object[] args =
        {
            increaseAmount
        };
        var data = await Evm.ContractSend(Web3Accessor.Web3, method, ContractManager.ArrayAndTotalAbi, ContractManager.ArrayAndTotalContract, args);
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