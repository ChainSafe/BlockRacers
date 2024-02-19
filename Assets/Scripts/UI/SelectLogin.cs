using UnityEngine;
using UnityEngine.EventSystems;

public class SelectLogin : MonoBehaviour
{
    [SerializeField] private GameObject MetaMaskButton, QRButton, w3AObject, menuObject;
    private AudioManager audioManager;
    /// <summary>
    /// Sets metamask login to false if not webgl
    /// </summary>
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            MetaMaskButton.SetActive(true);
            QRButton.SetActive(false);
        }
    }
    
    /// <summary>
    /// Opens the web3auth menu
    /// </summary>
    public void Web3AuthButton()
    {
        menuObject.SetActive(false);
        w3AObject.SetActive(true);
        audioManager.Play("MenuSelect");
    }
    
    /// <summary>
    /// Closes the web3auth menu
    /// </summary>
    public void ExitWeb3AuthMenu()
    {
        w3AObject.SetActive(false);
        menuObject.SetActive(true);
        audioManager.Play("MenuSelect");
    }
    
    /// <summary>
    /// Sets our selected button to what we've moused over
    /// </summary>
    /// <param name="button">The button being moused over</param>
    public void OnMouseOverButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }
}
