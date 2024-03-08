using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Post processing manager to make things look aesthetically pleasing
/// </summary>
public class PostProcessing : MonoBehaviour
{
    #region Fields

    // Singleton
    public static PostProcessing Instance { get; private set; }

    // Referencing Effect attached to PP Profile
    public ChromaticAberration Nitrous;

    // Variables for our Nitrous function
    public float lerpSpeed = 1f;

    public float currentLerpValue = 0f;

    // Referencing Post Processing Volume attached to Camera
    private PostProcessVolume MainProfile;

    #endregion

    #region Methods

    /// <summary>
    /// Sets our instance and applies the post processing profile to our camera
    /// </summary>
    private void Awake()
    {
        // Singleton
        Instance = this;
        // Reference our camera object
        MainProfile = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PostProcessVolume>();
        // Grabbing the Post Processing profile settings
        MainProfile.profile.TryGetSettings(out Nitrous);
    }

    /// <summary>
    /// Checks if we're using NOS and applies camera shake accoringly
    /// </summary>
    private void Update()
    {
        // If NOS is being used
        if (Input.GetKeyDown(KeyCode.LeftShift) && CountDownSystem.raceStarted)
        {
            PlayerController.nosActive = true;
        }

        // If NOS isn't being used
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            PlayerController.nosActive = false;
        }

        // If we are using NOS and we have more than 0, then activate our effects
        if (PlayerController.nosActive && NitrousManager.currentBoost > 0 && CountDownSystem.raceStarted)
        {
            // Calculate the new lerp value based on time and speed
            currentLerpValue = Mathf.Clamp01(currentLerpValue + (Time.deltaTime * lerpSpeed));
            // Rumble the camera a little
            if (CameraShake.Instance != null)
            {
                CameraShake.Instance.TopRig.m_AmplitudeGain = 1f;
                CameraShake.Instance.MiddleRig.m_AmplitudeGain = 1f;
                CameraShake.Instance.BottomRig.m_AmplitudeGain = 1f;
            }
        }
        else
        {
            // Reset the lerp value when the button is not held
            currentLerpValue = Mathf.Clamp01(currentLerpValue - (Time.deltaTime * lerpSpeed));
            if (CameraShake.Instance == null) return;
            // Reset our camera rumble
            //CameraShake.Instance.TopRig.m_AmplitudeGain = 0f;
            //CameraShake.Instance.MiddleRig.m_AmplitudeGain = 0f;
            //CameraShake.Instance.BottomRig.m_AmplitudeGain = 0f;
        }

        // Adjust the range of the lerp value based on the desired intensity range
        float intensityStart = 0f; // Starting intensity value
        float intensityEnd = 2f; // Ending intensity value
        float intensity = Mathf.Lerp(intensityStart, intensityEnd, currentLerpValue);
        Nitrous.intensity.value = intensity;
    }

    #endregion
}