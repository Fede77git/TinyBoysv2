using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    [Range(2, 4)]
    public int numberOfPlayers = 2;

    void Start()
    {
        SpawnPlayers();

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.SetupUI(numberOfPlayers);
        }
        else if (LevelManager2.Instance != null)
        {
            LevelManager2.Instance.SetupUI(numberOfPlayers);
        }
    }

    private void SpawnPlayers()
    {
        int limit = Mathf.Min(numberOfPlayers, playerPrefabs.Length, spawnPoints.Length);

        for (int i = 0; i < limit; i++)
        {
            if (playerPrefabs[i] != null && spawnPoints[i] != null)
            {
                Instantiate(playerPrefabs[i], spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }
    }
}
