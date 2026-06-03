using UnityEngine;
using UnityEngine.UI;

public class LevelManager8 : MonoBehaviour
{
    public static LevelManager8 Instance;

    public Text textWin;
    public Text textEsc;
    public ScoringManager scoringManager;

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
