using UnityEngine;
using UnityEngine.UI;

public class LevelManager10 : MonoBehaviour
{
    public static LevelManager10 Instance;

    public GameObject[] playerUI;
    public Image[] playerBars;
    public Text[] playerTexts;
    
    public Text textTime;
    public Text textWin;
    public Text textEsc;

    public float timeRemaining = 90f;
    private float[] playerCharge = new float[4];
    private bool gameEnding = false;

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
        
        for (int i = 0; i < 4; i++)
        {
            if (playerBars != null && playerBars.Length > i && playerBars[i] != null) 
                playerBars[i].fillAmount = 0f;
            if (playerTexts != null && playerTexts.Length > i && playerTexts[i] != null) 
                playerTexts[i].text = "0%";
        }
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
    }

    void Update()
    {
        if (gameEnding) return;

        timeRemaining -= Time.deltaTime;
        
        if (textTime != null)
        {
            int totalSeconds = Mathf.Max(0, Mathf.CeilToInt(timeRemaining));
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            textTime.text = string.Format("{0}:{1:00}", minutes, seconds);
        }

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            DetermineWinner();
        }
    }

    public void AddPlayerCharge(int playerIndex, float amount)
    {
        if (gameEnding || playerIndex < 0 || playerIndex >= 4) return;

        playerCharge[playerIndex] += amount;
        
        if (playerCharge[playerIndex] >= 100f)
        {
            playerCharge[playerIndex] = 100f;
        }

        UpdatePlayerUI(playerIndex);

        if (playerCharge[playerIndex] >= 100f)
        {
            DetermineWinner();
        }
    }

    private void UpdatePlayerUI(int playerIndex)
    {
        if (playerBars != null && playerBars.Length > playerIndex && playerBars[playerIndex] != null)
        {
            playerBars[playerIndex].fillAmount = playerCharge[playerIndex] / 100f;
        }
        if (playerTexts != null && playerTexts.Length > playerIndex && playerTexts[playerIndex] != null)
        {
            playerTexts[playerIndex].text = Mathf.FloorToInt(playerCharge[playerIndex]) + "%";
        }
    }

    private void DetermineWinner()
    {
        gameEnding = true;

        int activePlayersCount = GlobalGameManager.Instance != null ? GlobalGameManager.Instance.cantidadJugadores : FindObjectsOfType<PlayerController>().Length;
        if (activePlayersCount == 0) activePlayersCount = 2;

        int maxPercent = -1;
        int[] percents = new int[4];
        for (int i = 0; i < activePlayersCount; i++)
        {
            percents[i] = Mathf.FloorToInt(playerCharge[i]);
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

        losers.Sort((a, b) => playerCharge[a].CompareTo(playerCharge[b]));

        foreach (int loser in losers)
        {
            if (!GameManager.deathOrder.Contains(loser))
            {
                GameManager.deathOrder.Add(loser);
            }
        }

        StartCoroutine(EndGameRoutine(winners));
    }

    private System.Collections.IEnumerator EndGameRoutine(System.Collections.Generic.List<int> winners)
    {
        yield return new WaitForSeconds(1f);

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

    public void PlayerDied(int deadPlayerIndex)
    {
        
    }
}
