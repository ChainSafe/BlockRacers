using System;
using ChainSafe.Gaming.UnityPackage;
using Scripts.EVM.Token;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// An example of how Chainsafe's SDK verify functionality works
/// </summary>
public class VerifyMenu : MonoBehaviour
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
    /// Verifies user owner ship of a wallet address
    /// </summary>
    public async void Verify()
    {
        try
        {
            string message = "A man chooses, a slave obeys.";
            var data = await Evm.SignVerify(Web3Accessor.Web3, message);
            var response = data ? "Verified" : "Failed to verify";
            Debug.Log($"Verified: {response}");
            audioManager.Play("MenuSelect");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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