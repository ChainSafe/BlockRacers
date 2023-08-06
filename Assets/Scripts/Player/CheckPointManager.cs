using System;
using TMPro;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    #region Fields
    // DEBUG SERIALIZE--------------------
    [SerializeField] private int checkPointCrossed;
    private int carNumber;
    private int carPosition;
    private PositioningSystem positioningSystem;
    public TextMeshProUGUI positionText;

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
    
    /// <summary>
    /// Keeps track of our car position
    /// </summary>
    public int CarPosition
    {
        get => carPosition;
        set => carPosition = value;
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
        if (!other.gameObject.CompareTag("CheckPoint"+carNumber)) return;
        checkPointCrossed += 1;
        positioningSystem.CarCollectedCheckPoint(carNumber, checkPointCrossed);
    }

    #endregion
}
