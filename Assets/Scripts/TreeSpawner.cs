using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private Terrain terrain;

    [Header("Tree Settings")]
    [SerializeField] private int numberOfTrees = 50;
    [SerializeField] private float checkRadius = 2f; // Minimum distance from other trees
    [SerializeField] private LayerMask treeLayerMask; // Only check for trees

    void Start()
    {
        SpawnTrees();
    }

    private void SpawnTrees()
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        float terrainWidth = terrainData.size.x - 10;
        float terrainLength = terrainData.size.z - 10;

        int spawned = 0;
        int maxAttempts = numberOfTrees * 10; // Prevent infinite loops
        int attempts = 0;

        while (spawned < numberOfTrees && attempts < maxAttempts)
        {
            attempts++;

            float randomX = Random.Range(0, terrainWidth);
            float randomZ = Random.Range(0, terrainLength);

            float worldX = terrainPos.x + randomX;
            float worldZ = terrainPos.z + randomZ;
            float worldY = terrain.SampleHeight(new Vector3(worldX, 0f, worldZ)) + terrainPos.y;

            Vector3 treePosition = new Vector3(worldX, worldY, worldZ);

            // Only check for overlap with other trees
            if (Physics.OverlapSphere(treePosition, checkRadius, treeLayerMask).Length == 0)
            {
                if (treePrefabs.Length > 0)
                {
                    GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                    GameObject tree = Instantiate(prefab, treePosition, Quaternion.identity);
                    tree.layer = LayerMask.NameToLayer("Trees"); // Ensure spawned tree is on the correct layer
                    spawned++;
                }
            }
        }
    }
}
