using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    public PlayerController playerController;

    

    public void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.CompareTag("Floor")) 
        {
            playerController.floored = true; 

        }

    }

    private void OnCollisionExit(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            playerController.floored = false; 

        }
    }
}
