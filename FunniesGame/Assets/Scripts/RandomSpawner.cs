using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public GameObject spawnVfxPrefab;
    public float initialDelay = 3f;
    public float spawnInterval = 5f;
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);
    public int maxInstances = 3;

    private List<GameObject> spawnedItems = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating(nameof(SpawnItem), initialDelay, spawnInterval);
    }

    private void SpawnItem()
    {
        if (prefabToSpawn == null) return;

        spawnedItems.RemoveAll(item => item == null);

        if (spawnedItems.Count >= maxInstances) return;

        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
            Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
        );

        GameObject newItem = Instantiate(prefabToSpawn, randomPos, Quaternion.identity);
        spawnedItems.Add(newItem);

        if (spawnVfxPrefab != null)
        {
            GameObject vfx = Instantiate(spawnVfxPrefab, randomPos, Quaternion.identity);
            Destroy(vfx, 2f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position, spawnAreaSize);
    }
}
