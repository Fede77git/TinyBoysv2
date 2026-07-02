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

    public void SeleccionarJugadores(int cantidad)
    {
        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.SeteoJugadores(cantidad);
        }
        AbrirPanelModos();
    }

    public void SeleccionarModo(int modoIndex)
    {
        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.SeteoModo((ModoDeJuego)modoIndex);
        }
        AbrirPanelLevelSelector();
    }

    public void JugarNivel(string nombreNivel)
    {
        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.SeteoNivelACargar(nombreNivel);
        }
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
