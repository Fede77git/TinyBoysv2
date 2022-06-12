using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    public PlayerController playerController;

    //private void OnTriggerStay(Collider other)
    //{
    //    playerController.floored = true;

    //}

    //private void OnTriggerExit(Collider other)
    //{
    //   playerController.floored = false;
    //}

    public void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.CompareTag("Floor")) // si esta tocando el suelo
        {
            playerController.floored = true; // se activa floored

        }

    }

    private void OnCollisionExit(Collision collision) // cuando deja de chocar con la colision
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            playerController.floored = false; // se desactiva floored

        }
    }
}
