using UnityEngine;
using TMPro;
using Photon.Pun;

/// <summary>
/// The timer system used in conjunction with the lap system
/// </summary>
public class TimerSystem : MonoBehaviourPun
{
    #region Fields
    
    // Singleton
    public static TimerSystem instance;
    // UI Elements
    public TextMeshProUGUI LapTimeText;
    public TextMeshProUGUI splitTimeText;
    // Timer logic
    private float elapsedTime = 0f;
    private bool isTimerRunning = false;

    #endregion

    #region Methods
    
    /// <summary>
    /// Initializes our instance
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Starts the timer
    /// </summary>
    public void StartTimer()
    {
        isTimerRunning = true;
        elapsedTime = 0f;
    }

    /// <summary>
    /// Stops the timer
    /// </summary>
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    /// <summary>
    /// Resets the timer
    /// </summary>
    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    /// <summary>
    /// Shows the user's split time
    /// </summary>
    public void ShowSplitTime()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);
        string timeString = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        splitTimeText.text = timeString;
        Invoke(nameof(HideSplitTime), 5f);
    }

    /// <summary>
    /// Hides the user's split time
    /// </summary>
    public void HideSplitTime()
    {
        splitTimeText.text = "";
    }

    /// <summary>
    /// Checks if the start timer is running and displays it
    /// </summary>
    private void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);
            string timeString = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            LapTimeText.text = timeString;
        }
    }

    #endregion
}
