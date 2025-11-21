using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject[] individualObstaclePrefabs;
    public float[] spawnPointsY;

    [Range(0f, 1f)]
    public float individualSpawnChance = 0.4f;

    public GameObject healPackPrefab;
    [Range(0f, 1f)]
    public float healPackChance = 0.1f;

    private float timeBtwSpawn;
    public float startTimeBtwSpawn;
    public float decreaseTime;
    public float minTime = 10f;

    void Update()
    {
        if (timeBtwSpawn <= 0)
        {
            bool[] isLaneOccupied = new bool[spawnPointsY.Length];

            int maxObstaclesToSpawn = spawnPointsY.Length - 1;
            int currentSpawnedCount = 0;

            for (int i = 0; i < spawnPointsY.Length; i++)
            {
                float yPos = spawnPointsY[i];

                if (individualObstaclePrefabs.Length > 0 &&
                    Random.value < individualSpawnChance &&
                    currentSpawnedCount < maxObstaclesToSpawn)
                {
                    int obsRand = Random.Range(0, individualObstaclePrefabs.Length);
                    Vector2 spawnPosition = new Vector2(transform.position.x, yPos);

                    Instantiate(individualObstaclePrefabs[obsRand], spawnPosition, Quaternion.identity);

                    isLaneOccupied[i] = true;
                    currentSpawnedCount++;
                }
            }

            if (healPackPrefab != null && Random.value < healPackChance)
            {
                List<int> availableLanes = new List<int>();
                for (int i = 0; i < isLaneOccupied.Length; i++)
                {
                    if (!isLaneOccupied[i])
                    {
                        availableLanes.Add(i);
                    }
                }

                if (availableLanes.Count > 0)
                {
                    int randLaneIndex = Random.Range(0, availableLanes.Count);
                    int spawnIndex = availableLanes[randLaneIndex];

                    Vector2 healSpawnPosition = new Vector2(transform.position.x, spawnPointsY[spawnIndex]);
                    Instantiate(healPackPrefab, healSpawnPosition, Quaternion.identity);
                }
            }

            timeBtwSpawn = startTimeBtwSpawn;
            if (startTimeBtwSpawn > minTime)
            {
                startTimeBtwSpawn -= decreaseTime;
            }
        }
        else
        {
            timeBtwSpawn -= Time.deltaTime;
        }
    }
}