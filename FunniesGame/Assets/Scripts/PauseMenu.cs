using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    void Awake()
    {
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
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        SetFocus(pauseMenuUI);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        SetFocus(settingsMenuUI);
    }

    public void CloseSettings()
    {
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
