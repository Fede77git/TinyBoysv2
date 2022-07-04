
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerMenu : MonoBehaviour
{
   
    public void SceneLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void SceneLevel2()
    {
        SceneManager.LoadScene("Level2");
    }


    public void RandomScene()
    {
        int index = Random.Range(1, 4);
        SceneManager.LoadScene(index);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
