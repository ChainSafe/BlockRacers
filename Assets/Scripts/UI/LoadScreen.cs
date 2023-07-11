using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{
    private GlobalManager globalManager;

    void Awake()
    {
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
    }

    private void Start()
    {
        StartCoroutine(LoadTime());
    }

    private IEnumerator LoadTime()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(globalManager.sceneToLoad);
    }
}
