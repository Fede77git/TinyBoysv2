using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class VictoryManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public int id;
        public int points;
    }

    public GameObject[] playerPrefabs;
    public Transform[] podiumTransforms; 
    public TextMeshProUGUI[] podiumTexts; 

    void Start()
    {
        SetupVictoryScene();
    }

    private void SetupVictoryScene()
    {
        if (GlobalGameManager.Instance == null)
        {
            Debug.LogWarning("GlobalGameManager no encontrado.");
            return;
        }

        int cantidadJugadores = GlobalGameManager.Instance.cantidadJugadores;
        int[] puntajes = GlobalGameManager.Instance.puntajesJugadores;

        List<PlayerData> players = new List<PlayerData>();

        for (int i = 0; i < cantidadJugadores; i++)
        {
            players.Add(new PlayerData { id = i, points = puntajes[i] });
        }

        players = players.OrderByDescending(p => p.points).ToList();

        int lugaresAMostrar = Mathf.Min(3, players.Count);

        for (int i = 0; i < lugaresAMostrar; i++)
        {
            PlayerData pd = players[i];

            if (i < podiumTransforms.Length && podiumTransforms[i] != null)
            {
                if (pd.id >= 0 && pd.id < playerPrefabs.Length && playerPrefabs[pd.id] != null)
                {
                    Instantiate(playerPrefabs[pd.id], podiumTransforms[i].position, podiumTransforms[i].rotation);
                }

                if (i < podiumTexts.Length && podiumTexts[i] != null)
                {
                    podiumTexts[i].text = pd.points.ToString() + " PTS";
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalGameManager.Instance.volviendoDeNivel = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}
