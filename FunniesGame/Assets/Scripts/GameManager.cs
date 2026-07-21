
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool gameOver = false;
    public static System.Collections.Generic.List<int> deathOrder = new System.Collections.Generic.List<int>();
    private float tiempoEsperaTorneo = 10f;
    private float tiempoTranscurrido = 0f;

    void Start()
    {
        gameOver = false;
        tiempoTranscurrido = 0f;
        deathOrder.Clear();
    }

    void Update()
    {
        if (Time.timeScale == 0f && !PauseMenu.GameIsPaused && !gameOver)
        {
            gameOver = true;
            if (GlobalGameManager.Instance != null && GlobalGameManager.Instance.modoSeleccionado == ModoDeJuego.Torneo && GlobalGameManager.Instance.nivelesDelTorneo.Count > 0)
            {
                DetectWinnerAndUpdateUI();
            }
        }

        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
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
        }
    }

    private void ProceedToNext()
    {
        Time.timeScale = 1;
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
            if (t.text.Contains(" Wins!") && t.gameObject.activeInHierarchy)
            {
                textWin = t;
                break;
            }
        }

        if (textWin != null && GlobalGameManager.Instance != null)
        {
            int winnerIndex = 0;
            string txt = textWin.text;
            if (txt.Contains("Purple") || txt.Contains("Player 1")) winnerIndex = 0;
            else if (txt.Contains("Orange") || txt.Contains("Player 2")) winnerIndex = 1;
            else if (txt.Contains("Green") || txt.Contains("Player 3")) winnerIndex = 2;
            else if (txt.Contains("Blue") || txt.Contains("Player 4")) winnerIndex = 3;

            int[] puntosPorPosicion = { 3, 2, 1, 0 };
            GlobalGameManager.Instance.puntajesJugadores[winnerIndex] += puntosPorPosicion[0];

            int currentPosition = 1;
            for (int i = deathOrder.Count - 1; i >= 0; i--)
            {
                if (currentPosition < puntosPorPosicion.Length)
                {
                    int pIndex = deathOrder[i];
                    if (pIndex != winnerIndex)
                    {
                        GlobalGameManager.Instance.puntajesJugadores[pIndex] += puntosPorPosicion[currentPosition];
                        currentPosition++;
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
