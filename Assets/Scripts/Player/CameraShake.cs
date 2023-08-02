using UnityEngine;
using Cinemachine;

/// <summary>
/// Shakes the camera for dramatic effect
/// </summary>
public class CameraShake : MonoBehaviour
{
    #region Fields

    // Singleton
    public static CameraShake Instance;

    // Reference to your Cinemachine FreeLook camera
    public CinemachineFreeLook freeLookCamera;

    // Reference to CinemachineBasicMultiChannelPerlin component
    public CinemachineBasicMultiChannelPerlin TopRig;
    public CinemachineBasicMultiChannelPerlin MiddleRig;
    public CinemachineBasicMultiChannelPerlin BottomRig;

    #endregion

    #region Methods
    
    public void Start()
    {
        Instance = this;

        // Gets the CinemachineBasicMultiChannelPerlin component from the FreeLook camera
        TopRig = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        MiddleRig = freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        BottomRig = freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // Assign the CinemachineBasicMultiChannelPerlin component to the appropriate channel
        freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(0f, 0f, 0f);
    }

    // You can access the noise (shake) by using the following:
    // CameraShake.Instance.{RigNameHere}.m_AmplitudeGain = {IntensityFloat};
    
    #endregion
}
