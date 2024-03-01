using System.Collections;
using UnityEngine;

public class DestroyAfter1 : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSecondsRealtime(2);
        Destroy(gameObject);
    }
}