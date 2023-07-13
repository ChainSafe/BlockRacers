using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tachometer : MonoBehaviour
{
    // UI component
    [SerializeField] private Slider speedSlider;
    
    // Fill component
    [SerializeField] private Image speedBar;
    
    // The actual speed metric
    [SerializeField] private TextMeshProUGUI speedText;
    
    // Current gear
    [SerializeField] private TextMeshProUGUI currentGear;
    
    // Player objects
    [SerializeField] private PlayerController playerController;

    // Smoothing variables
    [SerializeField] private float targetSpeed;
    [SerializeField] private const float LerpSpeed = 5f;

    private void Update()
    {
        // Smoothly update the target speed for our speedo
        targetSpeed = Mathf.Lerp(targetSpeed, playerController.speed, Time.deltaTime * LerpSpeed);

        // Update speed text & slider with the smoothed speed value
        speedText.text = Mathf.Floor(targetSpeed).ToString();
        speedSlider.value = targetSpeed;

        playerController.currentGear = playerController.speed switch
        {
            // Changing our current gear based on speed 
            < 40 => 1,
            > 40 and < 80 => 2,
            > 80 and < 140 => 3,
            > 140 and < 190 => 4,
            > 190 and < 240 => 5,
            > 240 and < 280 => 6,
            _ => playerController.currentGear
        };
        currentGear.text = playerController.currentGear.ToString();
    }
}
