using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreenLoader : MonoBehaviour
{
    // video player renderer
    [SerializeField]
    private VideoPlayer videoPlayer;
    // the file name to play
    [SerializeField]
    private string videoFileName;

    void Awake()
    {
        // skips to connect screen if on mobile as they can't play videos properly
        if (Application.isMobilePlatform)
        {
            SceneManager.LoadScene("Connect");
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
        SceneManager.LoadScene("Connect");
    }
}