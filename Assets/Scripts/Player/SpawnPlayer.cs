using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages spawning our players when the race starts
/// </summary>
public class SpawnPlayer : MonoBehaviour
{
    #region Fields

    // Global manager
    private GlobalManager globalManager;

    // Spawn points for each player
    [SerializeField] private GameObject[] spawnPoints;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes needed objects
    /// </summary>
    private void Awake()
    {
        // Find our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
    }

    /// <summary>
    /// Sets vinyls based on chosen car
    /// </summary>
    private void Start()
    {
        // Instantiate chosen car if we're in the tutorial
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            Instantiate(globalManager.playerCar);
        }
        else
        {
            // Instantiate our multiplayer prefab
            PhotonNetwork.Instantiate(globalManager.playerCar.name,
                new Vector3(spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position.x,
                    spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position.y,
                    spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position.z),
                spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.rotation, 0);
        }
        
        if(SwapCars.instance == null || globalManager.playerCar == null) return;

        // If we're racing with the Camaro
        if (globalManager.playerCar == SwapCars.instance.car1)
        {
            GameObject.Find("CarBody").GetComponent<MeshRenderer>().material =
                SwapCars.instance.camaroLivery[SwapCars.currentLiveryIndex];
        }

        // If we're racing with the ford GT
        if (globalManager.playerCar == SwapCars.instance.car2)
        {
            GameObject.Find("CarBody").GetComponent<MeshRenderer>().material =
                SwapCars.instance.fordGTLivery[SwapCars.currentLiveryIndex];
        }

        // If we're racing with the ferrari
        if (globalManager.playerCar == SwapCars.instance.car3)
        {
            GameObject.Find("CarBody").GetComponent<MeshRenderer>().material =
                SwapCars.instance.ferrariLivery[SwapCars.currentLiveryIndex];
        }
    }

    #endregion
}