using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    [Range(2, 4)]
    public int numberOfPlayers = 2;

    void Start()
    {
        if (GlobalGameManager.Instance != null)
        {
            numberOfPlayers = GlobalGameManager.Instance.cantidadJugadores;
        }

        SpawnPlayers();

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.SetupUI(numberOfPlayers);
        }
        if (LevelManager2.Instance != null)
        {
            LevelManager2.Instance.SetupUI(numberOfPlayers);
        }
        if (LevelManager3.Instance != null)
        {
            LevelManager3.Instance.SetupUI(numberOfPlayers);
        }
        if (LevelManager4.Instance != null)
        {
            LevelManager4.Instance.SetupUI(numberOfPlayers);
        }
        if (LevelManager5.Instance != null)
        {
            LevelManager5.Instance.SetupUI(numberOfPlayers);
        }
        if (LevelManager6.Instance != null)
        {
            LevelManager6.Instance.SetupUI(numberOfPlayers);
        }
        if (LevelManager8.Instance != null)
        {
            LevelManager8.Instance.SetupUI(numberOfPlayers);
        }
        if (LevelManager9.Instance != null)
        {
            LevelManager9.Instance.SetupUI(numberOfPlayers);
        }
        if (LevelManager10.Instance != null)
        {
            LevelManager10.Instance.SetupUI(numberOfPlayers);
        }
    }

    private void SpawnPlayers()
    {
        int limit = Mathf.Min(numberOfPlayers, playerPrefabs.Length, spawnPoints.Length);

        for (int i = 0; i < limit; i++)
        {
            if (playerPrefabs[i] != null && spawnPoints[i] != null)
            {
                GameObject newPlayer = Instantiate(playerPrefabs[i], spawnPoints[i].position, spawnPoints[i].rotation);
                PlayerController pc = newPlayer.GetComponent<PlayerController>();
                if (pc != null)
                {
                    pc.playerIndex = i;
                }
            }
        }
    }
}
