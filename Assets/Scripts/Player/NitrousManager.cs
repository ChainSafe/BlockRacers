using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NitrousManager : MonoBehaviour
{

    public float maxBoost = 100f;        // Maximum boost value
    public float boostRate = 45f;        // Rate at which boost depletes per second
    public float rechargeRate = 5f;      // Rate at which boost recharges per second

    public static float currentBoost;    // Current boost value
    public Slider boostSlider;           // The UI component
    public Image BoostBar;               // The Fill Image for the slider
    public Image NOSIcon;                // The sprite representing the NOS 


    private void Awake()
    {
        // Ensure we start with full boost
        currentBoost = 100f;

        // What's the minimum value
        boostSlider.minValue = 0f;

        // What is the max value
        boostSlider.maxValue = maxBoost;
    }

    private void Update()
    {
        // Drain NOS when we use it
        if (PlayerController.nosActive && CountDownSystem.raceStarted)
        {
            currentBoost -= boostRate * Time.deltaTime;
            currentBoost = Mathf.Clamp(currentBoost, 0f, maxBoost);

            // Illuminate our NOS Bar
            BoostBar.color = Color.cyan;
            NOSIcon.color = Color.cyan;
        }
        // Recharge NOS when it's not in use
        else
        {
            currentBoost += rechargeRate * Time.deltaTime;
            currentBoost = Mathf.Clamp(currentBoost, 0f, maxBoost);

            // Make our bar slightly transparent again
            BoostBar.color = new Color(1, 1, 1, 0.4f);
            NOSIcon.color = new Color(1, 1, 1, 0.4f);
        }

        // Update the value of the UI Slider
        boostSlider.value = currentBoost;
    }
}
