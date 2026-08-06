using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    
    private float lastSliderNavTime = 0f;
    private float lastTransitionTime = 0f;

    void Awake()
    {
        GameIsPaused = false;
        
        if (FindObjectOfType<GameManager>() == null)
        {
            GameObject gmObj = new GameObject("GameManager");
            gmObj.AddComponent<GameManager>();
        }
    }

    void Update()
    {
        if (GameManager.gameOver || (Time.timeScale == 0f && !GameIsPaused))
        {
            bool goBack = Input.GetKeyDown(KeyCode.Escape);
            if (!goBack)
            {
                foreach (var gamepad in UnityEngine.InputSystem.Gamepad.all)
                {
                    if (gamepad.startButton.wasPressedThisFrame || gamepad.buttonSouth.wasPressedThisFrame)
                    {
                        goBack = true;
                        break;
                    }
                }
            }

            if (goBack)
            {
                GameManager gm = FindObjectOfType<GameManager>();
                if (gm == null)
                {
                    Time.timeScale = 1f;
                    AudioListener.pause = false;
                    if (GlobalGameManager.Instance != null)
                    {
                        if (GlobalGameManager.Instance.modoSeleccionado == ModoDeJuego.Torneo && GlobalGameManager.Instance.nivelesDelTorneo.Count > 0)
                        {
                            GlobalGameManager.Instance.rondaActual++;
                            if (GlobalGameManager.Instance.rondaActual < GlobalGameManager.Instance.nivelesDelTorneo.Count)
                            {
                                GlobalGameManager.Instance.nivelACargar = GlobalGameManager.Instance.nivelesDelTorneo[GlobalGameManager.Instance.rondaActual];
                                SceneManager.LoadScene("Scene_Loading");
                            }
                            else
                            {
                                SceneManager.LoadScene("Scene_Victoria");
                            }
                        }
                        else
                        {
                            GlobalGameManager.Instance.volviendoDeNivel = true;
                            SceneManager.LoadScene("MainMenu");
                        }
                    }
                    else
                    {
                        SceneManager.LoadScene("MainMenu");
                    }
                }
            }
            return;
        }

        bool pauseInput = Input.GetKeyDown(KeyCode.Escape);
        
        if (!pauseInput)
        {
            foreach (var gamepad in UnityEngine.InputSystem.Gamepad.all)
            {
                if (gamepad.startButton.wasPressedThisFrame)
                {
                    pauseInput = true;
                    break;
                }
            }
        }

        if (pauseInput)
        {
            if (GameIsPaused)
            {
                if (settingsMenuUI.activeSelf)
                {
                    CloseSettings();
                }
                else
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }

        if (GameIsPaused)
        {
            if (UnityEngine.EventSystems.EventSystem.current != null)
            {
                GameObject selected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                
                bool southPressed = false;
                bool eastPressed = false;

                foreach (var gamepad in UnityEngine.InputSystem.Gamepad.all)
                {
                    if (gamepad.buttonSouth.wasPressedThisFrame) southPressed = true;
                    if (gamepad.buttonEast.wasPressedThisFrame) eastPressed = true;
                }

                if (eastPressed)
                {
                    if (settingsMenuUI != null && settingsMenuUI.activeSelf) CloseSettings();
                    else Resume();
                }
                else if (southPressed && selected != null)
                {
                    UnityEngine.UI.Button btn = selected.GetComponent<UnityEngine.UI.Button>();
                    if (btn != null) btn.onClick.Invoke();
                }

                if (settingsMenuUI != null && settingsMenuUI.activeSelf && selected != null)
                {
                    UnityEngine.UI.Slider slider = selected.GetComponent<UnityEngine.UI.Slider>();
                    if (slider != null)
                    {
                        float horiz = 0f;
                        foreach (var gamepad in UnityEngine.InputSystem.Gamepad.all)
                        {
                            if (gamepad.dpad.left.wasPressedThisFrame) horiz = -1f;
                            if (gamepad.dpad.right.wasPressedThisFrame) horiz = 1f;
                            
                            float lx = gamepad.leftStick.x.ReadValue();
                            if (lx < -0.5f) horiz = -1f;
                            else if (lx > 0.5f) horiz = 1f;
                        }

                        if (horiz != 0f && Time.realtimeSinceStartup > lastSliderNavTime + 0.15f)
                        {
                            lastSliderNavTime = Time.realtimeSinceStartup;
                            float step = (slider.maxValue - slider.minValue) * 0.1f;
                            if (step <= 0f) step = 0.1f;
                            slider.value += horiz * step;
                        }
                    }
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        GameIsPaused = true;
        SetFocus(pauseMenuUI);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettings()
    {
        if (Time.unscaledTime - lastTransitionTime < 0.2f) return;
        lastTransitionTime = Time.unscaledTime;
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        SetFocus(settingsMenuUI);
    }

    public void CloseSettings()
    {
        if (Time.unscaledTime - lastTransitionTime < 0.2f) return;
        lastTransitionTime = Time.unscaledTime;
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        SetFocus(pauseMenuUI);
    }

    private void SetFocus(GameObject panel)
    {
        if (panel != null && UnityEngine.EventSystems.EventSystem.current != null)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            UnityEngine.UI.Selectable firstSelectable = panel.GetComponentInChildren<UnityEngine.UI.Selectable>();
            if (firstSelectable != null)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
            }
        }
    }
}
