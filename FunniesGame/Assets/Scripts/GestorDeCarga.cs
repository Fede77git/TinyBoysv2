using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestorDeCarga : MonoBehaviour
{
    public Slider barraCarga;
    public GameObject botonReady;
    public GameObject botonBack;
    public float tiempoDeCarga = 10f;

    public Text txtTitulo;
    public Text txtDescripcion;
    public Image imgMapa;
    public DatosNivel[] baseDeDatosNiveles;

    private AsyncOperation operacionCarga;

    private void Start()
    {
        botonReady.SetActive(false);
        if (botonBack != null)
        {
            if (GlobalGameManager.Instance != null && GlobalGameManager.Instance.modoSeleccionado == ModoDeJuego.SelectorNivel)
            {
                botonBack.SetActive(true);
            }
            else
            {
                botonBack.SetActive(false);
            }
        }

        if (barraCarga != null)
        {
            barraCarga.value = 0;
        }

        if (GlobalGameManager.Instance != null && !string.IsNullOrEmpty(GlobalGameManager.Instance.nivelACargar))
        {
            operacionCarga = SceneManager.LoadSceneAsync(GlobalGameManager.Instance.nivelACargar);
            if (operacionCarga != null)
            {
                operacionCarga.allowSceneActivation = false;
            }
            ActualizarPantalla(GlobalGameManager.Instance.nivelACargar);
        }

        StartCoroutine(AnimarBarraYEsperar());
    }

    private void ActualizarPantalla(string nombreEscena)
    {
        if (baseDeDatosNiveles == null) return;

        foreach (DatosNivel nivel in baseDeDatosNiveles)
        {
            if (nivel != null && nivel.escenaNombre == nombreEscena)
            {
                if (txtTitulo != null) txtTitulo.text = nivel.nombreNivel;
                if (txtDescripcion != null) txtDescripcion.text = nivel.descripcionNivel;
                if (imgMapa != null) imgMapa.sprite = nivel.fotoNivel;
                break;
            }
        }
    }

    private IEnumerator AnimarBarraYEsperar()
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < tiempoDeCarga)
        {
            tiempoTranscurrido += Time.deltaTime;
            
            if (barraCarga != null)
            {
                barraCarga.value = Mathf.Clamp01(tiempoTranscurrido / tiempoDeCarga);
            }

            yield return null;
        }

        if (barraCarga != null)
        {
            barraCarga.value = 1f;
            barraCarga.gameObject.SetActive(false);
        }

        botonReady.SetActive(true);
    }

    public void Play()
    {
        if (operacionCarga != null)
        {
            operacionCarga.allowSceneActivation = true;
        }
    }

    public void BackToMenu()
    {
        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.volviendoDeNivel = true;
        }
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        bool readyPressed = Input.GetKeyDown(KeyCode.Return);
        bool backPressed = Input.GetKeyDown(KeyCode.Escape);

        if (UnityEngine.InputSystem.Gamepad.current != null)
        {
            if (UnityEngine.InputSystem.Gamepad.current.buttonSouth.wasPressedThisFrame) readyPressed = true;
            if (UnityEngine.InputSystem.Gamepad.current.buttonEast.wasPressedThisFrame) backPressed = true;
        }
        else
        {
            if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown("joystick button 1")) readyPressed = true;
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown("joystick button 2")) backPressed = true;
        }

        if (botonReady != null && botonReady.activeInHierarchy)
        {
            if (readyPressed) Play();
        }

        if (botonBack != null && botonBack.activeInHierarchy)
        {
            if (backPressed) BackToMenu();
        }
    }
}
