using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerSystem : MonoBehaviour
{
    // Timer logic
    private float elapsedTime = 0f;
    private bool isTimerRunning = false;

    // UI Elements
    public TextMeshProUGUI LapTimeText;
    public TextMeshProUGUI splitTimeText;

    // Singleton
    public static TimerSystem instance;

    private void Awake()
    {
        instance = this;
    }

    // Start the timer
    public void StartTimer()
    {
        isTimerRunning = true;
        elapsedTime = 0f;
    }

    // Stop the timer
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    // Reset the timer
    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    // Show our split time
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

    // Hide our split time
    public void HideSplitTime()
    {
        splitTimeText.text = "";
    }


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
}
