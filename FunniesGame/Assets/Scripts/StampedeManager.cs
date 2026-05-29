using UnityEngine;
using System.Collections;

public class StampedeManager : MonoBehaviour
{
    public Transform[] spawnPoints = new Transform[5];
    public GameObject[] warningLights = new GameObject[5];
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
        
        if (warningLights != null)
        {
            foreach (GameObject wl in warningLights)
            {
                if (wl != null) wl.SetActive(false);
            }
        }
        
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            int randomIndex = 0;
            Transform spawnPoint = null;
            GameObject warningObj = null;

            if (spawnPoints.Length > 0)
            {
                randomIndex = Random.Range(0, spawnPoints.Length);
                spawnPoint = spawnPoints[randomIndex];
                
                if (warningLights != null && randomIndex < warningLights.Length)
                {
                    warningObj = warningLights[randomIndex];
                }
            }

            float waitBeforeLight = currentSpawnDelay - 1f;
            if (waitBeforeLight > 0f)
            {
                yield return new WaitForSeconds(waitBeforeLight);
            }

            if (warningObj != null) 
            {
                warningObj.SetActive(true);
            }
            else if (spawnPoint != null)
            {
                Light l = spawnPoint.GetComponentInChildren<Light>(true);
                if (l != null) l.enabled = true;
            }

            float lightDuration = currentSpawnDelay < 1f ? currentSpawnDelay : 1f;
            yield return new WaitForSeconds(lightDuration);

            if (warningObj != null) 
            {
                warningObj.SetActive(false);
            }
            else if (spawnPoint != null)
            {
                Light l = spawnPoint.GetComponentInChildren<Light>(true);
                if (l != null) l.enabled = false;
            }

            if (spawnPoint != null && obstaclePrefab != null)
            {
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
