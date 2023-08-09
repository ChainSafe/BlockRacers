using UnityEngine;

/// <summary>
/// Rotates the showroom platform to make the car display aesthetically pleasing
/// </summary>
public class RotationScript : MonoBehaviour
{
    #region Fields
    
    // The speed at which the platform rotates
    private float rotationSpeed = 10f;
    
    #endregion

    #region Methods

    /// <summary>
    /// Rotates the platform around the Y-axis based on the rotation speed
    /// </summary>
    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    
    #endregion
}
