using UnityEngine;
using UnityEngine.EventSystems;

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
    /// Reads a variable from a contract
    /// </summary>
    public void ReadContract()
    {
        if (audioManager == null) return;
        FindObjectOfType<AudioManager>().Play("MenuSelect");
    }
    
    /// <summary>
    /// Writes a variable to a contract
    /// </summary>
    public void WriteContract()
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
