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

        int winnerIndex = 0;
        float maxCharge = -1f;

        for (int i = 0; i < 4; i++)
        {
            if (playerCharge[i] > maxCharge)
            {
                maxCharge = playerCharge[i];
                winnerIndex = i;
            }
        }

        StartCoroutine(EndGameRoutine(winnerIndex));
    }

    private System.Collections.IEnumerator EndGameRoutine(int winnerIndex)
    {
        yield return new WaitForSeconds(1f);

        string playerName = "Player " + (winnerIndex + 1);
        if (winnerIndex == 0) playerName = "Purple Player";
        else if (winnerIndex == 1) playerName = "Orange Player";
        else if (winnerIndex == 2) playerName = "Green Player";
        else if (winnerIndex == 3) playerName = "Blue Player";

        if (textWin != null) textWin.text = playerName + " Wins!";
        if (textEsc != null) textEsc.text = "Press Escape to continue";

        UIHelper.ShowWinBackground(textWin);
        Time.timeScale = 0f;
    }

    public void PlayerDied(int deadPlayerIndex)
    {
        
    }
}
