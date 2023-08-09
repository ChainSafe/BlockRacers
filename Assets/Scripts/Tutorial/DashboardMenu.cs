using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Allows the user to go to the Chainsafe dashboard
/// </summary>
public class DashboardMenu : MonoBehaviour
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
        // Unlocks the cursor so the user can select things
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
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
    /// Opens the Chainsafe gaming dashboard
    /// </summary>
    public void OpenDashboard()
    {
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }

    /// <summary>
    /// Closes the menu and gives input control back to the user
    /// </summary>
    public void CloseMenu()
    {
        // Locks the cursor so the user can resume playing normally
        Cursor.lockState =  CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    #endregion
}