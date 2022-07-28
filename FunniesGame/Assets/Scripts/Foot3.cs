using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot3 : MonoBehaviour
{
    public Controller3 controller3;



    public void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.CompareTag("Floor"))
        {
            controller3.floored = true;

        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            controller3.floored = false;

        }
    }
}
