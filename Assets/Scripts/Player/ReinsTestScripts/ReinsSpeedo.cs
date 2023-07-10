using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReinsSpeedo : MonoBehaviour
{
    public Slider speedSlider;           // The UI component
    public Image speedBar;               // The fill component
    public TextMeshProUGUI speedText;        // The actual speed metric
    public TextMeshProUGUI currentGear;

    public GameObject player;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        speedText.text = Mathf.Floor(playerController.speed).ToString();

        speedSlider.value = playerController.speed;

        // Changing our current gear based on speed 
        // I tried the whole RPM + Gear ratio system, but it's just too complex. 
        // This is real mickey mouse, but hey, it works - Input and change always welcome
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
