using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads the finish screen once the race is over
/// </summary>
public class FinishScreen : MonoBehaviour
{
    #region Fields

    // Global Manager
    private GlobalManager globalManager;

    // Audio
    private AudioManager audioManager;

    // Buttons
    [SerializeField] private GameObject menuButton, claimButton;

    // Winning player text
    [SerializeField] private GameObject winningPlayerObj;
    [SerializeField] private TextMeshProUGUI winningPlayerText;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes needed objects, locks our cursor and changes BGM
    /// </summary>
    private void Awake()
    {
        // Stops scene sync
        PhotonNetwork.AutomaticallySyncScene = false;
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
        // Finds our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Changes Bgm
        audioManager.Pause("Bgm2");
        audioManager.Play("Bgm1");
        // Sets our first selected button
        EventSystem.current.SetSelectedGameObject(menuButton);
        PhotonNetwork.LeaveRoom();
        // Enables the claim button if we've won
        if (globalManager.raceWon && globalManager.wagering)
        {
            claimButton.SetActive(true);
        }
        // Sets the winning player text
        if (globalManager.raceWon)
        {
            winningPlayerObj.SetActive(true);
            winningPlayerText.text = "YOU WON";
        }
        // Resets our race won and wagering bools for the next match
        globalManager.raceWon = false;
        globalManager.wagering = false;
        globalManager.wagerAccepted = false;
    }

    /// <summary>
    /// Sets our selected button to what we've moused over
    /// </summary>
    /// <param name="button"></param>
    public void OnMouseOverButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }

    // public void ClaimWinnings()
    // {
    //     // Chain call to claim wager
    //     string ecdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
    //     string message = "secretmessage";
    //     var signature = Evm.EcdsaSignMessage(ecdsaKey, message);
    //     Debug.Log($"Signed Message: {signature}");
    //     string method = "claimPvpWager";
    //     string opponent = ""; // TO DO SET
    //     object[] args =
    //     {
    //         opponent,
    //         globalManager.wagerAmount,
    //         signature
    //     };
    //     var data = await Evm.ContractSend(Web3Accessor.Web3, method, ContractManager.ArrayAndTotalAbi, ContractManager.ArrayAndTotalContract, args);
    //     var response = SampleOutputUtil.BuildOutputValue(data);
    //     Debug.Log($"TX: {response}");
    //     Debug.Log("Claiming Winnings! Congratulations!");
    //     globalManager.wagerAmount = 0;
    // }

    /// <summary>
    /// Our main menu button, disconnects us from photon
    /// </summary>
    public void MainMenuButton()
    {
        globalManager.sceneToLoad = "MenuScene";
        SceneManager.LoadScene("LoadingScreen");
    }

    #endregion
}