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
    [SerializeField] private GameObject player;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Speed text & slider
        speedText.text = Mathf.Floor(playerController.speed).ToString();
        speedSlider.value = playerController.speed;

        // Changing our current gear based on speed 
        if (playerController.speed < 40)
        {
            currentGear.text = "1";
        }
        if (playerController.speed > 40 && playerController.speed < 80)
        {
            currentGear.text = "2";
        }
        if (playerController.speed > 80 && playerController.speed < 140)
        {
            currentGear.text = "3";
        }
        if (playerController.speed > 140 && playerController.speed < 190)
        {
            currentGear.text = "4";
        }
        if (playerController.speed > 190 && playerController.speed < 240)
        {
            currentGear.text = "5";
        }
        if (playerController.speed > 240 && playerController.speed < 280)
        {
            currentGear.text = "6";
        }
    }


}
