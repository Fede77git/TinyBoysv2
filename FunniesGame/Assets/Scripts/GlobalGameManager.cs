using UnityEngine;

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
}
