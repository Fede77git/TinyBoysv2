using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SceneMusic
{
    public string sceneName;
    public AudioClip musicClip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource;
    public SceneMusic[] sceneMusicMapping;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (musicSource != null) musicSource.ignoreListenerPause = true;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var mapping in sceneMusicMapping)
        {
            if (mapping.sceneName == scene.name)
            {
                if (musicSource.clip != mapping.musicClip)
                {
                    musicSource.clip = mapping.musicClip;
                    musicSource.Play();
                }
                return;
            }
        }
    }
}
