using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the lap system, used when a user completes a lap
/// </summary>
public class LapSystem : MonoBehaviour
{
    #region Fields
    
    // Our UI
    public TextMeshProUGUI lapCountText;
    // Our checkpoint colliders
    public GameObject[] checkPoints;
    // Our final lap reminder
    public GameObject LapReminder;
    // Logic for the checkpoints and laps
    [SerializeField] private static int checkpointCount;
    // Lap count
    [SerializeField] private int lapCount;
    // Global Manager
    private GlobalManager globalManager;

    #endregion

    #region Methods
    
    private void Awake()
    {
        // Find our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        lapCount = 1;
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
            if (this.gameObject.tag == "Checkpoint")
            {
                // Show our split time
                TimerSystem.instance.ShowSplitTime();
                // Increase our checkpoint count
                checkpointCount++;
                // Disable the checkpoint so that players can't cheat
                this.gameObject.SetActive(false);
            }
            // If we collide with the finish line..
            if (this.gameObject.tag == "LapCollider")
            {
                // If we have all three checkpoints
                if (checkpointCount > 2)
                {
                    // Increase our lap count
                    lapCount++;
                    // Reset our timer
                    TimerSystem.instance.ResetTimer();
                    // Show our lap count on the UI
                    lapCountText.text = lapCount.ToString();
                    // Reset our checkpoint count
                    checkpointCount = 0;
                    // Re-enable all our checkpoints
                    foreach (GameObject checkpoint in checkPoints)
                    {
                        checkpoint.SetActive(true);
                    }
                    // Let the player know it's their final lap..
                    if (lapCount > 2)
                    {
                        LapReminder.SetActive(true);
                    }
                    // When we've completed all three laps..
                    if (lapCount > 3)
                    {
                        // Race over logic goes here
                        // Probably some kind of UI, but I can't put it here just yet because MP logic is still required.
                        // Taking us back to the menu just as a temporary game loop
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
