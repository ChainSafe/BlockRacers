using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class GarageUI : MonoBehaviour
{
    public GameObject MenuItems;
    public GameObject ShowroomUI;

    // Go to the showroom
    public void ToShowRoom()
    {
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();

        // Rotate our camera to the showroom cars
        CameraController.instance.RotateCamera(95f, 0.5f);

        MenuItems.SetActive(false);

        ShowroomUI.SetActive(true);
    }

    // When we exit the showroom
    public void BackToMenu()
    {
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();

        // Rotate our camera back
        CameraController.instance.RotateCamera(-95f, 0.5f);

        MenuItems.SetActive(true);

        ShowroomUI.SetActive(false);
    }
}
