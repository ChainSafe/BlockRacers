using UnityEngine;
using TMPro;

/// <summary>
/// Manages the drift system also displays text & score
/// </summary>
public class DriftSystem : MonoBehaviour
{
    #region Fields

    public static DriftSystem instance;

    // Used for the UI
    public bool driftActive;

    // Our drift UI
    public GameObject driftStatus;
    public TextMeshProUGUI driftScoreText;

    public TextMeshProUGUI driftStatusText;

    // Adjust this value based on how fast you want the score to increase
    public int scoringRate = 1;

    // Adjust this value to set the minimum angle for a valid drift
    public float driftAngleThreshold = 30f;

    // Adjust this value to set the minimum angular velocity for a valid drift
    public float angularVelocityThreshold = 0.1f;

    // Our current drift score
    private int driftScore;

    // Our player controller
    [SerializeField] private PlayerController playerController;

    // Our player Rigidbody
    [SerializeField] private Rigidbody playerCar;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes our objects and fields
    /// </summary>
    private void Start()
    {
        // Singleton
        instance = this;
    }

    /// <summary>
    /// Updates the users drift score
    /// </summary>
    private void UpdateScore()
    {
        float carAngle = Vector3.Angle(playerCar.velocity, playerCar.transform.forward);
        float carAngularVelocity = playerCar.angularVelocity.magnitude;

        // Check to see if our car is sideways and sideways force is applied
        if (carAngle > driftAngleThreshold && carAngularVelocity > angularVelocityThreshold)
        {
            driftActive = true;
            driftScore += scoringRate;
            driftScoreText.text = driftScore.ToString();
        }
        else
        {
            driftActive = false;
            driftScore = 0;
            driftScoreText.text = "";
        }
    }

    /// <summary>
    /// Increments the users scoringRate based on how fast we're going
    /// </summary>
    private void UpdateScoreRate()
    {
        scoringRate = playerController.Speed switch
        {
            < 20 => 0,
            > 20 and < 80 => 1,
            > 80 and < 120 => 2,
            > 120 and < 180 => 3,
            > 180 => 4,
            _ => scoringRate
        };
    }

    /// <summary>
    /// Changes the user's drift status text based on how well they're doing
    /// </summary>
    private void UpdateDriftStatus()
    {
        // SetActive is used to trigger our animation everytime we drift
        if (driftActive && driftScore < 300)
        {
            driftStatusText.text = "good drift";
            driftStatus.SetActive(true);
        }

        if (driftActive && driftScore > 300 && driftScore < 500)
        {
            driftStatusText.text = "great drift";
        }

        if (driftActive && driftScore > 500 && driftScore < 750)
        {
            driftStatusText.text = "superb";
        }

        switch (driftActive)
        {
            case true when driftScore > 750:
                driftStatusText.text = "insane drift";
                break;
            case false:
                driftStatusText.text = "";
                driftStatus.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// Keeps the scripts functions running throughout the game
    /// </summary>
    private void Update()
    {
        // Update our score rate
        UpdateScoreRate();
        // Update our status text
        UpdateDriftStatus();
        // Update our drift score
        UpdateScore();
    }

    #endregion
}