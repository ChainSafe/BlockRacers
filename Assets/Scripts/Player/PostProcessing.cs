using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
    public static PostProcessing Instance { get; private set; }

    // Referencing Post Processing Volume attached to Camera
    private PostProcessVolume MainProfile;

    // Referencing Effect attached to PP Profile
    public ChromaticAberration Nitrous;

    // Variables for our Nitrous function
    public float lerpSpeed = 1f;
    public float currentLerpValue = 0f;


    private void Start()
    {
        // Singleton
        Instance = this;

        // Reference our camera object
        MainProfile = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PostProcessVolume>();

        // Grabbing the Post Processing profile settings
        MainProfile.profile.TryGetSettings(out Nitrous);
    }


    private void Update()
    {

        // Are we using NOS?
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayerController.nosActive = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            PlayerController.nosActive = false;
        }

        // If we are and we have more than 0, then activate our effects
        if (PlayerController.nosActive && NitrousManager.currentBoost > 0)
        {
            // Calculate the new lerp value based on time and speed
            currentLerpValue = Mathf.Clamp01(currentLerpValue + (Time.deltaTime * lerpSpeed));

            // Rumble the camera a little
            CameraShake.Instance.TopRig.m_AmplitudeGain = 1f;
            CameraShake.Instance.MiddleRig.m_AmplitudeGain = 1f;
            CameraShake.Instance.BottomRig.m_AmplitudeGain = 1f;
        }
        else
        {
            // Reset the lerp value when the button is not held
            currentLerpValue = Mathf.Clamp01(currentLerpValue - (Time.deltaTime * lerpSpeed));

            // Reset our camera rumble
            CameraShake.Instance.TopRig.m_AmplitudeGain = 0f;
            CameraShake.Instance.MiddleRig.m_AmplitudeGain = 0f;
            CameraShake.Instance.BottomRig.m_AmplitudeGain = 0f;
        }

        // Adjust the range of the lerp value based on the desired intensity range
        float intensityStart = 0f; // Starting intensity value
        float intensityEnd = 2f; // Ending intensity value
        float intensity = Mathf.Lerp(intensityStart, intensityEnd, currentLerpValue);
        Nitrous.intensity.value = intensity;
    }



}
