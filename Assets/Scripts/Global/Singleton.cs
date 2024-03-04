using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    
    public static T Instance => _instance;
        
    protected abstract bool NotDestroyOnLoad { get; }
        
    protected void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
                
            Debug.LogError("More than one Singleton Instance of " + _instance.name);
        }
        else
        {
            _instance = (T) this;
            if (NotDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }

        Init();
    }

    protected virtual void Init()
    {
            
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}