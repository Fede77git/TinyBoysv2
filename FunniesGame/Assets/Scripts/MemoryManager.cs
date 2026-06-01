using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryManager : MonoBehaviour
{
    public List<MemoryTile> tiles;
    public Sprite[] availableSprites;
    public Image displayScreen;

    public float memorizeTime = 5f;
    public float reactionTime = 3f;
    public float difficultyMultiplier = 0.8f;
    public float timeBetweenRounds = 2f;

    private void Start()
    {
        StartCoroutine(RoundRoutine());
    }

    private IEnumerator RoundRoutine()
    {
        while (tiles.Count > 0)
        {
            List<int> activeIds = new List<int>();

            foreach (MemoryTile tile in tiles)
            {
                int randomId = Random.Range(0, availableSprites.Length);
                tile.Setup(randomId, availableSprites[randomId]);
                tile.ShowImage(true);
                activeIds.Add(randomId);
            }

            if (activeIds.Count == 0) yield break;

            yield return new WaitForSeconds(memorizeTime);

            foreach (MemoryTile tile in tiles)
            {
                tile.ShowImage(false);
            }

            int chosenId = activeIds[Random.Range(0, activeIds.Count)];
            displayScreen.sprite = availableSprites[chosenId];
            displayScreen.enabled = true;

            yield return new WaitForSeconds(reactionTime);

            displayScreen.enabled = false;

            List<MemoryTile> remainingTiles = new List<MemoryTile>();

            foreach (MemoryTile tile in tiles)
            {
                if (tile.id != chosenId)
                {
                    tile.Drop();
                }
                else
                {
                    remainingTiles.Add(tile);
                }
            }

            tiles = remainingTiles;

            memorizeTime *= difficultyMultiplier;
            reactionTime *= difficultyMultiplier;

            yield return new WaitForSeconds(timeBetweenRounds);
        }
    }
}
