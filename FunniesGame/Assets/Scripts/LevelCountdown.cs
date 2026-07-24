using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class LevelCountdown : MonoBehaviour
{
    public static bool IsCountingDown = false;
    
    public AudioClip beepSound;
    [Range(0f, 1f)] public float beepVolume = 0.4f;
    public AudioClip goSound;
    [Range(0f, 1f)] public float goVolume = 0.5f;
    public AudioMixerGroup sfxMixerGroup;
    public Font customFont;
    public Color textColor = Color.yellow;
    
    private GameObject canvasObj;
    private Text countdownText;
    private AudioSource audioSource;

    void Start()
    {
        IsCountingDown = true;
        Time.timeScale = 0f;
        
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;
        if (sfxMixerGroup != null) audioSource.outputAudioMixerGroup = sfxMixerGroup;

        CreateUI();
        StartCoroutine(CountdownRoutine());
    }

    void CreateUI()
    {
        canvasObj = new GameObject("CountdownCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasObj.AddComponent<GraphicRaycaster>();

        GameObject textObj = new GameObject("CountdownText");
        textObj.transform.SetParent(canvasObj.transform, false);
        
        countdownText = textObj.AddComponent<Text>();
        countdownText.alignment = TextAnchor.MiddleCenter;
        if (customFont != null) countdownText.font = customFont;
        else countdownText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        
        countdownText.fontSize = 250;
        countdownText.color = textColor;
        
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(5, -5);
        
        RectTransform rt = textObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    IEnumerator CountdownRoutine()
    {
        string[] steps = { "3", "2", "1", "FIGHT!" };
        string[] hexColors = { "#FF3333", "#FFA500", "#FFFF33", "#39FF14" };
        
        for (int i = 0; i < steps.Length; i++)
        {
            string step = steps[i];
            countdownText.text = step;

            Color parsedColor;
            if (ColorUtility.TryParseHtmlString(hexColors[i], out parsedColor))
            {
                countdownText.color = parsedColor;
            }

            if (step == "FIGHT!") 
            {
                if (goSound != null) audioSource.PlayOneShot(goSound, goVolume);
            }
            else 
            {
                if (beepSound != null) audioSource.PlayOneShot(beepSound, beepVolume);
            }

            float timer = 0f;
            float duration = 1f;
            float animDuration = 0.5f;
            
            while (timer < duration)
            {
                timer += Time.unscaledDeltaTime;
                
                if (timer < animDuration)
                {
                    float t = timer / animDuration;
                    float c1 = 1.70158f;
                    float c3 = c1 + 1f;
                    float easeT = 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
                    
                    float scale = Mathf.LerpUnclamped(3f, 1f, easeT);
                    countdownText.transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    countdownText.transform.localScale = Vector3.one;
                }
                
                yield return null;
            }
        }
        
        IsCountingDown = false;
        Time.timeScale = 1f;
        Destroy(canvasObj);
        Destroy(gameObject); 
    }
}
