using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public string sceneToLoad;
    private void Awake()
    {
        // Makes object global and doesnt destroy it when changing scenes
        DontDestroyOnLoad(this);
    }
}
