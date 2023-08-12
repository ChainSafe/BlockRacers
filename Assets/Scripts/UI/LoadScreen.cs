using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load screen functionality
/// </summary>
public class LoadScreen : MonoBehaviour
{
    #region Fields

    // Global manager
    private GlobalManager globalManager;

    // Audio
    private AudioManager audioManager;

    #endregion

    #region Methods

    /// <summary>
    /// Finds our global manager so we can set the scene to load later
    /// </summary>
    private void Awake()
    {
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
        // Finds our audio manager
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        if (audioManager.sounds[4].source.isPlaying)
        {
            audioManager.sounds[4].source.Pause();
        }

        if (audioManager.sounds[8].source.isPlaying)
        {
            audioManager.sounds[8].source.Pause();
        }

        StartCoroutine(LoadTime());
    }

    /// <summary>
    /// Load time for the loading screen
    /// </summary>
    /// <returns>The scene to load</returns>
    private IEnumerator LoadTime()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(globalManager.sceneToLoad);
    }
}

#endregion