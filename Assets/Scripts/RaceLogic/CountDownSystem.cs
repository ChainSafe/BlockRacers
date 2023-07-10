using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownSystem : MonoBehaviour
{

    public GameObject[] countDownSprites;

    public float countdownDuration = 3f;

    public static bool raceStarted;

    private void Start()
    {
        if (PlayerController.isRacing)
        {
            // Prevent player input before the race starts
            raceStarted = false;

            // Start our countdown
            StartCoroutine(StartCountdown());
        }
        else
        {
            raceStarted = true;
        }

    }

    private IEnumerator StartCountdown()
    {
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
}
