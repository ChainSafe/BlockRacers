using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIConnectMenu : MonoBehaviour
{
    private void OnEnable()
    {
        // Gets a reference to the ui document so we can access things like buttons
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        // Button references so we can use them
        Button ConnectWalletButton = root.Q<Button>("ConnectWalletButton");
        
        // Button actions
        ConnectWalletButton.clicked += () =>
        {
            FindObjectOfType<AudioManager>().Play("MenuSelect");
            SceneManager.LoadScene("MainMenu");
        };
    }
}
