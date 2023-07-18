using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCars : MonoBehaviour
{
    // Array of prefabs to swap between
    public GameObject[] prefabs;

    // Index of the currently active prefab
    private int currentPrefabIndex = 0;

    // Reference to the currently instantiated prefab
    private GameObject currentPrefab;

    // Set the spawnpoint 
    private Vector3 spawnPoint = new Vector3(89.17f, 0.0f, -9.7f);

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the initial prefab
        currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation, transform);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for input to swap the prefabs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Destroy the current prefab
            Destroy(currentPrefab);

            // Increment the index to switch to the next prefab
            currentPrefabIndex++;
            if (currentPrefabIndex >= prefabs.Length)
            {
                currentPrefabIndex = 0;
            }

            // Instantiate the next prefab in the array
            currentPrefab = Instantiate(prefabs[currentPrefabIndex], spawnPoint, transform.rotation, transform);
        }
    }
}
