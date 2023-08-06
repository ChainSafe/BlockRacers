using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the lap system, used when a user completes a lap
/// </summary>
public class LapSystem : MonoBehaviour
{
    #region Fields
    
    // Placement for race
    private int placement;
    // Global manager
    private GlobalManager globalManager;
    // Player controller
    [SerializeField] private PlayerController playerController;

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
    /// When the player completes a lap after hitting all checkpoints
    /// </summary>
    public void LapComplete()
    {
        // If we have passed all check points
        if (playerController.GetComponent<CheckPointManager>().CheckPointCrossed == FindObjectOfType<PositioningSystem>().totalCheckPoints)
        {
            // Show our split time
            TimerSystem.instance.ShowSplitTime();
            // Increase our lap count
            playerController.LapCount++;
            // Reset our timer
            TimerSystem.instance.ResetTimer();
            // Show our lap count on the UI
            playerController.lapCountText.text = playerController.LapCount.ToString();
            // Let the player know it's their final lap..
            if (playerController.LapCount > 2)
            {
                playerController.finalLapReminder.SetActive(true);
            }
            // When we've completed all three laps..
            if (playerController.LapCount > 3)
            {
                // If we're first enable the global bool for claims
                if (placement == 1 && globalManager.wagering)
                {
                    globalManager.raceWon = true;
                }
                // Race over logic
                globalManager.sceneToLoad = "FinishRace";
                SceneManager.LoadScene("LoadingScreen");

            }
        }
    }

    #endregion
}
