using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationAnimal : MonoBehaviour
{

    [Header("Simulator Settings")]
    public QueueAnimalManager queueManager;
    public bool simulationRunning = false; // Flag to indicate if the game is running

    [Header("Time Settings")]
    public float currentTime = 0f; // Current elapsed time within the interval
    public float totalTime = 1440f; // The total duration of the interval in seconds : 1440f = 60 x 24 = 1h * 24 
    public float speed = 1f; 
    public float minSpawnDelay = 5f; // Minimum delay before spawning an object
    public float maxSpawnDelay = 30f; // Maximum delay before spawning an object
    private float nextSpawnTime = 0f;

    [Header("Chances")]
    //Priorities
    public int group1Chance = 23; // 30% chance for Group 1
    public int group2Chance = 22; // 20% chance for Group 2
    public int group3Chance = 21; // 25% chance for Group 3
    public int group4Chance = 34; // 25% chance for Group 4


    [Header("Spawns")]
    public int objectsSpawned = 0;
    public int objectTotalCount = 130; // Number of objects to spawn within the interval
    public int group1Spawn = 0; // 30% chance for Group 1
    public int group2Spawn = 0; // 20% chance for Group 2
    public int group3Spawn = 0; // 25% chance for Group 3
    public int group4Spawn = 0; // 25% chance for Group 4

    public Material red;
    public Material yellow;
    public Material blue;
    public Material green;

    // Start is called before the first frame update
    void Start()
    {
        StartSimulation();

    }

    public void StartSimulation()
    {
        simulationRunning = true;
        currentTime = 0f;
        // Additional code to initialize your game or reset any necessary variables
    }

    // Update is called once per frame
    void Update()
    {
        if (simulationRunning)
        {
            if (currentTime >= totalTime)
            {
                simulationRunning = false;
            }
        }

        if (currentTime < totalTime && objectsSpawned < objectTotalCount && simulationRunning)
        {
            currentTime += Time.deltaTime * speed;
            if (currentTime >= nextSpawnTime && ShouldSpawnObject())
            {
                CalculateNextSpawnTime();
                objectsSpawned++;
            }
        }

    }


    void CalculateNextSpawnTime()
    {
        nextSpawnTime = currentTime + Random.Range(minSpawnDelay, maxSpawnDelay);
    }
    bool ShouldSpawnObject()
    {
        int randomValue = Random.Range(1, 101); // Generate a random value between 1 and 100

        if (randomValue <= group1Chance && objectsSpawned < 25) // 30% chance for Group 1, spawn 25 objects
        {
            queueManager.SpawnAPatient(1, GetTotalTime(1), red);
            group1Spawn++;
            return true;
        }
        else if (randomValue <= group1Chance + group2Chance && objectsSpawned < 50) // 20% chance for Group 2, spawn 25 objects
        {
            queueManager.SpawnAPatient(2, GetTotalTime(2), yellow);
            group2Spawn++;
            return true;
        }
        else if (randomValue <= group1Chance + group2Chance + group3Chance && objectsSpawned < 75) // 25% chance for Group 3, spawn 25 objects
        {
            queueManager.SpawnAPatient(3, GetTotalTime(3), blue);
            group3Spawn++;
            return true;
        }
        else if (randomValue <= group1Chance + group2Chance + group3Chance + group4Chance && objectsSpawned < 100) // 25% chance for Group 4, spawn 25 objects
        {
            queueManager.SpawnAPatient(4, GetTotalTime(4), green);
            group4Spawn++;
            return true;
        }
        return false;
    }

    public float GetTotalTime(int priority)
    {
        float time = 100f;
        if (priority == 1) time = 355;//355;
        if (priority == 2) time = 380;//380;
        if (priority == 3) time = 383;//383;
        if (priority == 4) time = 357; //357;

        return time;
    }
}
