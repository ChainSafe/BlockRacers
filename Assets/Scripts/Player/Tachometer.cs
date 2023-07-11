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
            playerController.currentGear = 1;
            currentGear.text = playerController.currentGear.ToString();
        }
        if (playerController.speed > 40 && playerController.speed < 80)
        {
            playerController.currentGear = 2;
            currentGear.text = playerController.currentGear.ToString();
        }
        if (playerController.speed > 80 && playerController.speed < 140)
        {
            playerController.currentGear = 3;
            currentGear.text = playerController.currentGear.ToString();
        }
        if (playerController.speed > 140 && playerController.speed < 190)
        {
            playerController.currentGear = 4;
            currentGear.text = playerController.currentGear.ToString();
        }
        if (playerController.speed > 190 && playerController.speed < 240)
        {
            playerController.currentGear = 5;
            currentGear.text = playerController.currentGear.ToString();
        }
        if (playerController.speed > 240 && playerController.speed < 280)
        {
            playerController.currentGear = 6;
            currentGear.text = playerController.currentGear.ToString();
        }
    }


}
