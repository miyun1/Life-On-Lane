using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 3f;  // Time between spawns

    private Transform[] spawnPoints;

    void Start()
    {
        // Find all SpawnPoint instances in the scene
        SpawnPoint[] points = FindObjectsOfType<SpawnPoint>();
        spawnPoints = new Transform[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            spawnPoints[i] = points[i].transform;
        }

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        // Pick a random enemy prefab
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Pick a random spawn point
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 spawnPos = point.position;

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
