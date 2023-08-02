using UnityEngine;

/// <summary>
/// Handles the UI for the garage
/// </summary>
public class GarageUI : MonoBehaviour
{
    #region Fields
    
    public GameObject MenuItems;
    public GameObject ShowroomUI;
    public GameObject UpgradeMenu;

    #endregion

    #region Methods

    /// <summary>
    /// Exits the showroom and goes back to menu
    /// </summary>
    public void BackToMenu()
    {
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
        // Rotate our camera back
        CameraController.instance.RotateCamera(-95f, 0.5f);
        // Changes menu displays
        MenuItems.SetActive(true);
        ShowroomUI.SetActive(false);
    }

    /// <summary>
    /// Opens the car upgrade menu for the player
    /// </summary>
    public void OpenUpgrades()
    {
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
        // Changes menu displays
        UpgradeMenu.SetActive(true);
        ShowroomUI.SetActive(false);
    }

    /// <summary>
    /// Closes the car upgrade menu for the player
    /// </summary>
    public void CloseUpgrades()
    {
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
        // Changes menu displays
        UpgradeMenu.SetActive(false);
        ShowroomUI.SetActive(true);
    }
    
    /// <summary>
    /// Takes the user to the showroom
    /// </summary>
    private void ToShowRoom()
    {
        // Play our menu select audio
        GarageMenu.instance.PlayMenuSelect();
        // Rotate our camera to the showroom cars
        CameraController.instance.RotateCamera(95f, 0.5f);
        // Changes menu displays
        MenuItems.SetActive(false);
        ShowroomUI.SetActive(true);
    }
    
    #endregion
}
