using UnityEngine;

/// <summary>
/// Manages spawning our players when the race starts
/// </summary>
public class SpawnPlayer : MonoBehaviour
{
    #region Fields
    
    // Global manager
    private GlobalManager globalManager;

    private void Awake()
    {
        // Find our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
    }
    
    #endregion

    #region Methods
    
    private void Start()
    {
        // Instantiate chosen car
        Instantiate(globalManager.playerCar);
        
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
