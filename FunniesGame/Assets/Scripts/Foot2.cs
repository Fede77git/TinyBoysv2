using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot2 : MonoBehaviour
{
    public Controller2Pj controller2pj;



    public void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.CompareTag("Floor"))
        {
            controller2pj.floored = true;

        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            controller2pj.floored = false;

        }
    }
}
