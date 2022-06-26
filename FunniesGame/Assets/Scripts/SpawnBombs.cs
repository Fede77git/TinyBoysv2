
using UnityEngine;

public class SpawnBombs : MonoBehaviour
{
    public GameObject spawner;
    public bool stopSpawn = false;
    public float spawnTime;
    public float spawnDelay;

    void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }

    public void SpawnObject()
    {
        Instantiate(spawner, transform.position, transform.rotation);
        if (stopSpawn)
        {
            CancelInvoke("SpawnObject");
        }
    }
}
