using UnityEngine;

public class SelectLogin : MonoBehaviour
{
    /// <summary>
    /// Sets metamask login to false if not webgl
    /// </summary>
    private void Awake()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            gameObject.SetActive(false);
        }
    }
}
