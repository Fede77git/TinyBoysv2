using UnityEngine;
using UnityEngine.UI;

public class LevelManager3 : MonoBehaviour
{
    public static LevelManager3 Instance;

    public Text[] scoreTexts;
    public GameObject[] playerUI;
    public Text textWin;
    public Text textEsc;
    public Text textCut;

    private bool[] isPlayerDead = new bool[4];
    private int totalPlayers;

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
        if (textCut != null) textCut.text = "";
    }

    public void SetupUI(int activePlayers)
    {
        totalPlayers = activePlayers;

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

        if (scoreTexts != null)
        {
            for (int i = 0; i < scoreTexts.Length; i++)
            {
                if (scoreTexts[i] != null)
                {
                    scoreTexts[i].gameObject.SetActive(i < activePlayers);
                }
            }
        }
    }

    public void PlayerDied(int deadPlayerIndex)
    {
        if (deadPlayerIndex >= 0 && deadPlayerIndex < 4)
        {
            isPlayerDead[deadPlayerIndex] = true;
        }

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

        if (aliveCount <= 1)
        {
            string playerName = "Player " + (lastAliveIndex + 1);
            if (lastAliveIndex == 0) playerName = "Purple Player";
            else if (lastAliveIndex == 1) playerName = "Orange Player";
            else if (lastAliveIndex == 2) playerName = "Green Player";
            else if (lastAliveIndex == 3) playerName = "Blue Player";

            if (textWin != null) textWin.text = playerName + " Wins!";
            if (textEsc != null) textEsc.text = "Press Escape to continue";
            if (textCut != null) textCut.text = "All others were cut into pieces";

            Time.timeScale = 0f;
        }
    }
}
