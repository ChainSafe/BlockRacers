using UnityEngine;

/// <summary>
/// Opens a chosen menu for the tutorial
/// </summary>
public class OpenMenu : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject menuToOpen;

    #endregion

    #region Methods

    /// <summary>
    /// Opens a menu on collision for the user to learn about the SDK
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        menuToOpen.SetActive(true);
    }

    #endregion
}