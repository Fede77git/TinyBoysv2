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

    private System.Collections.IEnumerator EndGameRoutine()
    {
        yield return new WaitForSeconds(1f);

        int maxPoints = -1;
        int winnerIndex = -1;
        bool isTie = false;

        if (scoringManager != null)
        {
            for (int i = 0; i < scoringManager.playerScores.Length; i++)
            {
                if (scoringManager.playerScores[i] > maxPoints)
                {
                    maxPoints = scoringManager.playerScores[i];
                    winnerIndex = i;
                    isTie = false;
                }
                else if (scoringManager.playerScores[i] == maxPoints && maxPoints > -1)
                {
                    isTie = true;
                }
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

        Time.timeScale = 0f;
    }
}
