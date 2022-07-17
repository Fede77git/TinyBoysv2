using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpLvl4 : MonoBehaviour
{
    public PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp1") == true)
        {
            
            playerController.speed += 1f;
            other.gameObject.SetActive(false);
        }
    }
}
