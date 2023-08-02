using UnityEngine;

/// <summary>
/// Manages our car engine $ collision sounds
/// </summary>
public class RaceSoundManager : MonoBehaviour
{
    #region Fields

    // Audio objects
    [SerializeField] private AudioSource nosSound;
    [SerializeField] private AudioSource idleSound;
    [SerializeField] private AudioSource accelerateSound;
    [SerializeField] private AudioSource decelerateSound;
    [SerializeField] private AudioSource collisionSound;
    // Player controller so we can listen for changes
    [SerializeField] private PlayerController playerController;
    
    #endregion

    #region Methods
    
    /// <summary>
    /// Handles various sounds based on input and speed
    /// </summary>
    private void Update()
    {
        // Engine sounds
        idleSound.volume = Mathf.Lerp(0.25f, 0.25f, playerController.SpeedRatio);
        accelerateSound.volume = Mathf.Lerp(0.25f, 0.35f, playerController.SpeedRatio);
        accelerateSound.pitch = Mathf.Lerp(0.3f * playerController.CurrentGear / 2, 2, playerController.SpeedRatio);
        decelerateSound.volume = Mathf.Lerp(0.25f, 0.35f, playerController.SpeedRatio);
        decelerateSound.pitch = Mathf.Lerp(0.3f * playerController.CurrentGear, 2, playerController.SpeedRatio);
        
        // Nos sound based on input
        if (PlayerController.nosActive && CountDownSystem.raceStarted)
        {
            // Nos
            if (!nosSound.isPlaying)
            {
                nosSound.Play();
            }
        }
        // Engine sound based on input
        switch (playerController.input)
        {
            // Idle
            case 0 when (playerController.Speed == 0):
            {
                if (!idleSound.isPlaying)
                {
                    idleSound.Play();
                }

                break;
            }
            // Accelerating
            case > 0:
            {
                if (!accelerateSound.isPlaying)
                {
                    decelerateSound.Pause();
                    accelerateSound.Play();
                }

                break;
            }
            default:
            {
                // Decelerating
                if (!decelerateSound.isPlaying)
                {
                    accelerateSound.Pause();
                    decelerateSound.Play();
                }

                break;
            }
        }
        // Collision sound based on collisions
        if (!playerController.collision) return;
        playerController.collision = false;
        collisionSound.Pause();
        collisionSound.Play();
    }
    
    #endregion
}
