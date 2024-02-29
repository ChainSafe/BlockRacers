using Photon.Pun;
using TMPro;
using UnityEngine;

/// <summary>
/// Checkpoint manager for our car
/// </summary>
public class CheckPointManager : MonoBehaviour
{
    #region Fields

    // Checkpoint we cross
    private int checkPointCrossed;

    // Our car number
    private int carNumber;

    // Reference to the positioning script
    private PositioningSystem positioningSystem;

    #endregion

    #region Properties

    /// <summary>
    /// The current CheckPoint our player is at
    /// </summary>
    public int CheckPointCrossed
    {
        get => checkPointCrossed;
        set => checkPointCrossed = value;
    }

    /// <summary>
    /// Unique identifier for each car
    /// </summary>
    public int CarNumber
    {
        get => carNumber;
        set => carNumber = value;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Initialize needed objects
    /// </summary>
    private void Awake()
    {
        // finds the positioning system
        positioningSystem = FindObjectOfType<PositioningSystem>();
    }

    /// <summary>
    /// Increases the number of CheckPoints the car has crossed
    /// </summary>
    /// <param name="other">The checkpoint's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("CheckPoint" + carNumber)) return;
        if (!gameObject.GetComponent<PhotonView>().IsMine) return;
        checkPointCrossed += 1;
        positioningSystem.CarCollectedCheckPoint(carNumber, checkPointCrossed);
    }

    #endregion
}