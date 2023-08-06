using Photon.Pun;
using TMPro;
using UnityEngine;

public class PositioningSystem : MonoBehaviour
{
    #region Fields
    
    // WARNING -----------------------------------
    // change later to be photon multiplayer car prefabs, currently only works with camaro
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject checkPoint;
    [SerializeField] private GameObject checkPointHolder;
    [SerializeField] private GameObject[] cars;
    [SerializeField] private Transform[] checkPointPositions;
    [SerializeField] private GameObject[] checkPointForEachCar;
    private int totalCars;
    private int totalCheckPoints;
    private int position;

    #endregion

    #region Methods
    
    /// <summary>
    /// Initialize objects
    /// </summary>
    void Start()
    {
        // Sets array length by amount of players in room
        cars = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount];
        // Add cars to array
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i] = carPrefab;
        }
        // Number of total cars
        totalCars = cars.Length;
        // Number of total checkPoints
        totalCheckPoints = checkPointHolder.transform.childCount;
        // Sets our checkpoints
        SetCheckPoints();
        // Sets our positions
        SetCarPositions();
    }
    
    /// <summary>
    /// Set our CheckPoints
    /// </summary>
    public void SetCheckPoints()
    {
        // Initialize the array
        checkPointPositions = new Transform[totalCheckPoints];
        // Loop to get our checkpoints from the holders children
        for (int i = 0; i < totalCheckPoints; i++)
        {
            checkPointPositions[i] = checkPointHolder.transform.GetChild(i).transform;
        }
        // Initialize check point for each car array
        checkPointForEachCar = new GameObject[totalCars];
        for (int i = 0; i < totalCars; i++)
        {
            // Instantiate the initial check point for each car
            checkPointForEachCar[i] = Instantiate(checkPoint, checkPointPositions[0].position, checkPointPositions[0].rotation);
            // Gives each checkpoint a proper name as it's instantiated for each car
            checkPointForEachCar[i].name = $"CheckPoint{i}";
            // Assigns tags for each checkpoint
            checkPointForEachCar[i].tag = checkPointForEachCar[i].name;
        }
    }
    
    /// <summary>
    /// Updates our checkpoints as they are collected
    /// </summary>
    /// <param name="carNumber">Unique identifier for each car</param>
    /// <param name="checkPointNumber">The checkpoint number we're passing through</param>
    public void CarCollectedCheckPoint(int carNumber, int checkPointNumber)
    {
        // Updates position of checkpoint
        checkPointForEachCar[carNumber].transform.position = checkPointPositions[checkPointNumber].transform.position;
        // Updates rotation of checkpoint
        checkPointForEachCar[carNumber].transform.rotation = checkPointPositions[checkPointNumber].transform.rotation;
        ComparePositions(carNumber);
        // Show our split time
        TimerSystem.instance.ShowSplitTime();
    }
    
    /// <summary>
    /// Compares our car positions
    /// </summary>
    /// <param name="carNumber"></param>
    private void ComparePositions(int carNumber)
    {
        // Checks if we're first or not, returns if first as we don't need to calculate
        if (cars[carNumber].GetComponent<CheckPointManager>().CarPosition == 1) return;
        // Finds our car
        GameObject currentCar = cars[carNumber];
        // Checks our cars position
        int currentCarPos = currentCar.GetComponent<CheckPointManager>().CarPosition;
        //  Checks our cars checkpoints
        int currentCarCheckPoint = currentCar.GetComponent<CheckPointManager>().CheckPointCrossed;
        // Initialize the local variables car in front
        GameObject carInFront = null;
        int carInFrontPos = 0;
        int carInFrontCheckPoint = 0;
        // loop through our cars to set our position
        for (int i = 0; i < totalCars; i++)
        {
            // the car in front
            if (cars[i].GetComponent<CheckPointManager>().CarPosition == currentCarPos - 1)
            {
                // Finds the car
                carInFront = cars[i];
                // Checks the cars position
                carInFrontPos = currentCar.GetComponent<CheckPointManager>().CarPosition;
                //  Checks the cars checkpoints
                carInFrontCheckPoint = currentCar.GetComponent<CheckPointManager>().CheckPointCrossed;
                break;
            }
            // Updates our position if we have more checkpoints
            if (currentCarCheckPoint > carInFrontCheckPoint)
            {
                currentCar.GetComponent<CheckPointManager>().CarPosition = currentCarPos - 1;
                carInFront.GetComponent<CheckPointManager>().CarPosition = carInFrontPos + 1;
            }
            // Updates our position text
            UpdatePositionText();
        }
    }
    
    /// <summary>
    /// Sets ours cars number and position
    /// </summary>
    private void SetCarPositions()
    {
        // Loops through the cars
        for (int i = 0; i < totalCars; i++)
        {
            // Sets our cars position
            cars[i].GetComponent<CheckPointManager>().CarPosition = i + 1;
            // Sets our cars number
            cars[i].GetComponent<CheckPointManager>().CarNumber = i + 1;
        }
        // Updates our position text
        UpdatePositionText();
    }
    
    /// <summary>
    /// Updates our position text
    /// </summary>
    private void UpdatePositionText()
    {
        cars[0].GetComponent<CheckPointManager>().positionText.text = $"POS  {cars[0].GetComponent<CheckPointManager>().CarPosition}";
    }

    #endregion
}
