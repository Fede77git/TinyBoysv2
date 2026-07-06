using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum ModoDeJuego
{
    Torneo,
    SelectorNivel
}

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance;

    public int cantidadJugadores = 2;
    public ModoDeJuego modoSeleccionado;
    public string nivelACargar;

    public int[] puntajesJugadores = new int[4];
    public List<string> nivelesDelTorneo = new List<string>();
    public int rondaActual = 0;
    public bool volviendoDeNivel = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SeteoJugadores(int cantidad)
    {
        cantidadJugadores = cantidad;
    }

    public void SeteoModo(ModoDeJuego modo)
    {
        modoSeleccionado = modo;
    }

    public void SeteoNivelACargar(string nombreNivel)
    {
        nivelACargar = nombreNivel;
    }

    public void IniciarTorneo(List<string> todosLosNivelesDisponibles, int cantidadRondas)
    {
        List<string> nivelesDesordenados = new List<string>(todosLosNivelesDisponibles);
        for (int i = 0; i < nivelesDesordenados.Count; i++)
        {
            string temp = nivelesDesordenados[i];
            int randomIndex = Random.Range(i, nivelesDesordenados.Count);
            nivelesDesordenados[i] = nivelesDesordenados[randomIndex];
            nivelesDesordenados[randomIndex] = temp;
        }

        nivelesDelTorneo.Clear();
        int rondasTomadas = Mathf.Min(cantidadRondas, nivelesDesordenados.Count);
        for (int i = 0; i < rondasTomadas; i++)
        {
            nivelesDelTorneo.Add(nivelesDesordenados[i]);
        }

        for (int i = 0; i < puntajesJugadores.Length; i++)
        {
            puntajesJugadores[i] = 0;
        }
        rondaActual = 0;

        if (nivelesDelTorneo.Count > 0)
        {
            nivelACargar = nivelesDelTorneo[0];
            SceneManager.LoadScene("Scene_Loading");
        }
    }

    public void SumarPuntosYAvanzar(int[] posicionesJugadores)
    {
        int[] puntosPorPosicion = { 3, 2, 1, 0 };

        if (posicionesJugadores != null)
        {
            for (int i = 0; i < posicionesJugadores.Length && i < puntosPorPosicion.Length; i++)
            {
                int indiceJugador = posicionesJugadores[i];
                if (indiceJugador >= 0 && indiceJugador < puntajesJugadores.Length)
                {
                    puntajesJugadores[indiceJugador] += puntosPorPosicion[i];
                }
            }
        }

        rondaActual++;

        if (rondaActual < nivelesDelTorneo.Count)
        {
            nivelACargar = nivelesDelTorneo[rondaActual];
            SceneManager.LoadScene("Scene_Loading");
        }
        else
        {
            SceneManager.LoadScene("Scene_Victoria");
        }
    }
}
