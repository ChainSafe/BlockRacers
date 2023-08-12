using Photon.Pun;
using UnityEngine;

/// <summary>
/// Manages our cars position for multiplayer races
/// </summary>
public class PositioningSystem : MonoBehaviourPun
{
    #region Fields

    // Total amount of checkpoints
    public int totalCheckPoints;

    // Amount of cars in the race
    private int totalCars;

    [SerializeField] private GameObject car1, car2, car3;
    [SerializeField] private GameObject checkPoint;
    [SerializeField] private GameObject checkPointHolder;
    [SerializeField] private GameObject[] cars;
    [SerializeField] private Transform[] checkPointPositions;
    [SerializeField] private GameObject[] checkPointForEachCarLap1;
    [SerializeField] private GameObject[] checkPointForEachCarLap2;
    [SerializeField] private GameObject[] checkPointForEachCarLap3;
    [SerializeField] private GameObject[] checkPointForEachCarLap4;

    // Debug
    [SerializeField] private PlayerController playerController;

    #endregion

    #region Methods

    /// <summary>
    /// Initialize objects
    /// </summary>
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        // Sets array length by amount of players in room
        cars = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount];
        // Add cars to array
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i] = car1;
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
    private void SetCheckPoints()
    {
        // Initialize the array
        checkPointPositions = new Transform[totalCheckPoints];
        // Loop to get our checkpoints from the holders children
        for (int i = 0; i < totalCheckPoints; i++)
        {
            checkPointPositions[i] = checkPointHolder.transform.GetChild(i).transform;
        }

        // Initialize check point for each car array
        checkPointForEachCarLap1 = new GameObject[totalCars];
        checkPointForEachCarLap2 = new GameObject[totalCars];
        checkPointForEachCarLap3 = new GameObject[totalCars];
        checkPointForEachCarLap4 = new GameObject[totalCars];
        for (int i = 0; i < totalCars; i++)
        {
            // Instantiate the initial check point for each car
            checkPointForEachCarLap1[i] = Instantiate(checkPoint, checkPointPositions[0].position,
                checkPointPositions[0].rotation);
            // Instantiate the initial check point for each car
            checkPointForEachCarLap2[i] = Instantiate(checkPoint, checkPointPositions[0].position,
                checkPointPositions[0].rotation);
            // Instantiate the initial check point for each car
            checkPointForEachCarLap3[i] = Instantiate(checkPoint, checkPointPositions[0].position,
                checkPointPositions[0].rotation);
            // Instantiate the lap 4 checkpoints to stop errors on race finish
            checkPointForEachCarLap4[i] = Instantiate(checkPoint, checkPointPositions[0].position,
                checkPointPositions[0].rotation);
            // Gives each checkpoint a proper name as it's instantiated for each car
            checkPointForEachCarLap1[i].name = $"CheckPoint{i}";
            // Gives each checkpoint a proper name as it's instantiated for each car
            checkPointForEachCarLap2[i].name = $"CheckPoint{i}";
            // Gives each checkpoint a proper name as it's instantiated for each car
            checkPointForEachCarLap3[i].name = $"CheckPoint{i}";
            // Gives each checkpoint a proper name as it's instantiated for each car
            checkPointForEachCarLap4[i].name = $"CheckPoint{i}";
            // Assigns tags for each checkpoint
            checkPointForEachCarLap1[i].tag = checkPointForEachCarLap1[i].name;
            // Assigns tags for each checkpoint
            checkPointForEachCarLap2[i].tag = checkPointForEachCarLap2[i].name;
            // Assigns tags for each checkpoint
            checkPointForEachCarLap3[i].tag = checkPointForEachCarLap3[i].name;
            // Assigns tags for each checkpoint
            checkPointForEachCarLap4[i].tag = checkPointForEachCarLap4[i].name;
            checkPointForEachCarLap2[i].SetActive(false);
            checkPointForEachCarLap3[i].SetActive(false);
            checkPointForEachCarLap4[i].SetActive(false);
        }
    }

    /// <summary>
    /// Deactivates previous lap checkpoints and enables the next
    /// </summary>
    public void ResetCheckPoints()
    {
        // Resets our checkpoint counter
        playerController.GetComponent<CheckPointManager>().CheckPointCrossed = 0;
        // If we're on lap 2
        if (playerController.LapCount == 2)
        {
            for (int i = 0; i < totalCars; i++)
            {
                checkPointForEachCarLap1[i].SetActive(false);
                checkPointForEachCarLap2[i].SetActive(true);
            }
        }
        else if (playerController.LapCount == 3)
        {
            // If we're on lap 3
            for (int i = 0; i < totalCars; i++)
            {
                checkPointForEachCarLap2[i].SetActive(false);
                checkPointForEachCarLap3[i].SetActive(true);
            }
        }
        else if (playerController.LapCount == 4)
        {
            // If we've finished instantiate the last checkpoint to stop errors
            for (int i = 0; i < totalCars; i++)
            {
                checkPointForEachCarLap3[i].SetActive(false);
                checkPointForEachCarLap4[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// Updates our checkpoints as they are collected based on lap
    /// </summary>
    /// <param name="carNumber">Unique identifier for each car</param>
    /// <param name="checkPointNumber">The checkpoint number we're passing through</param>
    public void CarCollectedCheckPoint(int carNumber, int checkPointNumber)
    {
        switch (playerController.LapCount)
        {
            case 1:
                // Updates position of checkpoint
                checkPointForEachCarLap1[carNumber].transform.position =
                    checkPointPositions[checkPointNumber].transform.position;
                // Updates rotation of checkpoint
                checkPointForEachCarLap1[carNumber].transform.rotation =
                    checkPointPositions[checkPointNumber].transform.rotation;
                break;
            case 2:
                // Updates position of checkpoint
                checkPointForEachCarLap2[carNumber].transform.position =
                    checkPointPositions[checkPointNumber].transform.position;
                // Updates rotation of checkpoint
                checkPointForEachCarLap2[carNumber].transform.rotation =
                    checkPointPositions[checkPointNumber].transform.rotation;
                break;
            case 3:
                // Updates position of checkpoint
                checkPointForEachCarLap3[carNumber].transform.position =
                    checkPointPositions[checkPointNumber].transform.position;
                // Updates rotation of checkpoint
                checkPointForEachCarLap3[carNumber].transform.rotation =
                    checkPointPositions[checkPointNumber].transform.rotation;
                break;
            default:
                // Updates position of checkpoint
                checkPointForEachCarLap4[carNumber].transform.position =
                    checkPointPositions[checkPointNumber].transform.position;
                // Updates rotation of checkpoint
                checkPointForEachCarLap4[carNumber].transform.rotation =
                    checkPointPositions[checkPointNumber].transform.rotation;
                break;
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
            // Sets our cars number
            cars[i].GetComponent<CheckPointManager>().CarNumber =
                playerController.GetComponent<PhotonView>().OwnerActorNr -1;
        }
    }

    /// <summary>
    /// Finds our photon view component
    /// </summary>
    private void Update()
    {
        if (!PhotonNetwork.IsConnected) return;
        if (playerController.GetComponent<PhotonView>().IsMine) return;
        Debug.Log("Finding our Player!");
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (playerController.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("Player found!");
        }
    }

    #endregion
}