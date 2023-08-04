using Photon.Pun;
using Photon.Realtime;
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
    private string sceneName;
    [SerializeField] private GameObject[] cars;
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
        // Sets our scene name
        sceneName = SceneManager.GetActiveScene().name;
    }
    
    /// <summary>
    /// Sets vinyls based on chosen car
    /// </summary>
    private void Start()
    {
        // Instantiate chosen car if we're in the tutorial
        if (sceneName.ToString() == "Tutorial")
        {
            Instantiate(globalManager.playerCar);
        }
        else
        {
            // Instantiate our multiplayer prefab
            PhotonNetwork.Instantiate(cars[0].name, new Vector3(spawnPoints[0].transform.position.x, spawnPoints[0].transform.position.y, spawnPoints[0].transform.position.z), spawnPoints[0].transform.rotation, 0);
        }
        
        // Sanity check to see if the player has entered the garage or not
        // This breaks the vinyls from loading in
        //if (GarageMenu.instance == null) return;
        
        // If we're racing with the Camaro
        if (globalManager.playerCar == GarageMenu.instance.car1)
        {
            GameObject.Find("CarBody").GetComponent<MeshRenderer>().material = SwapCars.instance.camaroLivery[SwapCars.currentLiveryIndex];
        }
        // If we're racing with the ford GT
        if (globalManager.playerCar == GarageMenu.instance.car2)
        {
            GameObject.Find("CarBody").GetComponent<MeshRenderer>().material = SwapCars.instance.fordGTLivery[SwapCars.currentLiveryIndex];
        }
        // If we're racing with the ferrari
        if (globalManager.playerCar == GarageMenu.instance.car3)
        {
            GameObject.Find("CarBody").GetComponent<MeshRenderer>().material = SwapCars.instance.ferrariLivery[SwapCars.currentLiveryIndex];
        }
    }
    
    #endregion
}
