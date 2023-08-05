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
    // Logic for the checkpoints and laps
    private static int checkpointCount;
    // Our checkpoint colliders
    private GameObject[] checkPoints;
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
        checkPoints[0] = GameObject.FindWithTag("Checkpoint1");
        checkPoints[1] = GameObject.FindWithTag("Checkpoint2");
        checkPoints[2] = GameObject.FindWithTag("Checkpoint3");
        playerController.LapCount = 1;
    }
    
    /// <summary>
    /// When we collide with an object
    /// </summary>
    /// <param name="other">The object we're colliding with</param>
    private void OnTriggerEnter(Collider other)
    {
        // If it's the local player
        if (other.CompareTag("Player"))
        {
            // If we collide with a checkpoint..
            if (gameObject == checkPoints[0] || gameObject == checkPoints[1] || gameObject == checkPoints[2])
            {
                // Show our split time
                TimerSystem.instance.ShowSplitTime();
                // Increase our checkpoint count
                checkpointCount++;
                // Disable the checkpoint so that players can't cheat
                gameObject.SetActive(false);
            }
            // If we collide with the finish line..
            if (gameObject.tag == "LapCollider")
            {
                // If we have all three checkpoints
                if (checkpointCount > 2)
                {
                    // Increase our lap count
                    playerController.LapCount++;
                    // Reset our timer
                    TimerSystem.instance.ResetTimer();
                    // Show our lap count on the UI
                    playerController.lapCountText.text = playerController.LapCount.ToString();
                    // Reset our checkpoint count
                    checkpointCount = 0;
                    // Re-enable all our checkpoints
                    foreach (GameObject checkpoint in checkPoints)
                    {
                        checkpoint.SetActive(true);
                    }
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
                // Otherwise if we missed a checkpoint, nothing happens
                else
                {
                    return;
                }
            }
        }
    }

    #endregion
}
