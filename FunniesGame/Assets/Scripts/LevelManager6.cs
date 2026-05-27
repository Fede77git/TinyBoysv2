using UnityEngine;
using UnityEngine.UI;

public class LevelManager6 : MonoBehaviour
{
    public static LevelManager6 Instance;

    public int coinsToWin = 5;
    
    private int[] playerScores = new int[4];

    public Text[] scoreTexts; 
    public GameObject[] playerUI;
    public Text textWin;
    public Text textEsc;

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

    public void AddCoin(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < playerScores.Length)
        {
            playerScores[playerIndex]++;
            UpdateScoreUI();
            CheckWinCondition(playerIndex);
        }
    }

    private void UpdateScoreUI()
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (scoreTexts[i] != null)
            {
                scoreTexts[i].text = " " + playerScores[i].ToString();
            }
        }
    }

    private void CheckWinCondition(int playerIndex)
    {
        if (playerScores[playerIndex] >= coinsToWin)
        {
            string playerName = "Player " + (playerIndex + 1);
            
            if (playerIndex == 0) playerName = "Purple Player";
            else if (playerIndex == 1) playerName = "Orange Player";
            else if (playerIndex == 2) playerName = "Green Player";
            else if (playerIndex == 3) playerName = "Blue Player";

            if (textWin != null) textWin.text = playerName + " Wins!";
            if (textEsc != null) textEsc.text = "Press Escape to continue";

            Time.timeScale = 0f;
        }
    }

    private bool[] isPlayerDead = new bool[4];
    private bool gameEnding = false;

    public void PlayerDied(int deadPlayerIndex)
    {
        if (deadPlayerIndex >= 0 && deadPlayerIndex < 4)
        {
            isPlayerDead[deadPlayerIndex] = true;
        }

        int totalPlayers = FindObjectsOfType<PlayerController>().Length;
        if (totalPlayers == 0) totalPlayers = 2;

        int aliveCount = 0;
        int lastAliveIndex = 0;

        for (int i = 0; i < totalPlayers; i++)
        {
            if (!isPlayerDead[i])
            {
                aliveCount++;
                lastAliveIndex = i;
            }
        }

        if (aliveCount <= 1 && !gameEnding)
        {
            gameEnding = true;
            StartCoroutine(EndGameRoutine(lastAliveIndex));
        }
    }

    private System.Collections.IEnumerator EndGameRoutine(int lastAliveIndex)
    {
        yield return new WaitForSeconds(1f);

        string playerName = "Player " + (lastAliveIndex + 1);
        if (lastAliveIndex == 0) playerName = "Purple Player";
        else if (lastAliveIndex == 1) playerName = "Orange Player";
        else if (lastAliveIndex == 2) playerName = "Green Player";
        else if (lastAliveIndex == 3) playerName = "Blue Player";

        if (textWin != null) textWin.text = playerName + " Wins!";
        if (textEsc != null) textEsc.text = "Press Escape to continue";

        Time.timeScale = 0f;
    }
}
