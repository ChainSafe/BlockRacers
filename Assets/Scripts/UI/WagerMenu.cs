using Photon.Pun;
using TMPro;
using UnityEngine;

public class WagerMenu : MonoBehaviourPunCallbacks
{
    #region Fields
    
    // Global Manager
    private GlobalManager globalManager;
    // Wager config
    [SerializeField] private TMP_InputField wagerInput;
    private int wagerAmount;
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// Initializes needed objects
    /// </summary>
    void Awake()
    {
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
    }
    
    /// <summary>
    /// Sets our wager amount
    /// </summary>
    public void SetWager()
    {
        Debug.Log("Setting Wager!");
        if (int.Parse(wagerInput.text) > 100)
        {
            wagerAmount = 100;
        }
        else
        {
            wagerAmount = int.Parse(wagerInput.text);
        }
        // Chain call here to set wager
        Debug.Log($"Wager set at: {wagerAmount}");
        // Change this later to check both ends have accepted
        globalManager.wagerAccepted = true;
    }
    
    #endregion
}
