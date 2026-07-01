using UnityEngine;

public class GameManagerMenu : MonoBehaviour
{
    
    public GameObject panelPrincipal;
    public GameObject panelJugadores;
    public GameObject panelModos;
    public GameObject panelLevelSelector;

    private void Start()
    {
        ActivarPanel(panelPrincipal);
    }

    public void AbrirPanelJugadores()
    {
        panelPrincipal.SetActive(false);
        panelJugadores.SetActive(true);
    }

    public void AbrirPanelModos()
    {
        panelJugadores.SetActive(false);
        panelModos.SetActive(true);
    }

    public void AbrirPanelLevelSelector()
    {
        panelModos.SetActive(false);
        panelLevelSelector.SetActive(true);
    }

    public void VolverAlPrincipal()
    {
        ActivarPanel(panelPrincipal);
    }

    public void VolverAJugadores()
    {
        panelModos.SetActive(false);
        panelJugadores.SetActive(true);
    }

    public void VolverAModos()
    {
        panelLevelSelector.SetActive(false);
        panelModos.SetActive(true);
    }

    public void SalirDelJuego()
    {
        Debug.Log("quit salir del juego");
        Application.Quit();
    }

    private void ActivarPanel(GameObject panelAActivar)
    {
        if (panelPrincipal != null) panelPrincipal.SetActive(false);
        if (panelJugadores != null) panelJugadores.SetActive(false);
        if (panelModos != null) panelModos.SetActive(false);
        if (panelLevelSelector != null) panelLevelSelector.SetActive(false);

        if (panelAActivar != null)
        {
            panelAActivar.SetActive(true);
        }
    }
}
