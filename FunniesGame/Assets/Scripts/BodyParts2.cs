using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts2 : MonoBehaviour
{
    public Controller2Pj controller2pj;

    void Start()
    {
        controller2pj = GameObject.FindObjectOfType<Controller2Pj>().GetComponent<Controller2Pj>();
    }


    void OnCollisionEnter(Collision collision)
    {
        controller2pj.isGrounded = true;
    }
}
