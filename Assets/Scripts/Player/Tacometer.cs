using UnityEngine;

public class Tacometer : MonoBehaviour
{
    public float acceleration = 0f;
    private Transform needleTransform;
    private const float ZERO_SPEED_ANGLE = 136;
    private const float MAX_SPEED_ANGLE = -136;
    private float speedMax;
    private float speed;

    private void Awake()
    {
        needleTransform = transform.Find("Needle");
        speed = 0f;
        speedMax = 200f;
    }
    
    private float GetSpeedRotation()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;
        float speedNormalized = speed / speedMax;
        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }

    private void Update()
    {
        speed += 30f * Time.deltaTime;
        if (speed > speedMax) speed = speedMax;
        needleTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }
}
