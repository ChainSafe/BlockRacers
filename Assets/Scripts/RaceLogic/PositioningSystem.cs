using Photon.Pun;
using UnityEngine;

/// <summary>
/// Manages our cars position for multiplayer races
/// </summary>
public class PositioningSystem : MonoBehaviourPun
{
    #region Fields

    public int totalCheckPoints;

    // WARNING -----------------------------------
    // change later to be photon multiplayer car prefabs, currently only works with camaro
    [SerializeField] private GameObject car1, car2, car3;
    [SerializeField] private GameObject checkPoint;
    [SerializeField] private GameObject checkPointHolder;
    [SerializeField] private GameObject[] cars;
    [SerializeField] private Transform[] checkPointPositions;
    [SerializeField] private GameObject[] checkPointForEachCarLap1;
    [SerializeField] private GameObject[] checkPointForEachCarLap2;
    [SerializeField] private GameObject[] checkPointForEachCarLap3;
    [SerializeField] private GameObject[] checkPointForEachCarLap4;
    private int totalCars;

    private int position;

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
            // Gives each checkpoint a proper name as it's instantiated for each car
            checkPointForEachCarLap1[i].name = $"CheckPoint{i}";
            // Assigns tags for each checkpoint
            checkPointForEachCarLap1[i].tag = checkPointForEachCarLap1[i].name;
            // Gives each checkpoint a proper name as it's instantiated for each car
            checkPointForEachCarLap2[i].name = $"CheckPoint{i}";
            // Assigns tags for each checkpoint
            checkPointForEachCarLap2[i].tag = checkPointForEachCarLap2[i].name;
            // Gives each checkpoint a proper name as it's instantiated for each car
            checkPointForEachCarLap3[i].name = $"CheckPoint{i}";
            // Assigns tags for each checkpoint
            checkPointForEachCarLap3[i].tag = checkPointForEachCarLap3[i].name;
            checkPointForEachCarLap2[i].SetActive(false);
            checkPointForEachCarLap3[i].SetActive(false);
            // Instantiate the lap 4 checkpoints to stop errors on race finish
            checkPointForEachCarLap4[i] = Instantiate(checkPoint, checkPointPositions[0].position,
                checkPointPositions[0].rotation);
            // Gives each checkpoint a proper name as it's instantiated for each car
            checkPointForEachCarLap4[i].name = $"CheckPoint{i}";
            // Assigns tags for each checkpoint
            checkPointForEachCarLap4[i].tag = checkPointForEachCarLap4[i].name;
            checkPointForEachCarLap4[i].SetActive(false);
        }
    }

    /// <summary>
    /// Deactivates previous lap checkpoints and enables the next
    /// </summary>
    public void ResetCheckPoints()
    {
        Debug.Log("Resetting checkpoints");
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
        else if (playerController.LapCount == 2)
        {
            // If we're on lap 3
            for (int i = 0; i < totalCars; i++)
            {
                checkPointForEachCarLap2[i].SetActive(false);
                checkPointForEachCarLap3[i].SetActive(true);
            }
        }
        else
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
            default:
                // Updates position of checkpoint
                checkPointForEachCarLap3[carNumber].transform.position =
                    checkPointPositions[checkPointNumber].transform.position;
                // Updates rotation of checkpoint
                checkPointForEachCarLap3[carNumber].transform.rotation =
                    checkPointPositions[checkPointNumber].transform.rotation;
                break;
        }

        // Compares car positions
        ComparePositions(carNumber);
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
            cars[i].GetComponent<CheckPointManager>().CarPosition =
                playerController.GetComponent<PhotonView>().OwnerActorNr;
            // Sets our cars number
            cars[i].GetComponent<CheckPointManager>().CarNumber =
                playerController.GetComponent<PhotonView>().OwnerActorNr - 1;
            // Updates our position text
            UpdatePositionText();
        }
    }

    /// <summary>
    /// Updates our position text
    /// </summary>
    private void UpdatePositionText()
    {
        //cars[playerController.GetComponent<PhotonView>().OwnerActorNr - 1].GetComponent<CheckPointManager>().positionText.text = $"POS  {cars[playerController.GetComponent<PhotonView>().OwnerActorNr - 1].GetComponent<CheckPointManager>().CarPosition}";
    }

    /// <summary>
    /// Finds our photon view component
    /// </summary>
    private void Update()
    {
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