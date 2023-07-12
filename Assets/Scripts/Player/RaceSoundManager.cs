using UnityEngine;

public class RaceSoundManager : MonoBehaviour
{
    // Audio objects
    [SerializeField] private AudioSource nosSound;
    [SerializeField] private AudioSource idleSound;
    [SerializeField] private AudioSource accelerateSound;
    [SerializeField] private AudioSource decelerateSound;
    [SerializeField] private AudioSource collisionSound;
    
    // Player controller so we can listen for changes
    [SerializeField] private PlayerController playerController;
    
    void Update()
    {
        // Engine sounds
        idleSound.volume = Mathf.Lerp(0.25f, 0.25f, playerController.speedRatio);
        accelerateSound.volume = Mathf.Lerp(0.25f, 0.35f, playerController.speedRatio);
        accelerateSound.pitch = Mathf.Lerp(0.3f * playerController.currentGear / 2, 2, playerController.speedRatio);
        decelerateSound.volume = Mathf.Lerp(0.25f, 0.35f, playerController.speedRatio);
        decelerateSound.pitch = Mathf.Lerp(0.3f * playerController.currentGear, 2, playerController.speedRatio);
        
        // Sounds
        if (PlayerController.nosActive && CountDownSystem.raceStarted)
        {
            // Nos
            if (!nosSound.isPlaying)
            {
                nosSound.Play();
            }
        }

        switch (playerController.input)
        {
            // Idle
            case 0 when (playerController.speed == 0):
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
        
        // Collisions
        if (!playerController.collision) return;
        playerController.collision = false;
        collisionSound.Pause();
        collisionSound.Play();
    }
}
