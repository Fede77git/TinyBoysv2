using UnityEngine;
using UnityEngine.UI;

public class LevelManager9 : MonoBehaviour
{
    public static LevelManager9 Instance;

    public Text textWin;
    public Text textEsc;
    
    public Text[] scoreTexts; 
    public GameObject[] playerUI;
    public Text timerText;

    public float matchTimer = 90f;
    private bool isMatchActive = true;
    private bool gameEnding = false;

    private int totalTiles = 0;
    private int[] playerTileCounts = new int[4];

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (textWin != null) textWin.text = "";
        if (textEsc != null) textEsc.text = "";
        
        PaintableTile[] allTiles = FindObjectsOfType<PaintableTile>();
        totalTiles = allTiles.Length;

        foreach (PaintableTile t in allTiles)
        {
            if (t.OwnerID >= 0 && t.OwnerID < 4)
            {
                playerTileCounts[t.OwnerID]++;
            }
        }

        UpdateScoreUI();
    }

    void Update()
    {
        if (isMatchActive && timerText != null)
        {
            matchTimer -= Time.deltaTime;
            if (matchTimer <= 0f)
            {
                matchTimer = 0f;
                isMatchActive = false;
                TimeUp();
            }

            int timeInSeconds = Mathf.CeilToInt(matchTimer);
            int minutes = timeInSeconds / 60;
            int seconds = timeInSeconds % 60;
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void OnTilePainted(int oldOwner, int newOwner)
    {
        if (!isMatchActive) return;

        if (oldOwner >= 0 && oldOwner < 4)
        {
            playerTileCounts[oldOwner]--;
        }
        if (newOwner >= 0 && newOwner < 4)
        {
            playerTileCounts[newOwner]++;
        }
        UpdateScoreUI();
    }

    public void SetupUI(int activePlayers)
    {
        if (playerUI != null)
        {
            for (int i = 0; i < playerUI.Length; i++)
            {
                if (playerUI[i] != null)
                {
                    playerUI[i].SetActive(i < activePlayers);
                }
            }
        }

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (scoreTexts[i] != null)
            {
                scoreTexts[i].gameObject.SetActive(i < activePlayers);
            }
        }
    }

    public void UpdateScoreUI()
    {
        if (totalTiles <= 0) return;

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (scoreTexts[i] != null)
            {
                float percentage = ((float)playerTileCounts[i] / totalTiles) * 100f;
                scoreTexts[i].text = string.Format("{0:0}%", percentage);
            }
        }
    }

    public void TimeUp()
    {
        if (gameEnding) return;
        gameEnding = true;
        StartCoroutine(EndGameRoutine());
    }

    private System.Collections.IEnumerator EndGameRoutine()
    {
        yield return new WaitForSeconds(1f);

        int activePlayersCount = GlobalGameManager.Instance != null ? GlobalGameManager.Instance.cantidadJugadores : FindObjectsOfType<PlayerController>().Length;
        if (activePlayersCount == 0) activePlayersCount = 2;

        int maxPercent = -1;
        int[] percents = new int[4];
        for (int i = 0; i < activePlayersCount; i++)
        {
            percents[i] = totalTiles > 0 ? Mathf.RoundToInt(((float)playerTileCounts[i] / totalTiles) * 100f) : 0;
            if (percents[i] > maxPercent)
            {
                maxPercent = percents[i];
            }
        }

        System.Collections.Generic.List<int> winners = new System.Collections.Generic.List<int>();
        System.Collections.Generic.List<int> losers = new System.Collections.Generic.List<int>();

        for (int i = 0; i < activePlayersCount; i++)
        {
            if (percents[i] == maxPercent)
            {
                winners.Add(i);
            }
            else
            {
                losers.Add(i);
            }
        }

        losers.Sort((a, b) => playerTileCounts[a].CompareTo(playerTileCounts[b]));

        foreach (int loser in losers)
        {
            if (!GameManager.deathOrder.Contains(loser))
            {
                GameManager.deathOrder.Add(loser);
            }
        }

        GameManager.currentWinners = new System.Collections.Generic.List<int>(winners);

        string winText = "";
        if (winners.Count > 1)
        {
            winText = "It's a Tie!";
        }
        else
        {
            int w = winners[0];
            if (w == 0) winText = "Purple Player Wins!";
            else if (w == 1) winText = "Orange Player Wins!";
            else if (w == 2) winText = "Green Player Wins!";
            else if (w == 3) winText = "Blue Player Wins!";
        }

        if (textWin != null) textWin.text = winText;
        if (textEsc != null) textEsc.text = "Press Escape to continue";

        UIHelper.ShowWinBackground(textWin);
        Time.timeScale = 0f;
    }
}
