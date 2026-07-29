
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool gameOver = false;
    public static System.Collections.Generic.List<int> deathOrder = new System.Collections.Generic.List<int>();
    private float tiempoEsperaTorneo = 10f;
    private float tiempoTranscurrido = 0f;

    public static System.Collections.Generic.List<int> currentWinners = new System.Collections.Generic.List<int>();

    void Start()
    {
        gameOver = false;
        tiempoTranscurrido = 0f;
        deathOrder.Clear();
        currentWinners.Clear();
    }

    void Update()
    {
        int forcedWinnerIndex = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1)) forcedWinnerIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) forcedWinnerIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) forcedWinnerIndex = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) forcedWinnerIndex = 3;

        if (forcedWinnerIndex != -1)
        {
            UnityEngine.UI.Text winText = null;
            var allMono = FindObjectsOfType<MonoBehaviour>();
            foreach (var mono in allMono)
            {
                var field = mono.GetType().GetField("textWin");
                if (field != null)
                {
                    winText = field.GetValue(mono) as UnityEngine.UI.Text;
                    if (winText != null) break;
                }
            }

            if (winText != null)
            {
                winText.gameObject.SetActive(true);
                if (forcedWinnerIndex == 0) winText.text = "Purple Player Wins!";
                else if (forcedWinnerIndex == 1) winText.text = "Orange Player Wins!";
                else if (forcedWinnerIndex == 2) winText.text = "Green Player Wins!";
                else if (forcedWinnerIndex == 3) winText.text = "Blue Player Wins!";
                UIHelper.ShowWinBackground(winText);
            }

            deathOrder.Clear();
            currentWinners.Clear();
            currentWinners.Add(forcedWinnerIndex);
            Time.timeScale = 0f;
            return;
        }

        if (Time.timeScale == 0f && !PauseMenu.GameIsPaused && !gameOver && !LevelCountdown.IsCountingDown)
        {
            gameOver = true;
            if (GlobalGameManager.Instance != null && GlobalGameManager.Instance.modoSeleccionado == ModoDeJuego.Torneo && GlobalGameManager.Instance.nivelesDelTorneo.Count > 0)
            {
                DetectWinnerAndUpdateUI();
            }
        }

        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))
            {
                ProceedToNext();
                return;
            }

            if (GlobalGameManager.Instance != null && GlobalGameManager.Instance.modoSeleccionado == ModoDeJuego.Torneo && GlobalGameManager.Instance.nivelesDelTorneo.Count > 0)
            {
                tiempoTranscurrido += Time.unscaledDeltaTime;
                if (tiempoTranscurrido >= tiempoEsperaTorneo)
                {
                    ProceedToNext();
                    return;
                }
            }
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
    }

    private void ProceedToNext()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        if (GlobalGameManager.Instance != null)
        {
            if (GlobalGameManager.Instance.modoSeleccionado == ModoDeJuego.Torneo && GlobalGameManager.Instance.nivelesDelTorneo.Count > 0)
            {
                GlobalGameManager.Instance.rondaActual++;
                if (GlobalGameManager.Instance.rondaActual < GlobalGameManager.Instance.nivelesDelTorneo.Count)
                {
                    GlobalGameManager.Instance.nivelACargar = GlobalGameManager.Instance.nivelesDelTorneo[GlobalGameManager.Instance.rondaActual];
                    SceneManager.LoadScene("Scene_Loading");
                }
                else
                {
                    SceneManager.LoadScene("Scene_Victoria");
                }
            }
            else
            {
                GlobalGameManager.Instance.volviendoDeNivel = true;
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    private void DetectWinnerAndUpdateUI()
    {
        UnityEngine.UI.Text textWin = null;
        var allTexts = FindObjectsOfType<UnityEngine.UI.Text>();
        foreach (var t in allTexts)
        {
            if ((t.text.Contains(" Wins!") || t.text.Contains(" Win!") || t.text.Contains("Tie!") || t.text.Contains("Team")) && t.gameObject.activeInHierarchy)
            {
                textWin = t;
                break;
            }
        }

        if (textWin != null && GlobalGameManager.Instance != null)
        {
            for (int i = 0; i < GlobalGameManager.Instance.puntosUltimoNivel.Length; i++)
            {
                GlobalGameManager.Instance.puntosUltimoNivel[i] = 0;
            }

            string txt = textWin.text;

            if (txt.Contains("Team 1") || txt.Contains("Team 2"))
            {
                int[] team1 = { 0, 2 };
                int[] team2 = { 1, 3 }; 
                
                int[] winningTeam = txt.Contains("Team 1") ? team1 : team2;
                int[] losingTeam = txt.Contains("Team 1") ? team2 : team1;

                foreach (int pIndex in winningTeam)
                {
                    if (pIndex < GlobalGameManager.Instance.cantidadJugadores)
                    {
                        GlobalGameManager.Instance.puntajesJugadores[pIndex] += 3;
                        GlobalGameManager.Instance.puntosUltimoNivel[pIndex] = 3;
                    }
                }
                foreach (int pIndex in losingTeam)
                {
                    if (pIndex < GlobalGameManager.Instance.cantidadJugadores)
                    {
                        GlobalGameManager.Instance.puntajesJugadores[pIndex] += 1;
                        GlobalGameManager.Instance.puntosUltimoNivel[pIndex] = 1;
                    }
                }
            }
            else
            {
                System.Collections.Generic.List<int> winners = new System.Collections.Generic.List<int>(currentWinners);
                if (winners.Count == 0)
                {
                    if (txt.Contains("Purple") || txt.Contains("Player 1")) winners.Add(0);
                    if (txt.Contains("Orange") || txt.Contains("Player 2")) winners.Add(1);
                    if (txt.Contains("Green") || txt.Contains("Player 3")) winners.Add(2);
                    if (txt.Contains("Blue") || txt.Contains("Player 4")) winners.Add(3);
                }

                if (winners.Count == 0) winners.Add(0);

                int[] puntosPorPosicion = { 3, 2, 1, 0 };
                
                foreach (int w in winners)
                {
                    GlobalGameManager.Instance.puntajesJugadores[w] += puntosPorPosicion[0];
                    GlobalGameManager.Instance.puntosUltimoNivel[w] = puntosPorPosicion[0];
                }

                int currentPosition = 1;
                for (int i = deathOrder.Count - 1; i >= 0; i--)
                {
                    if (currentPosition < puntosPorPosicion.Length)
                    {
                        int pIndex = deathOrder[i];
                        if (!winners.Contains(pIndex))
                        {
                            GlobalGameManager.Instance.puntajesJugadores[pIndex] += puntosPorPosicion[currentPosition];
                            GlobalGameManager.Instance.puntosUltimoNivel[pIndex] = puntosPorPosicion[currentPosition];
                            currentPosition++;
                        }
                    }
                }
            }

            string scoresText = "\n<size=60>Tournament Scores</size>\n<size=90>";
            string[] playerColors = { "purple", "orange", "green", "blue" };
            for (int i = 0; i < GlobalGameManager.Instance.cantidadJugadores; i++)
            {
                string colorStr = i < playerColors.Length ? playerColors[i] : "white";
                string pName = "<color=" + colorStr + ">P" + (i + 1) + "</color>";
                scoresText += pName + ": " + GlobalGameManager.Instance.puntajesJugadores[i] + "    ";
            }
            scoresText += "</size>";
            
            textWin.text += scoresText;
        }
    }
}
