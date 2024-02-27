using System.Numerics;
using System.Text;
using ChainSafe.Gaming.UnityPackage;
using Photon.Pun;
using Scripts.EVM.Token;
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
        // Enables the claim button if we've won & sets the winning player text
        if (globalManager.raceWon)
        {
            winningPlayerText.text = "YOU WON";
            if (globalManager.wagering)
            {
                claimButton.SetActive(true);
            }
        }
        else
        {
            winningPlayerText.text = "YOU LOST";
        }
    }

    /// <summary>
    /// Sets our selected button to what we've moused over
    /// </summary>
    /// <param name="button"></param>
    public void OnMouseOverButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }

    public async void ClaimWinnings()
    {
        if (globalManager.wagering)
        {
            // TODO: Add ECDSA
            string method = "completeWager";
            // Additional function parameters
            BigInteger nonce = 1;
            byte[] opponentSig = Encoding.UTF8.GetBytes(globalManager.opponentSignature);
            object[] args =
            {
                nonce,
                globalManager.deadline,
                opponentSig
            };
            var data = await Evm.ContractSend(Web3Accessor.Web3, method, ContractManager.WagerAbi, ContractManager.WagerContract, args);
            var response = SampleOutputUtil.BuildOutputValue(data);
            Debug.Log($"TX: {response}");
            Debug.Log("Claiming Wager Winnings! Congratulations!");
            globalManager.wagerAmount = 0;
            MainMenuButton();
        }
        else
        {
            // TODO: Add ECDSA
            string method = "claimWinnings";
            BigInteger amount = (BigInteger)(50*1e18);
            object[] args =
            {
                amount
            };
            var data = await Evm.ContractSend(Web3Accessor.Web3, method, ContractManager.WagerAbi, ContractManager.WagerContract, args);
            var response = SampleOutputUtil.BuildOutputValue(data);
            Debug.Log($"TX: {response}");
            Debug.Log("Claiming Winnings! Congratulations!");
            MainMenuButton();
        }
    }

    /// <summary>
    /// Our main menu button, disconnects us from photon
    /// </summary>
    public void MainMenuButton()
    {
        // Resets our race won and wagering bools for the next match
        globalManager.raceWon = false;
        globalManager.wagering = false;
        globalManager.wagerAccepted = false;
        globalManager.sceneToLoad = "MenuScene";
        SceneManager.LoadScene("LoadingScreen");
    }

    #endregion
}