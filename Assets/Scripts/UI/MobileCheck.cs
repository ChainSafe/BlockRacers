using UnityEngine;

public class MobileCheck : MonoBehaviour
{
    // mobile script
    void Awake()
    {
        if (!Application.isMobilePlatform)
        {
            gameObject.SetActive(false);
        }
    }
}