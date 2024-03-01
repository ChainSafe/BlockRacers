using UnityEngine;

public class RotateLootbox : MonoBehaviour
{

    public float offset;

    void Update()
    {
        transform.Rotate(0, 0, (120 + offset) * Time.deltaTime);
    }
}