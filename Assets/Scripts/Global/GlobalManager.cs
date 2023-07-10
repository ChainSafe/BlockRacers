using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    private void Awake()
    {
        // Makes object global and doesnt destroy it when changing scenes
        DontDestroyOnLoad(this);
    }
}
