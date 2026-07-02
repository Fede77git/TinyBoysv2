using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestorDeCarga : MonoBehaviour
{
    public Slider barraCarga;
    public GameObject botonReady;
    public float tiempoDeCarga = 10f;

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
        }

        StartCoroutine(AnimarBarraYEsperar());
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
