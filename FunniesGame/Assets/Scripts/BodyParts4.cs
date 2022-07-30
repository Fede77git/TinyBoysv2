using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts4 : MonoBehaviour
{
    public Controller4 controller4;

    void Start()
    {
        controller4 = GameObject.FindObjectOfType<Controller4>().GetComponent<Controller4>();
    }


    void OnCollisionEnter(Collision collision)
    {
        controller4.isGrounded = true;
    }
}
