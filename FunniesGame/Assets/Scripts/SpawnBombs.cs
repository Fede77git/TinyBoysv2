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
    private float timer;

    void Start()
    {
        timer = spawnTime;
    }

    void Update()
    {
        for (int i = spawnedItems.Count - 1; i >= 0; i--)
        {
            if (spawnedItems[i] != null && spawnedItems[i].transform.position.y < -15f)
            {
                Destroy(spawnedItems[i]);
            }
        }

        spawnedItems.RemoveAll(item => item == null);

        if (stopSpawn)
            return;

        if (spawnedItems.Count < maxInstances)
        {
            timer -= Time.deltaTime;
            
            if (timer <= 0f)
            {
                SpawnObject();
                timer = spawnDelay;
            }
        }
        else
        {
            timer = spawnDelay;
        }
    }

    private void SpawnObject()
    {
        GameObject newBomb = Instantiate(spawner, transform.position, Random.rotation);
        spawnedItems.Add(newBomb);
    }
}
