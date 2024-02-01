using UnityEngine;

public class SelectLogin : MonoBehaviour
{
    [SerializeField] private GameObject MetaMaskButton, QRButton;
    /// <summary>
    /// Sets metamask login to false if not webgl
    /// </summary>
    private void Awake()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            MetaMaskButton.SetActive(true);
            QRButton.SetActive(false);
        }
    }
}
