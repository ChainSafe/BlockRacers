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
    }
}
