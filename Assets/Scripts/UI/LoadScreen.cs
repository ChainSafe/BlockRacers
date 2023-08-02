using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load screen functionality
/// </summary>
public class LoadScreen : MonoBehaviour
{
    #region Fields
    
    private GlobalManager globalManager;

    #endregion

    #region Methods
    
    void Awake()
    {
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
    }

    private void Start()
    {
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
