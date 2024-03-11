using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
/// Loads the splash screen
/// </summary>
public class SplashScreenLoader : MonoBehaviour
{
    #region Fields

    // Video player renderer
    [SerializeField] private VideoPlayer videoPlayer;

    // The file name to play
    [SerializeField] private string videoFileName;

    #endregion

    #region Methods

    /// <summary>
    /// Skips to connect screen if on mobile as they can't play videos properly
    /// </summary>
    private void Awake()
    {
        if (Application.isMobilePlatform)
        {
            //Screen.orientation = ScreenOrientation.LandscapeLeft;
            SceneManager.LoadScene("ConnectWallet");
        }
        else
        {
            // play video
            // Sets fullscreen (disabled as webgl has a permission warning)
            //Screen.fullScreen = !Screen.fullScreen;
            videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            videoPlayer.Play();
            videoPlayer.loopPointReached += CheckOver;
        }
    }

    /// <summary>
    /// Loads connect scene after video has finished playing
    /// </summary>
    /// <param name="videoPlayer">The video renderer being used to play the video</param>
    private void CheckOver(VideoPlayer videoPlayer)
    {
        // load this scene
        SceneManager.LoadScene("ConnectWallet");
    }

    #endregion
}