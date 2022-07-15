
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool gameOver = false;
    void Start()
    {
        gameOver = false;

    }

    
    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0.5f;
           
        }
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
