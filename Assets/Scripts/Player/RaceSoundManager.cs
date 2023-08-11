using System;
using UnityEngine;

/// <summary>
/// Manages our car engine $ collision sounds
/// </summary>
public class RaceSoundManager : MonoBehaviour
{
    #region Fields

    // Audio
    private AudioManager audioManager;

    // Player controller so we can listen for changes
    [SerializeField] private PlayerController playerController;
    private bool driftEnded;

    #endregion

    #region Methods

    private void Awake()
    {
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
    }

    /// <summary>
    /// Handles various sounds based on input and speed
    /// </summary>
    private void Update()
    {
        // Engine sounds
        // 3 nos, 4 idle, 5 acc, 6 dec, 7 collision, 8 drift counter, 9 drift end
        audioManager.sounds[5].volume = Mathf.Lerp(0.25f, 0.35f, playerController.SpeedRatio);
        audioManager.sounds[5].pitch =
            Mathf.Lerp(0.3f * playerController.CurrentGear / 2, 2, playerController.SpeedRatio);
        audioManager.sounds[6].volume = Mathf.Lerp(0.25f, 0.35f, playerController.SpeedRatio);
        audioManager.sounds[6].pitch = Mathf.Lerp(0.3f * playerController.CurrentGear, 2, playerController.SpeedRatio);

        // Nos sound based on input
        if (PlayerController.nosActive && CountDownSystem.raceStarted)
        {
            // Nos
            if (!audioManager.sounds[3].source.isPlaying)
            {
                audioManager.sounds[3].source.Play();
            }
        }

        // Engine sound based on input
        switch (playerController.Input)
        {
            // Idle
            case 0 when (playerController.Speed == 0 || playerController.Input == 0):
            {
                if (!audioManager.sounds[4].source.isPlaying)
                {
                    audioManager.sounds[4].source.Play();
                }

                break;
            }
            // Accelerating
            case > 0:
            {
                if (!audioManager.sounds[5].source.isPlaying)
                {
                    audioManager.sounds[6].source.Pause();
                    audioManager.sounds[5].source.Play();
                }

                break;
            }
            default:
            {
                // Decelerating
                if (!audioManager.sounds[6].source.isPlaying)
                {
                    audioManager.sounds[5].source.Pause();
                    audioManager.sounds[6].source.Play();
                }

                break;
            }
        }

        // Drift sounds (reversed for some reason)
        if (!DriftSystem.instance.driftActive)
        {
            audioManager.sounds[8].source.Play();
        }
        else
        {
            driftEnded = true;
        }

        if (driftEnded)
        {
            driftEnded = false;
            audioManager.sounds[9].source.Play();
        }

        // Collision sound based on collisions
        if (!playerController.collision) return;
        playerController.collision = false;
        audioManager.sounds[7].source.Pause();
        audioManager.sounds[7].source.Play();
    }

    #endregion
}