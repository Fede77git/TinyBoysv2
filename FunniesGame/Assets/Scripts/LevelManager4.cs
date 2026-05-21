using UnityEngine;
using UnityEngine.UI;

public class LevelManager4 : MonoBehaviour
{
    public static LevelManager4 Instance;

    
    public int scoreToWin = 5;
    public int scoreToWinTeam1 = 5;
    public int scoreToWinTeam2 = 5;

    private int team1Score = 0;
    private int team2Score = 0;

    
    public Text textScoreTeam1; // purple/Green
    public Text textScoreTeam2; // orange/Blue
    public GameObject[] playerUI;
    public Text textWin;
    public Text textEsc;

    private int activePlayerCount;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (textWin != null) textWin.text = "";
        if (textEsc != null) textEsc.text = "";
        UpdateScoreUI();
    }

    public void SetupUI(int activePlayers)
    {
        activePlayerCount = activePlayers;

        if (activePlayers == 3)
        {
            scoreToWinTeam1 = scoreToWin + 2; 
            scoreToWinTeam2 = scoreToWin;     
        }
        else
        {
            scoreToWinTeam1 = scoreToWin;
            scoreToWinTeam2 = scoreToWin;
        }

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

    public void AddScore(int teamIndex)
    {
        if (teamIndex == 1)
        {
            team1Score++;
        }
        else if (teamIndex == 2)
        {
            team2Score++;
        }

        UpdateScoreUI();
        CheckWinCondition();
    }

    private void UpdateScoreUI()
    {
        if (textScoreTeam1 != null) textScoreTeam1.text = " " + team1Score.ToString();
        if (textScoreTeam2 != null) textScoreTeam2.text = " " + team2Score.ToString();
    }

    private void CheckWinCondition()
    {
        if (team1Score >= scoreToWinTeam1)
        {
            EndGame("Team 1 Wins!");
        }
        else if (team2Score >= scoreToWinTeam2)
        {
            EndGame("Team 2 Wins!");
        }
    }

    private void EndGame(string message)
    {
        if (textWin != null) textWin.text = message;
        if (textEsc != null) textEsc.text = "Press Escape to continue";
        Time.timeScale = 0f;
    }
}
