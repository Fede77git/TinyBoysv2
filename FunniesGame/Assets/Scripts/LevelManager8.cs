using UnityEngine;
using UnityEngine.UI;

public class LevelManager8 : MonoBehaviour
{
    public static LevelManager8 Instance;

    public Text textWin;
    public Text textEsc;
    public ScoringManager scoringManager;

    public Text[] scoreTexts; 
    public GameObject[] playerUI;
    public Text timerText;
    public Light respawnLight;

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
        if (respawnLight != null) respawnLight.enabled = false;
        UpdateScoreUI();
    }

    void Update()
    {
        if (scoringManager != null && scoringManager.isMatchActive && timerText != null)
        {
            int timeInSeconds = Mathf.CeilToInt(scoringManager.matchTimer);
            int minutes = timeInSeconds / 60;
            int seconds = timeInSeconds % 60;
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
        if (scoringManager == null) return;
        
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (scoreTexts[i] != null)
            {
                scoreTexts[i].text = " " + scoringManager.playerScores[i].ToString();
            }
        }
    }

    public void TimeUp()
    {
        if (gameEnding) return;
        gameEnding = true;
        StartCoroutine(EndGameRoutine());
    }

    public void TriggerRespawnLight()
    {
        if (respawnLight != null)
        {
            StartCoroutine(RespawnLightRoutine());
        }
    }

    private System.Collections.IEnumerator RespawnLightRoutine()
    {
        respawnLight.enabled = true;
        yield return new WaitForSeconds(1f);
        respawnLight.enabled = false;
    }

    private System.Collections.IEnumerator EndGameRoutine()
    {
        yield return new WaitForSeconds(1f);

        int activePlayersCount = GlobalGameManager.Instance != null ? GlobalGameManager.Instance.cantidadJugadores : FindObjectsOfType<PlayerController>().Length;
        if (activePlayersCount == 0) activePlayersCount = 2;

        int maxPoints = -1;
        if (scoringManager != null)
        {
            for (int i = 0; i < activePlayersCount; i++)
            {
                if (scoringManager.playerScores[i] > maxPoints)
                {
                    maxPoints = scoringManager.playerScores[i];
                }
            }
        }

        System.Collections.Generic.List<int> winners = new System.Collections.Generic.List<int>();
        System.Collections.Generic.List<int> losers = new System.Collections.Generic.List<int>();

        if (scoringManager != null)
        {
            for (int i = 0; i < activePlayersCount; i++)
            {
                if (scoringManager.playerScores[i] == maxPoints)
                {
                    winners.Add(i);
                }
                else
                {
                    losers.Add(i);
                }
            }
            losers.Sort((a, b) => scoringManager.playerScores[a].CompareTo(scoringManager.playerScores[b]));
        }

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
