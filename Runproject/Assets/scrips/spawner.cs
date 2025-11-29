using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // List 관련 기능을 사용하기 위해 추가

public class spawner : MonoBehaviour
{
    public GameObject[] individualObstaclePrefabs;
    public float[] spawnPointsY;

    [Range(0f, 1f)]
    public float individualSpawnChance = 0.4f;

    public GameObject healPackPrefab;
    [Range(0f, 1f)]
    public float healPackChance = 0.1f;

    // --- 새로 추가된/수정된 변수 ---
    // 한 번의 스폰 주기에서 생성될 수 있는 최대 장애물 개수 (1개 또는 2개로 조절하려면 2로 설정)
    public int maxObstaclesPerSpawn = 2;

    private float timeBtwSpawn;
    public float startTimeBtwSpawn;
    public float decreaseTime;
    public float minTime = 10f;

    void Update()
    {
        if (timeBtwSpawn <= 0)
        {
            // 1. 이번 스폰 주기에서 실제로 생성할 장애물 개수를 무작위로 결정합니다.
            // 최소 1개, 최대 maxObstaclesPerSpawn (예: 2) 개 중에서 랜덤 선택
            int numObstaclesToSpawn = Random.Range(1, maxObstaclesPerSpawn + 1);

            // 2. 현재 사용 가능한 모든 라인의 인덱스를 목록으로 만듭니다.
            List<int> availableLanes = Enumerable.Range(0, spawnPointsY.Length).ToList();

            // 3. 장애물 생성 루프: 결정된 개수만큼 무작위 라인에 생성합니다.
            int currentSpawnedCount = 0;
            while (currentSpawnedCount < numObstaclesToSpawn && availableLanes.Count > 0)
            {
                // 사용 가능한 라인 중에서 무작위로 하나를 선택합니다.
                int randLaneListIndex = Random.Range(0, availableLanes.Count);
                int spawnIndex = availableLanes[randLaneListIndex];

                // 장애물 생성 확률 검사 (확률이 충족될 때만 생성)
                if (Random.value < individualSpawnChance)
                {
                    int obsRand = Random.Range(0, individualObstaclePrefabs.Length);
                    float yPos = spawnPointsY[spawnIndex];
                    Vector2 spawnPosition = new Vector2(transform.position.x, yPos);

                    Instantiate(individualObstaclePrefabs[obsRand], spawnPosition, Quaternion.identity);
                    currentSpawnedCount++;
                }

                // 선택된 라인은 사용 불가능하게 목록에서 제거합니다.
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