using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2Pj : MonoBehaviour
{ 
    public float speed = 150;
    public float strafeSpeed = 100;
    public float jumpForce;

    public Rigidbody pelvis;
    public bool isGrounded;
    public bool floored;

    public Animator animator;


    void Start()
    {
    pelvis = GetComponent<Rigidbody>();

    }


    void FixedUpdate()
{
    if (Input.GetKey(KeyCode.W))
    {
        pelvis.AddForce(pelvis.transform.forward * speed);
        animator.SetBool("isWalking", true);
    }
    else
    {
        animator.SetBool("isWalking", false);
    }

    if (Input.GetKey(KeyCode.A))
    {
        pelvis.AddForce(-pelvis.transform.right * strafeSpeed);
        animator.SetBool("isWalking", true);
    }


    if (Input.GetKey(KeyCode.D))
    {
        pelvis.AddForce(pelvis.transform.right * strafeSpeed);
        animator.SetBool("isWalking", true);
    }

    if (Input.GetKey(KeyCode.S))
    {
        pelvis.AddForce(-pelvis.transform.forward * speed);
        animator.SetBool("isWalking", true);
    }

    if (isGrounded && floored)
    {
        if (Input.GetKey(KeyCode.Space))
        {

            pelvis.AddForce(new Vector3(0, jumpForce, 0));
            isGrounded = false;

        }
    }

}


}
