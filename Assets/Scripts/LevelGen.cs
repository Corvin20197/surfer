using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject roadPrefab;
    public GameObject normalObstaclePrefab; // Обычный куб (уворот)
    public GameObject highObstaclePrefab;   // Забор (прыжок)
    public GameObject slideObstaclePrefab;  // Препятствие для подката

    [Header("Generation Settings")]
    private float zSpawn = 0f;
    private float chunkLength = 30f;
    public int numberOfChunks = 5;
    public Transform playerTransform;

    [Header("Obstacle Size Settings")]
    public Vector3 normalObstacleSize = new Vector3(1.5f, 1.2f, 1.5f);
    public Vector3 highObstacleSize = new Vector3(2.5f, 0.8f, 0.5f);
    public Vector3 slideObstacleSize = new Vector3(2.5f, 0.5f, 0.5f);

    private List<GameObject> activeChunks = new List<GameObject>();
    private float[] lanes = { -2.5f, 0f, 2.5f };

    void Start()
    {
        for (int i = 0; i < numberOfChunks; i++)
        {
            SpawnRoad();
        }
    }

    void Update()
    {
        if (playerTransform.position.z - 35f > zSpawn - (numberOfChunks * chunkLength))
        {
            SpawnRoad();
            DeleteRoad();
        }
    }

    private void SpawnRoad()
    {
        Vector3 spawnPosition = new Vector3(0, 0, zSpawn);
        GameObject road = Instantiate(roadPrefab, spawnPosition, transform.rotation);
        activeChunks.Add(road);

        if (zSpawn > 10f)
        {
            float[] zOffsets = { 10f, 22f };

            foreach (float zOffset in zOffsets)
            {
                if (Random.value < 0.55f)
                {
                    SpawnPattern(zOffset);
                }
                else
                {
                    SpawnCoinLine(zOffset);
                }
            }
        }

        zSpawn += chunkLength;
    }

    private void SpawnPattern(float zOffset)
    {
        int patternType = Random.Range(0, 13);

        switch (patternType)
        {
            case 0:
                SpawnObstacle(normalObstaclePrefab, normalObstacleSize, lanes[0], zOffset, 0f);
                SpawnObstacle(normalObstaclePrefab, normalObstacleSize, lanes[2], zOffset, 0f);
                SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[1], zOffset, 0f);
                break;
            case 1:
                SpawnObstacle(normalObstaclePrefab, normalObstacleSize, lanes[0], zOffset, 0f);
                SpawnObstacle(normalObstaclePrefab, normalObstacleSize, lanes[1], zOffset, 0f);
                SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[2], zOffset, 0f);
                break;
            case 2:
                SpawnObstacle(normalObstaclePrefab, normalObstacleSize, lanes[1], zOffset, 0f);
                SpawnObstacle(normalObstaclePrefab, normalObstacleSize, lanes[2], zOffset, 0f);
                SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[0], zOffset, 0f);
                break;
            case 3:
                SpawnObstacle(normalObstaclePrefab, normalObstacleSize, lanes[1], zOffset, 0f);
                SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[0], zOffset, 0f);
                SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[2], zOffset, 0f);
                break;
            case 4:
                SpawnObstacle(normalObstaclePrefab, normalObstacleSize, lanes[0], zOffset, 0f);
                SpawnObstacle(normalObstaclePrefab, normalObstacleSize, lanes[2], zOffset, 0f);
                SpawnObstacle(slideObstaclePrefab, slideObstacleSize, lanes[1], zOffset, 1.8f);
                break;
            case 5:
                SpawnObstacle(normalObstaclePrefab, normalObstacleSize, lanes[1], zOffset, 0f);
                SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[0], zOffset, 0f);
                SpawnObstacle(slideObstaclePrefab, slideObstacleSize, lanes[2], zOffset, 1.8f);
                break;
            case 6:
                SpawnObstacle(normalObstaclePrefab, normalObstacleSize, lanes[1], zOffset, 0f);
                SpawnObstacle(slideObstaclePrefab, slideObstacleSize, lanes[0], zOffset, 1.8f);
                SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[2], zOffset, 0f);
                break;
            case 7:
                int freeLane = Random.Range(0, lanes.Length);
                for (int i = 0; i < lanes.Length; i++)
                {
                    if (i != freeLane)
                    {
                        SpawnObstacle(normalObstaclePrefab, normalObstacleSize, lanes[i], zOffset, 0f);
                    }
                }
                break;
            case 8:
                SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[0], zOffset, 0f);
                SpawnObstacle(slideObstaclePrefab, slideObstacleSize, lanes[1], zOffset, 1.8f);
                break;
            case 9:
                SpawnObstacle(slideObstaclePrefab, slideObstacleSize, lanes[1], zOffset, 1.8f);
                SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[2], zOffset, 0f);
                break;
            case 10:
                SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[0], zOffset, 0f);
                SpawnObstacle(slideObstaclePrefab, slideObstacleSize, lanes[1], zOffset, 1.8f);
                SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[2], zOffset, 0f);
                break;
            case 11:
                SpawnObstacle(slideObstaclePrefab, slideObstacleSize, lanes[0], zOffset, 1.8f);
                SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[1], zOffset, 0f);
                SpawnObstacle(slideObstaclePrefab, slideObstacleSize, lanes[2], zOffset, 1.8f);
                break;
            case 12:
            default:
                int single = Random.Range(0, lanes.Length);
                if (Random.value < 0.5f)
                {
                    SpawnObstacle(highObstaclePrefab, highObstacleSize, lanes[single], zOffset, 0f);
                }
                else
                {
                    SpawnObstacle(slideObstaclePrefab, slideObstacleSize, lanes[single], zOffset, 1.8f);
                }
                break;
        }
    }

    private void SpawnObstacle(GameObject prefab, Vector3 size, float laneX, float zOffset, float customY)
    {
        if (prefab == null) return;

        float worldZ = (zSpawn - chunkLength) + zOffset;
        float posY = (customY > 0f) ? customY : (size.y / 2f);
        Vector3 obstaclePos = new Vector3(laneX, posY, worldZ);

        GameObject obstacle = Instantiate(prefab, obstaclePos, Quaternion.identity);
        obstacle.transform.localScale = size;

        GameObject currentRoad = activeChunks[activeChunks.Count - 1];
        obstacle.transform.SetParent(currentRoad.transform);
    }

    private void SpawnCoinLine(float zOffset)
    {
        int randomLaneIndex = Random.Range(0, lanes.Length);
        float laneX = lanes[randomLaneIndex];

        int coinCount = 6;
        float coinSpacing = 1.8f;

        for (int i = 0; i < coinCount; i++)
        {
            float worldZ = (zSpawn - chunkLength) + zOffset + (i * coinSpacing);
            Vector3 coinPos = new Vector3(laneX, 0.75f, worldZ);

            // Создаем чистый цилиндр для монетки
            GameObject coin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            coin.transform.position = coinPos;
            coin.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            // Жестко задаем размер (теперь родительская дорога не сможет его исказить)
            coin.transform.localScale = new Vector3(0.8f, 0.1f, 0.8f);

            // Настраиваем коллайдер в триггер
            Collider col = coin.GetComponent<Collider>();
            if (col != null) col.isTrigger = true;

            // Вешаем скрипт сбора и автоудаления
            if (coin.GetComponent<Coin>() == null)
            {
                coin.AddComponent<Coin>();
            }

            // Красим в желтый цвет
            Renderer rend = coin.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = Color.yellow;
            }
        }
    }

    private void DeleteRoad()
    {
        Destroy(activeChunks[0]);
        activeChunks.RemoveAt(0);
    }
}