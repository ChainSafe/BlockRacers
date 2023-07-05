using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIGarage : MonoBehaviour
{
    private void OnEnable()
    {
        // Gets a reference to the ui document so we can access things like buttons
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        // Button references so we can use them
        Button BackButton = root.Q<Button>("BackButton");

        // Button actions
        BackButton.clicked += () =>
        {
            FindObjectOfType<AudioManager>().Play("MenuSelect");
            SceneManager.LoadScene("MainMenu");
        };
    }
}