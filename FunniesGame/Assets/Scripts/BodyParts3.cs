using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts3 : MonoBehaviour
{
    public Controller3 controller3;

    void Start()
    {
        controller3 = GameObject.FindObjectOfType<Controller3>().GetComponent<Controller3>();
    }


    void OnCollisionEnter(Collision collision)
    {
        controller3.isGrounded = true;
    }
}
