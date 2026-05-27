using UnityEngine;
using System.Collections;

public class StampedeManager : MonoBehaviour
{
    public Transform[] spawnPoints = new Transform[5];
    public GameObject obstaclePrefab;

    public float initialSpawnDelay = 2f;
    public float minSpawnDelay = 0.5f;
    public float delayDecreaseRate = 0.05f;

    public float initialSpeed = 10f;
    public float maxSpeed = 30f;
    public float speedIncreaseRate = 0.5f;

    private float currentSpawnDelay;
    private float currentSpeed;

    void Start()
    {
        currentSpawnDelay = initialSpawnDelay;
        currentSpeed = initialSpeed;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnDelay);

            if (spawnPoints.Length > 0 && obstaclePrefab != null)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Transform spawnPoint = spawnPoints[randomIndex];

                GameObject newObstacle = Instantiate(obstaclePrefab, spawnPoint.position, spawnPoint.rotation);
                StampedeObstacle obsScript = newObstacle.GetComponent<StampedeObstacle>();
                
                if (obsScript != null)
                {
                    obsScript.speed = currentSpeed;
                }
            }

            if (currentSpawnDelay > minSpawnDelay)
            {
                currentSpawnDelay -= delayDecreaseRate;
                if (currentSpawnDelay < minSpawnDelay) currentSpawnDelay = minSpawnDelay;
            }

            if (currentSpeed < maxSpeed)
            {
                currentSpeed += speedIncreaseRate;
                if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;
            }
        }
    }
}
