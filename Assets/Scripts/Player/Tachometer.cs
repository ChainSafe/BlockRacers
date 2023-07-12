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

    private void Update()
    {
        // Speed text & slider
        speedText.text = Mathf.Floor(playerController.speed).ToString();
        speedSlider.value = playerController.speed;

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
