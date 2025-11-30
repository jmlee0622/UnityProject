using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class spawner : MonoBehaviour
{
    public GameObject[] individualObstaclePrefabs;
    public float[] spawnPointsY;

    [Range(0f, 1f)]
    public float individualSpawnChance = 0.4f;

    public GameObject healPackPrefab;
    [Range(0f, 1f)]
    public float healPackChance = 0.1f;


    public int maxObstaclesPerSpawn = 2;

    private float timeBtwSpawn;
    public float startTimeBtwSpawn;
    public float decreaseTime;
    public float minTime = 10f;

    void Update()
    {
        if (timeBtwSpawn <= 0)
        {
 
            int numObstaclesToSpawn = Random.Range(1, maxObstaclesPerSpawn + 1);

           
            List<int> availableLanes = Enumerable.Range(0, spawnPointsY.Length).ToList();

         
            int currentSpawnedCount = 0;
            while (currentSpawnedCount < numObstaclesToSpawn && availableLanes.Count > 0)
            {
          
                int randLaneListIndex = Random.Range(0, availableLanes.Count);
                int spawnIndex = availableLanes[randLaneListIndex];

               
                if (Random.value < individualSpawnChance)
                {
                    int obsRand = Random.Range(0, individualObstaclePrefabs.Length);
                    float yPos = spawnPointsY[spawnIndex];
                    Vector2 spawnPosition = new Vector2(transform.position.x, yPos);

                    Instantiate(individualObstaclePrefabs[obsRand], spawnPosition, Quaternion.identity);
                    currentSpawnedCount++;
                }

               
                availableLanes.RemoveAt(randLaneListIndex);
            }

            // 4. 회복 팩 생성 시도 (남은 빈 라인 중에서 무작위로 하나 선택)
            if (healPackPrefab != null && Random.value < healPackChance)
            {
                if (availableLanes.Count > 0)
                {
                    // 남은 라인 중에서 무작위로 힐 팩 스폰 위치를 선택합니다.
                    int randLaneIndex = Random.Range(0, availableLanes.Count);
                    int spawnIndex = availableLanes[randLaneIndex];

                    float yPos = spawnPointsY[spawnIndex];
                    Vector2 healSpawnPosition = new Vector2(transform.position.x, yPos);

                    Instantiate(healPackPrefab, healSpawnPosition, Quaternion.identity);
                }
            }

            // 5. 타이머 리셋 및 난이도 조절
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