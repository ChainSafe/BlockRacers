using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class DriftSystem : MonoBehaviour
{
    // Our current drift score
    private int driftScore = 0;

    // Adjust this value based on how fast you want the score to increase
    public int scoringRate = 1;

    // Adjust this value to set the minimum angle for a valid drift
    public float driftAngleThreshold = 30f;

    // Adjust this value to set the minimum angular velocity for a valid drift
    public float angularVelocityThreshold = 0.1f;

    // Our player Rigidbody
    private Rigidbody playerCar;

    // Used for the UI
    private bool driftActive;

    // Our audiosource
    public AudioSource[] driftUISounds;
    private bool activateDriftSound;

    // Our drift UI
    public GameObject driftStatus;
    public TextMeshProUGUI driftScoreText;
    public TextMeshProUGUI driftStatusText;

    // Singleton for access to drifting system
    public static DriftSystem instance;

    private void Start()
    {
        // Find our car object
        playerCar = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        // Singleton
        instance = this;

        // For some reason, the drift audio doesn't work when I drift, but only when I don't, so I swapped logic, and for some reason now it works
        // when I'm drifting, by saying it should play when I'm not drifting, it's strange and this probably needs to be checked.
        activateDriftSound = false;
        Invoke(nameof(EnableDriftAudio), 3f);
    }

    private void Update()
    {
        // Update our score rate
        UpdateScoreRate();

        // Update our status text
        UpdateDriftStatus();

        // Update our drift score
        UpdateScore();
    }

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
            if (activateDriftSound)
            {
                driftUISounds[0].Play();
            }
            driftActive = false;
            driftScore = 0;
            driftScoreText.text = "";
        }
    }




    public void UpdateScoreRate()
    {
        // Increment our scoringRate based on how fast we're going
        if (PlayerController.instance.speed < 20)
        {
            scoringRate = 0;
        }
        if (PlayerController.instance.speed > 20 && PlayerController.instance.speed < 80)
        {
            scoringRate = 1;
        }
        if (PlayerController.instance.speed > 80 && PlayerController.instance.speed < 120)
        {
            scoringRate = 2;
        }
        if (PlayerController.instance.speed > 120 && PlayerController.instance.speed < 180)
        {
            scoringRate = 3;
        }
        if (PlayerController.instance.speed > 180)
        {
            scoringRate = 4;
        }
    }

    public void UpdateDriftStatus()
    {
        // Change our drift status text based on how well we're doing
        // SetActive is used to trigger our animation everytime we drift
        if (driftActive && driftScore < 4000)
        {
            driftUISounds[1].Play();
            driftStatusText.text = "good drift";
            driftStatus.SetActive(true);

        }
        if (driftActive && driftScore > 4000 && driftScore < 8000)
        {
            driftStatusText.text = "great drift";
        }
        if (driftActive && driftScore > 12000 && driftScore < 20000)
        {
            driftStatusText.text = "superb";
        }
        if (driftActive && driftScore > 20000)
        {
            driftStatusText.text = "insane drift";
        }
        else if (!driftActive)
        {
            driftStatusText.text = "";
            driftStatus.SetActive(false);
        }
    }

    public void EnableDriftAudio()
    {
        activateDriftSound = true;
    }
}
