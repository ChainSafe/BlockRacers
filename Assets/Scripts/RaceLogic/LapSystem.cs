using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

/// <summary>
/// Manages the lap system, used when a user completes a lap
/// </summary>
public class LapSystem : MonoBehaviourPun
{
    #region Fields

    // Placement for race
    private int placement;

    // Global manager
    private GlobalManager globalManager;

    // Player controller
    [SerializeField] private PlayerController playerController;

    // Race over bool
    public bool raceOver;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes our objects and fields
    /// </summary>
    private void Awake()
    {
        // Find our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        playerController.LapCount = 1;
    }

    /// <summary>
    /// Initializes when we collide with the lap collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;
        if (other.CompareTag("LapCollider"))
        {
            LapComplete();
        }
    }

    /// <summary>
    /// When the player completes a lap after hitting all checkpoints
    /// </summary>
    public void LapComplete()
    {
        if (!playerController.GetComponent<PhotonView>().IsMine) return;
        // If we have passed all check points
        if (playerController.GetComponent<CheckPointManager>().CheckPointCrossed ==
            FindObjectOfType<PositioningSystem>().totalCheckPoints)
        {
            // Show our split time
            TimerSystem.instance.ShowSplitTime();
            // Increase our lap count
            playerController.LapCount++;
            // Reset our timer
            TimerSystem.instance.ResetTimer();
            // reset checkpoints
            FindObjectOfType<PositioningSystem>().ResetCheckPoints();
            // Show our lap count on the UI
            playerController.lapCountText.text = playerController.LapCount.ToString();
            // Let the player know it's their final lap
            if (playerController.LapCount > 2)
            {
                playerController.finalLapReminder.SetActive(true);
            }
            // When we've completed all three laps
            else if (playerController.LapCount > 3)
            {
                // If we're first enable the global bool for claims
                if (raceOver) return;
                if (globalManager.wagering)
                {
                    globalManager.raceWon = true;
                }
                // Sends RPC to other users
                photonView.RPC("RaceOver", RpcTarget.All,
                    playerController.GetComponent<PhotonView>().Owner.NickName);
            }
        }
    }

    /// <summary>
    /// Lets the players know the race is over
    /// </summary>
    [PunRPC]
    private void RaceOver(string userName)
    {
        raceOver = true;
        globalManager.winningPlayer = userName;
        playerController.RaceEnding();
        Invoke(nameof(RaceEndingTimer), 3);
    }

    /// <summary>
    /// Displays race ending text and moves scenes after 3 seconds
    /// </summary>
    private void RaceEndingTimer()
    {
        // Race over logic
        globalManager.sceneToLoad = "FinishRace";
        SceneManager.LoadScene("LoadingScreen");
    }

    #endregion
}