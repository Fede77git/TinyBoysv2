using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryManager : MonoBehaviour
{
    public List<MemoryTile> tiles;
    public Sprite[] availableSprites;
    public Image displayScreen;
    public Text[] timerTexts;

    public float memorizeTime = 5f;
    public float reactionTime = 3f;
    public float difficultyMultiplier = 0.8f;
    public float timeBetweenRounds = 2f;

    private void Start()
    {
        if (displayScreen != null)
        {
            displayScreen.enabled = false;
        }
        StartCoroutine(RoundRoutine());
    }

    private IEnumerator RoundRoutine()
    {
        int currentPhase = 1;

        while (true)
        {
            List<int> activeIds = new List<int>();
            int currentSpriteCount = Mathf.Min(2 + (currentPhase + 1) / 2, availableSprites.Length);

            foreach (MemoryTile tile in tiles)
            {
                tile.ResetTile();
                int randomId = Random.Range(0, currentSpriteCount);
                tile.Setup(randomId, availableSprites[randomId]);
                tile.ShowImage(true);
                activeIds.Add(randomId);
            }

            if (activeIds.Count == 0) yield break;

            yield return StartCoroutine(WaitAndUpdateTimer(memorizeTime));

            foreach (MemoryTile tile in tiles)
            {
                tile.ShowImage(false);
            }

            int chosenId = activeIds[Random.Range(0, activeIds.Count)];
            displayScreen.sprite = availableSprites[chosenId];
            displayScreen.enabled = true;

            yield return StartCoroutine(WaitAndUpdateTimer(reactionTime));

            displayScreen.enabled = false;

            foreach (MemoryTile tile in tiles)
            {
                if (tile.id != chosenId)
                {
                    tile.Drop();
                }
            }

            if (currentPhase >= 4)
            {
                memorizeTime *= difficultyMultiplier;
                reactionTime *= difficultyMultiplier;
            }

            currentPhase++;

            yield return new WaitForSeconds(timeBetweenRounds);
        }
    }

    private IEnumerator WaitAndUpdateTimer(float duration)
    {
        float timeLeft = duration;
        while (timeLeft > 0f)
        {
            string timeString = Mathf.CeilToInt(timeLeft).ToString();
            foreach (Text t in timerTexts)
            {
                if (t != null) t.text = timeString;
            }
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        foreach (Text t in timerTexts)
        {
            if (t != null) t.text = "";
        }
    }
}
