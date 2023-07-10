using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        // Gets a reference to the ui document so we can access things like buttons
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Button references so we can use them
        Button RaceButton = root.Q<Button>("RaceButton");
        Button FreeRoamButton = root.Q<Button>("FreeRoamButton");
        Button GarageButton = root.Q<Button>("GarageButton");
        Button QuitButton = root.Q<Button>("QuitButton");

        // Button actions
        RaceButton.clicked += () =>
        {
            FindObjectOfType<AudioManager>().Play("MenuSelect");
            SceneManager.LoadScene("Track1");
        };
        FreeRoamButton.clicked += () =>
        {
            FindObjectOfType<AudioManager>().Play("MenuSelect");
            SceneManager.LoadScene("FreeRoam");
            CountDownSystem.raceStarted = true; ;
        };
        GarageButton.clicked += () =>
        {
            FindObjectOfType<AudioManager>().Play("MenuSelect");
            SceneManager.LoadScene("Garage");
        };
        QuitButton.clicked += () =>
        {
            FindObjectOfType<AudioManager>().Play("MenuSelect");
            Application.Quit();
        };
    }

    // Reins test button. This needs to be deleted after reviewing the 3D Menu
    public void StartGame()
    {
        SceneManager.LoadScene("RaceTrack");
        PlayerController.isRacing = true;
    }
}
