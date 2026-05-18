using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int coinsToWin = 5;
    
    private int[] playerScores = new int[4];

    public Text[] scoreTexts; 
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
}
