
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

    public void SceneLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void SceneLevel4()
    {
        SceneManager.LoadScene("Level4");
    }

    public void RandomScene()
    {
        int index = Random.Range(1, 5);
        SceneManager.LoadScene(index);
    }

    public void SceneLevelSelectro()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void LavaCoinsMenu()
    {
        SceneManager.LoadScene("LavaCoinsMenu");
    }
    public void LaserRingMenu()
    {
        SceneManager.LoadScene("LaserRingMenu");
    }
    public void CapsuleCollectorMenu()
    {
        SceneManager.LoadScene("CapsuleCollectorMenu");
    }
    public void BombThreatMenu()
    {
        SceneManager.LoadScene("BombThreatMenu");
    }

}
