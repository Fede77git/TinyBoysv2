using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManagerMenu : MonoBehaviour
{
    public GameObject panelPrincipal;
    public GameObject panelJugadores;
    public GameObject panelModos;
    public GameObject panelLevelSelector;
    public GameObject panelSettings;

    private void Start()
    {
        if (GlobalGameManager.Instance != null && GlobalGameManager.Instance.volviendoDeNivel)
        {
            GlobalGameManager.Instance.volviendoDeNivel = false;
            ActivarPanel(panelLevelSelector);
        }
        else
        {
            ActivarPanel(panelPrincipal);
        }
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
        SceneManager.LoadScene("Scene_Loading");
    }

    public List<string> todosLosNivelesJuego;

    public void JugarTorneo()
    {
        if (GlobalGameManager.Instance != null && todosLosNivelesJuego != null && todosLosNivelesJuego.Count > 0)
        {
            GlobalGameManager.Instance.IniciarTorneo(todosLosNivelesJuego, 10);
        }
    }

    public void AbrirPanelJugadores()
    {
        panelPrincipal.SetActive(false);
        panelJugadores.SetActive(true);
    }

    public void AbrirPanelSettings()
    {
        ActivarPanel(panelSettings);
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
        if (panelSettings != null) panelSettings.SetActive(false);

        if (panelAActivar != null)
        {
            panelAActivar.SetActive(true);
        }
    }
}
