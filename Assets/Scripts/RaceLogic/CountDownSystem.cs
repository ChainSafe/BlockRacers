using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the countdown system when races are started
/// </summary>
public class CountDownSystem : MonoBehaviour
{
    #region Fields

    // Has the race started? - Used for player input
    public static bool raceStarted;

    // Our UI for the countdown circles
    [SerializeField] private GameObject[] countDownSprites;

    // How long is the countdown
    private float countdownDuration = 3f;

    // Audio
    private AudioManager audioManager;

    #endregion

    #region Methods

    private void Start()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();

        // If we chose to race, we initiate the countdown
        if (PlayerController.isRacing)
        {
            // Prevent player input before the race starts
            raceStarted = false;
            // Start our countdown
            StartCoroutine(StartCountdown());
        }
        else
        {
            // Otherwise if this is the tutorial, we allow users to drive immediately.
            raceStarted = true;
        }
    }

    private IEnumerator StartCountdown()
    {
        // Starts the countdown and displays each change
        audioManager.sounds[10].source.Play();
        countDownSprites[0].SetActive(true);
        yield return new WaitForSeconds(1f);
        countDownSprites[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        countDownSprites[2].SetActive(true);
        yield return new WaitForSeconds(1f);
        // Turn our lights green
        foreach (GameObject sprite in countDownSprites)
        {
            sprite.GetComponent<Image>().color = Color.green;
        }

        // Start the race and enable player input here
        raceStarted = true;
        // Start our timer
        TimerSystem.instance.StartTimer();
        yield return new WaitForSeconds(1f);
        // Remove our lights UI 
        foreach (GameObject sprite in countDownSprites)
        {
            sprite.SetActive(false);
        }
    }

    #endregion
}