using UnityEngine;

/// <summary>
/// Controls the users camera
/// </summary>
public class CameraController : MonoBehaviour
{
    #region Fields
    
    // Singleton
    public static CameraController instance;    
    // Reference to the camera you want to rotate
    public Camera cameraToRotate;
    // Desired rotation for the camera
    public Vector3 desiredRotation;
    // The duration of the rotation in seconds
    public float rotationDuration;
    // Initial rotation of the camera
    private Quaternion initialRotation;
    // Target rotation of the camera
    private Quaternion targetRotation;
    // Timer for tracking the rotation progress
    private float rotationTimer;
    // Flag to check if rotation is in progress
    private bool isRotating;

    #endregion

    #region Methods
    
    /// <summary>
    ///  Initializes objects, sets our instance and rotation of camera
    /// </summary>
    private void Start()
    {
        instance = this;
        initialRotation = Quaternion.Euler(0f, -45f, 0f);
        targetRotation = Quaternion.Euler(desiredRotation);
        cameraToRotate.transform.rotation = initialRotation;
        isRotating = false;
    }

    /// <summary>
    /// Rotate the camera with specified rotation amount and duration
    /// </summary>
    /// <param name="rotationAmount">The amount to rotate the camera by</param>
    /// <param name="duration">The duration of the rotation</param>
    public void RotateCamera(float rotationAmount, float duration)
    {
        if (isRotating) return;
        isRotating = true;
        initialRotation = cameraToRotate.transform.rotation;
        targetRotation = Quaternion.Euler(cameraToRotate.transform.eulerAngles + new Vector3(0f, rotationAmount, 0f));
        rotationTimer = 0f;
        rotationDuration = duration;
    }
    
    /// <summary>
    /// Checks if the camera is rotating and adjusts accordingly
    /// </summary>
    private void Update()
    {
        if (!isRotating) return;
        rotationTimer += Time.deltaTime;
        float t = Mathf.Clamp01(rotationTimer / rotationDuration);
        cameraToRotate.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
        if (rotationTimer >= rotationDuration)
        {
            isRotating = false;
        }
    }
    
    #endregion
}
