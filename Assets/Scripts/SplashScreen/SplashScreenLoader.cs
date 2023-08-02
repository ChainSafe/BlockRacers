using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
/// Loads the splash screen
/// </summary>
public class SplashScreenLoader : MonoBehaviour
{
    #region Fields

    // video player renderer
    [SerializeField] private VideoPlayer videoPlayer;
    
    // The file name to play
    [SerializeField] private string videoFileName;

    #endregion

    #region Methods
    
    void Awake()
    {
        // skips to connect screen if on mobile as they can't play videos properly
        if (Application.isMobilePlatform)
        {
            SceneManager.LoadScene("MenuScene");
        }
        else
        {
            // play video
            Screen.fullScreen = !Screen.fullScreen;
            videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            videoPlayer.Play();
            videoPlayer.loopPointReached += CheckOver;
        }
    }
    
    // loads connect scene after video has finished playing
    void CheckOver(UnityEngine.Video.VideoPlayer videoPlayer)
    {
        // load this scene
        SceneManager.LoadScene("MenuScene");
    }

    #endregion
}
