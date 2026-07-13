using UnityEngine;
using System.Collections.Generic;

public class SpawnBombs : MonoBehaviour
{
    public GameObject spawner;
    public bool stopSpawn = false;
    public float spawnTime;
    public float spawnDelay;
    public int maxInstances = 20;

    private List<GameObject> spawnedItems = new List<GameObject>();

    void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }

    public void SpawnObject()
    {
        spawnedItems.RemoveAll(item => item == null);

        if (spawnedItems.Count >= maxInstances)
            return;

        GameObject newBomb = Instantiate(spawner, transform.position, Random.rotation);
        spawnedItems.Add(newBomb);

        if (stopSpawn)
        {
            CancelInvoke("SpawnObject");
        }
    }
}
