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

        int maxTiles = -1;
        int winnerIndex = -1;
        bool isTie = false;

        for (int i = 0; i < playerTileCounts.Length; i++)
        {
            if (playerTileCounts[i] > maxTiles)
            {
                maxTiles = playerTileCounts[i];
                winnerIndex = i;
                isTie = false;
            }
            else if (playerTileCounts[i] == maxTiles && maxTiles > -1)
            {
                isTie = true;
            }
        }

        if (isTie || winnerIndex == -1)
        {
            if (textWin != null) textWin.text = "It's a Tie!";
        }
        else
        {
            string playerName = "Player " + (winnerIndex + 1);
            if (winnerIndex == 0) playerName = "Purple Player";
            else if (winnerIndex == 1) playerName = "Orange Player";
            else if (winnerIndex == 2) playerName = "Green Player";
            else if (winnerIndex == 3) playerName = "Blue Player";

            if (textWin != null) textWin.text = playerName + " Wins!";
        }

        if (textEsc != null) textEsc.text = "Press Escape to continue";

        UIHelper.ShowWinBackground(textWin);
        Time.timeScale = 0f;
    }
}
