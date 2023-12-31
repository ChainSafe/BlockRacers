using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages NOS display
/// </summary>
public class NitrousManager : MonoBehaviour
{
    #region Fields

    // Rate at which boost depletes per second
    public static float boostRate = 20f;

    // Current boost value
    public static float currentBoost;

    // Maximum boost value
    [SerializeField] private float maxBoost = 100f;

    // Rate at which boost recharges per second
    [SerializeField] private float rechargeRate = 5f;

    // The UI component
    [SerializeField] private Slider boostSlider;

    // The Fill Image for the slider
    [SerializeField] private Image BoostBar;

    // The sprite representing the NOS
    [SerializeField] private Image NOSIcon;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes NOS values
    /// </summary>
    private void Awake()
    {
        // Ensure we start with full boost
        currentBoost = 100f;
        // What's the minimum value
        boostSlider.minValue = 0f;
        // What is the max value
        boostSlider.maxValue = maxBoost;
    }

    /// <summary>
    /// Manages NOS and displays it accordingly
    /// </summary>
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
            // Illuminate our NOS Bar
            BoostBar.color = Color.cyan;
            NOSIcon.color = Color.cyan;
            currentBoost += rechargeRate * Time.deltaTime;
            currentBoost = Mathf.Clamp(currentBoost, 0f, maxBoost);
        }

        // Update the value of the UI Slider
        boostSlider.value = currentBoost;
    }

    #endregion
}