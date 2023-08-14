using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

/// <summary>
/// Manages the lap system, used when a user completes a lap
/// </summary>
public class LapSystem : MonoBehaviourPunCallbacks
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
        // If we have passed all check points & this is our photon view
        if (!playerController.GetComponent<PhotonView>().IsMine) return;
        if (playerController.GetComponent<CheckPointManager>().CheckPointCrossed !=
            FindObjectOfType<PositioningSystem>().totalCheckPoints - 1) return;
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
        switch (playerController.LapCount)
        {
            // Let the player know it's their final lap
            case 3:
                playerController.finalLapReminder.SetActive(true);
                break;
            // When we've completed all three laps
            case 4:
            {
                // If we're first enable the global bool for claims
                if (raceOver) return;
                if (globalManager.wagering)
                {
                    globalManager.raceWon = true;
                }
                // Sends RPC to other users
                playerController.GetComponent<PhotonView>().RPC("RaceOver", RpcTarget.All);
                break;
            }
        }
    }

    #endregion
}