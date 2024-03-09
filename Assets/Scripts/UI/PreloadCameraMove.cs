using UnityEngine;

public class PreloadCameraMove : MonoBehaviour
{
    public GameObject menu, mainCamera;
    public Transform objectToMove;
    public float targetX = -170;
    public float duration = 3f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float startTime;

    void Start()
    {
        startPosition = objectToMove.position;
        targetPosition = new Vector3(targetX, startPosition.y, startPosition.z);
        startTime = Time.time;
    }

    void Update()
    {
        float timeSinceStarted = Time.time - startTime;
        float percentageComplete = timeSinceStarted / duration;

        objectToMove.position = Vector3.Lerp(startPosition, targetPosition, percentageComplete);

        if (percentageComplete >= 1.0f)
        {
            mainCamera.SetActive(true);
            menu.SetActive(true);
            Destroy(gameObject);
        }
    }
}
