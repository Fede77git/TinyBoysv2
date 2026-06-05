using UnityEngine;

public class ScoringManager : MonoBehaviour
{
    public int[] playerScores = new int[4];
    public float matchTimer = 90f;
    public bool isMatchActive = true;

    public GameObject[] lootPrefabs;
    public Vector2 spawnArea = new Vector2(10f, 10f);
    public float spawnHeight = 15f;
    public float spawnInterval = 3f;
    
    private float spawnTimer;

    private void Update()
    {
        if (!isMatchActive) return;

        matchTimer -= Time.deltaTime;
        if (matchTimer <= 0f)
        {
            matchTimer = 0f;
            isMatchActive = false;
            
            if (LevelManager8.Instance != null)
            {
                LevelManager8.Instance.TimeUp();
            }
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnLoot();
            spawnTimer = 0f;
        }
    }

    private void SpawnLoot()
    {
        if (lootPrefabs == null || lootPrefabs.Length == 0) return;

        int randomLootIndex = Random.Range(0, lootPrefabs.Length);
        float randomX = Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f);
        float randomZ = Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f);
        
        Vector3 spawnPosition = new Vector3(transform.position.x + randomX, spawnHeight, transform.position.z + randomZ);
        
        Instantiate(lootPrefabs[randomLootIndex], spawnPosition, Quaternion.identity);

        if (LevelManager8.Instance != null)
        {
            LevelManager8.Instance.TriggerRespawnLight();
        }
    }

    public void AddScore(int playerIndex, int points)
    {
        if (playerIndex >= 0 && playerIndex < 4)
        {
            playerScores[playerIndex] += points;
            if (LevelManager8.Instance != null) LevelManager8.Instance.UpdateScoreUI();
        }
    }

    public void RemoveScore(int playerIndex, int points)
    {
        if (playerIndex >= 0 && playerIndex < 4)
        {
            playerScores[playerIndex] -= points;
            if (LevelManager8.Instance != null) LevelManager8.Instance.UpdateScoreUI();
        }
    }
}
