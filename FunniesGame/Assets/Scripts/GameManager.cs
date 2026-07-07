
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool gameOver = false;
    private float tiempoEsperaTorneo = 10f;
    private float tiempoTranscurrido = 0f;

    void Start()
    {
        gameOver = false;
        tiempoTranscurrido = 0f;
    }

    
    void Update()
    {
        if (gameOver)
        {
            if (GlobalGameManager.Instance != null && GlobalGameManager.Instance.modoSeleccionado == ModoDeJuego.Torneo)
            {
                tiempoTranscurrido += Time.unscaledDeltaTime;
                if (tiempoTranscurrido >= tiempoEsperaTorneo)
                {
                    Time.timeScale = 1;
                    GlobalGameManager.Instance.SumarPuntosYAvanzar(new int[] { 0, 1, 2, 3 });
                    return;
                }
            }
            Time.timeScale = 0;
        }
    }
}
