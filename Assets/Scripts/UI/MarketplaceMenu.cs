using UnityEngine;

/// <summary>
/// Marketplace functionality
/// </summary>
public class MarketplaceMenu : MonoBehaviour
{
    #region Fields
    
    // Global manager
    private GlobalManager globalManager;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes objects
    /// </summary>
    private void Awake()
    {
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
    }

    #endregion
}