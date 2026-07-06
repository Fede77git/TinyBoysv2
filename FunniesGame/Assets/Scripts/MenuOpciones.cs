using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuOpciones : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider sliderMusica;
    public Slider sliderSonidos;
    public Toggle togglePantallaCompleta;

    void Start()
    {
        float musicaVol = PlayerPrefs.GetFloat("VolumenMusica", 1f);
        float sfxVol = PlayerPrefs.GetFloat("VolumenSonidos", 1f);
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        if (sliderMusica != null) sliderMusica.value = musicaVol;
        if (sliderSonidos != null) sliderSonidos.value = sfxVol;
        if (togglePantallaCompleta != null) togglePantallaCompleta.isOn = isFullscreen;

        SetVolumenMusica(musicaVol);
        SetVolumenSonidos(sfxVol);
        SetPantallaCompleta(isFullscreen);
    }

    public void SetVolumenMusica(float volumen)
    {
        float volumenSeguro = Mathf.Clamp(volumen, 0.0001f, 1f);
        float decibelios = Mathf.Log10(volumenSeguro) * 20f;
        
        if (audioMixer != null) 
        {
            audioMixer.SetFloat("Musica", decibelios);
        }
        
        PlayerPrefs.SetFloat("VolumenMusica", volumenSeguro);
        PlayerPrefs.Save();
    }

    public void SetVolumenSonidos(float volumen)
    {
        float volumenSeguro = Mathf.Clamp(volumen, 0.0001f, 1f);
        float decibelios = Mathf.Log10(volumenSeguro) * 20f;
        
        if (audioMixer != null) 
        {
            audioMixer.SetFloat("SFX", decibelios);
        }
        
        PlayerPrefs.SetFloat("VolumenSonidos", volumenSeguro);
        PlayerPrefs.Save();
    }

    public void SetPantallaCompleta(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}
