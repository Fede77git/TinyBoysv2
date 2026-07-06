using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestorDeCarga : MonoBehaviour
{
    public Slider barraCarga;
    public GameObject botonReady;
    public float tiempoDeCarga = 10f;

    public Text txtTitulo;
    public Text txtDescripcion;
    public Image imgMapa;
    public DatosNivel[] baseDeDatosNiveles;

    private AsyncOperation operacionCarga;

    private void Start()
    {
        botonReady.SetActive(false);
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
}
