using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;
using UnityEngine.UI;

public class Tacometer : MonoBehaviour
{
    public GameObject player;
    public Text speedText;
    private PlayerController playerController;
    private Transform needleTransform;
    private const float ZERO_SPEED_ANGLE = 136;
    private const float MAX_SPEED_ANGLE = -136;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        needleTransform = transform.Find("Needle");
    }
    
    private float GetSpeedRotation()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;
        float speedNormalized = playerController.speed / playerController.maxSpeed;
        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }

    private void Update()
    {
        if (playerController.speed > playerController.maxSpeed) playerController.speed = playerController.maxSpeed;
        if (playerController.speed < 0) playerController.speed = 0;
        needleTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
        speedText.text = Mathf.Floor(playerController.speed).ToString();
    }
}
