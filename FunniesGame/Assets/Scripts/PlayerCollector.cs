using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        
        if (playerController == null)
        {
            Debug.LogError("playercontroller not found in " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coleccionable"))
        {
            if (LevelManager.Instance != null && playerController != null)
            {
                LevelManager.Instance.AddCoin(playerController.playerIndex);
                
                if (playerController != null)
                {
                    playerController.PlayCoinSound();
                }

                other.gameObject.SetActive(false);
            }
        }
    }
}
