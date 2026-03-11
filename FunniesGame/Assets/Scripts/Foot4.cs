using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Foot4 : MonoBehaviour

{

    public Controller4 controller4;



    public void OnCollisionEnter(Collision collision)

    {


        if (collision.gameObject.CompareTag("Floor"))

        {

            controller4.floored = true;



        }



    }


    private void OnCollisionExit(Collision collision)

    {

        if (collision.gameObject.CompareTag("Floor"))

        {

            controller4.floored = false;



        }

    }

}