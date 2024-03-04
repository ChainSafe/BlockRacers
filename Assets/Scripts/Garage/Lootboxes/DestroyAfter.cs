using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    // Destroys object after 2 seconds
    void Start()
    {
        Destroy(gameObject, 2);
    }
}