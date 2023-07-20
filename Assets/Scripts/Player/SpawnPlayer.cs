using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    // Global manager
    private GlobalManager globalManager;

    private void Awake()
    {
        // Find our global manager
        globalManager = GameObject.FindWithTag("GlobalManager").GetComponent<GlobalManager>();
    }

    private void Start()
    {
        // Instantiate chosen car
        Instantiate(globalManager.playerCar);

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
}
